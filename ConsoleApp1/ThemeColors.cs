using System;

namespace ConsoleApp1
{
    public class ThemeColors
    {
        public ConsoleColor Player { get; set; }
        public ConsoleColor Items { get; set; }
        public ConsoleColor Enemies { get; set; }
        public ConsoleColor Health { get; set; }
        public ConsoleColor Puzzle { get; set; }
        public ConsoleColor UI { get; set; }
        public ConsoleColor Walls { get; set; }
        
        public ThemeColors(ConsoleColor player, ConsoleColor items, ConsoleColor enemies, ConsoleColor health, ConsoleColor puzzle, ConsoleColor ui, ConsoleColor walls)
        {
            Player = player;
            Items = items;
            Enemies = enemies;
            Health = health;
            Puzzle = puzzle;
            UI = ui;
            Walls = walls;
        }
    }
}
