namespace ConsoleApp1
{
    public class Boss
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Hp { get; set; } = 10;
        public int MoveCounter { get; set; } = 0;
        
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

            int newR = Row + dr;
            int newC = Col + dc;
            
            // Boss oyuncunun konumuna gitmeye çalışıyorsa, zaten yanındadır
            if (newR == pRow && newC == pCol)
            {
                return; // Boss oyuncunun yanında, hareket etmesin
            }
            
            if (newR > 0 && newR < map.GetLength(0) - 1 && newC > 0 && newC < map.GetLength(1) - 1)
            {
                // Boss duvarlara giremez ama itemlerin üstünden geçebilir
                if (map[newR, newC] != '#')
                {
                    Row = newR;
                    Col = newC;
                }
            }
        }
    }
}
