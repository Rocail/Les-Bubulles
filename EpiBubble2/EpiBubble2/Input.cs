using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace EpiBubble2
{
    class Input
    {
        public Board Menu()
        {
            Console.Clear();
            Console.WriteLine("Play New Game: 1");
            Console.WriteLine("Play Old Game: 2");
            Console.Write("Enter a number: ");
            int nb;
            string line = Console.ReadLine();
            while (!Int32.TryParse(line, out nb) || nb < 1 || nb > 2)
            {
                Console.Clear();
                Console.WriteLine("Play New Game: 1");
                Console.WriteLine("Play Old Game: 2");
                Console.Write("Enter a number between 1 and 2: ");
                line = Console.ReadLine();
            }
            Console.Clear();
            switch (nb)
            {
                case 1:
                    return NewGame();
                case 2:
                    return OldGame();
            }
            return null;
        }

        public Board OldGame()
        {
            using (StreamReader r = new StreamReader("save.txt"))
            {
                string json = r.ReadToEnd();
                Board board = JsonConvert.DeserializeObject<Board>(json);
                board.LoadConfig();
                return board;
            }
        }

        public Board NewGame()
        {
            Console.Clear();
            Console.WriteLine("Play Easy: 1");
            Console.WriteLine("Play Classic: 2");
            Console.WriteLine("Play Hard: 3");
            int nb;
            string line = Console.ReadLine();
            while (!Int32.TryParse(line, out nb) || nb < 1 || nb > 3)
            {
                Console.Clear();
                Console.WriteLine("Play Easy: 1");
                Console.WriteLine("Play Classic: 2");
                Console.WriteLine("Play Hard: 3");
                Console.Write("Enter a number between 1 and 3: ");
                line = Console.ReadLine();
            }
            switch (nb)
            {
                case 1:
                    return new Board(Board.Level.Easy);
                case 2:
                    return new Board(Board.Level.Classic);
                case 3:
                    return new Board(Board.Level.Hard);
            }
            return null;
        }

        public void Save(Board board)
        {
            using (StreamWriter file = File.CreateText("save.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, board);
            }
        }

        public Board Read(Board board)
        {
            double angle = 0;
            bool parse = false;

            while (!parse)
            {
                Console.Write("Enter an angle: ");
                string line = Console.ReadLine();
                if (double.TryParse(line, out angle) && angle > -80 && angle < 80)
                {
                    board.Shoot(angle, true);
                    return board;
                }
                else if (line.ToLower() == "save")
                {
                    Save(board);
                }
                else if (line.ToLower() == "quit")
                {
                    Console.Write("Are you sure you want to quit the game? [Yes/No]");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        return null;
                    }
                    else
                    {
                        continue ;
                    }
                }
            }
            return board;
            while (!parse)
            {
                string line = "";
                char key = (char)0;
                while (key != 13)
                {
                    key = Console.ReadKey().KeyChar;
                    line += key;
                    Board tmp = board.Copy();
                    parse = double.TryParse(line, out angle);
                    if (parse && angle > -80 && angle < 80)
                    {
                        tmp.Shoot(angle, false);
                        tmp.DisplayBoard();
                    }
                    else
                    {
                        board.DisplayBoard();
                        parse = false;
                    }
                }
            }

            if (!board.Shoot(angle, true))
                return null;
            return board;
        }
    }
}
