# Obstacles

Obstacles are environmental hazards that stun players on contact. Unlike pickups they are not collected — they are persistent scene objects. Implementation priority: Saw → Snail.

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
- `StunOnTouch` (`Tiggy.Obstacles`) — subclass of `DamageOnTouch`, zero health damage, calls `CharacterStunOnHit.StunFor(StunDuration)` on the hit character. `StunDuration` is set per obstacle instance in the Inspector.
- `MMRotate` (MoreMountains Tools) — spins the transform on Z axis, no physics involvement
- `MMPathMovement` for pathed variants — waypoints are **relative offsets** from the object's starting position, not world coordinates. Waypoint 0 should be `(0,0,0)`.

**Stun handling:**
- Stun duration set on `StunOnTouch`, constant per obstacle instance
- Knockback from `StunOnTouch` pushes player away — `CharacterStunOnHit.ProcessAbility()` applies natural deceleration while stunned so the player bleeds off speed via physics rather than stopping instantly
- `InvincibilityDuration` on `StunOnTouch` prevents permastun — set to at least the stun duration

---

## Snail

A mobile creature that patrols autonomously. Stomping it retreats it into its shell; the shell can then be kicked to slide it as a weapon.

**GameObject structure:**

```
SnailRoot (empty)
├── SnailBody  [starts active]
└── SnailShell [starts inactive]
```

**States:**

| State | Lives on | Behaviour | Dangerous |
|---|---|---|---|
| Patrolling | SnailBody | Walks back and forth | Yes — `StunOnTouch` active |
| Shelled | SnailShell | Stationary, kickable, respawn timer counting | No |
| Sliding | SnailShell | Moving at speed, bounces off walls, decelerates | Yes — `StunOnTouch` active |

**State transitions:**
- Patrolling → Shelled: player stomps SnailBody (`SnailStompable.PerformStomp`) — deactivates SnailBody, activates SnailShell
- Shelled → Sliding: player makes lateral trigger contact with shell (`Shell.OnTriggerEnter2D`) — shell launches in opposite direction
- Sliding → Shelled: shell decelerates to ~0, or player stomps shell (`ShellStompable.PerformStomp`) — respawn timer starts
- Shelled (timer expires) → Patrolling: SnailShell deactivates, SnailBody re-enables at shell position

**SnailBody components:**
- `CorgiController`, `Character` (AI type), `Health`
- `CharacterHorizontalMovement` — required by `AIActionPatrol`
- `AIBrain` (one state: Patrolling, `ResetBrainOnEnable = true`), `AIActionPatrol`
- `StunOnTouch` — always active while body is active
- `SnailStompable` — subclass of `Stompable`; `PerformStomp` bounces the stomper, positions and activates shell via `Shell.Activate()`, deactivates self
- `BoxCollider2D` (trigger) — one collider serves both CorgiController and StunOnTouch

**SnailShell components:**
- `CorgiController`, `Health`
- `StunOnTouch` (`GiveGracePeriod = true`) — toggled off in Shelled state, on in Sliding
- `Shell` — state machine script managing Shelled/Sliding, wall bouncing, deceleration, respawn
- `ShellStompable` — subclass of `Stompable`; calls `Shell.HandleStomp()` which stops if sliding or kicks if shelled
- `BoxCollider2D` (trigger) — **single trigger collider** works for both CorgiController physics and StunOnTouch/Shell trigger detection; no second non-trigger collider needed

**Kicker grace period:**
`StunOnTouch.GiveGracePeriod = true` on the shell. When a player kicks or stomps the shell into sliding, `Shell` calls `StunOnTouch.SetGracePeriod(kicker, duration)` — that player is skipped in `OnCollideWithDamageable` for `KickGracePeriod` seconds, giving them time to move away. Other players are still stun-eligible immediately.

**Key script: `Shell.cs`**
- `Activate()` — called by `SnailStompable` on first stomp; enters Shelled state
- `HandleStomp(CorgiController)` — called by `ShellStompable`; stops if sliding, kicks if shelled
- Wall bounce: checks `IsCollidingLeft`/`IsCollidingRight` each frame and flips `_slideDirection`; owns `_slideSpeed` separately so wall impact (which zeros `Speed.x`) doesn't lose direction
