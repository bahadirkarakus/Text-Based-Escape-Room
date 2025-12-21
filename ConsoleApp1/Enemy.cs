using System;

namespace ConsoleApp1
{
    public class Enemy
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int MoveCounter { get; set; } = 0;
        private static Random rand = new Random();
        
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
}
