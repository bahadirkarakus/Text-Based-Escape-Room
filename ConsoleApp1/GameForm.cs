using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Media;
using System.IO;
using Timer = System.Windows.Forms.Timer;

namespace ConsoleApp1
{
    public class GameForm : Form
    {
        // Level definitions
        private List<char[,]> levels = new List<char[,]>
        {
            // LEVEL 1
            new char[,] {
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                { '#','@',' ',' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ',' ',' ',' ','K',' ','#' },
                { '#',' ','#','#','#',' ','#','#','#','#','#',' ','#','#','#',' ','#','S',' ','#' },
                { '#',' ',' ',' ',' ',' ','H',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                { '#',' ','#','#','#',' ','#','#','#',' ','#','#','#',' ','#','#','#','F',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','D',' ','#' },
                { '#',' ','#',' ','#','#','#',' ','#','#','#',' ','#','#','#',' ','#','L',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','E',' ','#' },
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' }
            },
            // LEVEL 2
            new char[,] {
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                { '#','@',' ',' ','#','M',' ','W',' ','#','P',' ',' ','#',' ',' ','K',' ','R','#' },
                { '#',' ','#',' ','#',' ','#','#',' ','#','#',' ','#','#',' ','#','#','#',' ','#' },
                { '#',' ','#',' ',' ',' ',' ','M',' ',' ',' ',' ',' ',' ',' ','R',' ','O',' ','#' },
                { '#',' ','#','#','#','#','#',' ','#','#','#','#','#',' ','#','#','#','#',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','H',' ','#' },
                { '#',' ','#',' ','#','#','#',' ','#','#','#','#','#',' ','#','#','#','D',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','E',' ','#' },
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' }
            },
            // LEVEL 3
            new char[,] {
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                { '#','@',' ','M',' ',' ','P',' ','W',' ',' ','R',' ',' ',' ','K',' ','M',' ','#' },
                { '#',' ','#','#','#','#','#','#','#',' ','#','#','#','#','#','#','#','#','S','#' },
                { '#',' ',' ',' ',' ','R',' ',' ',' ',' ',' ',' ',' ',' ','R',' ',' ',' ',' ','#' },
                { '#',' ','#','H',' ','#','O',' ','#',' ','#',' ','#','H',' ','#','F',' ','#','#' },
                { '#',' ','R',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','R',' ','#' },
                { '#',' ','#','#','#','#','#','#','#',' ','#','#','#','#','#','#','#','D','L','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','E',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' }
            },
            // LEVEL 4 - BOSS LEVEL
            new char[,] {
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                { '#','@',' ',' ','H',' ','W',' ',' ',' ','K',' ',' ',' ','W',' ','H',' ',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ','S',' ','B',' ','S',' ',' ',' ',' ',' ',' ',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                { '#',' ',' ','O',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','O',' ',' ',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ','P',' ',' ',' ',' ','L',' ',' ','D',' ','#' },
                { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','E',' ','#' },
                { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' }
            }
        };

        // Game state
        private int currentLevel = 0;
        private char[,] map = null!;
        private int pRow = 1, pCol = 1, maxHp = 5, hp = 5, o2 = 150, score = 0;
        private bool key = false, puzzle = false;
        private string difficulty = "NORMAL";
        private int moveCount = 0, itemsCollected = 0;
        private List<string> achievements = new List<string>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<ShooterEnemy> shooterEnemies = new List<ShooterEnemy>();
        private List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
        private Boss? boss = null;
        private Random rand = new Random();
        
        // Checkpoint system
        private int checkpointLevel = 0;
        private int checkpointRow = 1, checkpointCol = 1;
        private int checkpointHp = 5, checkpointO2 = 150;
        private int checkpointScore = 0;
        private bool checkpointKey = false, checkpointPuzzle = false;
        private int checkpointMoves = 0, checkpointItems = 0;
        private bool doorOpened = false;
        private bool checkpointDoorOpened = false;
        private bool checkpointHasWeapon = false;
        private int checkpointAmmo = 0;
        private int deathCount = 0;
        private char[,]? checkpointMap = null;
        private List<Enemy> checkpointEnemies = new List<Enemy>();
        private List<ShooterEnemy> checkpointShooterEnemies = new List<ShooterEnemy>();
        private Boss? checkpointBoss = null;
        
        // Weapon System
        private List<Bullet> bullets = new List<Bullet>();
        private bool hasWeapon = false;
        private int ammo = 0;
        private int maxAmmo = 30;
        
        // Power-ups System
        private bool hasShield = false;
        private int shieldHits = 0;
        private bool hasSpeedBoost = false;
        private int speedBoostTimer = 0;
        private bool isInvisible = false;
        private int invisibilityTimer = 0;
        private bool hasDoubleScore = false;
        private int doubleScoreTimer = 0;
        
        // Theme System
        private string currentTheme = "Normal";
        private Dictionary<string, ThemeColors> themes = new Dictionary<string, ThemeColors>();
        
        // Story Mode
        private List<string> levelStories = new List<string>();
        
        // Background Music
        private SoundPlayer? backgroundMusic = null;
        
        private Timer gameTimer = null!;
        private const int CellSize = 30;
        private Panel gamePanel = null!;
        private Label statusLabel = null!;
        private Label infoLabel = null!;

        // Enemy class
        class Enemy
        {
            public int Row, Col;
            public int MoveCounter = 0;
            
            public void Move(char[,] map, Random rand)
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
                    
                    if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
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

        // Boss class
        class Boss
        {
            public int Row, Col, Hp = 10;
            public int MoveCounter = 0;
            
            public void MoveTowardsPlayer(char[,] map, int pRow, int pCol)
            {
                MoveCounter++;
                if (MoveCounter < 2) return;
                MoveCounter = 0;

                int dr = 0, dc = 0;
                if (pRow < Row) dr = -1;
                else if (pRow > Row) dr = 1;
                if (pCol < Col) dc = -1;
                else if (pCol > Col) dc = 1;

                // Önce ideal hareketi dene
                int newR = Row + dr;
                int newC = Col + dc;
                
                if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
                {
                    if (map[newR, newC] == ' ')
                    {
                        Row = newR;
                        Col = newC;
                        return;
                    }
                }
                
                // Duvar varsa alternatif yollar dene
                // Önce sadece yatay hareket dene
                if (dc != 0)
                {
                    newR = Row;
                    newC = Col + dc;
                    if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
                    {
                        if (map[newR, newC] == ' ')
                        {
                            Row = newR;
                            Col = newC;
                            return;
                        }
                    }
                }
                
                // Sonra sadece dikey hareket dene
                if (dr != 0)
                {
                    newR = Row + dr;
                    newC = Col;
                    if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
                    {
                        if (map[newR, newC] == ' ')
                        {
                            Row = newR;
                            Col = newC;
                            return;
                        }
                    }
                }
                
                // Hala hareket edemediyse, rastgele bir yön dene
                int[] dirs = { -1, 1, 0, 0 };
                int[] dcols = { 0, 0, -1, 1 };
                Random rand = new Random();
                
                for (int i = 0; i < 4; i++)
                {
                    int idx = rand.Next(4);
                    newR = Row + dirs[idx];
                    newC = Col + dcols[idx];
                    
                    if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
                    {
                        if (map[newR, newC] == ' ')
                        {
                            Row = newR;
                            Col = newC;
                            return;
                        }
                    }
                }
            }
        }

        // Bullet class for weapon system
        class Bullet
        {
            public float X, Y;
            public float Dx, Dy;
            public bool Active = true;
        }

        // Enemy bullet class
        class EnemyBullet
        {
            public float X, Y;
            public float Dx, Dy;
        }

        // Shooter Enemy class
        class ShooterEnemy
        {
            public int Row, Col;
            public int ShootCounter = 0;
            
            public void Shoot(List<EnemyBullet> enemyBullets, int pRow, int pCol)
            {
                ShootCounter++;
                if (ShootCounter < 5) return; // Her 5 tick'te bir ateş et
                ShootCounter = 0;

                // Oyuncuya doğru ateş et
                float dx = pCol - Col;
                float dy = pRow - Row;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                
                if (distance > 0 && distance < 10) // Sadece 10 birim uzaklıkta ateş et
                {
                    dx /= distance;
                    dy /= distance;
                    enemyBullets.Add(new EnemyBullet { X = Col, Y = Row, Dx = dx * 0.3f, Dy = dy * 0.3f });
                }
            }
        }

        // Theme colors class
        class ThemeColors
        {
            public Color Background { get; set; }
            public Color Wall { get; set; }
            public Color Player { get; set; }
            public Color Enemy { get; set; }
            public Color Boss { get; set; }
            public Color Item { get; set; }
            public Color Exit { get; set; }
            public Color UI { get; set; }
        }

        public GameForm()
        {
            InitializeThemes();
            InitializeStories();
            InitializeComponents();
            InitializeMusic();
            ShowIntroStory();
            ShowDifficultyDialog();
            LoadLevel(0);
            
            gameTimer = new Timer();
            gameTimer.Interval = 200;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void InitializeMusic()
        {
            try
            {
                // Müzik dosyası konumu: ConsoleApp1/music/background.wav veya background.mp3
                string musicPath = Path.Combine(AppContext.BaseDirectory, "music", "background.wav");
                
                if (File.Exists(musicPath))
                {
                    backgroundMusic = new SoundPlayer(musicPath);
                    backgroundMusic.PlayLooping();
                }
                else
                {
                    // Alternatif isimler dene
                    string[] alternatives = { "stranger_things.wav", "shape_of_my_heart.wav", "game_music.wav", "background_music.wav" };
                    foreach (var alt in alternatives)
                    {
                        string altPath = Path.Combine(AppContext.BaseDirectory, "music", alt);
                        if (File.Exists(altPath))
                        {
                            backgroundMusic = new SoundPlayer(altPath);
                            backgroundMusic.PlayLooping();
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Müzik yüklenemezse sessiz devam et
            }
        }

        private void InitializeThemes()
        {
            themes["Normal"] = new ThemeColors
            {
                Background = Color.Black,
                Wall = Color.Gray,
                Player = Color.Cyan,
                Enemy = Color.Red,
                Boss = Color.DarkRed,
                Item = Color.Yellow,
                Exit = Color.Lime,
                UI = Color.FromArgb(30, 30, 40)
            };
            
            themes["Dark"] = new ThemeColors
            {
                Background = Color.FromArgb(10, 10, 15),
                Wall = Color.FromArgb(40, 40, 45),
                Player = Color.FromArgb(100, 200, 255),
                Enemy = Color.FromArgb(180, 0, 0),
                Boss = Color.FromArgb(120, 0, 0),
                Item = Color.FromArgb(200, 200, 100),
                Exit = Color.FromArgb(0, 180, 0),
                UI = Color.FromArgb(20, 20, 25)
            };
            
            themes["Neon"] = new ThemeColors
            {
                Background = Color.FromArgb(5, 0, 20),
                Wall = Color.FromArgb(255, 0, 255),
                Player = Color.FromArgb(0, 255, 255),
                Enemy = Color.FromArgb(255, 0, 100),
                Boss = Color.FromArgb(255, 0, 0),
                Item = Color.FromArgb(0, 255, 0),
                Exit = Color.FromArgb(255, 255, 0),
                UI = Color.FromArgb(20, 0, 40)
            };
            
            themes["Retro"] = new ThemeColors
            {
                Background = Color.FromArgb(40, 30, 20),
                Wall = Color.FromArgb(139, 90, 43),
                Player = Color.FromArgb(255, 215, 0),
                Enemy = Color.FromArgb(178, 34, 34),
                Boss = Color.FromArgb(139, 0, 0),
                Item = Color.FromArgb(255, 165, 0),
                Exit = Color.FromArgb(154, 205, 50),
                UI = Color.FromArgb(60, 50, 40)
            };
        }

        private void InitializeStories()
        {
            levelStories.Add(
                "CHAPTER 1: THE AWAKENING\n\n" +
                "You wake up in a cold, metallic chamber.\n" +
                "The last thing you remember is a bright flash...\n" +
                "Now you're trapped inside some kind of device.\n" +
                "You must find the exit and escape!"
            );
            
            levelStories.Add(
                "CHAPTER 2: THE DANGER\n\n" +
                "You made it through, but something is wrong.\n" +
                "You hear strange noises echoing through the halls.\n" +
                "Hostile entities are patrolling the corridors.\n" +
                "Stay alert and survive!"
            );
            
            levelStories.Add(
                "CHAPTER 3: THE CHAOS\n\n" +
                "The situation is getting worse.\n" +
                "More enemies appear at every corner.\n" +
                "Your oxygen is running low.\n" +
                "You must hurry to find the way out!"
            );
            
            levelStories.Add(
                "CHAPTER 4: THE FINAL SHOWDOWN\n\n" +
                "You've reached the core of the device.\n" +
                "A massive hostile entity guards the exit.\n" +
                "This is your last chance.\n" +
                "Defeat the guardian and escape to freedom!"
            );
        }

        private void ShowIntroStory()
        {
            using (var storyForm = new Form())
            {
                storyForm.Text = "Escape Device - Story";
                storyForm.Size = new Size(600, 400);
                storyForm.StartPosition = FormStartPosition.CenterScreen;
                storyForm.BackColor = Color.FromArgb(20, 20, 30);
                storyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                storyForm.MaximizeBox = false;
                storyForm.MinimizeBox = false;

                var titleLabel = new Label
                {
                    Text = "ESCAPE DEVICE\nULTIMATE EDITION",
                    Font = new Font("Consolas", 20, FontStyle.Bold),
                    ForeColor = Color.Cyan,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 100
                };
                storyForm.Controls.Add(titleLabel);

                var storyLabel = new Label
                {
                    Text = "PROLOGUE\n\n" +
                           "Year 2157. Human consciousness transfer experiments\n" +
                           "have gone terribly wrong.\n\n" +
                           "You are Subject #42, trapped inside a digital prison.\n" +
                           "The facility's AI has turned hostile.\n\n" +
                           "Your mission: Escape at any cost.\n" +
                           "Your weapon: Experimental neural disruptor.\n" +
                           "Your chance: One.",
                    Font = new Font("Consolas", 11),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.TopCenter,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20)
                };
                storyForm.Controls.Add(storyLabel);

                var continueBtn = new Button
                {
                    Text = "BEGIN MISSION",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                continueBtn.Click += (s, e) => storyForm.Close();
                storyForm.Controls.Add(continueBtn);

                storyForm.ShowDialog();
            }
        }

        private void InitializeComponents()
        {
            this.Text = "Escape Device - Ultimate Edition";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            this.BackColor = Color.FromArgb(20, 20, 30);

            // Info Label (top)
            infoLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 80,
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.FromArgb(30, 30, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10)
            };
            this.Controls.Add(infoLabel);

            // Game Panel (center)
            gamePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };
            gamePanel.Paint += GamePanel_Paint;
            
            // Enable double buffering to prevent flickering
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, gamePanel, new object[] { true });
            
            this.Controls.Add(gamePanel);

            // Status Label (bottom)
            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Font = new Font("Consolas", 9, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(30, 30, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(statusLabel);
        }

        private void ShowDifficultyDialog()
        {
            using (var dialog = new Form())
            {
                dialog.Text = "Select Difficulty";
                dialog.Size = new Size(350, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.BackColor = Color.FromArgb(30, 30, 40);

                var label = new Label
                {
                    Text = "Choose your difficulty level:",
                    Location = new Point(20, 20),
                    Size = new Size(300, 30),
                    Font = new Font("Consolas", 12, FontStyle.Bold),
                    ForeColor = Color.White
                };
                dialog.Controls.Add(label);

                var easyBtn = new Button
                {
                    Text = "EASY - More HP and Oxygen",
                    Location = new Point(50, 60),
                    Size = new Size(250, 35),
                    BackColor = Color.Green,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10, FontStyle.Bold)
                };
                easyBtn.Click += (s, e) =>
                {
                    difficulty = "EASY";
                    maxHp = 8;
                    hp = 8;
                    o2 = 200;
                    dialog.Close();
                };
                dialog.Controls.Add(easyBtn);

                var normalBtn = new Button
                {
                    Text = "NORMAL - Balanced gameplay",
                    Location = new Point(50, 100),
                    Size = new Size(250, 35),
                    BackColor = Color.Orange,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10, FontStyle.Bold)
                };
                normalBtn.Click += (s, e) =>
                {
                    difficulty = "NORMAL";
                    dialog.Close();
                };
                dialog.Controls.Add(normalBtn);

                var hardBtn = new Button
                {
                    Text = "HARD - Low HP, fast enemies!",
                    Location = new Point(50, 140),
                    Size = new Size(250, 35),
                    BackColor = Color.Red,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10, FontStyle.Bold)
                };
                hardBtn.Click += (s, e) =>
                {
                    difficulty = "HARD";
                    maxHp = 3;
                    hp = 3;
                    o2 = 130;
                    dialog.Close();
                };
                dialog.Controls.Add(hardBtn);

                dialog.ShowDialog();
            }
        }

        private void LoadLevel(int levelIndex)
        {
            if (levelIndex >= levels.Count)
            {
                Victory();
                return;
            }
            
            currentLevel = levelIndex;
            map = (char[,])levels[levelIndex].Clone();
            enemies.Clear();
            shooterEnemies.Clear();
            enemyBullets.Clear();
            boss = null;
            
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
                        enemies.Add(new Enemy { Row = r, Col = c });
                        map[r, c] = ' ';
                    }
                    else if (map[r, c] == 'R')
                    {
                        shooterEnemies.Add(new ShooterEnemy { Row = r, Col = c });
                        map[r, c] = ' ';
                    }
                    else if (map[r, c] == 'B')
                    {
                        boss = new Boss { Row = r, Col = c };
                        map[r, c] = ' ';
                    }
                }
            }
            
            puzzle = false;
            key = false;
            bullets.Clear();
            hasWeapon = false;
            ammo = 0;
            doorOpened = false;
            
            ShowLevelStory();
            UpdateLabels();
        }

        private void ShowLevelStory()
        {
            if (currentLevel < levelStories.Count)
            {
                using (var storyForm = new Form())
                {
                    storyForm.Text = $"Level {currentLevel + 1}";
                    storyForm.Size = new Size(500, 350);
                    storyForm.StartPosition = FormStartPosition.CenterScreen;
                    storyForm.BackColor = Color.FromArgb(20, 20, 30);
                    storyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    storyForm.MaximizeBox = false;
                    storyForm.MinimizeBox = false;

                    var storyLabel = new Label
                    {
                        Text = levelStories[currentLevel],
                        Font = new Font("Consolas", 11),
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Padding = new Padding(30)
                    };
                    storyForm.Controls.Add(storyLabel);

                    var continueBtn = new Button
                    {
                        Text = "CONTINUE",
                        Dock = DockStyle.Bottom,
                        Height = 50,
                        BackColor = Color.FromArgb(0, 120, 215),
                        ForeColor = Color.White,
                        Font = new Font("Consolas", 11, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat
                    };
                    continueBtn.Click += (s, e) => storyForm.Close();
                    storyForm.Controls.Add(continueBtn);

                    storyForm.ShowDialog();
                }
            }
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            // Update power-ups timers
            if (speedBoostTimer > 0)
            {
                speedBoostTimer--;
                if (speedBoostTimer == 0) hasSpeedBoost = false;
            }
            
            if (invisibilityTimer > 0)
            {
                invisibilityTimer--;
                if (invisibilityTimer == 0) isInvisible = false;
            }
            
            if (doubleScoreTimer > 0)
            {
                doubleScoreTimer--;
                if (doubleScoreTimer == 0) hasDoubleScore = false;
            }
            
            // Move enemies (slower if speed boost is active)
            foreach (var enemy in enemies)
            {
                if (!hasSpeedBoost || rand.Next(2) == 0)
                    enemy.Move(map, rand);
            }
            
            // Shooter enemies shoot
            foreach (var shooter in shooterEnemies)
            {
                shooter.Shoot(enemyBullets, pRow, pCol);
            }
            
            // Move enemy bullets
            UpdateEnemyBullets();
            
            if (boss != null && boss.Hp > 0)
            {
                if (!hasSpeedBoost || rand.Next(2) == 0)
                    boss.MoveTowardsPlayer(map, pRow, pCol);
            }
            
            UpdateBullets();
            CheckCollisions();
            UpdateLabels();
            gamePanel.Invalidate();
        }

        private void UpdateBullets()
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bullet = bullets[i];
                bullet.X += bullet.Dx * 0.3f;
                bullet.Y += bullet.Dy * 0.3f;
                
                int bRow = (int)bullet.Y;
                int bCol = (int)bullet.X;
                
                // Check wall collision
                if (bRow < 0 || bRow >= rows || bCol < 0 || bCol >= cols || map[bRow, bCol] == '#')
                {
                    bullets.RemoveAt(i);
                    continue;
                }
                
                // Check enemy collision
                for (int e = enemies.Count - 1; e >= 0; e--)
                {
                    if (Math.Abs(enemies[e].Row - bRow) < 1 && Math.Abs(enemies[e].Col - bCol) < 1)
                    {
                        enemies.RemoveAt(e);
                        bullets.RemoveAt(i);
                        score += (hasDoubleScore ? 200 : 100);
                        Console.Beep(900, 80);
                        Console.Beep(700, 80);
                        break;
                    }
                }
                
                // Check boss collision
                if (boss != null && boss.Hp > 0)
                {
                    if (Math.Abs(boss.Row - bRow) < 1 && Math.Abs(boss.Col - bCol) < 1)
                    {
                        boss.Hp--;
                        bullets.RemoveAt(i);
                        score += (hasDoubleScore ? 100 : 50);
                        Console.Beep(500, 100);
                        
                        if (boss.Hp <= 0)
                        {
                            gameTimer.Stop(); // Timer'ı durdur
                            boss = null; // Boss'u hemen temizle!
                            Console.Beep(600, 100);
                            Console.Beep(800, 100);
                            Console.Beep(1000, 100);
                            Console.Beep(1200, 200);
                            MessageBox.Show("BOSS DEFEATED! The exit is now open!", "Victory!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            gameTimer.Start(); // Timer'ı tekrar başlat
                        }
                    }
                }
                
                // Check shooter enemy collision
                for (int s = shooterEnemies.Count - 1; s >= 0; s--)
                {
                    if (Math.Abs(shooterEnemies[s].Row - bRow) < 1 && Math.Abs(shooterEnemies[s].Col - bCol) < 1)
                    {
                        shooterEnemies.RemoveAt(s);
                        bullets.RemoveAt(i);
                        score += (hasDoubleScore ? 300 : 150);
                        Console.Beep(1000, 80);
                        Console.Beep(800, 80);
                        break;
                    }
                }
            }
        }

        private void UpdateEnemyBullets()
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                var bullet = enemyBullets[i];
                bullet.X += bullet.Dx;
                bullet.Y += bullet.Dy;
                
                int bRow = (int)bullet.Y;
                int bCol = (int)bullet.X;
                
                // Check wall collision or out of bounds
                if (bRow < 0 || bRow >= rows || bCol < 0 || bCol >= cols || map[bRow, bCol] == '#')
                {
                    enemyBullets.RemoveAt(i);
                    continue;
                }
                
                // Check player collision
                if (Math.Abs(pRow - bRow) < 1 && Math.Abs(pCol - bCol) < 1)
                {
                    enemyBullets.RemoveAt(i);
                    
                    if (hasShield && shieldHits > 0)
                    {
                        shieldHits--;
                        if (shieldHits <= 0) hasShield = false;
                        Console.Beep(1000, 100);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 150);
                        if (hp <= 0)
                        {
                            GameOver("Defeated by enemy fire!");
                        }
                    }
                }
            }
        }

        private void GameForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    Move(-1, 0);
                    break;
                case Keys.S:
                case Keys.Down:
                    Move(1, 0);
                    break;
                case Keys.A:
                case Keys.Left:
                    Move(0, -1);
                    break;
                case Keys.D:
                case Keys.Right:
                    Move(0, 1);
                    break;
                case Keys.E:
                case Keys.Space:
                    Interact();
                    break;
                case Keys.I:
                    ShowInventory();
                    break;
                case Keys.P:
                case Keys.Escape:
                    PauseGame();
                    break;
                case Keys.F:
                    Shoot();
                    break;
                case Keys.T:
                    ChangeTheme();
                    break;
            }
            
            gamePanel.Invalidate();
            UpdateLabels();
        }

        private void Shoot()
        {
            if (!hasWeapon)
            {
                Console.Beep(300, 100);
                MessageBox.Show("You need a WEAPON first!\n\nLook for 'W' on the map.", "No Weapon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (ammo <= 0)
            {
                Console.Beep(300, 100);
                return;
            }
            
            ammo--;
            score += (hasDoubleScore ? 10 : 5);
            Console.Beep(1500, 50);
            
            // Create bullets in 4 directions
            bullets.Add(new Bullet { X = pCol, Y = pRow, Dx = 0, Dy = -1 }); // Up
            bullets.Add(new Bullet { X = pCol, Y = pRow, Dx = 0, Dy = 1 });  // Down
            bullets.Add(new Bullet { X = pCol, Y = pRow, Dx = -1, Dy = 0 }); // Left
            bullets.Add(new Bullet { X = pCol, Y = pRow, Dx = 1, Dy = 0 });  // Right
        }

        private void ChangeTheme()
        {
            var themeNames = new List<string>(themes.Keys);
            int currentIndex = themeNames.IndexOf(currentTheme);
            currentIndex = (currentIndex + 1) % themeNames.Count;
            currentTheme = themeNames[currentIndex];
            
            var theme = themes[currentTheme];
            this.BackColor = theme.Background;
            gamePanel.BackColor = theme.Background;
            infoLabel.BackColor = theme.UI;
            statusLabel.BackColor = theme.UI;
            
            MessageBox.Show($"Theme changed to: {currentTheme}\n\nPress T to cycle themes", "Theme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private new void Move(int dr, int dc)
        {
            int nr = pRow + dr, nc = pCol + dc;
            if (nr < 0 || nr >= map.GetLength(0) || nc < 0 || nc >= map.GetLength(1)) return;
            
            char t = map[nr, nc];
            if (t == '#')
            {
                Console.Beep(200, 100);
                return;
            }
            
            if (boss != null && boss.Hp > 0 && nr == boss.Row && nc == boss.Col)
            {
                BossFight();
                return;
            }
            
            moveCount++;
            o2--;
            score += (hasDoubleScore ? 2 : 1);
            
            if (o2 <= 0)
            {
                GameOver("Oxygen depleted!");
                return;
            }
            
            pRow = nr;
            pCol = nc;
            UpdateLabels();
        }

        private void Interact()
        {
            char c = map[pRow, pCol];
            
            switch (c)
            {
                case 'K':
                    map[pRow, pCol] = ' ';
                    key = true;
                    itemsCollected++;
                    score += 50;
                    Console.Beep(800, 150);
                    MessageBox.Show("You found the KEY!", "Item Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'P':
                    RunPuzzleForCurrentLevel();
                    break;
                    
                case 'H':
                    map[pRow, pCol] = ' ';
                    hp = Math.Min(hp + 2, maxHp);
                    itemsCollected++;
                    score += 30;
                    Console.Beep(600, 100);
                    Console.Beep(800, 100);
                    MessageBox.Show("Health restored!", "Health Pack", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'O':
                    map[pRow, pCol] = ' ';
                    o2 = Math.Min(o2 + 40, 200);
                    itemsCollected++;
                    score += 30;
                    Console.Beep(500, 100);
                    Console.Beep(700, 100);
                    MessageBox.Show("Oxygen refilled!", "Oxygen Tank", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'F':
                    map[pRow, pCol] = ' ';
                    hasShield = true;
                    shieldHits = 3;
                    itemsCollected++;
                    score += (hasDoubleScore ? 200 : 100);
                    Console.Beep(1200, 100);
                    Console.Beep(1400, 150);
                    MessageBox.Show($"SHIELD activated! Protects from 3 hits!\n\nShield Status: {hasShield}\nShield Hits: {shieldHits}", "Power-up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'S':
                    map[pRow, pCol] = ' ';
                    hasSpeedBoost = true;
                    speedBoostTimer = 50;
                    itemsCollected++;
                    score += (hasDoubleScore ? 200 : 100);
                    Console.Beep(1000, 80);
                    Console.Beep(1200, 80);
                    Console.Beep(1400, 100);
                    MessageBox.Show("SPEED BOOST activated! Enemies move slower!", "Power-up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'W':
                    map[pRow, pCol] = ' ';
                    hasWeapon = true;
                    ammo = maxAmmo;
                    itemsCollected++;
                    score += 150;
                    Console.Beep(1500, 100);
                    Console.Beep(1700, 100);
                    Console.Beep(1900, 150);
                    MessageBox.Show($"WEAPON acquired!\n\nYou now have {ammo} bullets!\nPress F to shoot in all directions.", "Weapon!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'I':
                    map[pRow, pCol] = ' ';
                    isInvisible = true;
                    invisibilityTimer = 25;
                    itemsCollected++;
                    score += (hasDoubleScore ? 200 : 100);
                    Console.Beep(800, 80);
                    Console.Beep(600, 80);
                    Console.Beep(400, 100);
                    MessageBox.Show("INVISIBILITY activated! Enemies can't see you!", "Power-up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'L':
                    map[pRow, pCol] = ' ';
                    hp = maxHp;
                    itemsCollected++;
                    score += (hasDoubleScore ? 200 : 100);
                    Console.Beep(700, 100);
                    Console.Beep(900, 100);
                    Console.Beep(1100, 150);
                    MessageBox.Show("FULL RESTORE! HP restored to maximum!", "Power-up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'X':
                    map[pRow, pCol] = ' ';
                    hasDoubleScore = true;
                    doubleScoreTimer = 50;
                    itemsCollected++;
                    score += 100;
                    Console.Beep(1100, 100);
                    Console.Beep(1300, 100);
                    Console.Beep(1500, 150);
                    MessageBox.Show("DOUBLE SCORE activated! 2x points for 10 seconds!", "Power-up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
                case 'D':
                    if (key && puzzle)
                    {
                        Console.Beep(600, 100);
                        Console.Beep(800, 100);
                        Console.Beep(1000, 150);
                        doorOpened = true;
                        map[pRow, pCol] = ' ';
                        score += 50;
                        // D'yi açtığımızda checkpoint kaydediyoruz!
                        SaveCheckpoint();
                        MessageBox.Show(
                            $"✓ Door unlocked!\n\n" +
                            $"🔖 CHECKPOINT SAVED!\n" +
                            $"   Level: {currentLevel + 1}\n" +
                            $"   Position: ({pRow}, {pCol})\n\n" +
                            $"If you die in any level, you will respawn HERE!",
                            "Door & Checkpoint",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        Console.Beep(300, 200);
                        string msg = "Door is locked! You need: ";
                        if (!key) msg += "KEY ";
                        if (!puzzle) msg += "PUZZLE ";
                        MessageBox.Show(msg, "Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                    
                case 'E':
                    bool isBossLevel = (currentLevel == levels.Count - 1);
                    
                    if (!doorOpened)
                    {
                        Console.Beep(300, 200);
                        MessageBox.Show("EXIT LOCKED: Find and open the DOOR first!", "Exit Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    
                    if (isBossLevel)
                    {
                        if (boss != null && boss.Hp > 0)
                        {
                            Console.Beep(300, 200);
                            MessageBox.Show("The exit is sealed! Defeat the BOSS first!", "Exit Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                    else
                    {
                        if (!key || !puzzle)
                        {
                            Console.Beep(300, 200);
                            string needMsg = "Exit is locked! You still need: ";
                            if (!key) needMsg += "KEY ";
                            if (!puzzle) needMsg += "PUZZLE ";
                            MessageBox.Show(needMsg, "Exit Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                    
                    score += 200;
                    Console.Beep(800, 100);
                    Console.Beep(1000, 100);
                    Console.Beep(1200, 200);
                    LoadLevel(currentLevel + 1);
                    break;
            }
        }

        private void RunPuzzleForCurrentLevel()
        {
            string answer;
            
            switch (currentLevel)
            {
                case 0:
                    answer = ShowInputDialog(
                        "Level 1 Puzzle - LOG PARSING WITH REGEX",
                        "Security log received:\n" +
                        "[2024-12-26 14:32:15 ERROR 192.168.1.105] User:admin Login:FAILED Attempts:3\n\n" +
                        "Extract the IP address from this security log using regex.\n" +
                        "Hint: IP format is XXX.XXX.XXX.XXX (numbers separated by dots)");
                    answer = answer.Trim();
                    if (string.IsNullOrEmpty(answer)) return;
                    
                    // Regex validation
                    string text = "[2024-12-26 14:32:15 ERROR 192.168.1.105] User:admin Login:FAILED Attempts:3";
                    var match = System.Text.RegularExpressions.Regex.Match(text, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    string correctAnswer = match.Success ? match.Value : "192.168.1.105";
                    
                    if (answer == correctAnswer)
                    {
                        puzzle = true;
                        map[pRow, pCol] = ' ';
                        itemsCollected++;
                        score += 150;
                        Console.Beep(1000, 150);
                        MessageBox.Show(
                            $"Correct! IP Address extracted: {correctAnswer}\n\n" +
                            "Regex pattern: \\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\n\n" +
                            "Breakdown:\n" +
                            "\\d{1,3} → matches 1-3 digits\n" +
                            "\\. → matches literal dot\n" +
                            "Repeats 4 times for IP format",
                            "Puzzle Solved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 200);
                        MessageBox.Show("Wrong answer! Try to extract the IP address using regex.\nFormat: XXX.XXX.XXX.XXX", "Puzzle Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (hp <= 0)
                            GameOver("You failed the puzzle and died!");
                    }
                    break;
                
                case 1:
                    answer = ShowInputDialog(
                        "Level 2 Puzzle - EMAIL VALIDATION WITH REGEX",
                        "Which of these emails is valid?\n\n" +
                        "A) user@example.com\n" +
                        "B) invalid.email@\n" +
                        "C) @nouser.com\n" +
                        "D) test@@double.com\n\n" +
                        "Hint: Valid format → username@domain.extension\n" +
                        "Answer with A, B, C, or D");
                    answer = answer.Trim().ToUpper();
                    if (string.IsNullOrEmpty(answer)) return;
                    
                    if (answer == "A")
                    {
                        puzzle = true;
                        map[pRow, pCol] = ' ';
                        itemsCollected++;
                        score += 150;
                        Console.Beep(1000, 150);
                        MessageBox.Show(
                            "Correct! user@example.com is valid!\n\n" +
                            "Regex pattern: ^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$\n\n" +
                            "Why others are INVALID:\n" +
                            "B) invalid.email@ → Missing domain\n" +
                            "C) @nouser.com → Missing username\n" +
                            "D) test@@double.com → Double @@ invalid",
                            "Puzzle Solved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 200);
                        MessageBox.Show("Wrong answer! Think about the valid email format.", "Puzzle Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (hp <= 0)
                            GameOver("You failed the puzzle and died!");
                    }
                    break;
                
                case 2:
                    answer = ShowInputDialog(
                        "Level 3 Puzzle - URL VALIDATION WITH REGEX",
                        "Which of these URLs has a valid HTTPS format?\n\n" +
                        "A) https://github.com/user/repo\n" +
                        "B) http://example.com\n" +
                        "C) ftp://files.server.net\n" +
                        "D) https://test\n\n" +
                        "Hint: Look for HTTPS protocol + domain + path\n" +
                        "Answer with A, B, C, or D");
                    answer = answer.Trim().ToUpper();
                    if (string.IsNullOrEmpty(answer)) return;
                    
                    if (answer == "A")
                    {
                        puzzle = true;
                        map[pRow, pCol] = ' ';
                        itemsCollected++;
                        score += 150;
                        Console.Beep(1000, 150);
                        MessageBox.Show(
                            "Correct! https://github.com/user/repo is valid!\n\n" +
                            "Regex: ^https://[\\w.-]+\\.[a-z]{2,}/[\\w/.-]*$\n\n" +
                            "Why others are INVALID:\n" +
                            "B) http:// → HTTP not HTTPS\n" +
                            "C) ftp:// → FTP not HTTPS\n" +
                            "D) Missing domain extension & path",
                            "Puzzle Solved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 200);
                        MessageBox.Show("Wrong answer! Remember: HTTPS with proper domain and path.", "Puzzle Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (hp <= 0)
                            GameOver("You failed the puzzle and died!");
                    }
                    break;
                
                default:
                    answer = ShowInputDialog(
                        "ULTIMATE REGEX CHALLENGE - Password Strength",
                        "Which password is STRONG enough?\n" +
                        "(Must have: 8+ chars, uppercase, lowercase, digit, special char)\n\n" +
                        "A) Password123!\n" +
                        "B) pass123\n" +
                        "C) HELLO@2024\n" +
                        "D) test\n\n" +
                        "Answer with A, B, C, or D");
                    answer = answer.Trim().ToUpper();
                    if (string.IsNullOrEmpty(answer)) return;
                    
                    if (answer == "A")
                    {
                        puzzle = true;
                        map[pRow, pCol] = ' ';
                        itemsCollected++;
                        score += 200;
                        Console.Beep(1000, 150);
                        MessageBox.Show(
                            "Correct! Password123! is STRONG!\n\n" +
                            "Regex (lookaheads):\n" +
                            "(?=.*[a-z]) → Has lowercase\n" +
                            "(?=.*[A-Z]) → Has uppercase\n" +
                            "(?=.*\\d) → Has digit\n" +
                            "(?=.*[@$!%*?&#]) → Has special char\n" +
                            "{8,} → At least 8 characters\n\n" +
                            "Why others are WEAK:\n" +
                            "B) No uppercase/special\n" +
                            "C) No lowercase\n" +
                            "D) Too short",
                            "ULTIMATE CHALLENGE COMPLETED!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 200);
                        MessageBox.Show("Wrong answer! Check all password requirements carefully.", "Puzzle Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (hp <= 0)
                            GameOver("You failed the puzzle and died!");
                    }
                    break;
            }
        }

        private string ShowInputDialog(string title, string prompt)
        {
            using (var form = new Form())
            using (var label = new Label())
            using (var textBox = new TextBox())
            using (var buttonOk = new Button())
            using (var buttonCancel = new Button())
            {
                form.Text = title;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ClientSize = new Size(420, 180);

                label.Text = prompt;
                label.SetBounds(10, 10, 400, 90);
                label.TextAlign = ContentAlignment.MiddleLeft;

                textBox.SetBounds(10, 110, 400, 20);

                buttonOk.Text = "OK";
                buttonOk.DialogResult = DialogResult.OK;
                buttonOk.SetBounds(230, 135, 80, 30);

                buttonCancel.Text = "Cancel";
                buttonCancel.DialogResult = DialogResult.Cancel;
                buttonCancel.SetBounds(330, 135, 80, 30);

                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                return form.ShowDialog(this) == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }

        private void BossFight()
        {
            if (boss == null || boss.Hp <= 0)
            {
                boss = null; // Emin olmak için temizle
                return;
            }
            
            Console.Beep(400, 100);
            Console.Beep(300, 150);
            
            // Boss oyuncuya vuruyor - sadece oyuncuya hasar ver, boss'a değil
            // Oyuncuya 3 hasar ver
            if (hasShield && shieldHits > 0)
            {
                shieldHits--;
                if (shieldHits <= 0) hasShield = false;
            }
            else
            {
                hp -= 3; // Boss çok güçlü, 3 hasar veriyor!
            }
            
            score += 20;
            
            // Oyuncu öldü mü kontrol et
            if (hp <= 0)
            {
                boss = null; // Boss'u temizle
                GameOver("Defeated by the boss!");
                return; // GameOver timer'ı yönetiyor
            }
        }

        private void CheckCollisions()
        {
            if (isInvisible) return; // Enemies can't detect invisible player
            
            foreach (var enemy in enemies)
            {
                if (enemy.Row == pRow && enemy.Col == pCol)
                {
                    if (hasShield && shieldHits > 0)
                    {
                        shieldHits--;
                        Console.Beep(1000, 100);
                        Console.WriteLine($"[DEBUG] SHIELD BLOCKED! Remaining: {shieldHits} hits");
                        if (shieldHits == 0) hasShield = false;
                    }
                    else
                    {
                        hp--;
                        Console.Beep(300, 150);
                        Console.WriteLine($"[DEBUG] HIT! Shield status: {hasShield}, Hits: {shieldHits}, HP: {hp}");
                        if (hp <= 0)
                        {
                            GameOver("Killed by enemy!");
                            return;
                        }
                    }
                }
            }
            
            // Shooter enemy collision
            foreach (var shooter in shooterEnemies)
            {
                if (shooter.Row == pRow && shooter.Col == pCol)
                {
                    if (hasShield && shieldHits > 0)
                    {
                        shieldHits--;
                        Console.Beep(1000, 100);
                        if (shieldHits == 0) hasShield = false;
                    }
                    else
                    {
                        hp -= 2; // Shooter düşman 2 hasar veriyor
                        Console.Beep(300, 150);
                        if (hp <= 0)
                        {
                            GameOver("Killed by shooter enemy!");
                            return;
                        }
                    }
                }
            }
            
            if (boss != null && boss.Hp > 0 && boss.Row == pRow && boss.Col == pCol)
            {
                BossFight();
            }
        }

        private void ShowInventory()
        {
            string inv = $"=== INVENTORY ===\n\n";
            inv += $"Key: {(key ? "✓" : "✗")}\n";
            inv += $"Puzzle: {(puzzle ? "✓" : "✗")}\n";
            inv += $"Items Collected: {itemsCollected}\n";
            inv += $"Score: {score}\n";
            inv += $"Moves: {moveCount}\n";
            
            MessageBox.Show(inv, "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PauseGame()
        {
            gameTimer.Stop();
            var result = MessageBox.Show("Game Paused\n\nPress OK to continue\nPress Cancel to quit", "Pause", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                this.Close();
            }
            else
            {
                gameTimer.Start();
            }
        }

        private void Victory()
        {
            gameTimer.Stop();
            
            using (var endingForm = new Form())
            {
                endingForm.Text = "MISSION COMPLETE";
                endingForm.Size = new Size(600, 450);
                endingForm.StartPosition = FormStartPosition.CenterScreen;
                endingForm.BackColor = Color.FromArgb(20, 20, 30);
                endingForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                endingForm.MaximizeBox = false;
                endingForm.MinimizeBox = false;

                var titleLabel = new Label
                {
                    Text = "MISSION COMPLETE",
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    ForeColor = Color.Lime,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 60
                };
                endingForm.Controls.Add(titleLabel);

                var storyLabel = new Label
                {
                    Text = "EPILOGUE\n\n" +
                           "You defeated the guardian and escaped the device.\n" +
                           "As you emerge from the digital prison,\n" +
                           "you realize this was just one facility.\n\n" +
                           "Thousands of subjects remain trapped.\n" +
                           "Your fight has only just begun...\n\n" +
                           $"Final Score: {score}\n" +
                           $"Moves: {moveCount}\n" +
                           $"Items Collected: {itemsCollected}\n" +
                           $"Difficulty: {difficulty}",
                    Font = new Font("Consolas", 10),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.TopCenter,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20)
                };
                endingForm.Controls.Add(storyLabel);

                var exitBtn = new Button
                {
                    Text = "EXIT",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                exitBtn.Click += (s, e) => endingForm.Close();
                endingForm.Controls.Add(exitBtn);

                endingForm.ShowDialog();
            }
            
            this.Close();
        }

        private void GameOver(string reason)
        {
            deathCount++;
            gameTimer.Stop();
            
            var result = MessageBox.Show(
                $"YOU DIED!\n\n{reason}\n\n" +
                $"Score: {score}\n" +
                $"Level: {currentLevel + 1}\n" +
                $"Deaths: {deathCount}\n\n" +
                "Do you want to respawn at the last checkpoint?\n" +
                "(No will close the game)",
                "Game Over",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);
            
            if (result == DialogResult.Yes)
            {
                LoadCheckpoint();
                gameTimer.Start();
            }
            else
            {
                this.Close();
            }
        }

        private void UpdateLabels()
        {
            infoLabel.Text = $"Level: {currentLevel + 1}  |  Difficulty: {difficulty}  |  Score: {score}\n" +
                           $"Moves: {moveCount}  |  Items: {itemsCollected}";
            
            string hpBar = new string('█', hp) + new string('░', maxHp - hp);
            string o2Bar = o2 > 50 ? "████" : o2 > 25 ? "██░░" : "█░░░";
            
            // Power-up status display
            string powerups = "";
            if (hasShield) powerups += $"  🛡️({shieldHits})";
            if (hasSpeedBoost) powerups += $"  ⚡({speedBoostTimer})";
            if (isInvisible) powerups += $"  👻({invisibilityTimer})";
            if (hasDoubleScore) powerups += $"  2X({doubleScoreTimer})";
            
            string weaponStatus = hasWeapon ? "✓" : "✗";
            statusLabel.Text = $"HP: {hpBar} {hp}/{maxHp}  |  O2: {o2Bar} {o2}%  |  Weapon: {weaponStatus}  |  Ammo: {ammo}/{maxAmmo}  |  Key: {(key ? "✓" : "✗")}  |  Puzzle: {(puzzle ? "✓" : "✗")}  |  Door: {(doorOpened ? "✓" : "✗")}{powerups}";
            
            if (boss != null && boss.Hp > 0)
            {
                statusLabel.Text += $"\nBOSS HP: {new string('█', boss.Hp) + new string('░', 10 - boss.Hp)}  |  Theme: {currentTheme}";
            }
            else
            {
                statusLabel.Text += $"\nWASD: Move  |  E: Interact  |  F: Shoot  |  T: Theme  |  I: Inventory  |  P: Pause";
            }
        }

        private void SaveCheckpoint()
        {
            checkpointLevel = currentLevel;
            checkpointRow = pRow;
            checkpointCol = pCol;
            checkpointHp = hp;
            checkpointO2 = o2;
            checkpointScore = score;
            checkpointKey = key;
            checkpointPuzzle = puzzle;
            checkpointDoorOpened = doorOpened;
            checkpointMoves = moveCount;
            checkpointItems = itemsCollected;
            checkpointHasWeapon = hasWeapon;
            checkpointAmmo = ammo;
            
            // Map ve düşman durumlarını kaydet
            checkpointMap = (char[,])map.Clone();
            checkpointEnemies.Clear();
            foreach (var enemy in enemies)
            {
                checkpointEnemies.Add(new Enemy { Row = enemy.Row, Col = enemy.Col, MoveCounter = enemy.MoveCounter });
            }
            checkpointShooterEnemies.Clear();
            foreach (var shooter in shooterEnemies)
            {
                checkpointShooterEnemies.Add(new ShooterEnemy { Row = shooter.Row, Col = shooter.Col, ShootCounter = shooter.ShootCounter });
            }
            if (boss != null)
            {
                checkpointBoss = new Boss { Row = boss.Row, Col = boss.Col, Hp = boss.Hp, MoveCounter = boss.MoveCounter };
            }
            else
            {
                checkpointBoss = null;
            }
        }
        
        private void LoadCheckpoint()
        {
            // Checkpoint verilerini al
            int cpLevel = checkpointLevel;
            int cpRow = checkpointRow;
            int cpCol = checkpointCol;
            int cpHp = checkpointHp;
            int cpO2 = checkpointO2;
            int cpScore = checkpointScore;
            bool cpKey = checkpointKey;
            bool cpPuzzle = checkpointPuzzle;
            bool cpDoorOpened = checkpointDoorOpened;
            int cpMoves = checkpointMoves;
            int cpItems = checkpointItems;
            bool cpHasWeapon = checkpointHasWeapon;
            int cpAmmo = checkpointAmmo;

            // Level'ı yükle
            currentLevel = cpLevel;
            
            // Eğer checkpoint map'i varsa onu kullan, yoksa temiz map yükle
            if (checkpointMap != null)
            {
                map = (char[,])checkpointMap.Clone();
                
                // Kaydedilmiş düşmanları geri yükle
                enemies.Clear();
                foreach (var enemy in checkpointEnemies)
                {
                    enemies.Add(new Enemy { Row = enemy.Row, Col = enemy.Col, MoveCounter = enemy.MoveCounter });
                }
                
                shooterEnemies.Clear();
                foreach (var shooter in checkpointShooterEnemies)
                {
                    shooterEnemies.Add(new ShooterEnemy { Row = shooter.Row, Col = shooter.Col, ShootCounter = shooter.ShootCounter });
                }
                
                if (checkpointBoss != null)
                {
                    boss = new Boss { Row = checkpointBoss.Row, Col = checkpointBoss.Col, Hp = checkpointBoss.Hp, MoveCounter = checkpointBoss.MoveCounter };
                }
                else
                {
                    boss = null;
                }
            }
            else
            {
                // İlk kez checkpoint yoksa normal yükleme yap
                map = (char[,])levels[currentLevel].Clone();
                enemies.Clear();
                shooterEnemies.Clear();
                boss = null;
                
                for (int r = 0; r < map.GetLength(0); r++)
                {
                    for (int c = 0; c < map.GetLength(1); c++)
                    {
                        if (map[r, c] == '@')
                            map[r, c] = ' ';
                        else if (map[r, c] == 'M')
                        {
                            enemies.Add(new Enemy { Row = r, Col = c });
                            map[r, c] = ' ';
                        }
                        else if (map[r, c] == 'R')
                        {
                            shooterEnemies.Add(new ShooterEnemy { Row = r, Col = c });
                            map[r, c] = ' ';
                        }
                        else if (map[r, c] == 'B')
                        {
                            boss = new Boss { Row = r, Col = c };
                            map[r, c] = ' ';
                        }
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
            doorOpened = cpDoorOpened;
            moveCount = cpMoves;
            itemsCollected = cpItems;
            hasWeapon = cpHasWeapon;
            ammo = cpAmmo;
            
            bullets.Clear();
            enemyBullets.Clear();
            
            UpdateLabels();
            gamePanel.Invalidate();
            
            // Checkpoint yüklendiğinde bilgilendirme
            MessageBox.Show(
                $"🔖 RESPAWNED AT CHECKPOINT!\n\n" +
                $"   Level: {currentLevel + 1}\n" +
                $"   HP: {hp}/{maxHp}\n" +
                $"   O2: {o2}%\n" +
                $"   Score: {score}\n" +
                $"   Deaths: {deathCount}",
                "Checkpoint Loaded",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void GamePanel_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            
            int offsetX = (gamePanel.Width - (cols * CellSize)) / 2;
            int offsetY = (gamePanel.Height - (rows * CellSize)) / 2;
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int x = offsetX + c * CellSize;
                    int y = offsetY + r * CellSize;
                    
                    char ch = map[r, c];
                    Color color = Color.Black;
                    
                    // Draw player
                    if (r == pRow && c == pCol)
                    {
                        g.FillRectangle(Brushes.Cyan, x, y, CellSize, CellSize);
                        g.DrawString("@", new Font("Consolas", 18, FontStyle.Bold), Brushes.White, x + 5, y + 2);
                        continue;
                    }
                    
                    // Draw boss
                    if (boss != null && boss.Hp > 0 && r == boss.Row && c == boss.Col)
                    {
                        g.FillRectangle(Brushes.DarkRed, x, y, CellSize, CellSize);
                        g.DrawString("B", new Font("Consolas", 18, FontStyle.Bold), Brushes.Yellow, x + 5, y + 2);
                        continue;
                    }
                    
                    // Draw enemies
                    bool isEnemy = false;
                    foreach (var enemy in enemies)
                    {
                        if (r == enemy.Row && c == enemy.Col)
                        {
                            g.FillRectangle(Brushes.Red, x, y, CellSize, CellSize);
                            g.DrawString("M", new Font("Consolas", 18, FontStyle.Bold), Brushes.White, x + 5, y + 2);
                            isEnemy = true;
                            break;
                        }
                    }
                    if (isEnemy) continue;
                    
                    // Draw shooter enemies
                    bool isShooter = false;
                    foreach (var shooter in shooterEnemies)
                    {
                        if (r == shooter.Row && c == shooter.Col)
                        {
                            g.FillRectangle(Brushes.DarkOrange, x, y, CellSize, CellSize);
                            g.DrawString("R", new Font("Consolas", 18, FontStyle.Bold), Brushes.White, x + 5, y + 2);
                            isShooter = true;
                            break;
                        }
                    }
                    if (isShooter) continue;
                    
                    // Draw map elements
                    switch (ch)
                    {
                        case '#':
                            g.FillRectangle(Brushes.Gray, x, y, CellSize, CellSize);
                            g.DrawRectangle(Pens.DarkGray, x, y, CellSize, CellSize);
                            break;
                        case 'P':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("P", new Font("Consolas", 18, FontStyle.Bold), Brushes.Magenta, x + 5, y + 2);
                            break;
                        case 'K':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("K", new Font("Consolas", 18, FontStyle.Bold), Brushes.Yellow, x + 5, y + 2);
                            break;
                        case 'F':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("F", new Font("Consolas", 18, FontStyle.Bold), Brushes.Gold, x + 5, y + 2);
                            break;
                        case 'H':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("H", new Font("Consolas", 18, FontStyle.Bold), Brushes.Green, x + 5, y + 2);
                            break;
                        case 'W':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("W", new Font("Consolas", 18, FontStyle.Bold), Brushes.Orange, x + 5, y + 2);
                            break;
                        case 'O':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("O", new Font("Consolas", 18, FontStyle.Bold), Brushes.Cyan, x + 5, y + 2);
                            break;
                        case 'D':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("D", new Font("Consolas", 18, FontStyle.Bold), Brushes.DarkRed, x + 5, y + 2);
                            break;
                        case 'E':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("E", new Font("Consolas", 18, FontStyle.Bold), Brushes.Lime, x + 5, y + 2);
                            break;
                        case 'S':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("S", new Font("Consolas", 18, FontStyle.Bold), Brushes.Yellow, x + 5, y + 2);
                            break;
                        case 'I':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("I", new Font("Consolas", 18, FontStyle.Bold), Brushes.Gray, x + 5, y + 2);
                            break;
                        case 'L':
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            g.DrawString("L", new Font("Consolas", 18, FontStyle.Bold), Brushes.LightGreen, x + 5, y + 2);
                            break;
                        default:
                            g.FillRectangle(Brushes.Black, x, y, CellSize, CellSize);
                            break;
                    }
                }
            }
            
            // Draw bullets
            foreach (var bullet in bullets)
            {
                int bx = offsetX + (int)(bullet.X * CellSize) + CellSize / 2;
                int by = offsetY + (int)(bullet.Y * CellSize) + CellSize / 2;
                g.FillEllipse(Brushes.Yellow, bx - 4, by - 4, 8, 8);
            }
            
            // Draw enemy bullets
            foreach (var bullet in enemyBullets)
            {
                int bx = offsetX + (int)(bullet.X * CellSize) + CellSize / 2;
                int by = offsetY + (int)(bullet.Y * CellSize) + CellSize / 2;
                g.FillEllipse(Brushes.Red, bx - 5, by - 5, 10, 10);
                g.FillEllipse(Brushes.Orange, bx - 3, by - 3, 6, 6);
            }
        }
    }
}
