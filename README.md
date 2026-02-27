# BitSprint: 8-Bit Runner

BitSprint is an arcade-style 2D endless runner built in Unity with retro pixel visuals, three difficulty levels, and a high-score chase loop.

## Demo
- [Watch the gameplay demo video](./Demo/bitsprint-demo.mov)

## Gameplay
- Survive as long as possible to maximize score.
- Pick your mode at start:
  - `1` Easy
  - `2` Medium
  - `3` Hard
- Lose if you hit an obstacle or fall off the stage.

## Controls
- `Space` or `Up Arrow`: Jump
- `R`: Restart after game over
- `1 / 2 / 3`: Difficulty selection from menu

## Features
- Runtime bootstrap scene setup (no prefab dependency)
- 8-bit procedural sprites (player, obstacles, ground)
- Parallax background layers and stars
- Dynamic obstacle spawning by difficulty
- Score + persistent high score (`PlayerPrefs`)

## Project Layout
- `Assets/Scripts/Runner8Bit/RunnerBootstrap.cs`: Camera/world/UI bootstrap
- `Assets/Scripts/Runner8Bit/RunnerGameManager.cs`: State, score, difficulty, fail/restart
- `Assets/Scripts/Runner8Bit/RunnerPlayer.cs`: Input, jump, collisions
- `Assets/Scripts/Runner8Bit/RunnerSpawner.cs`: Obstacle generation and pacing
- `Assets/Scripts/Runner8Bit/PixelSpriteFactory.cs`: Pixel sprite generation

## Run
1. Open the project in Unity.
2. Open or create `Assets/Scenes/RunnerScene.unity`.
3. Add an empty GameObject named `Bootstrap`.
4. Attach component `Runner8Bit.RunnerBootstrap`.
5. Press Play.

## Tech
- Unity (2D)
- Supports both Legacy Input and Input System

## License
MIT
