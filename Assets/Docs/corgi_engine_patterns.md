# Corgi Engine Patterns

Gotchas, conventions, and patterns learned while working with Corgi Engine in this project.

---

## Event listening

`CorgiEngineEventTypes` is an **enum**, not a type hierarchy. `LevelStart` is a value, not a class.
The correct listener pattern is:

```csharp
public class Foo : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    void OnEnable()  => this.MMEventStartListening<CorgiEngineEvent>();
    void OnDisable() => this.MMEventStopListening<CorgiEngineEvent>();
    public void OnMMEvent(CorgiEngineEvent e)
    {
        if (e.EventType != CorgiEngineEventTypes.LevelStart) return;
        // ...
    }
}
```

The explicit generic on `MMEventStartListening<CorgiEngineEvent>()` is required — without it the compiler picks the wrong overload.

The MM docs example uses `MMAchievementUnlockedEvent` (a unique struct per event) which maps directly to the type parameter. `CorgiEngineEvent` is different: one shared struct carrying a `CorgiEngineEventTypes` enum value. The type passed to `MMEventListener<T>` is always the **struct**, never the enum value.

---

## Namespace convention

Namespaces follow the directory path under `Assets/Scripts/`: `Tiggy.Level`, `Tiggy.Characters`, `Tiggy.Modifiers`, `Tiggy.Pickups`, `Tiggy.Obstacles`, etc.

- The folder is `Level/` not `LevelManager/` — `Tiggy.LevelManager` was avoided because it shadowed the Corgi `LevelManager` class name inside that namespace.
- Always use **block-scoped** syntax — Unity 6000.x compiles C# 9 and does not support file-scoped namespaces (`namespace Foo;`), which is C# 10. File-scoped namespaces cause Unity to fail to recognise MonoBehaviours.

---

## Script placement

Level-scoped helper scripts (no prefab of their own, react to level events) go on the **same GameObject as MultiplayerLevelManager**. Only split to a child object if the count grows unwieldy.

The `Manager` suffix means a script owns and coordinates an ongoing system. Don't use it for one-shot or stateless helpers — prefer descriptive verb-noun names like `DifferentiatePlayers`.

---

## Single trigger collider is enough

A single **trigger** `BoxCollider2D` serves both purposes on an obstacle or AI enemy:
- `CorgiController` uses the collider bounds for its raycasting regardless of the trigger flag
- `DamageOnTouch` / `OnTriggerEnter2D` fire on the trigger as normal

No second non-trigger collider is needed.

---

## Trigger events between non-player objects require Rigidbody2D

Unity `OnTriggerEnter2D` only fires between two objects if at least one has a `Rigidbody2D`. Player prefabs have one, so player↔obstacle triggers work out of the box. For obstacle↔AI-enemy triggers (e.g. saw hitting snail body), add a `Rigidbody2D` (Body Type: **Kinematic**) to the obstacle. Kinematic means no gravity or force response — safe for objects moved via `MMPathMovement` or direct transform manipulation.

CorgiController raycasts are **not** subject to this rule — they hit any collider regardless of Rigidbody2D. That's why `IsCollidingLeft`/`IsCollidingRight` wall bounce detection works without a Rigidbody2D on the obstacle.

---

## Layer reference

Defined in `Assets/ThirdParty/CorgiEngine/Common/Scripts/Managers/LayerManager.cs`.

| Layer name | Number | Notes |
|---|---|---|
| Platforms | 8 | Ground and walls — included in CorgiController platform mask |
| Player | 9 | Player characters |
| OneWayPlatforms | 11 | Platforms passable from below |
| Projectiles | 12 | |
| Enemies | 13 | AI enemies (e.g. snail body) |
| MovingObjects | 17 | Moving hazards (e.g. saw). Only referenced by `CharacterCrushDetection` |
| MovingPlatforms | 18 | Moving solid platforms |
| Pushables | 27 | |
| Stairs | 28 | |

**`ObstaclesLayerMask` is not a layer** — it's a combined mask: `Platforms | MovingPlatforms | OneWayPlatforms`. There is no single "Obstacles" layer to assign objects to.
