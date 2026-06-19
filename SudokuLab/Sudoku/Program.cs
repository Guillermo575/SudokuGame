using System;
using System.Text;
namespace Sudoku
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Tools.MainMenu.MostrarMenu();
        }
    }
}