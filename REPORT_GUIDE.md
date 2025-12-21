# CS305 - Programming Languages Course Project Report Guide
## Escape Device - Ultimate Edition

---

## 📋 REPORT STRUCTURE (According to Project Requirements)

### 1. TITLE PAGE
- Project Title: **Escape Device - Ultimate Edition**
- Course: CS305 - Programming Languages
- Your Name & Student ID
- Date: December 2025
- Programming Language: **C#**

---

### 2. TABLE OF CONTENTS
1. Introduction
2. Project Overview
3. Programming Language Features Used
4. Implementation Details
5. Game Mechanics
6. Screenshots & Code Examples
7. Challenges & Solutions
8. Conclusion
9. References

---

## 📝 DETAILED SECTIONS

### SECTION 1: INTRODUCTION (1 page)
**What to write:**
- Brief description of the project
- Why you chose C# for this project
- What the game does (escape room puzzle game)
- Project objectives

**Example:**
```
This project implements a console-based escape room game called 
"Escape Device - Ultimate Edition" using C#. The game demonstrates 
various programming language concepts including Object-Oriented 
Programming, Regular Expressions, Collections, and more. The player 
must navigate through 4 levels, solving puzzles and avoiding enemies 
to escape from a mysterious device.
```

---

### SECTION 2: PROJECT OVERVIEW (1-2 pages)

**Game Description:**
- 4 levels with increasing difficulty
- Puzzle-based gameplay with regex challenges
- Enemy AI and boss fights
- Achievement system
- Multiple difficulty modes (Easy/Normal/Hard)
- Score tracking system

**Game Features:**
- Movement system (WASD)
- Interactive puzzles (Regex, Email validation, Code entry)
- Enemy movement AI
- Boss battle mechanics
- Power-ups (Health, Oxygen)
- Inventory system
- Pause menu

---

### SECTION 3: PROGRAMMING LANGUAGE FEATURES USED (3-4 pages)

#### 3.1 Object-Oriented Programming (OOP)
**Code to include:**

```csharp
// Enemy Class - Encapsulation & Methods
class Enemy
{
    public int Row, Col;
    public int MoveCounter = 0;
    
    public void Move(char[,] map)
    {
        MoveCounter++;
        if (MoveCounter < 3) return;
        MoveCounter = 0;
        // Movement logic...
    }
}

// Boss Class - Inheritance concept
class Boss
{
    public int Row, Col, Hp = 10;
    public int MoveCounter = 0;
    
    public void MoveTowardsPlayer(char[,] map, int pRow, int pCol)
    {
        // AI logic to chase player
    }
}
```

**Screenshot to include:** 
- Class definitions screenshot (Enemy & Boss classes)

#### 3.2 Collections & Data Structures
**Code to include:**

```csharp
// List of Levels (2D Arrays)
static List<char[,]> levels = new List<char[,]>
{
    // LEVEL 1
    new char[,] {
        { '#','#','#','#','#','#' },
        { '#','@',' ','P','K','#' },
        // ...
    }
};

// Dynamic Collections
static List<string> achievements = new List<string>();
static List<Enemy> enemies = new List<Enemy>();
```

**Screenshot to include:**
- Achievements list in action
- Inventory screen

#### 3.3 Regular Expressions (Regex) ⭐
**Code to include:**

```csharp
// Level 2 Puzzle - Regex Challenge
string text = "Error404-System:LOCKED-Code:7593-Access:DENIED";
var match = Regex.Match(text, @"Code:(\d{4})");
string correctAnswer = match.Success ? match.Groups[1].Value : "7593";

if (answer == correctAnswer)
{
    Console.WriteLine("✓ CORRECT! Code extracted: {0}", correctAnswer);
    Console.WriteLine("Regex pattern used: Code:(\\d{{4}})");
}

// Email Validation Regex
string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
```

**Screenshots to include:**
- Level 2 Regex puzzle screen
- Level 3 Email validation puzzle screen

#### 3.4 Control Structures
**Code to include:**

```csharp
// Game Loop - While loop
while (run)
{
    DrawGame();
    var k = Console.ReadKey(true).Key;
    
    if (k == ConsoleKey.W) Move(-1, 0);
    else if (k == ConsoleKey.S) Move(1, 0);
    else if (k == ConsoleKey.A) Move(0, -1);
    else if (k == ConsoleKey.D) Move(0, 1);
    
    // Update enemies
    foreach (var enemy in enemies)
        enemy.Move(map);
}

// Conditional Logic
if (t == 'K') {
    key = true;
    map[nr, nc] = ' ';
    score += 50;
}
```

#### 3.5 Methods & Functions
**Code to include:**

```csharp
// Movement Method
static void Move(int dr, int dc)
{
    int nr = pRow + dr, nc = pCol + dc;
    if (nr < 0 || nr >= map.GetLength(0)) return;
    
    char t = map[nr, nc];
    if (t == '#') { 
        Console.Beep(200, 100); 
        return; 
    }
    
    moveCount++;
    o2--;
    score += 1;
    // ... rest of logic
}

// Puzzle Interaction
static void DoPuzzle()
{
    if (currentLevel == 0) {
        // Level 1 puzzle logic
    }
    else if (currentLevel == 1) {
        // Regex challenge
    }
    // ...
}
```

#### 3.6 LINQ (Language Integrated Query)
**Code to include:**

```csharp
using System.Linq;

// Achievement checking
if (!achievements.Contains("First Key"))
    achievement = "First Key - Found your first key!";

// Could add more LINQ examples like:
var enemyCount = enemies.Count(e => e.Row == pRow);
```

#### 3.7 String Manipulation
**Code to include:**

```csharp
// Case-insensitive comparison
string answer = Console.ReadLine()?.ToLower();
if (answer == "console.writeline" || 
    answer == "console.write" || 
    answer == "writeline")

// String formatting
Console.WriteLine($"HP:{hp}/5 O2:{o2}% Score:{score}");
string hpBar = new string('█', hp) + new string('░', maxHp - hp);
```

#### 3.8 Exception Handling (if you add it)
**Code to add & include:**

```csharp
try {
    Console.SetWindowSize(80, 30);
    Console.SetBufferSize(80, 300);
}
catch {
    // Ignore window size errors on some systems
}
```

---

### SECTION 4: IMPLEMENTATION DETAILS (2-3 pages)

#### 4.1 Game Architecture
**Diagram to create:**
```
┌─────────────────┐
│   Main Loop     │
└────────┬────────┘
         │
    ┌────▼────┐
    │ DrawGame│
    └────┬────┘
         │
    ┌────▼──────┐
    │ Input     │
    │ Handler   │
    └────┬──────┘
         │
    ┌────▼────────┐
    │ Game Logic  │
    │ - Move      │
    │ - Interact  │
    │ - Puzzle    │
    └────┬────────┘
         │
    ┌────▼────────┐
    │ AI Update   │
    │ - Enemies   │
    │ - Boss      │
    └─────────────┘
```

#### 4.2 Level Design
**Include:**
- Level map screenshots
- Level progression table

| Level | Features | Enemies | Puzzle Type |
|-------|----------|---------|-------------|
| 1 | Basic navigation | 0 | Code Entry |
| 2 | Moving enemies | 2 | Regex Challenge |
| 3 | More enemies | 3 | Email Validation |
| 4 | Boss fight | Boss | Combat |

#### 4.3 AI Behavior
**Code to include:**

```csharp
// Enemy Random Movement
public void Move(char[,] map)
{
    MoveCounter++;
    if (MoveCounter < 3) return; // Move every 3 turns
    
    int[] dirs = { -1, 1, 0, 0 };
    int[] dcols = { 0, 0, -1, 1 };
    
    int idx = rand.Next(4);
    int newR = Row + dirs[idx];
    int newC = Col + dcols[idx];
    
    if (map[newR, newC] == ' ') {
        Row = newR;
        Col = newC;
    }
}

// Boss AI - Chases Player
public void MoveTowardsPlayer(char[,] map, int pRow, int pCol)
{
    MoveCounter++;
    if (MoveCounter < 2) return; // Boss moves faster
    
    int dr = 0, dc = 0;
    if (pRow < Row) dr = -1;
    else if (pRow > Row) dr = 1;
    if (pCol < Col) dc = -1;
    else if (pCol > Col) dc = 1;
    
    // Move towards player
}
```

---

### SECTION 5: GAME MECHANICS (2 pages)

#### 5.1 Movement System
- WASD controls
- Collision detection
- Oxygen consumption per move

#### 5.2 Combat System
- Enemy collision = -1 HP
- Boss fight = mutual damage
- Health packs restore +2 HP

#### 5.3 Puzzle System
**Include screenshots of all 3 puzzle types:**
1. Code entry puzzle (Level 1)
2. Regex extraction puzzle (Level 2)
3. Email validation puzzle (Level 3)

#### 5.4 Achievement System
**List all achievements:**
- First Key
- Puzzle Master
- Boss Slayer
- First Steps
- Speed Runner

#### 5.5 Score System
**Scoring table:**
| Action | Points |
|--------|--------|
| Move | +1 |
| Collect Key | +50 |
| Collect Item | +20-30 |
| Solve Puzzle | +100-150 |
| Defeat Boss | +500 |
| Complete Level | +100 |
| Achievement | +200 |

---

### SECTION 6: SCREENSHOTS & CODE EXAMPLES (3-4 pages)

#### Required Screenshots:
1. ✅ **Intro Screen** - Game title and story
2. ✅ **Difficulty Selection** - Easy/Normal/Hard choice
3. ✅ **Level 1 Gameplay** - Main game screen with UI
4. ✅ **Level 1 Puzzle** - Code entry challenge
5. ✅ **Level 2 with Enemies** - Red 'M' monsters visible
6. ✅ **Level 2 Regex Puzzle** - Regex challenge screen
7. ✅ **Level 3 Email Puzzle** - Email validation challenge
8. ✅ **Level 4 Boss Fight** - Boss with health bar
9. ✅ **Inventory Screen** - Stats and achievements
10. ✅ **Victory Screen** - Game completion
11. ✅ **Game Over Screen** - Death screen

#### Code Sections to Include:
1. Main game loop
2. Enemy class definition
3. Boss class definition
4. Regex puzzle implementation
5. Movement function
6. Puzzle solving function
7. Achievement checking
8. Drawing function

---

### SECTION 7: CHALLENGES & SOLUTIONS (1-2 pages)

**Challenges faced:**

#### Challenge 1: Console Input Blocking
**Problem:** Game would freeze waiting for input
**Solution:** Used `Console.ReadKey(true)` for non-blocking input

#### Challenge 2: Enemy Movement
**Problem:** Enemies moving too fast/erratic
**Solution:** Implemented move counter to control speed

```csharp
if (MoveCounter < 3) return; // Move every 3rd turn
```

#### Challenge 3: Regex Pattern Matching
**Problem:** Extracting specific patterns from text
**Solution:** Used capture groups in regex

```csharp
var match = Regex.Match(text, @"Code:(\d{4})");
string code = match.Groups[1].Value;
```

#### Challenge 4: Cross-Platform Compatibility
**Problem:** Console.Beep() only works on Windows
**Solution:** Added platform check (optional)

---

### SECTION 8: WHAT I LEARNED (1 page)

**Key Learnings:**
1. **OOP Principles** - Created reusable Enemy and Boss classes
2. **Regex Mastery** - Implemented pattern matching for puzzles
3. **Game Loop Design** - Managed game state and updates
4. **AI Programming** - Basic pathfinding for boss
5. **Data Structures** - Used Lists, 2D arrays effectively
6. **User Interface** - Created ASCII-based game interface
7. **Problem Solving** - Debugged complex game logic

---

### SECTION 9: CONCLUSION (1 page)

**Summary:**
- Successfully implemented a complete game in C#
- Demonstrated understanding of OOP, Regex, Collections
- Created engaging gameplay with multiple difficulty levels
- Implemented AI for enemies and boss
- Added educational value through regex puzzles

**Future Improvements:**
- Save/Load game functionality
- More levels and puzzle types
- Sound effects (beyond beeps)
- Multiplayer support
- Better graphics (if moving beyond console)

---

### SECTION 10: REFERENCES

```
1. Microsoft C# Documentation
   https://docs.microsoft.com/en-us/dotnet/csharp/

2. Regular Expressions Tutorial
   https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expressions

3. Game Programming Patterns
   https://gameprogrammingpatterns.com/

4. C# Collections
   https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic
```

---

## 📸 SCREENSHOT CHECKLIST

### How to take screenshots:
1. Run the game: `cd ConsoleApp1; dotnet run`
2. Use **Windows + Shift + S** to capture screenshots
3. Save to a folder named "Screenshots"

### Required Screenshots (in order):

```
1. screenshot_01_intro.png
   - Intro screen with game title

2. screenshot_02_difficulty.png
   - Difficulty selection screen

3. screenshot_03_level1_start.png
   - Level 1 gameplay (player at start)

4. screenshot_04_level1_puzzle.png
   - Level 1 puzzle screen (code entry)

5. screenshot_05_level2_enemies.png
   - Level 2 with red enemies visible

6. screenshot_06_level2_regex.png
   - Regex challenge puzzle screen

7. screenshot_07_level3_email.png
   - Email validation puzzle screen

8. screenshot_08_boss_fight.png
   - Boss with health bar visible

9. screenshot_09_inventory.png
   - Inventory/Stats screen (press I)

10. screenshot_10_victory.png
    - Victory screen after completing game

11. screenshot_11_gameover.png
    - Game over screen (let oxygen run out)

12. screenshot_12_pause.png
    - Pause menu (press P)
```

---

## 💻 CODE SNIPPETS TO INCLUDE

### Create a "code_snippets.txt" file with these sections:

```csharp
// ==========================================
// 1. GAME INITIALIZATION
// ==========================================
static void Main()
{
    Console.Title = "Escape Device - Ultimate Edition";
    Console.CursorVisible = false;
    
    ShowIntro();
    SelectDifficulty();
    LoadLevel(currentLevel);
    
    while (run)
    {
        DrawGame();
        // Handle input...
    }
}

// ==========================================
// 2. ENEMY CLASS (OOP)
// ==========================================
class Enemy
{
    public int Row, Col;
    public int MoveCounter = 0;
    
    public void Move(char[,] map)
    {
        MoveCounter++;
        if (MoveCounter < 3) return;
        MoveCounter = 0;
        
        int[] dirs = { -1, 1, 0, 0 };
        int[] dcols = { 0, 0, -1, 1 };
        
        for (int i = 0; i < 10; i++)
        {
            int idx = rand.Next(4);
            int newR = Row + dirs[idx];
            int newC = Col + dcols[idx];
            
            if (newR > 0 && newR < map.GetLength(0) - 1)
            {
                if (map[newR, newC] == ' ')
                {
                    Row = newR;
                    Col = newC;
                    break;
                }
            }
        }
    }
}

// ==========================================
// 3. REGEX PUZZLE (Level 2)
// ==========================================
string text = "Error404-System:LOCKED-Code:7593-Access:DENIED";
var match = Regex.Match(text, @"Code:(\d{4})");
string correctAnswer = match.Success ? match.Groups[1].Value : "7593";

if (answer == correctAnswer)
{
    puzzle = true;
    score += 150;
    Console.WriteLine("✓ CORRECT! Code extracted: {0}", correctAnswer);
    Console.WriteLine("Regex pattern used: Code:(\\d{{4}})");
}

// ==========================================
// 4. COLLECTIONS USAGE
// ==========================================
static List<char[,]> levels = new List<char[,]>();
static List<string> achievements = new List<string>();
static List<Enemy> enemies = new List<Enemy>();

// Adding elements
achievements.Add("First Key - Found your first key!");
enemies.Add(new Enemy { Row = r, Col = c });

// Iterating
foreach (var enemy in enemies)
    enemy.Move(map);

// ==========================================
// 5. ACHIEVEMENT SYSTEM
// ==========================================
static void CheckAchievement(string type)
{
    string achievement = "";
    
    if (type == "key" && !achievements.Contains("First Key"))
        achievement = "First Key - Found your first key!";
    else if (type == "puzzle" && !achievements.Contains("Puzzle Master"))
        achievement = "Puzzle Master - Solved a puzzle!";
    
    if (achievement != "" && !achievements.Contains(achievement))
    {
        achievements.Add(achievement);
        score += 200;
        Console.Beep(1200, 200);
    }
}
```

---

## 📊 REPORT FORMATTING TIPS

### Document Format:
- **Font:** Times New Roman or Arial, 12pt
- **Line Spacing:** 1.5
- **Margins:** 1 inch (2.54 cm) all sides
- **Page Numbers:** Bottom center
- **Headings:** Bold, larger font
- **Code:** Courier New, 10pt, with syntax highlighting if possible

### Total Length:
- **Minimum:** 10 pages (excluding title page)
- **Maximum:** 15 pages
- **Screenshots:** Don't count towards page limit

### Structure:
```
Title Page           (1 page)
Table of Contents    (1 page)
Introduction        (1 page)
Project Overview    (2 pages)
Features Used       (3-4 pages)
Implementation      (2-3 pages)
Screenshots         (3-4 pages)
Challenges          (1-2 pages)
Conclusion          (1 page)
References          (1 page)
```

---

## 🎯 GRADING RUBRIC ALIGNMENT

### Based on typical course requirements:

| Criteria | Points | How Your Project Addresses It |
|----------|--------|-------------------------------|
| **Code Quality** | 25 | Clean, organized, well-commented code |
| **OOP Usage** | 20 | Enemy & Boss classes with methods |
| **Advanced Features** | 20 | Regex, Collections, LINQ |
| **Documentation** | 15 | This comprehensive report |
| **Creativity** | 10 | Unique game mechanics, puzzles |
| **Functionality** | 10 | Fully working game with no crashes |
| **TOTAL** | 100 | |

---

## ✅ FINAL CHECKLIST

Before submitting:

### Code:
- [ ] Code compiles without errors
- [ ] All features work as expected
- [ ] Code has comments explaining complex logic
- [ ] Variable names are descriptive
- [ ] No debug/test code left in

### Report:
- [ ] All sections completed
- [ ] 10+ pages of content
- [ ] 12+ screenshots included
- [ ] Code snippets properly formatted
- [ ] Grammar and spelling checked
- [ ] References properly cited

### Submission:
- [ ] Source code (Program.cs)
- [ ] .csproj file
- [ ] Report (PDF)
- [ ] Screenshots folder
- [ ] README.md (optional)

---

## 📝 SAMPLE INTRODUCTION TEXT

```
Introduction

This project presents "Escape Device - Ultimate Edition," a console-based 
escape room puzzle game developed in C# as part of the CS305 Programming 
Languages course. The game demonstrates the practical application of various 
programming concepts including Object-Oriented Programming (OOP), Regular 
Expressions, Collections, and Game Logic.

The player finds themselves trapped inside a mysterious device and must 
navigate through four increasingly challenging levels. Each level presents 
unique obstacles including puzzles that test regex knowledge, enemy AI 
that dynamically responds to player actions, and culminates in a boss 
battle requiring strategic thinking.

This project showcases:
- OOP principles through Enemy and Boss classes
- Regex pattern matching for puzzle solving
- Dynamic data structures using C# Collections
- Game loop architecture and state management
- AI behavior programming for enemies
- User interface design in a console environment

The game includes multiple difficulty settings, an achievement system, 
and score tracking, providing both entertainment and educational value 
in demonstrating programming language concepts.
```

---

## 🎮 TESTING CHECKLIST

Document these test cases in your report:

### Functional Testing:
1. ✅ Player movement in all directions (W/A/S/D)
2. ✅ Collision detection with walls
3. ✅ Item pickup (Key, Health, Oxygen)
4. ✅ Puzzle solving (all 3 types)
5. ✅ Enemy movement and collision
6. ✅ Boss fight mechanics
7. ✅ Level progression
8. ✅ Achievement unlocking
9. ✅ Score calculation
10. ✅ Game over conditions
11. ✅ Victory condition
12. ✅ Pause menu functionality

### Edge Cases:
- [ ] Running out of oxygen
- [ ] Trying to exit without solving puzzle
- [ ] Trying to open door without key
- [ ] Boss fight with low HP

---

## 📚 APPENDIX SUGGESTIONS

### Appendix A: Complete Source Code
Include the full Program.cs file

### Appendix B: User Manual
```
Controls:
- W/A/S/D: Move up/left/down/right
- E: Interact with puzzles
- I: Open inventory
- P: Pause menu
- Q: Quit game

Game Symbols:
@ - Player
# - Wall
M - Enemy
B - Boss
P - Puzzle
K - Key
H - Health pack
O - Oxygen refill
F - Flashlight
D - Door
E - Exit

Objectives:
1. Collect the key (K)
2. Solve the puzzle (P)
3. Open the door (D)
4. Reach the exit (E)
5. Defeat the boss (Level 4)
```

### Appendix C: Regex Patterns Used
```
1. Four-digit extraction:
   Pattern: Code:(\d{4})
   Matches: "Code:1234" → extracts "1234"

2. Email validation:
   Pattern: ^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$
   Validates: user@example.com ✓
   Rejects: invalid@, @nouser.com ✗
```

---

## 💡 REPORT WRITING TIPS

1. **Use active voice:** "I implemented..." not "It was implemented..."
2. **Be specific:** Include actual code, not just descriptions
3. **Show understanding:** Explain WHY you made design decisions
4. **Use diagrams:** Flow charts help explain logic
5. **Compare alternatives:** "I chose X over Y because..."
6. **Cite sources:** Reference C# documentation
7. **Proofread:** Check for typos and grammar errors

---

## 🎯 SUBMISSION PACKAGE

Create a folder named: `CS305_Project_YourName`

Contents:
```
CS305_Project_YourName/
│
├── Source/
│   ├── Program.cs
│   ├── ConsoleApp1.csproj
│   └── ConsoleApp1.sln
│
├── Screenshots/
│   ├── 01_intro.png
│   ├── 02_difficulty.png
│   ├── ... (all 12 screenshots)
│
├── Report/
│   └── CS305_Project_Report.pdf
│
└── README.md (optional)
```

Compress to: `CS305_Project_YourName.zip`

---

## END OF REPORT GUIDE

This guide ensures your report covers all requirements and showcases 
your programming skills effectively. Follow it section by section for 
a comprehensive, professional submission!

Good luck! 🚀
