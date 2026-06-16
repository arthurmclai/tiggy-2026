# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

2D couch co-op tag game (tiggy/tip chasey) built on **Unity 6000.3.17f1** using the **Corgi Engine** (MoreMountains) platformer framework with URP rendering and Unity Input System. Players can pick up power-ups that manipulate gravity, time-scale, or other physics — applied to themselves, all players, or as area-of-effect.

## Development

This is a Unity project. All building, testing, and running is done via the Unity Editor on Windows. There is no CLI build step. The solution file `tiggy-2026.sln` can be opened in Rider or Visual Studio for C# editing with IntelliSense.

Tests are run through Unity's Test Runner window (`Window > General > Test Runner`).

## Architecture

### Corgi Engine overview

All core engine code lives in `Assets/ThirdParty/CorgiEngine/Common/Scripts/`. Treat this as a third-party library — extend by subclassing, don't edit in place.

Key classes:

- **`Character`** (`Agents/Core/Character.cs`) — the mandatory root component on every player prefab. Set `CharacterType = Player` and `PlayerID = "Player1"` … `"Player4"` to wire it up to input.
- **`CorgiController`** — handles 2D physics/collision; sits alongside `Character` on the prefab.
- **`CharacterAbility`** (`Agents/CharacterAbilities/CharacterAbility.cs`) — base class for all abilities (jump, dash, gravity, etc.). Add ability components to the Character GameObject. New abilities override `HandleInput()` and `ProcessAbility()`.
- **`LevelManager`** (`Managers/LevelManager.cs`) — singleton that spawns player prefabs and manages checkpoints. For multiplayer use **`MultiplayerLevelManager`**, which adds last-survivor win detection.
- **`PickableItem`** (`Items/PickableItem.cs`) — base class for all pickups. Override `Pick(GameObject picker)` to apply the power-up effect.

### Event system

MoreMountains uses a struct-based event bus. To listen:

```csharp
public class MyClass : MonoBehaviour, MMEventListener<SomeEvent>
{
    void OnEnable()  => this.MMEventStartListening();
    void OnDisable() => this.MMEventStopListening();
    public void OnMMEvent(SomeEvent e) { /* handle */ }
}
```

To fire: `SomeEvent.Trigger(...)` or `MMEventManager.TriggerEvent(new SomeEvent(...))`.

### Power-up building blocks already in the engine

- **Gravity manipulation** — `CharacterGravity` ability + `GravityZone` / `GravityPoint` environment objects.
- **Time scale** — `MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, speed, duration, ...)`. `TimeModifier` item (`Items/TimeModifier.cs`) shows the pattern for a timed effect pickup.
- **AOE / zones** — use `GravityZone`/`ForceZone` as templates for area-based power-ups.

### Custom game code

All game-specific scripts live in `Assets/Scripts/`, namespaced by folder:

| Folder | Namespace | Contents |
|---|---|---|
| `Characters/` | `Tiggy.Characters` | `InfiniteHealth` (death resets health instead of dying), `CharacterStunOnHit` (preserves knockback through stun, applies natural deceleration while stunned), `AnimatorSync` (mirrors parent Character's animator bool parameters to a child GameObject's animator; only fires SetBool on change to avoid restarting looping animations) |
| `Level/` | `Tiggy.Level` | Scene-level helpers: `DifferentiatePlayers`, `AssignPlayersToCinemachine`, `ConfineCinemachineToBounds` |
| `Obstacles/` | `Tiggy.Obstacles` | `StunOnTouch` (DamageOnTouch subclass, zero damage, optional per-kicker grace period), `SnailBody` (shell transition owner), `SnailStompable` + `ShellStompable` (Stompable subclasses), `Shell` (Shelled/Sliding state machine) |
| `Roles/` | `Tiggy.Roles` | `Role` enum, `RoleEffect` (abstract ScriptableObject), `PlayerRole` (per-character component), `Tagger` (fires `TagEvent` on It→Free contact), `TagEvent` (MM-style struct event), `MarkerConfiguration` (per-character marker slots and current selection). Effects in `Roles/Effects/`: `DisableMovementEffect`, `DisableJumpEffect`, `SetMarkerEffect` (signals show/hide to `MarkerConfiguration`) |
| `GameTypes/` | `Tiggy.GameTypes` | `GameTypeManager` (abstract base — owns character→role map, tag grace period, `SetRole`/`GetRole`/`GetPlayersWithRole`), `TiggyGameTypeManager` (picks random It at start, swaps It on `TagEvent`) |
| `Utilities/` | `Tiggy.Utilities` | `TransformExtensions` — `FindDeep(name)` extension method for deep child search by name |
| `Modifiers/` | `Tiggy.Modifiers` | Abstract ScriptableObject bases: `Power`, `Usage`, `Resource`, `Area` — not yet implemented |
| `Pickups/` | `Tiggy.Pickups` | `Pickup` — abstract `MMPoolableObject` composing the four modifier types — not yet implemented |

### Player prefab

`Assets/Prefabs/Characters/Rectangle.prefab` — current player prefab. Components: `Character`, `CorgiController`, `InfiniteHealth`, `CharacterStunOnHit`, and desired `CharacterAbility` components.

### Obstacle prefabs

`Assets/Prefabs/Obstacles/` — `Saw.prefab` (spinning, uses `MMRotate` + `StunOnTouch` + `MMPathMovement` + kinematic `Rigidbody2D`), `Snail.prefab` (two-child: SnailBody patrol AI + SnailShell sliding state machine).

### Scenes

`Assets/Scenes/Pipeline.unity` — the main (currently only) scene. `MultiplayerLevelManager` in the scene drives player spawning; set `PlayerPrefabs` on it.

### Input

Uses Unity Input System (`Assets/InputSystem_Actions.inputactions`). Corgi Engine bridges this via `InputSystemManagerEventsBased` (`ScriptsInputSystem/Scripts/`). Each player's input is routed to the matching `PlayerID`.

## Design directives

See `Assets/Docs/design_directives.md` for the full list. Read it before making any gameplay, UI, or scoring decision.

## Key conventions

- Player IDs must be `"Player1"` through `"Player4"` — this string must match between the `Character` component and the `InputManager`.
- New pickups extend `PickableItem` and override `Pick(GameObject picker)`. Use `MMTimeScaleEvent` or `CharacterGravity` public methods rather than touching `Time.timeScale` or physics directly.
- Singletons (LevelManager, GameManager, GUIManager) are accessed via `.Instance`; never cache them in `Awake` — use `Start` or later.
- Namespaces follow the directory path under `Assets/Scripts/`: `Tiggy.LevelManager`, `Tiggy.Modifiers`, `Tiggy.Pickups`, `Tiggy.Obstacles`, etc. Use block-scoped namespace syntax — Unity 6000.x targets C# 9 and does not support file-scoped namespaces (`namespace Foo;`).
