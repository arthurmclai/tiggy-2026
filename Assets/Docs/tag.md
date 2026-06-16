# Tag Mechanic

**Status:** Implemented  
**Depends on:** Role system  
**Blocks:** Scoring system, all tag-based game types

---

## What it does

Player-to-player contact where the **It** player touches a **Free** player, transferring the It role. Distinct from `StunOnTouch` — a tag does not stun; it transfers a role. The stun on obstacle contact is a separate mechanic.

## Key design decisions

- **Detection:** Player layer check first (layer 9, one bitwise op, zero allocations), then `PlayerRole.CurrentRole == Free` confirmation. Cheap and unambiguous — no string comparisons, no GetComponent fishing.
- **Directionality:** Only It can initiate a tag. The `Tagger` component acts only when `PlayerRole.CurrentRole == Role.It`.
- **Back-tag grace period:** After being tagged, the newly-free player cannot be immediately re-tagged. Duration tuned during playtesting.
- **No stun on tag:** A tag is a role transfer only. The previous It is free to move immediately.
- **Feedback:** Visual/audio pop on tag is a polish TODO — not MVP.

## Architecture

### `TagEvent`

MM-style struct event fired when a valid tag occurs:

```
TagEvent
  ├── Tagger (Character)
  └── Tagged (Character)
```

`GameTypeManager` listens for this and applies the role transition rules for the current game type.

### `Tagger` component (on every player)

`OnTriggerEnter2D`:
1. Check own `PlayerRole.CurrentRole == Role.It`
2. Check `other.gameObject.layer == Player layer`
3. Get `PlayerRole` from other — check `CurrentRole == Role.Free`
4. Check grace period has expired for the other player
5. Fire `TagEvent(self, other)`
6. Start grace period on the tagged player

Grace period is tracked on `Tagger` as a `Dictionary<Character, float>` of expiry times — same pattern as `StunOnTouch`.

## Success criteria

- `TagEvent` struct defined and fireable
- `Tagger` component on player prefab — fires `TagEvent` on valid It→Free contact, respects grace period
- Game still fully playable with no active game type — `Tagger` does nothing when role is `None`
