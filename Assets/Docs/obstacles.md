# Obstacles

Obstacles are environmental hazards that stun players on contact. Unlike pickups they are not collected — they are persistent scene objects. Implementation priority: Saw → Mace → Snail.

---

## Saw

A spinning blade. May be stationary or follow a path.

**Behaviour:**
- Rotates continuously (visual only, no gameplay meaning to the spin angle)
- On contact with any player → `CharacterStun.StunFor(duration)` on that player
- Stuns from all sides — no safe face

**Movement variants (configured per instance):**
- Stationary — fixed position
- Pathed — follows a `MovingPlatform` path or a simple back-and-forth along one axis

**Corgi building blocks:**
- `DamageOnTouch` with target layer set to Player, zero health damage, knockback disabled — used as the contact trigger. Override or subclass to call `CharacterStun` instead of dealing health damage.
- `MovingPlatform` for pathed variants

**Stun handling:**
- Stun duration is constant regardless of contact time
- On stun, `DamageOnTouch` knockback pushes the player away from the saw — if the knockback force is sufficient the player clears the collider before the stun ends, avoiding permastun naturally
- If knockback alone isn't enough, `DamageOnTouch` has a built-in `InvincibilityDuration` — set this to at least the stun duration so the player can't be re-stunned mid-stun by the same or another saw

---

## Mace

A heavy head on a chain/arm that swings or pumps in a single axis (vertical or horizontal). Has a safe side and a dangerous side.

**Behaviour:**
- Oscillates between two points (think a piston or pendulum motion) on a configured axis
- Dangerous face: the striking head — stuns on contact
- Safe faces: the sides and back of the swing — player can stand on them as a moving platform

**Movement:**
- Driven by `MovingPlatform` (back-and-forth between two waypoints)
- The head collider is a separate child GameObject from the platform surface collider

**Corgi building blocks:**
- `MovingPlatform` on the arm/body for player to ride the safe sides
- `DamageOnTouch` (subclassed, same as Saw) on the head child object only — stun, no health damage
- Dangerous direction is enforced by collider placement: the head collider only exists on the striking face, so contact is naturally directional

**Variants:**
- Vertical mace — pumps up/down, dangerous on the downswing underside
- Horizontal mace — swings left/right, dangerous on the leading face

**Strike direction:**
- Only the active strike stuns — the return swing is safe
- Detected by checking `MovingPlatform` velocity direction at moment of contact; `DamageOnTouch` is only active when the head is moving toward its strike target point

---

## Snail

A mobile creature that patrols autonomously. Can be stomped into its shell, and the shell can then be hit to send it sliding.

**States:**

| State | Behaviour | Dangerous to players |
|---|---|---|
| Patrolling | Walks back and forth using `AIActionPatrol` | Yes — `DamageOnTouch` active, stuns on contact |
| Shelled | Stationary after being stomped | No |
| Sliding | Shell moving at speed after being hit | Yes — `DamageOnTouch` active |

**State transitions:**
- Patrolling → Shelled: player lands on top (`Stompable` subclassed to trigger brain state change instead of health damage)
- Shelled → Sliding: player makes lateral contact with the shell (`DamageOnTouch` on shell detects player contact, fires state transition and applies directional force via `CorgiController.SetForce`)
- Sliding → (destroyed or returns to Patrolling): TBD

**Corgi building blocks:**
- `AIBrain` with three states wired to the above transitions
- `AIActionPatrol` for the Patrolling state
- `Stompable` subclassed — `PerformStomp` triggers state transition rather than health damage
- `DamageOnTouch` subclassed — calls `CharacterStun.StunFor(duration)` instead of health damage; toggled on/off per state
- `CharacterStun` on players (must already be present on player prefab)

**Reference:** `SuperHipsterBros/Prefabs/AI/BulkyDude.prefab` and `BadassDude.prefab` show how patrol AI + `Stompable` + `DamageOnTouch` are wired together. `SuperHipsterBrosHealth` shows intercepting damage to change state rather than die.
