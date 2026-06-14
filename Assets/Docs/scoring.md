# Scoring System

**Status:** Not started  
**Depends on:** Role system (for tag events), Character abilities (for jump events)  
**Blocks:** Nothing — but needed for any game type to feel meaningful

---

## What it does

Listens to action events from across all systems and accumulates a single shared cumulative session score. The score never decreases. Every player action contributes. New systems (powerups, new game types) just fire events — scoring hooks in without requiring changes to those systems.

## Score model

- **One shared score** for the whole group. No per-player score at MVP.
- Persists across rounds within a session. Resets when players choose to start a new session.
- Displayed as a running total — format TBD (world object preferred over HUD number per playground-as-world directive).

## Novelty multiplier

Every action carries a multiplier based on recency in two stacked dimensions:

- **Personal novelty:** time since *this player* last performed this action type
- **Global novelty:** time since *anyone* last performed this action type

Both decay toward 1.0x over time. Both are above 1.0x when the action is novel. They multiply together. Exact decay curves to be tuned during playtesting.

Effect: discourages repetitive optimal play (two players back-tagging for cheap points), rewards variety and creativity.

## Combo

Separate from novelty. A combo builds when a player performs many *different* action types in a short time window. Each distinct action type within the window extends the combo and applies a flat bonus multiplier to that action's score. Performing the same action type twice does not extend the combo.

Combo window duration TBD — tune during playtesting.

## Action registry (initial set)

| Action | Fired by |
|---|---|
| Tag | Tag mechanic |
| Avoid tag (close call) | Tag mechanic |
| Jump | `CharacterJump` ability |
| Pick up powerup | Powerup system |
| Use power | Powerup system |
| Stomp obstacle | `SnailStompable` / `ShellStompable` |
| Kick shell | `Shell` |
| Proximity tension bonus | Tag mechanic / game type scripts |
| Game-type specific actions | Game type scripts |

**Proximity tension bonus:** Awarded when a Free player is surrounded by multiple chasers and escapes. Rewards the most exciting moments in any chase game — the narrow escape, the outmanoeuvring of several It players at once. Appears in Gang Up, Red Rover, and Octopus. Exact detection (radius, number of nearby It players, escape threshold) TBD during playtesting.

Close call detection (avoid tag) needs definition — proximity threshold plus directional escape, TBD.

## Display

- **Physical scoreboard** in the level — like a sports scoreboard (basketball, baseball). Reacts visually as the score climbs.
- **On-screen toasts** when notable scoring moments happen — high combo, novelty spike, proximity bonus. Paired with audio call-outs and celebratory sound fx to build excitement.

## Stretch targets

- **Currency payout:** When a session ends, the cumulative score pays out as in-game currency used to purchase cosmetic customisations.
- **Unlockables by play style:** Cosmetics earnt by achieving specific action combinations (only jumping, never jumping, no horizontal movement, etc.) — rewards creative and unusual play.
- **Per-player contribution tracking:** Stored alongside cumulative total for the celebration screen and future systems. Not displayed as a ranking.