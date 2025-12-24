# 🎮 Escape Device - Ultimate Edition

A text-based adventure game built with C# featuring both Console and GUI versions. Navigate through challenging levels, defeat enemies, solve puzzles, and face the ultimate boss!

## ✨ Features

### 🎯 Gameplay
- **4 Challenging Levels** - Progress through increasingly difficult stages
- **Multiple Enemy Types**:
  - 🔴 **Melee Enemies (M)** - Chase and attack you
  - 🟠 **Shooter Enemies (R)** - Fire projectiles from a distance
  - 👹 **Boss Enemy (B)** - Final challenge with 10 HP
- **Weapon System** - Collect weapons (W) and shoot in all 4 directions with F key
- **Power-ups**:
  - 🛡️ **Shield (S)** - Blocks damage
  - ⚡ **Speed Boost** - Move faster
  - 👻 **Invisibility (I)** - Enemies can't see you
  - ❤️ **Health Pack (H)** - Restore HP
  - 🫁 **Oxygen Tank (O)** - Refill oxygen
  - 💊 **Full Restore (L)** - Restore to maximum HP

### 🎨 Visual Modes
- **Console Mode** - Classic text-based experience
- **GUI Mode** - Colorful Windows Forms interface with:
  - Multiple color themes
  - Smooth animations
  - Real-time bullet physics
  - Dynamic enemy AI

### 🎲 Game Modes
- **3 Difficulty Levels**: Easy, Normal, Hard
- **Checkpoint System** - Respawn at saved points
- **Puzzle Challenges** - Solve riddles to progress
- **Achievement System** - Track your accomplishments

## 🕹️ Controls

### Console Mode
- **WASD** - Movement
- **E** - Interact with items/doors
- **I** - Show inventory
- **P** - Pause menu
- **T** - Change theme
- **Q** - Quit game

### GUI Mode
- **Arrow Keys / WASD** - Movement
- **E / Space** - Interact
- **F** - Shoot (requires weapon)
- **I** - Inventory
- **P / ESC** - Pause
- **T** - Change theme

## 🚀 How to Run

### Prerequisites
- .NET 9.0 SDK or later
- Windows OS (for GUI mode)

### Running the Game
```bash
# Clone the repository
git clone https://github.com/yourusername/escape-device.git
cd escape-device/ConsoleApp1

# Build the project
dotnet build

# Run the game
dotnet run
```

## 🎯 Game Objectives

1. **Find the Key (K)** - Required to unlock doors
2. **Solve Puzzles (P)** - Complete riddles to progress
3. **Collect Items** - Health, oxygen, and power-ups
4. **Defeat Enemies** - Survive enemy encounters
5. **Face the Boss** - Final challenge in Level 4
6. **Reach the Exit (E)** - Complete each level

## 🏗️ Project Structure

```
ConsoleApp1/
├── Program.cs          # Console version entry point
├── ProgramGUI.cs       # GUI version launcher
├── GameForm.cs         # Main GUI game logic
├── GameState.cs        # Shared game state
├── LevelData.cs        # Level definitions
├── Enemy.cs           # Enemy AI
├── Boss.cs            # Boss AI
├── ThemeColors.cs     # Color themes
└── map.txt            # Level map data
```

## 🛠️ Technologies Used

- **C# (.NET 9.0)**
- **Windows Forms** - GUI interface
- **Console Application** - Text-based version
- **System.Drawing** - Graphics rendering

## 📝 Game Legend

| Symbol | Description |
|--------|-------------|
| @ | Player |
| # | Wall |
| M | Melee Enemy |
| R | Shooter Enemy |
| B | Boss |
| K | Key |
| P | Puzzle |
| H | Health Pack |
| O | Oxygen Tank |
| W | Weapon |
| S | Shield |
| D | Door (locked) |
| E | Exit |
| L | Full Restore |
| I | Invisibility |

## 🎨 Themes

- **Normal** - Classic colors
- **Dark** - Dark mode
- **Neon** - Vibrant neon colors
- **Ocean** - Blue theme
- **Fire** - Red/Orange theme
- **Forest** - Green theme

## 🏆 Tips & Tricks

- 💡 Collect shields before facing shooter enemies
- 💡 Save weapons for the boss fight
- 💡 Use invisibility to sneak past groups of enemies
- 💡 Manage your oxygen carefully
- 💡 Checkpoints save your progress automatically
- 💡 Shooter enemies have limited range - keep your distance!

## 📜 License

This project is for educational purposes.

## 👨‍💻 Author

**Bahadır Karakuş** - C# Game Development Project

---

Made with ❤️ using C# and Windows Forms
