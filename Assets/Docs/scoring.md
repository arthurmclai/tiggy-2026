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

## Event registry

Scoring works by listening to domain events that already describe "what happened" — gameplay systems never know scoring exists. `ScoreManager` (and eventually SFX, VFX, celebration-screen stat tracking) all subscribe independently to the same events.

This table is the running catalogue: every meaningful gameplay event, whether it already exists (Corgi or our own code) or needs to be added, and who would fire it. Add to this list whenever a new system is built — it's useful far beyond scoring.

| Event | Fired by | Exists? |
|---|---|---|
| `MMCharacterEvent` (`Jump`) | Corgi `CharacterJump` | Yes |
| `MMCharacterEvent` (`Dash`) | Corgi `CharacterDash` | Yes |
| `MMCharacterEvent` (`WallJump`) | Corgi `CharacterWallJump` | Yes |
| `MMCharacterEvent` (`WallCling`) | Corgi `CharacterWallClinging` | Yes |
| `MMCharacterEvent` (`Crouch`) | Corgi `CharacterCrouch` | Yes |
| `MMCharacterEvent` (`Glide`) | Corgi `CharacterGlide` | Yes |
| `MMCharacterEvent` (`Dive`) | Corgi `CharacterDive` | Yes |
| `MMCharacterEvent` (`Roll`) | Corgi `CharacterRoll` | Yes |
| `MMCharacterEvent` (`Stun`) | Corgi `CharacterStun` (via `CharacterStunOnHit`) | Yes |
| `MMDamageTakenEvent` | Corgi `Health.Damage()` — fires even at zero damage, so `StunOnTouch` hits included | Yes |
| `HealthChangeEvent` / `HealthDeathEvent` | Corgi `Health` | Yes (death not used — `InfiniteHealth` resets instead) |
| `TagEvent` | `Tagger` | Yes |
| Shell kicked (Shelled → Sliding) | `Shell` | No — needs new event |
| Shell stomped while sliding (Sliding → Shelled) | `Shell` | No — needs new event |
| Shell respawn (snail re-appears) | `Shell` | No — needs new event |
| Snail shelled by player stomp | `SnailBody` / `SnailStompable` | No — needs new event |
| Snail shelled by obstacle (saw hits snail) | `SnailBody` | No — needs new event |
| Role changed (`PlayerRole.OnRoleChanged`) | `PlayerRole` | Yes, but C# `event` not MM event — fine for in-process listeners on the same character, would need converting to an MM event if cross-system listeners (scoring) want it directly. Scoring likely doesn't need this — `TagEvent` already captures the moment |
| Avoid tag (close call) | Tag mechanic | No — needs definition: proximity threshold + directional escape, TBD |
| Proximity tension bonus | Tag mechanic / game type scripts | No — needs definition: radius, number of nearby It players, escape threshold, TBD. Appears in Gang Up, Red Rover, Octopus |
| Pick up powerup | Powerup system | No — powerup system not yet implemented |
| Use power | Powerup system | No — powerup system not yet implemented |
| Game-type specific moments (e.g. last Free player caught) | Game type managers | No — define per game type as built |

## Action → score mapping

`ScoreManager` is the only place that knows which events score and how much. It implements `MMEventListener<T>` per event type above that's marked scoring-relevant, extracts `(actor, actionId)` from each, and feeds a shared novelty/combo routine. Adding a new scoring action later means adding one listener method to `ScoreManager` — no changes anywhere else.

## Display

- **Physical scoreboard** in the level — like a sports scoreboard (basketball, baseball). Reacts visually as the score climbs.
- **On-screen toasts** when notable scoring moments happen — high combo, novelty spike, proximity bonus. Paired with audio call-outs and celebratory sound fx to build excitement.

## Stretch targets

- **Currency payout:** When a session ends, the cumulative score pays out as in-game currency used to purchase cosmetic customisations.
- **Unlockables by play style:** Cosmetics earnt by achieving specific action combinations (only jumping, never jumping, no horizontal movement, etc.) — rewards creative and unusual play.
- **Per-player contribution tracking:** Stored alongside cumulative total for the celebration screen and future systems. Not displayed as a ranking.