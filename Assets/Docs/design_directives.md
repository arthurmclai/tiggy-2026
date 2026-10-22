# Design Directives

Design principles and guidelines to follow but can be consciously broken for the purpose of keeping things fun and cooperative. Apply when making any gameplay, UI, or scoring decision.

## Playtesting notes

- **Flag marker creates pressure, not fun** (playtested with a child). The It indicator made the game feel too competitive/pressured. Consider making role visuals more subtle or celebratory rather than a singling-out marker. To revisit during polish.

## Co-op first, minimise losing

Games are cooperative where possible. Competitive tension comes from chaos and fun, not from punishment or ranking. Where individual performance is tracked, frame it as contribution to a shared outcome.

## Cumulative shared score

The session score always goes up — it never drops. Every player action contributes to it. No player "loses points". The group is collectively trying to beat their own high score.

## Novelty multiplier

Actions score more when they are novel — both per-player (you haven't done this recently) and globally (nobody has done this recently). Both dimensions stack and decay over time. This discourages optimal/repetitive play (e.g. two players back-tagging each other) and rewards creativity and variety.

## Universal action scoring

Scoring is not game-type specific. Jumps, dodges, tags, avoiding a tag, picking up a powerup, using a power, stomping an obstacle — all score. Game types add scoring on top of this baseline; they do not replace it.

## Playground as world

Convey game state through physical objects in the world, not UI panels or text. Think of a real playground: the timer is a physical kids' timer object, the game type is represented by a physical prop, the energy state of the game is a glowing object. Avoid HUD elements and text wherever a world object can do the job.

## Open-ended rules

Encode mechanics, not rules. Players self-organise the rules like a physical card or board game. The game provides the toys; players decide how to play. Avoid forcing turn structures or role assignments where the players can handle it themselves.
