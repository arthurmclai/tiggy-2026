# Role System

**Status:** Not started  
**Depends on:** Nothing (foundational)  
**Blocks:** Tag mechanic, scoring system, all tag-based game types

---

## What it does

Tracks the current role of each player. Roles change during gameplay in response to tags and game type rules. Other systems (scoring, game types) listen to role change events. `PlayerRole` applies all consequences of a role change by executing a list of `RoleEffect` strategies supplied by the `GameTypeManager`.

## Roles

| Role | Mobile | Can Tag | Can Use Powers | Description |
|---|---|---|---|---|
| None | Yes | No | Yes | No game type active — lobby / sandbox state |
| Free | Yes | No | Yes | Default in-game state — being chased |
| It | Yes | Yes | Yes | Chasing, initiates tags |
| Assist | No | Yes | Yes | Immobilised, can tag players in reach and use powerups to disrupt (Octopus) |

Not all roles are active in every game type. Sandbox and Keepy modes use `None`.

---

## Architecture

### `RoleEffect` (abstract)

A small self-contained strategy that knows how to apply and revert one specific change to a character:

```
RoleEffect (abstract)
  ├── Apply(Character)
  └── Revert(Character)
```

Concrete implementations:

| Class | Effect |
|---|---|
| `DisableMovementEffect` | Disables `CharacterHorizontalMovement` |
| `DisableJumpEffect` | Disables `CharacterJump` |
| `SetHatEffect` | Swaps hat GameObject above player GO |

Adding a new constraint (slow movement, disable dash) is a new `RoleEffect` subclass only — no changes to `PlayerRole` or `GameTypeManager`. Mirrors the `Power` pattern in the modifier system.

### `GameTypeManager` (central)

Owns the authoritative map of `Character → Role` for the current game. The only thing that *writes* roles. Defines a list of `RoleEffect` instances per role for its game type. Responsibilities:

- Assigns initial roles at game start
- Listens for `TagEvent` and applies transition rules for the current game type
- Answers game-logic queries: "who is currently It?", "how many Free players remain?", "is the game over?"
- Supplies the `RoleEffect` list when calling `playerRole.SetRole(role, effects)`

### `PlayerRole` component (per character)

Reflects the manager's assignment on the character. When `SetRole(role, effects)` is called:

1. Reverts all effects from the previous role
2. Applies all effects for the new role
3. Fires `OnRoleChanged` event

Everything on the character reads `PlayerRole.CurrentRole`. Nothing reads directly from the manager.

### Tag ability (on character)

Checks `PlayerRole.CurrentRole == Role.It` before initiating a tag. Fires `TagEvent` on valid contact with a Free player. Does not apply role transitions — that is the manager's job.

---

## Role visuals

`SetHatEffect` handles hat swapping as one of the role effects — consistent with how movement is handled. Hat prefab is configured on the effect instance in the Inspector. `None` role has no `SetHatEffect` so no hat is shown.

**MVP block-out:** Placeholder shape prefabs, one per role.  
**Stretch goal:** Cosmetics system replaces `SetHatEffect` with a `SetCosmeticEffect` that defers to per-player customisation.

---

## Success criteria (implementation complete when)

The game runs identically to today — no visible change during play. The following classes exist and are ready for the tag mechanic and game type managers to use:

- `PlayerRole` enum (`None`, `Free`, `It`, `Assist`)
- `RoleEffect` abstract class with `Apply(Character)` / `Revert(Character)`
- `DisableMovementEffect`, `DisableJumpEffect`, `SetHatEffect` concrete effects
- `PlayerRole` MonoBehaviour on the player prefab — holds `CurrentRole`, accepts `SetRole(role, effects)`, fires `OnRoleChanged`, applies/reverts effects
- `GameTypeManager` abstract base class — holds `Character → Role` map, exposes `SetRole`, `GetRole`, `GetPlayersWithRole(role)`, `OnRoleChanged` listener hook
- All players start with `PlayerRole` at `None` with no effects applied
