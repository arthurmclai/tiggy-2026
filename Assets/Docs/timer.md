# Timer

**Status:** Not started  
**Depends on:** Nothing  
**Blocks:** Lobby, game type end conditions

---

## What it does

Counts down a round duration. Fires events at key intervals and at zero. Physically represented in the world as a kids' timer object — not a HUD element.

## Physical object behaviour

- **In lobby:** Interactable. Players interact with it to set duration (presets: short / medium / long, plus custom).
- **During a game:** Not interactable as a timer. Functions as a solid obstacle — wall or floor surface that players and obstacles can collide with.
- **At zero:** Fires `TimerExpiredEvent`. Audio cue plays. Round ends or transitions to celebration screen.
- **Audio:** Counts down audibly near the end. Sounds off at key intervals during the game (halfway, last 30s, etc.) — exact intervals TBD.

## Presets

| Label  | Duration   |
| ------ | ---------- |
| Short  | 2          |
| Medium | 5          |
| Long   | 10         |
| Custom | Player-set |

Durations to be decided during playtesting with the actual game.

## Open questions

- Exact interaction mechanic for setting the timer — does a player stand near it and press a button to cycle through presets, or is it more physical (spinning it, jumping on it)?
Hold to interact. Up and down through presets. Left and right to adjust by minute.

- Visual representation of the countdown on the object itself — analogue dial, filling bar, colour change? Must read without text.
Analogue dial

- What happens if no timer is set (Sandbox mode)? Timer object is present but idle — no countdown, no expiry event.
