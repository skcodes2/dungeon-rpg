# Mad Cow

A 2D dark fantasy dungeon crawler built in Unity, featuring multi-phase boss fights, coin-based progression, and integrated narrative systems. Built by Group 26 for COMPSCI 4483B: Game Design.

---

## Table of Contents

- [Game Summary](#game-summary)  
- [Demo](#demo)  
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

**Mad Cow** is set in a fractured world where cows, once enslaved by humans, now rule the dungeons. You play as a lone survivor navigating hostile territory, collecting coins, unlocking abilities, and surviving an escalating threat. The tone is grim with subtle absurdity.

---

## Demo

Watch a short gameplay demo:  
[https://youtu.be/2AJdj0Fw_Xo](https://youtu.be/2AJdj0Fw_Xo)

---

## Features

- Fast-paced melee combat with dodge rolls and ability chaining  
- Coin-based skill tree for health, mobility, and damage upgrades  
- Boss fights with multi-phase logic and vulnerability windows  
- Light-based navigation with torch mechanics  
- Embedded lore via interactive books and dynamic dialogue  
- Scene gating and puzzle-based exploration

---

## Visuals & UI

- Dark dungeon visuals with glowing effects for coins and UI  
- Minimalist UI with health bar, coin counter, and ability bar  
- Sprite flickering on damage  
- Clear visual feedback on ability cooldowns  
- Victory, death, and game over animations

---

## Narrative

- Lore revealed through books and environmental cues  
- Descriptive titles for lore items (e.g. *The Cow Rebellion*)  
- Coins have narrative function as ancient relics  
- Dialogue triggered by item pickups for storytelling and tutorials  
- Puzzle sequences tied to story progression

---

## Progression & Balance

- Coins earned from exploration and combat  
- Scaling upgrade costs discourage grinding  
- Lifesteal abilities are the only healing method  
- Skill tree supports different playstyles  
- Gates prevent progress until enemies are cleared  
- Boss fight gated behind combat waves with final damage phase

---

## Technical Design

Built with **Unity** and **C#**, using Git and AnchorPoint for version control.

Key systems:
- Scene persistence using `DontDestroyOnLoad`  
- Cinemachine for dynamic camera control  
- Custom enemy AI with collision-aware navigation  
- Multi-phase boss logic using coroutines and NavMesh  
- AudioManager singleton for volume control  
- Fast-skip dialogue system with item-based triggers  
- Contextual UI prompts and cooldown indicators  
- Optimization using Unity Profiler and Frame Debugger

---

## Playtesting & Iteration

Changes based on tester feedback:
- Adjusted enemy stat scaling and pacing  
- Skill tree redesigned with visual clarity and progression logic  
- Boss encounter redesigned to include wave-based progression  
- UI and tooltip improvements  
- Added environmental lighting and gating to guide flow

---

## Team

Group 26  
- Elbert Chao  
- Bryson Crook  
- Dasol Lim  
- Sarabjot Kahlon  

Western University – Department of Computer Science  
COMPSCI 4483B – Game Design  
Instructor: Prof. Umair Rehman

---

## How to Run

### Option 1: Run from Source in Unity

1. Open the Unity project in Unity Hub  
2. Load the scene: `Assets/Scenes/MainMenu.unity`  
3. Press **Play** to start  
4. Press `ESC` to open the skill tree mid-game

**Controls**:
- Move: WASD  
- Attack: Left Click  
- Roll / Ability: Right Click / Assigned Key  
- Interact: E  

---

### Option 2: Run Windows Build

You can also create and run a standalone Windows version:

1. Open the project in Unity  
2. Go to: `File → Build Settings → Build`  
3. Select **Windows (x86_64)**  
4. Choose an output folder and click **Build**  
5. Run the generated `.exe` file to play without Unity  
