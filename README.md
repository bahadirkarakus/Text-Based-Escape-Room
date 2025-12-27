<div align="center">

# 🎮 Text-Based Escape Room




[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-GUI-0078D6?style=for-the-badge&logo=windows)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
[![License](https://img.shields.io/badge/License-Educational-blue?style=for-the-badge)](LICENSE)

*A sophisticated text-based escape room game demonstrating advanced C# programming concepts including control flow, regex pattern matching, object-oriented design, and Windows Forms integration.*

[🎯 Features](#-key-features) • [🚀 Getting Started](#-getting-started) • [🎓 Educational Value](#-educational-value) • [📖 Documentation](#-overview)

</div>

---

## 📖 Overview

**Text-Based Escape Room** is an educational game project that combines programming fundamentals with engaging gameplay mechanics. Players navigate through four progressively challenging levels, solving regex-based puzzles, managing resources, and battling enemies to escape from a mysterious device.

The project showcases practical application of programming language concepts taught in CS305, including:
- **Control Statements** - Complex conditional logic and nested decisions
- **Loops** - Game loop patterns and collection iteration
- **Functions** - Modular code organization and reusable components
- **Regular Expressions** - Pattern matching and validation in puzzles
- **Data Structures** - 2D arrays, lists, dictionaries, and custom objects
- **State Management** - Checkpoint systems and game state persistence

## ✨ Key Features

### 🎯 Core Gameplay
- **4 Challenging Levels** - Progressively difficult maze-based stages
- **Three Difficulty Modes** - Easy, Normal, and Hard with different starting resources
- **Resource Management** - Health Points (HP) and Oxygen (O2) tracking
- **Checkpoint System** - Auto-save at door openings with death recovery
- **Achievement Tracking** - Unlock achievements for special actions

### 🧩 Puzzle System
- **Regex-Based Challenges** - Educational puzzles requiring pattern matching knowledge
  - **Level 1**: IP address extraction from security log entries
  - **Level 2**: Email address format validation
  - **Level 3**: HTTPS URL structure verification
  - **Level 4**: Password strength validation with multiple criteria
- **Educational Feedback** - Detailed explanations of regex patterns and solutions
- **Penalty System** - HP loss for incorrect answers

### ⚔️ Combat & Enemies
- **Multiple Enemy Types**:
  - **Melee Enemies (M)** - Random movement AI with collision detection
  - **Shooter Enemies (R)** - Ranged attackers firing tracking projectiles
  - **Boss (B)** - Player-tracking AI with 10 HP and pathfinding
- **Weapon System** - Collect weapons and shoot in 4 directions with limited ammo
- **Collision Detection** - Real-time bullet and enemy collision checking

### 🎨 Visual Customization
- **Four Color Themes** - Dark, Normal, Retro, and Neon visual styles
- **Dual Interface Modes**:
  - **Console Mode** - Classic text-based terminal experience
  - **GUI Mode** - Windows Forms with enhanced graphics and animations
- **Real-time Status Display** - HP bars, oxygen gauges, score tracking
- **Background Music** - WAV audio playback with sound effects

### 💊 Power-ups & Items
- **Shield (S)** - Absorbs 3 enemy hits
- **Speed Boost (L)** - Double movement for 10 turns
- **Invisibility (I)** - Temporary enemy detection immunity
- **Health Packs (H)** - Restore 2 HP
- **Oxygen Tanks (O)** - Refill 30-40 oxygen points
- **Weapons (W)** - Enable shooting with 30 bullets
- **Keys (K)** - Required to open doors

## 📸 Screenshots

<div align="center">

### Main Gameplay
*Navigate through challenging mazes with enemies, puzzles, and power-ups*

### Regex Puzzles
*Educational challenges requiring pattern matching knowledge*

### Boss Fight
*Face the final boss with player-tracking AI and 10 HP*

### Victory Screen
*Complete all levels and view your achievement summary*

</div>

## 🕹️ Controls

### GUI Mode (Windows Forms)
- **WASD / Arrow Keys** - Move player
- **E** - Interact with items and puzzles
- **F** - Shoot weapon (4 directions simultaneously)
- **I** - Open inventory screen
- **P** - Pause menu
- **T** - Switch color theme
- **Q** - Quit game (with confirmation)

### Console Mode
- **WASD** - Movement controls
- **E** - Interact with objects
- **I** - View inventory and achievements
- **P** - Pause and menu options
- **T** - Change visual theme
- **Q** - Exit game

## 🚀 Getting Started

### System Requirements
- **Operating System**: Windows 10/11
- **.NET Version**: .NET 9.0 SDK or Runtime
- **Display**: 1280x720 resolution recommended
- **Audio**: Sound card (optional, for music and effects)

### Installation & Running

```bash
# Clone the repository
git clone https://github.com/bahadirkarakus/Text-Based-Escape-Room.git
cd Text-Based-Escape-Room/ConsoleApp1

# Build the project
dotnet build

# Run GUI version (recommended)
dotnet run --project ConsoleApp1.csproj

# Or open in Visual Studio 2022
# Open ConsoleApp1.sln and press F5
```

### Game Flow
1. **Introduction Screen** - Read the story and mission briefing
2. **Difficulty Selection** - Choose Easy, Normal, or Hard
3. **Theme Selection** - Pick your preferred color scheme
4. **Level 1-4** - Navigate mazes, solve puzzles, defeat enemies
5. **Victory Screen** - View final score and statistics

## 🏗️ Project Architecture

```
ConsoleApp1/
├── GameForm.cs         # Main Windows Forms game (GUI version)
├── Program.cs          # Console version entry point
├── ProgramGUI.cs       # GUI launcher
├── GameState.cs        # Static game state management
├── LevelData.cs        # Level map definitions
├── Enemy.cs            # Melee enemy AI
├── Boss.cs             # Boss pathfinding and tracking
├── ThemeColors.cs      # Color scheme definitions
├── Levels/             # External level files
│   ├── level1.txt
│   ├── level2.txt
│   ├── level3.txt
│   └── level4.txt
└── bin/Debug/net9.0-windows/
    └── Levels/         # Runtime level files
```

## 🎓 Educational Value

This project demonstrates practical application of CS305 course concepts:

### Programming Language Features
- **Control Flow**: Complex if-else chains, switch statements, nested conditionals
- **Loops**: while (game loop), for (2D array traversal), foreach (collections)
- **Functions**: Modular design with Move(), Interact(), DoPuzzle(), CheckCollisions()
- **Regular Expressions**: Pattern matching in all four puzzle challenges
- **Data Structures**: 2D char arrays (maps), List<Enemy>, Dictionary<string, ThemeColors>

### Software Engineering Practices
- **Object-Oriented Design**: Enemy, Boss, Bullet classes with encapsulation
- **State Management**: Centralized GameState class for global state
- **Event-Driven Programming**: Keyboard events, timer events, form events
- **Separation of Concerns**: LevelData, GameState, and UI are separate modules
- **Error Handling**: Input validation, collision detection, boundary checking

### Algorithms Implemented
- **Random Movement AI** - Enemy pathfinding with collision avoidance
- **Player Tracking** - Boss AI using distance calculations
- **Collision Detection** - Entity overlap checking for bullets and enemies
- **Checkpoint System** - State serialization and restoration

## 🧩 Puzzle Examples

### Level 1: IP Address Extraction from Security Log
```
Security Log Entry: "[2024-12-26 14:32:15 ERROR 192.168.1.105] User:admin Login:FAILED Attempts:3"
Task: Extract the IP address from this security log using regex
Pattern: \d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}
Answer: 192.168.1.105
```

### Level 2: Email Validation
```
Which email is valid?
A) user@example.com ✓
B) invalid.email@
C) @nouser.com
D) test@@double.com

Pattern: ^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$
```

### Level 3: URL Validation
```
Which URL has valid HTTPS format?
A) https://github.com/user/repo ✓
B) http://example.com
C) ftp://files.server.net
D) https://test

Pattern: ^https://[\w\.-]+/[\w/\.-]*$
```

### Level 4: Password Strength
```
Which password is STRONG? (8+ chars, uppercase, lowercase, digit, special)
A) Password123! ✓
B) pass123
C) HELLO@2024
D) test

Pattern: ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$
```

## 📝 Game Legend

| Symbol | Item | Function |
|--------|------|----------|
| `@` | Player | Your character |
| `#` | Wall | Impassable obstacle |
| `M` | Melee Enemy | Random movement, damages on contact |
| `R` | Shooter | Stationary, fires projectiles |
| `B` | Boss | Tracks player, 10 HP |
| `K` | Key | Opens doors (D) |
| `P` | Puzzle | Regex challenge |
| `H` | Health Pack | +2 HP |
| `O` | Oxygen Tank | +30-40 O2 |
| `W` | Weapon | Enables shooting, 30 ammo |
| `S` | Shield | Blocks 3 hits |
| `L` | Speed Boost | Double movement, 10 turns |
| `D` | Door | Requires key, auto-saves checkpoint |
| `E` | Exit | Level completion |

## 🎨 Available Themes

The game includes four fully customizable color themes:

- **Dark** - Dark background with high contrast
- **Normal** - Balanced colors for standard play
- **Retro** - Classic terminal green aesthetic
- **Neon** - Vibrant cyberpunk colors

*Press T during gameplay to cycle through themes.*

## 🏆 Game Tips & Strategy

- 💡 **Save Ammunition** - Weapons are limited, conserve bullets for boss fight
- 💡 **Shield First** - Collect shields before engaging shooter enemies
- 💡 **Oxygen Management** - Watch your O2 gauge, every move consumes oxygen
- 💡 **Checkpoint Awareness** - Doors auto-save progress, safe zones for experimentation
- 💡 **Invisibility Timing** - Use invisibility power-up to bypass enemy clusters
- 💡 **Puzzle Preparation** - Study regex patterns before attempting puzzles (HP penalty for wrong answers)
- 💡 **Boss Strategy** - Boss tracks your position, use obstacles to break line of sight
- 💡 **Speed Boost Usage** - Double movement is crucial for escaping dangerous situations

## 🛠️ Technologies & Libraries

### Core Technologies
- **Language**: C# 12.0
- **Framework**: .NET 9.0
- **Target Platform**: Windows (Windows Forms)

### Libraries Used
- `System.Windows.Forms` - GUI components and event handling
- `System.Drawing` - Graphics rendering and color management
- `System.Text.RegularExpressions` - Regex pattern matching in puzzles
- `System.Collections.Generic` - Lists, dictionaries for game entities
- `System.Media` - Background music and sound effects
- `System.Threading` - Game loop timing and delays
- `System.IO` - External level file loading

### Development Tools
- **IDE**: Visual Studio 2022
- **Build System**: MSBuild / .NET CLI
- **Version Control**: Git

## 🎯 Learning Outcomes

By studying this project, students will understand:

1. **Game Loop Architecture** - Continuous execution with input, update, render cycle
2. **State Management** - Centralized game state across multiple components
3. **Event-Driven Programming** - Keyboard events, timer events, form events
4. **Collision Detection** - 2D grid-based collision and entity overlap
5. **AI Pathfinding** - Random movement, player tracking, and obstacle avoidance
6. **Regex Applications** - Practical pattern matching in puzzle validation
7. **Object-Oriented Design** - Classes, encapsulation, inheritance concepts
8. **Resource Management** - HP, O2, ammunition, inventory systems
9. **User Interface Design** - Status displays, menus, visual feedback
10. **Audio Integration** - Background music and sound effect implementation

## 📊 Project Statistics

- **Lines of Code**: ~2,500+ LOC
- **Classes**: 8+ custom classes (Enemy, Boss, Bullet, ThemeColors, GameState, etc.)
- **Levels**: 4 complete levels with unique layouts
- **Enemy Types**: 3 (Melee, Shooter, Boss)
- **Power-ups**: 7 collectible types
- **Themes**: 4 visual color schemes
- **Puzzles**: 4 regex-based challenges
- **Achievements**: Multiple unlock conditions

## 🤝 Contributing

This is an educational project for CS305. Contributions for bug fixes and feature enhancements are welcome:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



## 👨‍💻 Authors

**Fatih Bahadır Karakuş** 




## 📞 Contact & Links

- 🔗 **GitHub Repository**: [Text-Based-Escape-Room](https://github.com/bahadirkarakus/Text-Based-Escape-Room)
- 📧 **Email**: bahadirkarakus261@gmail.com


---

<div align="center">

**Made with ❤️ using C# and Windows Forms**

*Part of CS305 Programming Languages Course Project*

</div>
