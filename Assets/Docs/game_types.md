# Game Types

Design document for all game modes. See `CLAUDE.md` design directives and `scoring.md`, `roles.md`, `lobby.md`, `timer.md` for the systems that underpin every mode.

---

### Sandbox

The default state. No enforced rules. All universal scoring active. Players decide how to play.

The timer is optional. If set, it sounds off at the end; that's all it does. Players self-organise everything else.

Sandbox is also the lobby state between games.

---

### Tiggy (Tip Chasey)

One player is **It**. It chases and tags a Free player, who then becomes It. The group scores by making as many tags as possible before the timer ends.

**Roles:** It, Free

**Scoring:**
- Successful tag → scores for group (novelty multiplier applies: tagging someone who hasn't been It in a while scores more)
- Back-tag prevention: a grace period after being tagged means the previous It cannot be immediately re-tagged (mechanical, not just scoring)
- All universal actions score on top

~~**Win condition:** None. Cumulative score. Try to beat your last session.~~ (scoring is for fun across all game types, lets not focus on win conditions)
**End condition**: timer ends → celebration moment → timer either ends the round or players reset and go again.

Role visuals (hat block-out, customisable wearables) are detailed in `roles.md`.

---

### Gang Up

All tagged players become **It** — the It count grows until everyone is It. The group scores for every tag, and ~~for how fast everyone ends up It together~~ (can be exploited and not very fun)

**Roles:** It, Free

**Scoring:**
- Each tag scores
- Bonus when the last Free player is caught (group celebration moment)
- Bonus for every second the last Free player is not caught (incentivise the reverse to prevent giving up)
- Bonus for every player close to a Free player that is avoiding being caught by multiple players
- All universal actions score

~~**Win condition:** None. Collective score. Optional: time-trial framing — how fast can the group gang up on everyone?~~

**End condition:** All players are It → celebration moment → timer either ends the round or players reset and go again.

---

### Red Rover

**It** players form a line/wall in the middle. **Free** players are on one side. The It side calls — all Free players run to the other side at once. Tagged players during the run join the It side. Repeat until all players are It.

**Roles:** It, Free

**Turn structure:** Open-ended — the It side decides when to call. No enforced timing. Players self-organise.

**Scoring:**
- Successful run across (Free player makes it) → scores
- Successful tag during a run → scores
- Both sides contribute to the cumulative score — being caught is not a punishment, it's a scoring moment
- Bonus for every player close to a Free player that is avoiding being caught by multiple players
- All universal actions score

**End condition:** All players are It → celebration moment → timer either ends the round or players reset and go again.

---

### Octopus

Two players start as **It**. Tagged players become **Assist** — immobilised but still able to tag players who come within reach, somehow able to collect powers and use powerups to disrupt. The It and Assist players work together to catch the remaining Free players.

**Roles:** It (mobile, can tag), Assist (immobile, can tag), Free

**Scoring:**
- Tag by It → scores
- Tag by Assist → scores (novelty bonus — Assist tags are harder to land)
- Bonus for every player close to a Free player that is avoiding being caught by multiple players
- All universal actions score

**End condition:** All players are It or Assist → celebration moment → timer either ends the round or players reset and go again.

---

### Keepy Uppy

Players keep an **object** (ball, puck, balloon) in the air. Co-operative by default — everyone loses if it hits the ground. How long can the group keep it going? High score is total hits or total airtime. 

**Scoring:**
- Each hit that keeps the object up → scores
- Consecutive hits without the object touching the ground → combo multiplier builds
- All universal actions score

**End condition:** When object hits the ground.

**Ground contact:** Resets the combo, but the round continues. The object doesn't disappear — players retrieve it and go again.

---

### Keepy Shelly

Players interact with the **snail shell** to keep it active and moving. Open-ended — players can self-organise into two informal groups if they want (one group tries to keep it shelled and sliding, another tries to keep the snail alive and unshelled) or play co-operatively toward a shared goal.

**Scoring:**
- Shell hit (keeps it moving) → scores
- Snail stomp (shells it) → scores
- Long shell slide duration → bonus
- All universal actions score

Scoring is action-based in both directions so any style of play accumulates score — no play style is penalised.

**End condition:** Timer fires or players choose to stop.

