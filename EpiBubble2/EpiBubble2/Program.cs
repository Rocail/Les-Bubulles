using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EpiBubble2
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = null;
            Input input = new Input();
            while (board == null)
            {
                board = input.Menu();
            }
            while (board != null)
            {
                board.DisplayBoard();
                board = input.Read(board);
            }
        }
    }
}
