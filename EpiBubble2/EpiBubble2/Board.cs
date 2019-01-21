using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace EpiBubble2
{
    class Board
    {
        public enum Color
        {
            Removed = -1,
            Blank = 0,
            Red = 1,
            Blue,
            Cyan,
            Yellow,
            Fushia,
            Lime
        }
        public enum Level
        {
            Easy = 0,
            Classic,
            Hard
        }

        public int XSIZE = 15;
        public int YSIZE = 17;
        public int MaxShoot = 6;
        public int Shoots = 0;
        public Color CurrentColor;
        public Color NextColor;
        Random random = new Random();
        public Color[,] board;
        Config config;
        public Level level;

        private void WriteWithForegroundColor(string msg, Color color)
        {
            switch (color)
            {
                case Color.Blank:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Color.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Color.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Color.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case Color.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Color.Fushia:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case Color.Lime:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DisplayBoard()
        {
            Console.Clear();
            for (int x = 0; x < XSIZE; x++)
            {
                Console.Write("     |");
                for (int y = 0; y < YSIZE; y++)
                    WriteWithForegroundColor("O", board[x, y]);
                Console.Write("|\n");
            }
            Console.Write("     ");
            WriteWithForegroundColor("O", NextColor);
            for (int i = 0; i < (YSIZE - 1) / 2; i++)
            {
                Console.Write(" ");
            }
            WriteWithForegroundColor("O\n", CurrentColor);
        }

        private void Addbubble(int x, int y, Color bubble)
        {
            board[x, y] = bubble;
        }

        private bool AddLine()
        {
            for (int x = XSIZE - 1; x > 0; x--)
                for (int y = 0; y < YSIZE; y++)
                    board[x, y] = board[x - 1, y];

            for (int y = 0; y < YSIZE; y++)
            {
                int num = random.Next(config.bubbleCount) + 1;
                board[0, y] = (Color)num;
            }

            return true;
        }

        private int DestroyRec(int x, int y, Color color)
        {
            int ret = 0;

            board[x, y] = Color.Removed;
            if (x + 1 < XSIZE && board[x + 1, y] == color)
                ret += DestroyRec(x + 1, y, color);
            if (x - 1 >= 0 && board[x - 1, y] == color)
                ret += DestroyRec(x, y + 1, color);
            if (y + 1 < YSIZE && board[x, y + 1] == color)
                ret += DestroyRec(x, y + 1, color);
            if (y - 1 >= 0 && board[x, y - 1] == color)
                ret += DestroyRec(x, y + 1, color);
            return ret + 1;
        }

        private void Destroy(int x, int y)
        {
            Color replace = board[x, y];
            if (DestroyRec(x, y, board[x, y]) > 3)
            {
                replace = Color.Blank;
            }

            for (x = 0; x < XSIZE; x++)
                for (y = 0; y < YSIZE; y++)
                    if (board[x, y] == Color.Removed)
                        board[x, y] = replace;
        }

        public bool Shoot(double angle, bool real)
        {
            bool negative = false;
            if (angle < 0)
            {
                negative = true;
                angle *= -1;
            }
            angle = (90 - angle) * (Math.PI / 180);
            double a = (double)YSIZE / 2;
            double b = Math.Tan(angle) * a;
            b = b < 1 ? 1 : b;

            double xMove, yMove;
            if (a > b)
            {
                xMove = a / b - (b / a - 1);
                yMove = 1;
            }
            else if (a < b)
            {
                xMove = 1;
                yMove = a / b - (b / a - 1);
            }
            else
            {
                xMove = 1;
                yMove = 0;
            }

            double xDouble = XSIZE;
            double yDouble = (double)YSIZE / 2;
            int x = Convert.ToInt32(xDouble) - 1;
            int y = Convert.ToInt32(yDouble) - 1;
            while (x >= 0 && y < YSIZE && y >= 0)
            {
                if (board[x, y] != Color.Blank)
                    break;
                xDouble -= xMove;
                if (negative)
                    yDouble -= yMove;
                else
                    yDouble += yMove;
                x = Convert.ToInt32(xDouble) - 1;
                y = Convert.ToInt32(yDouble) - 1;
            }
            if (x < 0 || y < 0 || y >= YSIZE || board[x, y] != Color.Blank)
            {
                xDouble += xMove;
                if (negative)
                    yDouble += yMove;
                else
                    yDouble -= yMove;
                x = Convert.ToInt32(xDouble) - 1;
                y = Convert.ToInt32(yDouble) - 1;
            }
            if (x == XSIZE)
            {
                Loose();
                return false;
            }

            board[x, y] = CurrentColor;

            if (real)
            {
                Console.WriteLine(Shoots);
                Console.WriteLine(MaxShoot);
                Shoots++;
                if (Shoots == MaxShoot)
                {
                    Shoots = 0;
                    AddLine();
                }
                CurrentColor = NextColor;
                int num = random.Next(6) + 1;
                NextColor = (Color)num;
            }
            //Destroy(x, y);
            return true;
        }

        public void Loose()
        {
            Console.WriteLine("Sorry You loose");
            Console.ReadLine();
        }

        public Board Copy()
        {
            Board ret = new Board(level);
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    ret.Addbubble(x, y, board[x, y]);
            ret.Shoots = Shoots;
            return ret;
        }

        public void LoadConfig()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(json);
            }
        }

        public void Build()
        {
            LoadConfig();
            XSIZE = config.rows;
            YSIZE = config.column;
            board = new Color[XSIZE, YSIZE];
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    board[x, y] = Color.Blank;
            for (int i = 0; i < 5; i++)
                AddLine();
            int num = random.Next(config.bubbleCount) + 1;
            CurrentColor = (Color)num;
            num = random.Next(config.bubbleCount) + 1;
            NextColor = (Color)num;
            switch (level)
            {
                case Level.Easy:
                    MaxShoot = config.easy;
                    break ;
                case Level.Classic:
                    MaxShoot = config.classic;
                    break ;
                case Level.Hard:
                    MaxShoot = config.hard;
                    break ;
            }
        }

        public Board(Level lvl)
        {
            level = lvl;
            Build();
        }

        public Board()
        {
        }
    }
}
