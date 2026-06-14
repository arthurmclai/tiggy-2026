# Lobby

**Status:** Not started  
**Depends on:** Timer, Role system, at least one game type implemented  
**Blocks:** Nothing — last system to build

---

## What it does

The lobby is not a separate scene or menu. It is the default in-game state — players drop in, move around freely, and interact with physical world objects to choose a game and set a timer. When a game ends (timer fires or players choose to stop), the level returns to lobby state.

Sandbox mode and the lobby are effectively the same state. The only difference is intent.

## Game type selection

Each game type is represented by a **physical prop** in the level. Props should be abstract but legible — evocative of the game, not a sign with text.

- Players interact with a prop to cast a vote for that game type.
- The most-voted prop wins. Tie-breaking rule TBD.
- Once a game type is selected, the round starts (initial roles assigned, timer begins if set).

Props land back in the level when a round ends, ready to be voted on again.

## Lobby state

- All universal scoring is active (players are still accumulating score even in lobby/sandbox).
- No roles assigned — all players are Free.
- Timer is interactable.
- Game type props are interactable.

## Celebration screen

Triggered by `TimerExpiredEvent`. Overlays the level (or transitions briefly) to show:
- Cumulative session score
- Total actions by type (tags, jumps, powerups used, etc.)
- Favourite powerup (most used this round)

No player ranking. Stays up until all players choose to continue, then returns to lobby state.

Celebration screen is optional — skippable when setting up a round.

Feels like celebration screen should be moved into a stretch target. Stats etc is a nice to have. But we should keep track of it all not just for celebration screen but for unlockables, additional game types, other systems that might need it in future.

## Open questions

- How does a player interact with a prop to vote? Walk into it, stand near it for N seconds, press a button while near it?
- How is the current vote count shown on each prop without text? Size, glow intensity, number of orbiting particles equal to vote count?
Players pitch a flag next to the prop that represents the game they want to play. The flag indicates the vote count. Players can change their mind and interact with another prop which will move their flag next to the new prop. Once all players have pitched their flags, a counter will count until game starts.

- What is the tiebreak rule for equal votes?
Do not codify, the counter will not start if there is a tie. Need to indicate somehow that there is a tie. Let the players convince each other which game to play next. They can settle it with a self designated game within the lobby itself if they want.

- How are game type props physically designed? Each needs to be readable without a label — to be handled by the art/level design pass.
Initially, while blocking out the game, we can have text or a cube or some basic shape or free asset and improve as the game nears polish