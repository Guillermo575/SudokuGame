using System;
using System.Text;
namespace SudokuML
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Tools.MainMenu.MostrarMenu();
            //var sudokuRapido = new Sudoku.SudokuGenerator(3, 3, QuickFunction: true);
            //var sudoku4x4 = new Sudoku.SudokuGenerator(4, 4, QuickFunction: true);
            //var sudoku5x4 = new Sudoku.SudokuGenerator(5, 4, QuickFunction: true);
            //var sudoku5x5 = new Sudoku.SudokuGenerator(5, 5, QuickFunction: true);
            //var sudoku6x6 = new Sudoku.SudokuGenerator(6, 6, QuickFunction: true);
        }
    }
}