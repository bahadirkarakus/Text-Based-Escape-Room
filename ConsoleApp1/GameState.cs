using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public static class GameState
    {
        // Game state
        public static int CurrentLevel = 0;
        public static char[,] Map = null!;
        public static int PRow = 1, PCol = 1, MaxHp = 5, Hp = 5, O2 = 100, Score = 0;
        public static bool Key = false, Puzzle = false, DoorOpened = false, Run = true;
        public static string Difficulty = "NORMAL";
        public static int MoveCount = 0, ItemsCollected = 0;
        public static List<string> Achievements = new List<string>();
        public static List<Enemy> Enemies = new List<Enemy>();
        public static Boss? Boss = null;
        public static Random Rand = new Random();
        
        // Checkpoint system
        public static int CheckpointLevel = 0;
        public static int CheckpointRow = 1, CheckpointCol = 1;
        public static int CheckpointHp = 5, CheckpointO2 = 100;
        public static int CheckpointScore = 0;
        public static bool CheckpointKey = false, CheckpointPuzzle = false, CheckpointDoorOpened = false;
        public static int CheckpointMoves = 0, CheckpointItems = 0;
        public static int DeathCount = 0;
        
        // Power-ups
        public static bool ShieldActive = false;
        public static bool SpeedBoostActive = false;
        public static int SpeedBoostTurns = 0;
        
        // Theme system
        public static string CurrentTheme = "CLASSIC";
        public static Dictionary<string, ThemeColors> Themes = new Dictionary<string, ThemeColors>
        {
            { "CLASSIC", new ThemeColors(ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.DarkRed) },
            { "MATRIX", new ThemeColors(ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.DarkRed) },
            { "OCEAN", new ThemeColors(ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Blue, ConsoleColor.DarkBlue) },
            { "FIRE", new ThemeColors(ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.DarkRed) },
            { "DARK", new ThemeColors(ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Black) },
            { "NEON", new ThemeColors(ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.DarkMagenta) }
        };
    }
}
