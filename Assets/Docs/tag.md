# Tag Mechanic

**Status:** Not started  
**Depends on:** Player prefab (done)  
**Blocks:** Role system, Scoring system

---

## What it does

Player-to-player contact where the **It** player touches a **Free** player, transferring the It role. Distinct from `StunOnTouch` — a tag does not stun; it transfers a role. The stun on obstacle contact is a separate mechanic.

## Key design decisions

- **Detection:** Trigger-based, player↔player. The It player's collider detects contact with a Free player's collider.
- **Directionality:** Only It can initiate a tag. Free players touching each other do nothing.
- **Back-tag grace period:** After being tagged, the newly-free player cannot be immediately re-tagged. Duration TBD — start with the same grace period pattern already used in `StunOnTouch`.
- **No stun on tag:** A tag is a role transfer only. The previous It is free to move immediately.

## Open questions

- Does a tag feel satisfying without any impact feedback? May need a visual/audio pop even without stun.
Yes definitely needs feedback but can be a TODO during polish phases.

- Grace period duration — to be tuned during playtesting.

- How does the It player's collider differentiate Free players from obstacles, shells, etc.? Layer mask or tag check.
Good question, recommendations? Whats cheapest and simplest without being hacky?