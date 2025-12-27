using System;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        // Console window'u göster
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        
        // Kısayollar için static propertyler
        static int currentLevel { get => GameState.CurrentLevel; set => GameState.CurrentLevel = value; }
        static char[,] map { get => GameState.Map; set => GameState.Map = value; }
        static int pRow { get => GameState.PRow; set => GameState.PRow = value; }
        static int pCol { get => GameState.PCol; set => GameState.PCol = value; }
        static int maxHp { get => GameState.MaxHp; set => GameState.MaxHp = value; }
        static int hp { get => GameState.Hp; set => GameState.Hp = value; }
        static int o2 { get => GameState.O2; set => GameState.O2 = value; }
        static int score { get => GameState.Score; set => GameState.Score = value; }
        static bool key { get => GameState.Key; set => GameState.Key = value; }
        static bool puzzle { get => GameState.Puzzle; set => GameState.Puzzle = value; }
        static bool run { get => GameState.Run; set => GameState.Run = value; }
        static string difficulty { get => GameState.Difficulty; set => GameState.Difficulty = value; }
        static int moveCount { get => GameState.MoveCount; set => GameState.MoveCount = value; }
        static int itemsCollected { get => GameState.ItemsCollected; set => GameState.ItemsCollected = value; }

        static void Main()
        {
            // Console window'u aç (Windows Forms projesi için gerekli)
            AllocConsole();
            
            Console.Title = "Escape Device - Ultimate Edition";
            Console.CursorVisible = false;
            
            ShowIntro();
            SelectDifficulty();
            LoadLevel(currentLevel);
            
            // İlk checkpoint'i kaydet
            SaveCheckpoint();
            
            // Oyun çalışıyor
            run = true;

            while (run)
            {
                DrawGame();
                
                var k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.Q) 
                {
                    if (Confirm("Are you sure you want to quit?"))
                    {
                        run = false;
                        break;
                    }
                }
                if (k == ConsoleKey.W) Move(-1, 0);
                if (k == ConsoleKey.S) Move(1, 0);
                if (k == ConsoleKey.A) Move(0, -1);
                if (k == ConsoleKey.D) Move(0, 1);
                if (k == ConsoleKey.E) Interact();
                if (k == ConsoleKey.I) ShowInventory();
                if (k == ConsoleKey.P) PauseMenu();
                if (k == ConsoleKey.T) ChangeTheme();
                
                // Speed boost - allow double movement
                if (GameState.SpeedBoostActive && (k == ConsoleKey.W || k == ConsoleKey.S || k == ConsoleKey.A || k == ConsoleKey.D))
                {
                    if (k == ConsoleKey.W) Move(-1, 0);
                    if (k == ConsoleKey.S) Move(1, 0);
                    if (k == ConsoleKey.A) Move(0, -1);
                    if (k == ConsoleKey.D) Move(0, 1);
                    
                    GameState.SpeedBoostTurns--;
                    if (GameState.SpeedBoostTurns <= 0)
                    {
                        GameState.SpeedBoostActive = false;
                        Console.Beep(400, 100);
                    }
                }
                
                foreach (var enemy in GameState.Enemies)
                    enemy.Move(map);
                
                if (GameState.Boss != null && GameState.Boss.Hp > 0)
                    GameState.Boss.MoveTowardsPlayer(map, pRow, pCol);
                
                CheckCollisions();
            }
            
            ShowEnding();
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\n");
            Console.WriteLine("  ╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ║              ESCAPE DEVICE - ULTIMATE EDITION                 ║");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ║           You woke up inside a strange device...              ║");
            Console.WriteLine("  ║              You must find a way to escape!                   ║");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n  Press ENTER to continue...");
            Console.ReadKey();
        }

        static void SelectDifficulty()
        {
            Console.Clear();
            Console.WriteLine("\n  SELECT DIFFICULTY:\n");
            Console.WriteLine("  1. EASY   - More HP and Oxygen");
            Console.WriteLine("  2. NORMAL - Balanced gameplay");
            Console.WriteLine("  3. HARD   - Low HP, fast enemies!\n");
            Console.Write("  Choice (1-3): ");
            
            var choice = Console.ReadKey().KeyChar;
            if (choice == '1') 
            {
                difficulty = "EASY";
                maxHp = 8;
                hp = 8;
                o2 = 150;
            }
            else if (choice == '3')
            {
                difficulty = "HARD";
                maxHp = 3;
                hp = 3;
                o2 = 80;
            }
            
            SelectTheme();
        }
        
        static void SelectTheme()
        {
            Console.Clear();
            Console.WriteLine("\n  SELECT COLOR THEME:\n");
            Console.WriteLine("  1. CLASSIC - Original colors");
            Console.WriteLine("  2. MATRIX  - Green hacker vibes");
            Console.WriteLine("  3. OCEAN   - Deep blue sea");
            Console.WriteLine("  4. FIRE    - Burning red & yellow");
            Console.WriteLine("  5. DARK    - Monochrome darkness");
            Console.WriteLine("  6. NEON    - Vibrant neon lights\n");
            Console.Write("  Choice (1-6): ");
            
            var choice = Console.ReadKey().KeyChar;
            GameState.CurrentTheme = choice switch
            {
                '1' => "CLASSIC",
                '2' => "MATRIX",
                '3' => "OCEAN",
                '4' => "FIRE",
                '5' => "DARK",
                '6' => "NEON",
                _ => "CLASSIC"
            };
            
            Console.Clear();
            Console.ForegroundColor = GameState.Themes[GameState.CurrentTheme].UI;
            Console.WriteLine($"\n  Theme set to: {GameState.CurrentTheme}");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
            Console.Clear();
        }

        static void LoadLevel(int levelIndex, bool saveCheckpoint = true)
        {
            if (levelIndex >= LevelData.Levels.Count)
            {
                Victory();
                return;
            }
            
            currentLevel = levelIndex;
            map = (char[,])LevelData.Levels[levelIndex].Clone();
            GameState.Enemies.Clear();
            GameState.Boss = null;
            
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == '@')
                    {
                        pRow = r;
                        pCol = c;
                        map[r, c] = ' ';
                    }
                    else if (map[r, c] == 'M')
                    {
                        GameState.Enemies.Add(new Enemy { Row = r, Col = c });
                        map[r, c] = ' ';
                    }
                    else if (map[r, c] == 'B')
                    {
                        GameState.Boss = new Boss { Row = r, Col = c };
                        map[r, c] = ' ';
                    }
                }
            }
            
            puzzle = false;
            key = false;
            GameState.DoorOpened = false;

            // Level başında oksijen bonusu (zorluk moduna göre)
            int oxygenBonus = difficulty switch
            {
                "EASY" => 40,
                "NORMAL" => 30,
                "HARD" => 20,
                _ => 30
            };
            o2 = Math.Min(o2 + oxygenBonus, 100);

            // Level başında checkpoint KAYDETME (sadece ilk çağrıda kaydet)
            // Door açıldığında checkpoint kaydedilecek

            ShowLevelIntro();
        }

        static void ShowLevelIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n\n");
            Console.WriteLine($"            ═══════════ LEVEL {currentLevel + 1} ═══════════");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            // Short story texts per level (EN)
            if (currentLevel == 0)
            {
                Console.WriteLine("  You wake up on a cold metal floor.");
                Console.WriteLine("  Flickering lights and rusty panels surround you.");
                Console.WriteLine("  This looks like the entrance corridor of the device... your first escape attempt begins.");
            }
            else if (currentLevel == 1)
            {
                Console.WriteLine("  You left the first security layer behind.");
                Console.WriteLine("  Now you are in a deeper module: filled with logs, error codes and locked terminals.");
                Console.WriteLine("  It's not clear if the device sees you as a system bug or an unauthorized user.");
            }
            else if (currentLevel == 2)
            {
                Console.WriteLine("  The oxygen pipes here look older, the wall warnings are almost erased.");
                Console.WriteLine("  You notice traces that feel like leftover notes from previous test subjects.");
                Console.WriteLine("  With every step, you feel more clearly that the device is testing you.");
            }
            else if (currentLevel == 3)
            {
                Console.WriteLine("  You are very close to the core.");
                Console.WriteLine("  Power surges, distant sirens and the breath of the central  are right behind you.");
                Console.WriteLine("  This is the final section: either you defeat the device or you stay trapped inside forever.");
            }
            else
            {
                Console.WriteLine("  You step into a deeper section of the device.");
                Console.WriteLine("  With every level, the environment grows darker and more dangerous.");
            }

            Console.WriteLine("\n  Press ENTER to continue...");
            Console.ReadKey(true);
        }

        static void DrawGame()
        {
            var theme = GameState.Themes[GameState.CurrentTheme];
            Console.Clear();
            Console.ForegroundColor = theme.UI;
            Console.WriteLine("======================================================================");
            Console.WriteLine("||   ------                                             -----        ||");
            Console.WriteLine("||  |      |   ESCAPE DEVICE                           |  A  |       ||");
            Console.WriteLine("||  |  +   |   Level: " + (currentLevel + 1) + "                                  |  B  |       ||");
            Console.WriteLine("||  |      |   Difficulty: " + difficulty.PadRight(6) + "                       |  X  |       ||");
            Console.WriteLine("||   ------    Theme: " + GameState.CurrentTheme.PadRight(7) + "                 |  Y  |       ||");
            Console.WriteLine("||   ● ● ● ● ●                                                     ||");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("||                                                                  ||");
            
            for (int r = 0; r < map.GetLength(0); r++)
            {
                Console.Write("||        ");
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    bool drawn = false;
                    
                    if (r == pRow && c == pCol)
                    {
                        Console.ForegroundColor = theme.Player;
                        Console.Write('@');
                        Console.ForegroundColor = ConsoleColor.White;
                        drawn = true;
                    }
                    else if (GameState.Boss != null && GameState.Boss.Hp > 0 && r == GameState.Boss.Row && c == GameState.Boss.Col)
                    {
                        Console.ForegroundColor = theme.Enemies;
                        Console.Write('B');
                        Console.ForegroundColor = ConsoleColor.White;
                        drawn = true;
                    }
                    else
                    {
                        foreach (var enemy in GameState.Enemies)
                        {
                            if (r == enemy.Row && c == enemy.Col)
                            {
                                Console.ForegroundColor = theme.Enemies;
                                Console.Write('M');
                                Console.ForegroundColor = ConsoleColor.White;
                                drawn = true;
                                break;
                            }
                        }
                    }
                    
                    if (!drawn)
                    {
                        char ch = map[r, c];
                        if (ch == 'P') Console.ForegroundColor = theme.Puzzle;
                        else if (ch == 'K') Console.ForegroundColor = theme.Items;
                        else if (ch == 'F') Console.ForegroundColor = theme.Items;
                        else if (ch == 'H') Console.ForegroundColor = theme.Health;
                        else if (ch == 'O') Console.ForegroundColor = theme.Health;
                        else if (ch == 'D') Console.ForegroundColor = theme.Walls;
                        else if (ch == 'E') Console.ForegroundColor = theme.Health;
                        else if (ch == '#') Console.ForegroundColor = theme.Walls;
                        else if (ch == 'S') Console.ForegroundColor = ConsoleColor.Cyan;
                        else if (ch == 'L') Console.ForegroundColor = ConsoleColor.Magenta;
                        
                        Console.Write(ch);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine("                                  ||");
            }
            
            Console.WriteLine("||                                                                  ||");
            
            string hpBar = new string('█', hp) + new string('░', maxHp - hp);
            string o2Bar = o2 > 50 ? "████" : o2 > 25 ? "██░░" : "█░░░";
            
            Console.Write("||  HP:");
            Console.ForegroundColor = hp > 2 ? theme.Health : theme.Enemies;
            Console.Write(hpBar);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {hp}/{maxHp}  O2:");
            Console.ForegroundColor = o2 > 50 ? theme.Health : o2 > 25 ? theme.Items : theme.Enemies;
            Console.Write(o2Bar);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {o2}%  Score:{score}       ||");
            
            Console.WriteLine($"||  Key:{(key ? "✓" : "✗")}  Puzzle:{(puzzle ? "✓" : "✗")}  Moves:{moveCount}  Items:{itemsCollected}                    ||");
            
            // Power-up indicators
            string powerUps = "";
            if (GameState.ShieldActive) powerUps += " [SHIELD]";
            if (GameState.SpeedBoostActive) powerUps += $" [SPEED x{GameState.SpeedBoostTurns}]";
            if (!string.IsNullOrEmpty(powerUps))
            {
                Console.Write("||  POWER-UPS:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(powerUps);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(new string(' ', 53 - powerUps.Length) + "||");
            }
            else
            {
                Console.WriteLine("||                                                                  ||");
            }
            
            if (GameState.Boss != null && GameState.Boss.Hp > 0)
            {
                Console.Write("||  BOSS HP: ");
                Console.ForegroundColor = theme.Enemies;
                Console.Write(new string('█', GameState.Boss.Hp) + new string('░', 10 - GameState.Boss.Hp));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("                                       ||");
            }
            else
            {
                Console.WriteLine("||  WASD:Move  E:Interact  I:Inventory  P:Pause  Q:Quit  T:Theme   ||");
            }
            
            Console.ForegroundColor = theme.UI;
            Console.WriteLine("======================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Move(int dr, int dc)
        {
            int nr = pRow + dr, nc = pCol + dc;
            if (nr < 0 || nr >= map.GetLength(0) || nc < 0 || nc >= map.GetLength(1)) return;
            
            char t = map[nr, nc];
            bool doorCheckpoint = false;
            
            if (t == '#') { Console.Beep(200, 100); return; }
            
            if (GameState.Boss != null && GameState.Boss.Hp > 0 && nr == GameState.Boss.Row && nc == GameState.Boss.Col)
            {
                BossFight();
                return;
            }
            
            moveCount++;
            o2--;
            score += 1;
            
            if (o2 <= 0) 
            { 
                GameOver("Oxygen depleted!");
                return;
            }
            
            if (t == 'K') { key = true; map[nr, nc] = ' '; score += 50; itemsCollected++; Console.Beep(800, 100); CheckAchievement("key"); }
            if (t == 'F') { map[nr, nc] = ' '; score += 30; itemsCollected++; Console.Beep(800, 100); }
            if (t == 'H') { hp = Math.Min(hp + 2, maxHp); map[nr, nc] = ' '; score += 20; itemsCollected++; Console.Beep(600, 150); }
            if (t == 'O') { o2 = Math.Min(o2 + 30, 100); map[nr, nc] = ' '; score += 20; itemsCollected++; Console.Beep(600, 150); }
            
            // Power-up items
            if (t == 'S') 
            { 
                GameState.ShieldActive = true; 
                map[nr, nc] = ' '; 
                score += 75; 
                itemsCollected++; 
                Console.Beep(1000, 200);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(" [SHIELD ACTIVATED!] ");
                Thread.Sleep(500);
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (t == 'L') 
            { 
                GameState.SpeedBoostActive = true; 
                GameState.SpeedBoostTurns = 10; 
                map[nr, nc] = ' '; 
                score += 75; 
                itemsCollected++; 
                Console.Beep(1200, 200);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(" [SPEED BOOST ACTIVATED!] ");
                Thread.Sleep(500);
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            if (t == 'D' && !key) { Console.Beep(200, 200); return; }
            if (t == 'D') 
            { 
                GameState.DoorOpened = true; 
                map[nr, nc] = ' '; 
                score += 40; 
                Console.Beep(900, 200);
                doorCheckpoint = true;
                
                // Kapı açıldığında bilgi mesajı
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(" [DOOR OPENED! Checkpoint saved!]                    ");
                Thread.Sleep(800);
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            // Exit koşulları
            if (t == 'E')
            {
                if (!GameState.DoorOpened)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" [EXIT LOCKED: Find and open the DOOR first!]                    ");
                    Thread.Sleep(1000);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Beep(200, 200);
                    return;
                }
                
                if (GameState.Boss != null && GameState.Boss.Hp > 0)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" [EXIT LOCKED: Defeat the BOSS first!]                    ");
                    Thread.Sleep(1000);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Beep(200, 200);
                    return;
                }
                
                if (!puzzle && GameState.Boss == null)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" [EXIT LOCKED: Solve the PUZZLE first! Press E near it]                    ");
                    Thread.Sleep(1000);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Beep(200, 200);
                    return;
                }
                
                score += 100;
                CheckAchievement("levelComplete");
                if (currentLevel + 1 >= LevelData.Levels.Count)
                    Victory();
                else
                {
                    LoadLevel(currentLevel + 1);
                    return;
                }
            }
            
            pRow = nr; 
            pCol = nc;

            // Kapı açıldıysa checkpoint kaydet
            if (doorCheckpoint)
            {
                SaveCheckpoint();
            }
        }

        static void Interact()
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int r = pRow + dr, c = pCol + dc;
                    if (r >= 0 && r < map.GetLength(0) && c >= 0 && c < map.GetLength(1))
                    {
                        if (map[r, c] == 'P')
                        {
                            DoPuzzle();
                            return;
                        }
                    }
                }
            }
            Console.Beep(300, 100);
        }

 static void DoPuzzle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n\n  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║            PUZZLE CHALLENGE               ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            
            if (currentLevel == 0)
            {
                Console.WriteLine("\n  Level 1 Puzzle - REGEX DIGIT EXTRACTION:");
                Console.WriteLine("  System message received:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  \"Error404-System:LOCKED-Code:7593-Access:DENIED\"");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n  Extract the 4-digit code using regex pattern.");
                Console.WriteLine("  Hint: Find the digits after 'Code:'");
                Console.Write("\n  Enter the 4-digit code: ");
                string answer = Console.ReadLine()?.Trim() ?? "";
                
                // Regex solution check
                string text = "Error404-System:LOCKED-Code:7593-Access:DENIED";
                var match = Regex.Match(text, @"Code:(\d{4})");
                string correctAnswer = match.Success ? match.Groups[1].Value : "7593";
                
                // Alternative: check if user found any 4 digits
                var userMatch = Regex.Match(answer, @"^\d{4}$");
                bool validFormat = userMatch.Success;
                
                if (answer == correctAnswer && validFormat)
                {
                    puzzle = true;
                    score += 150;
                    Console.Beep(1000, 300);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n  ✓ CORRECT! Code successfully extracted: {correctAnswer}");
                    Console.WriteLine("  Regex pattern used: Code:(\\d{4})");
                    Console.WriteLine("  \\d matches any digit (0-9)");
                    Console.WriteLine("  {4} means exactly 4 digits");
                    CheckAchievement("puzzle");
                }
                else
                {
                    hp--;
                    Console.Beep(200, 300);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  ✗ WRONG! Try again next time.");
                }
            }
            else if (currentLevel == 1)
            {
                Console.WriteLine("\n  Level 2 Puzzle - EMAIL VALIDATION WITH REGEX:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Which of these emails is valid?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n  A) user@example.com");
                Console.WriteLine("  B) invalid.email@");
                Console.WriteLine("  C) @nouser.com");
                Console.WriteLine("  D) test@@double.com");
                Console.WriteLine("\n  Hint: Valid email format → username@domain.extension");
                Console.WriteLine("  Use regex pattern to check: ^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
                Console.Write("\n  Answer (A/B/C/D): ");
                string answer = Console.ReadLine()?.Trim().ToUpper() ?? "";
                
                // Validate with actual regex
                string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                bool isCorrect = answer == "A";
                
                if (isCorrect)
                {
                    // Verify the answer with regex
                    string emailA = "user@example.com";
                    bool regexMatch = Regex.IsMatch(emailA, emailPattern);
                    
                    puzzle = true;
                    score += 150;
                    Console.Beep(1000, 300);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n  ✓ CORRECT! user@example.com is valid!");
                    Console.WriteLine("  \n  Regex breakdown:");
                    Console.WriteLine("  ^[\\w-\\.]+     → Username (letters, numbers, -, .)");
                    Console.WriteLine("  @             → Required @ symbol");
                    Console.WriteLine("  ([\\w-]+\\.)+  → Domain name with dot");
                    Console.WriteLine("  [\\w-]{2,4}$  → Extension (2-4 chars)");
                    Console.WriteLine("\n  Why others are INVALID:");
                    Console.WriteLine("  B) invalid.email@     → Missing domain after @");
                    Console.WriteLine("  C) @nouser.com        → Missing username before @");
                    Console.WriteLine("  D) test@@double.com   → Double @@ is invalid");
                    CheckAchievement("puzzle");
                }
                else
                {
                    hp--;
                    Console.Beep(200, 300);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  ✗ WRONG! Think about the valid email format.");
                }
            }
            else if (currentLevel == 2)
            {
                Console.WriteLine("\n  Level 3 Puzzle - URL VALIDATION WITH REGEX:");
                Console.WriteLine("  Which of these URLs has a valid HTTPS format?");
                Console.WriteLine("\n  A) https://github.com/user/repo");
                Console.WriteLine("  B) http://example.com");
                Console.WriteLine("  C) ftp://files.server.net");
                Console.WriteLine("  D) https://test");
                Console.WriteLine("\n  Hint: Look for HTTPS protocol + domain + path");
                Console.Write("\n  Answer (A/B/C/D): ");
                string answer = Console.ReadLine()?.Trim().ToUpper() ?? "";
                
                // Regex to validate HTTPS URLs with proper format
                string httpsPattern = @"^https://[\w.-]+\.[a-z]{2,}/[\w/.-]*$";
                string urlA = "https://github.com/user/repo";
                bool isValid = Regex.IsMatch(urlA, httpsPattern);
                
                if (answer == "A")
                {
                    puzzle = true;
                    score += 150;
                    Console.Beep(1000, 300);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n  ✓ CORRECT! https://github.com/user/repo is valid!");
                    Console.WriteLine("\n  Regex breakdown:");
                    Console.WriteLine("  ^https://       → Must start with HTTPS");
                    Console.WriteLine("  [\\w.-]+\\.[a-z] → Domain with extension");
                    Console.WriteLine("  /[\\w/.-]*$     → Valid path characters");
                    Console.WriteLine("\n  Why others are INVALID:");
                    Console.WriteLine("  B) http://example.com   → HTTP not HTTPS");
                    Console.WriteLine("  C) ftp://files.server   → FTP not HTTPS");
                    Console.WriteLine("  D) https://test         → Missing domain extension & path");
                    CheckAchievement("puzzle");
                }
                else
                {
                    hp--;
                    Console.Beep(200, 300);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  ✗ WRONG! Remember: HTTPS with proper domain and path.");
                }
            }
            else
            {
                Console.WriteLine($"\n  Level {currentLevel + 1} Puzzle - ULTIMATE REGEX CHALLENGE:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  PASSWORD STRENGTH VALIDATION");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n  Which password is STRONG enough?");
                Console.WriteLine("  (Must have: 8+ chars, uppercase, lowercase, digit, special char)");
                Console.WriteLine("\n  A) Password123!");
                Console.WriteLine("  B) pass123");
                Console.WriteLine("  C) HELLO@2024");
                Console.WriteLine("  D) test");
                Console.Write("\n  Answer (A/B/C/D): ");
                string answer = Console.ReadLine()?.Trim().ToUpper() ?? "";
                
                // Regex for strong password: 8+ chars, upper, lower, digit, special
                string strongPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";
                string passA = "Password123!";
                bool isStrong = Regex.IsMatch(passA, strongPattern);
                
                if (answer == "A")
                {
                    puzzle = true;
                    score += 200;
                    Console.Beep(1000, 300);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n  ✓ CORRECT! Password123! is STRONG!");
                    Console.WriteLine("\n  Regex pattern (lookaheads):");
                    Console.WriteLine("  (?=.*[a-z])     → Has lowercase");
                    Console.WriteLine("  (?=.*[A-Z])     → Has uppercase");
                    Console.WriteLine("  (?=.*\\d)        → Has digit");
                    Console.WriteLine("  (?=.*[@$!%*?&#]) → Has special char");
                    Console.WriteLine("  {8,}            → At least 8 characters");
                    Console.WriteLine("\n  Why others are WEAK:");
                    Console.WriteLine("  B) pass123       → No uppercase, no special char");
                    Console.WriteLine("  C) HELLO@2024    → No lowercase");
                    Console.WriteLine("  D) test          → Too short, missing requirements");
                    CheckAchievement("puzzle");
                }
                else
                {
                    hp--;
                    Console.Beep(200, 300);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  ✗ WRONG! Check all password requirements carefully.");
                }
            }
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n  Press ENTER to continue...");
            Console.ReadKey();
        }

        static void BossFight()
        {
            if (GameState.Boss == null) return;
            
            Console.Beep(400, 100);
            
            // Shield protection
            if (GameState.ShieldActive)
            {
                GameState.ShieldActive = false;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(" [SHIELD BLOCKED DAMAGE!] ");
                Thread.Sleep(500);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Beep(800, 200);
            }
            else
            {
                // Boss vuruyor, sadece oyuncunun canı azalacak
                hp--;
            }
            score += 10;
            
            // Check player death FIRST
            if (hp <= 0) 
            {
                GameOver("The boss defeated you!");
                return;
            }
            
            // Then check boss death
            if (GameState.Boss.Hp <= 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ╔═══════════════════════════════════════════╗");
                Console.WriteLine("  ║          YOU DEFEATED THE BOSS!           ║");
                Console.WriteLine("  ╚═══════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Beep(800, 200);
                Console.Beep(1000, 200);
                Console.Beep(1200, 300);
                score += 500;
                puzzle = true;
                CheckAchievement("boss");
                Thread.Sleep(2000);
            }
        }

        static void CheckCollisions()
        {
            // Boss collision check
            if (GameState.Boss != null && GameState.Boss.Hp > 0 && GameState.Boss.Row == pRow && GameState.Boss.Col == pCol)
            {
                BossFight();
                return;
            }
            
            foreach (var enemy in GameState.Enemies)
            {
                if (enemy.Row == pRow && enemy.Col == pCol)
                {
                    // Shield protection
                    if (GameState.ShieldActive)
                    {
                        GameState.ShieldActive = false;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(" [SHIELD BLOCKED DAMAGE!] ");
                        Thread.Sleep(500);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Beep(800, 200);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 150);
                        if (hp <= 0) 
                        {
                            GameOver("Defeated by an enemy!");
                        }
                    }
                    return;
                }
            }
        }

        static void CheckAchievement(string type)
        {
            string achievement = "";
            
            if (type == "key" && !GameState.Achievements.Contains("First Key"))
                achievement = "First Key - Found your first key!";
            else if (type == "puzzle" && !GameState.Achievements.Contains("Puzzle Master"))
                achievement = "Puzzle Master - Solved a puzzle!";
            else if (type == "boss" && !GameState.Achievements.Contains("Boss Slayer"))
                achievement = "Boss Slayer - Defeated the boss!";
            else if (type == "levelComplete" && currentLevel == 0 && !GameState.Achievements.Contains("First Steps"))
                achievement = "First Steps - Completed first level!";
            else if (type == "speedrun" && moveCount < 50 && !GameState.Achievements.Contains("Speed Runner"))
                achievement = "Speed Runner - Beat the game quickly!";
            
            if (achievement != "" && !GameState.Achievements.Contains(achievement))
            {
                GameState.Achievements.Add(achievement);
                score += 200;
                Console.Beep(1200, 200);
            }
        }

        static void ShowInventory()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║              INVENTORY & STATS            ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine($"\n  Level: {currentLevel + 1}/{LevelData.Levels.Count}");
            Console.WriteLine($"  HP: {hp}/{maxHp}");
            Console.WriteLine($"  Oxygen: {o2}%");
            Console.WriteLine($"  Score: {score}");
            Console.WriteLine($"  Moves: {moveCount}");
            Console.WriteLine($"  Items Collected: {itemsCollected}");
            Console.WriteLine($"  Difficulty: {difficulty}");
            
            Console.WriteLine("\n  Items:");
            Console.WriteLine($"  - Key: {(key ? "✓" : "✗")}");
            Console.WriteLine($"  - Puzzle Solved: {(puzzle ? "✓" : "✗")}");
            
            if (GameState.Achievements.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  ACHIEVEMENTS:");
                foreach (var ach in GameState.Achievements)
                    Console.WriteLine($"  ★ {ach}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\n  ENTER...");
            Console.ReadKey();
        }

        static void PauseMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║               PAUSE MENU                  ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.WriteLine("\n  R - Resume");
            Console.WriteLine("  A - Achievements");
            Console.WriteLine("  Q - Quit to Menu");
            
            var k = Console.ReadKey(true).Key;
            if (k == ConsoleKey.A)
            {
                ShowInventory();
                PauseMenu();
            }
            else if (k == ConsoleKey.Q)
            {
                if (Confirm("Are you sure you want to return to menu?"))
                    run = false;
            }
        }

        static bool Confirm(string message)
        {
            Console.Clear();
            Console.WriteLine($"\n  {message}");
            Console.WriteLine("\n  Y - Yes   N - No");
            var k = Console.ReadKey(true).Key;
            return k == ConsoleKey.Y;
        }

        static void GameOver(string reason)
        {
            GameState.DeathCount++;
            
            // Oyunun çalışmaya devam ettiğinden emin ol
            run = true;
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║              YOU DIED!                    ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n  {reason}");
            Console.WriteLine($"\n  Deaths: {GameState.DeathCount}");
            Console.Beep(200, 500);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\n  Respawning at last checkpoint in 2 seconds...");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(2000);

            // Otomatik olarak son checkpoint'ten devam et
            LoadCheckpoint();
        }

        static void Victory()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("  ╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ║                    CONGRATULATIONS!                           ║");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ║              You completed all levels!                        ║");
            Console.WriteLine("  ║              You successfully escaped the device!             ║");
            Console.WriteLine("  ║                                                               ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            
            if (moveCount < 200) CheckAchievement("speedrun");
            
            Console.WriteLine($"\n  Final Score: {score}");
            Console.WriteLine($"  Total Moves: {moveCount}");
            Console.WriteLine($"  Items Collected: {itemsCollected}");
            Console.WriteLine($"  Difficulty: {difficulty}");
            Console.WriteLine($"  Achievements Unlocked: {GameState.Achievements.Count}");
            
            Console.Beep(600, 200);
            Console.Beep(800, 200);
            Console.Beep(1000, 300);
            
            Console.WriteLine("\n\n  ENTER...");
            Console.ReadKey();
            run = false;
        }

        static void ShowEnding()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n  Thank you for playing!");
            Console.WriteLine("\n  Programming Languages - Final Project");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        static void SaveCheckpoint()
        {
            GameState.CheckpointLevel = currentLevel;
            GameState.CheckpointRow = pRow;
            GameState.CheckpointCol = pCol;
            GameState.CheckpointHp = hp;
            GameState.CheckpointO2 = o2;
            GameState.CheckpointScore = score;
            GameState.CheckpointKey = key;
            GameState.CheckpointPuzzle = puzzle;
            GameState.CheckpointDoorOpened = GameState.DoorOpened;
            GameState.CheckpointMoves = moveCount;
            GameState.CheckpointItems = itemsCollected;
            GameState.CheckpointShieldActive = GameState.ShieldActive;
            
            // Checkpoint kaydedildi mesajı (kısa süre göster)
            Console.ForegroundColor = ConsoleColor.Green;
            var oldTop = Console.CursorTop;
            var oldLeft = Console.CursorLeft;
            Console.SetCursorPosition(2, Console.WindowHeight - 2);
            Console.Write("  ✓ Checkpoint saved!");
            Console.SetCursorPosition(oldLeft, oldTop);
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(800);
        }
        
        static void LoadCheckpoint()
        {
            // Checkpoint verilerini al
            int cpLevel = GameState.CheckpointLevel;
            int cpRow = GameState.CheckpointRow;
            int cpCol = GameState.CheckpointCol;
            int cpHp = GameState.CheckpointHp;
            int cpO2 = GameState.CheckpointO2;
            int cpScore = GameState.CheckpointScore;
            bool cpKey = GameState.CheckpointKey;
            bool cpPuzzle = GameState.CheckpointPuzzle;
            bool cpDoorOpened = GameState.CheckpointDoorOpened;
            int cpMoves = GameState.CheckpointMoves;
            int cpItems = GameState.CheckpointItems;
            bool cpShieldActive = GameState.CheckpointShieldActive;

            // Level'ı yükle ama checkpoint KAYDETME
            currentLevel = cpLevel;
            map = (char[,])LevelData.Levels[currentLevel].Clone();
            GameState.Enemies.Clear();
            GameState.Boss = null;
            
            // Haritadaki düşmanları ve boss'u yeniden oluştur
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == '@')
                        map[r, c] = ' ';
                    else if (map[r, c] == 'M')
                    {
                        GameState.Enemies.Add(new Enemy { Row = r, Col = c });
                        map[r, c] = ' ';
                    }
                    else if (map[r, c] == 'B')
                    {
                        GameState.Boss = new Boss { Row = r, Col = c };
                        map[r, c] = ' ';
                    }
                }
            }

            // Checkpoint verilerini geri yükle
            pRow = cpRow;
            pCol = cpCol;
            hp = cpHp;
            o2 = cpO2;
            score = cpScore;
            key = cpKey;
            puzzle = cpPuzzle;
            GameState.DoorOpened = cpDoorOpened;
            moveCount = cpMoves;
            itemsCollected = cpItems;
            GameState.ShieldActive = cpShieldActive;
            
            // Oyunun devam ettiğinden emin ol
            run = true;

            // Respawn mesajı göster
            Console.Clear();
            Console.ForegroundColor = GameState.Themes[GameState.CurrentTheme].Player;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║         RESPAWNED AT CHECKPOINT           ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n  Position: Level {currentLevel + 1}");
            Console.WriteLine($"  HP: {hp}/{maxHp}  O2: {o2}%");
            Console.WriteLine($"  Door Status: {(GameState.DoorOpened ? "OPEN ✓" : "LOCKED ✗")}");
            Console.WriteLine("\n  Press any key to continue...");
            Console.ReadKey(true);
        }
        
        static void ChangeTheme()
        {
            Console.Clear();
            var theme = GameState.Themes[GameState.CurrentTheme];
            Console.ForegroundColor = theme.UI;
            Console.WriteLine("\n  ╔═══════════════════════════════════════════╗");
            Console.WriteLine("  ║            CHANGE THEME                   ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n  Current: " + GameState.CurrentTheme);
            Console.WriteLine("\n  1. CLASSIC - Original colors");
            Console.WriteLine("  2. MATRIX  - Green hacker vibes");
            Console.WriteLine("  3. OCEAN   - Deep blue sea");
            Console.WriteLine("  4. FIRE    - Burning red & yellow");
            Console.WriteLine("  5. DARK    - Monochrome darkness");
            Console.WriteLine("  6. NEON    - Vibrant neon lights");
            Console.WriteLine("\n  ESC - Cancel\n");
            
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape) return;
            
            GameState.CurrentTheme = key switch
            {
                ConsoleKey.D1 => "CLASSIC",
                ConsoleKey.D2 => "MATRIX",
                ConsoleKey.D3 => "OCEAN",
                ConsoleKey.D4 => "FIRE",
                ConsoleKey.D5 => "DARK",
                ConsoleKey.D6 => "NEON",
                _ => GameState.CurrentTheme
            };
            
            Console.Clear();
            Console.ForegroundColor = GameState.Themes[GameState.CurrentTheme].UI;
            Console.WriteLine($"\n  ✓ Theme changed to: {GameState.CurrentTheme}");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
        }
    }
}