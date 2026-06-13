# Modifier System

Pickups in Tiggy are composed of four orthogonal concerns. Each is an abstract ScriptableObject subclass, serialized onto the `Pickup` MonoBehaviour. Concrete implementations can be mixed and matched to produce new pickup behaviours without new code.

```
Pickup (MMPoolableObject)
  ├── Power    — what effect is applied to a character
  ├── Usage    — when/how the player triggers the effect
  ├── Resource — how much the effect can be used before it expires
  └── Area     — which characters are affected
```

---

## Power

Defines the effect applied to a `Character`. Each subclass applies and reverts one specific change.

**API:** `Apply(Character target)` / `Revert(Character target)`

Revert must restore the character exactly to pre-apply state. Powers that are inherently permanent (gravity flip) simply do nothing in `Revert`. Powers that are one-way by design (stun) revert via the ability's own cleanup.

**Planned concrete types:**

| Class | Effect | Revertible |
|---|---|---|
| `GravityFlipPower` | Calls `CharacterGravity.SetGravityAngle(180)` | Optional — can be permanent or timed |
| `SpeedPower` | Sets `CharacterHorizontalMovement.MovementSpeedMultiplier` | Yes — restores original multiplier |
| `StunPower` | Calls `CharacterStun.StunFor(duration)` | Managed by `CharacterStun` internally |
| `TimeDilationPower` | Fires `MMTimeScaleEvent` (global) | Yes — fires Unfreeze on revert |

---

## Usage

Defines when the effect fires relative to pickup.

**API:** `Trigger(Pickup pickup)` — called by `Pickup` at the appropriate moment, invokes `pickup.Activate()`.

**Planned concrete types:**

| Class | Behaviour |
|---|---|
| `InstantUsage` | Activates immediately on pickup — no player input required |
| `TriggeredUsage` | Pickup parents to player; activates when player presses the use button |

For `TriggeredUsage`, the pickup is reparented to the character on collection (colliders disabled, visual remains). The player's `PowerUpHolder : CharacterAbility` holds a reference to the carried pickup and forwards the use input to it.

---

## Resource

Defines how much the effect can be delivered before the pickup is exhausted and returned to the pool.

**API:** `Used()` — called by `Pickup` each time the power activates. Returns whether the resource is exhausted.

**Planned concrete types:**

| Class | Behaviour |
|---|---|
| `TimedResource` | Effect lasts for a fixed duration; expires after time elapses |
| `ChargeResource` | Fixed number of uses (e.g. 3 gravity flips); expires when charges hit zero |
| `UnlimitedResource` | Never expires — permanent effect for the remainder of the round |

`TimedResource` drives the revert: it starts a coroutine on the `Pickup`, waits the duration, calls `Power.Revert()`, then disables the object back to the pool.

A `TriggeredUsage` + `ChargeResource` combination produces the "hold item, use it N times" feel.

---

## Area

Defines which characters `Power.Apply()` is called on.

**API:** `GetAffected()` — returns `IEnumerable<Character>`.

**Planned concrete types:**

| Class | Behaviour |
|---|---|
| `SelfArea` | Only the character who picked it up |
| `AllPlayersArea` | Every player currently in `LevelManager.Instance.Players` |
| `OthersArea` | Every player except the one who picked it up |
| `RadiusArea` | All players within a world-space radius at the moment of activation |

> `Area` is noted as potentially awkward in `Pickup.cs`. The concern is that `GetAffected()` needs to know who the picker was — the picker reference should be passed in on `Activate(Character picker)` and forwarded to `Area.GetAffected(Character picker)`.

---

## Pickup lifecycle

```
1. Spawned by Loot system, sits in scene with collider active
2. Player walks into it → PickableItem.Pick(picker)
3. Usage decides: activate now (Instant) or parent and wait (Triggered)
4. On activation → Area.GetAffected(picker) → Power.Apply() per character
5. Resource.Used() called; if exhausted → Power.Revert() → gameObject.SetActive(false) → returned to pool
6. For Triggered: pickup is visually parented to picker until exhausted
```

---

## Example combinations

| Pickup | Power | Usage | Resource | Area |
|---|---|---|---|---|
| Speed boost (self) | `SpeedPower` | `InstantUsage` | `TimedResource(5s)` | `SelfArea` |
| Gravity flip (all, permanent) | `GravityFlipPower` | `InstantUsage` | `UnlimitedResource` | `AllPlayersArea` |
| Stun grenade | `StunPower` | `TriggeredUsage` | `ChargeResource(1)` | `RadiusArea` |
| Time slow (self, 3 uses) | `TimeDilationPower` | `TriggeredUsage` | `ChargeResource(3)` | `SelfArea` |
