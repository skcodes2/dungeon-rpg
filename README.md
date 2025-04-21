# Mad Cow

A 2D dark fantasy dungeon crawler built in Unity, featuring multi-phase boss fights, coin-based progression, and integrated narrative systems. Built by Group 26 for COMPSCI 4483B: Game Design.

## Table of Contents

- [Game Summary](#game-summary)
- [Features](#features)
- [Visuals & UI](#visuals--ui)
- [Narrative](#narrative)
- [Progression & Balance](#progression--balance)
- [Technical Design](#technical-design)
- [Playtesting & Iteration](#playtesting--iteration)
- [Team](#team)
- [How to Run](#how-to-run)

---

## Game Summary

**Mad Cow** is set in a fractured world where cows, once enslaved by humans, now rule the dungeons. You play as a lone survivor navigating hostile territory, collecting coins, unlocking abilities, and surviving an escalating threat. The tone is grim, with subtle absurdity that adds flavor without breaking immersion.

---

## Features

- **Combat and Mobility**: Fast-paced melee combat, dodge rolls, and ability chaining.
- **Skill Tree**: Coin-based upgrades for health, speed, damage, and lifesteal.
- **Boss Fights**: Multi-phase, gated encounters with vulnerability windows.
- **Torch System**: Light-driven exploration that affects navigation and environment readability.
- **Dialogue**: Click-to-scroll narrative system with dynamic item-triggered lore.
- **Gating and Puzzle Systems**: Enemy-based area locking and environmental interaction.

---

## Visuals & UI

- **Dark Fantasy Visuals**: Earthy, muted dungeon environments with glowing coin and blood UI effects.
- **Health and Ability Feedback**: Player sprite changes with damage; cooldowns have clear indicators.
- **Minimalist UI**: Coin counter, ability bar, and health bar with unobtrusive overlays.
- **Effects**:
  - Player and enemy flicker on hit.
  - Dynamic lighting from torches shows explored paths.
  - Custom death, victory, and game over animations.

---

## Narrative

- **Diegetic Storytelling**: Embedded books and environmental storytelling.
- **Lore Entries**: Descriptive titles like *The Cow Rebellion* and *Sabsab's Last Log* replace generic labels.
- **Coin System**: Coins are remnants of a fallen civilization—part mechanic, part narrative.
- **Environmental Interaction**: Puzzles like pressure plates and light-based navigation reinforce world logic.

---

## Progression & Balance

- **Economy**: Coin collection rewards exploration and risk-taking. Scaling costs prevent grinding.
- **Skill Tree Paths**: Offense, mobility, and survivability build options.
- **No Passive Healing**: Lifesteal abilities are the only way to recover health, increasing tension.
- **Encounter Gates**: Clear enemy areas to proceed.
- **Final Boss**: Multi-phase encounter testing all acquired skills.

---

## Technical Design

Built using **Unity** and **C#**, with Git-based version control.

Key implementations:

- **Scene Persistence**: Player stats and camera preserved across scenes with `DontDestroyOnLoad`.
- **Cinemachine**: Tight camera control for limited field of view and tension.
- **Enemy AI**: Custom pathfinding logic avoids collision issues.
- **Boss Logic**: Multi-phase AI with coroutine-timed vulnerability.
- **Audio**: AudioManager singleton controls volume and cue consistency.
- **Performance**: Optimized with Unity Profiler and Frame Debugger.
- **Dialogue System**: Fast-skip interaction, item-triggered lore reveals.

---

## Playtesting & Iteration

We used feedback loops from classmates and external testers to improve the game.

Key changes:

- Adjusted enemy stat scaling and spawn pacing.
- Redesigned skill tree for clarity and pacing.
- Overhauled boss fight with telegraphed vulnerability phases.
- Added UI indicators and tooltips.
- Improved level signposting using light and architecture.

---

## Team

Group 26:
- Elbert Chao  
- Bryson Crook  
- Dasol Lim  
- Sarabjot Kahlon  

Western University — Department of Computer Science  
Course: COMPSCI 4483B – Game Design  
Instructor: Prof. Umair Rehman

---

## How to Run

1. Open the Unity project in Unity Hub.
2. Load the main scene (`Assets/Scenes/MainMenu.unity`).
3. Press Play to start the game.
4. Use `ESC` to access the skill tree mid-run.

**Controls**:
- Move: WASD  
- Attack: Left Click  
- Roll / Ability: Right Click / Assigned Key  
- Interact: E  
