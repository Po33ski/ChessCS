using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCS
{

    using FigureDictionary = Dictionary<char, Board.Figure>;
    using BoardList = List<List<Board.Figure>>;
    using MemoryList = List<List<List<Board.Figure>>>;

    public partial class Board
    {
        public bool mat = false;
        public bool turn = false;
        private readonly List<List<char>> _temp;
        private readonly List<List<char>> _tempColor;

        private FigureDictionary _LostPieces = new Dictionary<char, Figure>();
        private BoardList _boardList = new BoardList();
        private MemoryList _memoryList = new MemoryList();
        public Board()
        {
            // 	my board initialization lists:
            _temp = new List<List<char>>
            {
                new List<char> { 'w', 's', 'h', 'q', 'k', 'h', 's', 'w' },
                new List<char> { 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' },
                new List<char> { 'w', 's', 'h', 'q', 'k', 'h', 's', 'w' }
            };

            _tempColor = new List<List<char>>
            {
                new List<char> { 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b' },
                new List<char> { 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                new List<char> { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
                new List<char> { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
            };


           // List<Figure> Row = new List<Figure>();
            
            for (int i = 0; i < 8; i++)
            {
                List<Figure> Row = new List<Figure>();
                for (int j = 0; j < 8; j++)
                {

                    try
                    {
                        char name = _temp[i][j];
                        SetFigure(name, ref Row);
                        Row.Last().figure_name = _temp[i][j];
                        Row.Last().figure_color = _tempColor[i][j];
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Smth went wrong, check your code");
                    }
                }
                _boardList.Add(Row);
                
            }
        }





        static void SetBeaten(Figure f, ref FigureDictionary lost_pieces)
        {
            var newfigure = f.CopyFigure();
            lost_pieces.Add( newfigure.figure_name,newfigure);
        }

        static void MatCheck(char color, ref bool mat)
        {
            if (color == 'w')
                Console.WriteLine("The white player wins!!");
            else if (color == 'b')
                Console.WriteLine("The black player wins!!");
            mat = true;
        }

        static void SaveInMemory(BoardList board, MemoryList MemoryList) 
        {
            MemoryList.Add(board);


        }

        // It's helper function to recognition the correct chess piece. It creates the pointer to the new figure without create a prototype. 
        static void SetFigure(char name, ref List<Figure> Row)
        {

            switch (name)
            {
                case 'x': { EmptyField newFigure = new EmptyField(); Row.Add(newFigure); } break;
                case 'p': { Pawn newFigure = new Pawn(); Row.Add(newFigure); } break;
                case 'w': { Rook newFigure = new Rook(); Row.Add(newFigure); } break;
                case 'h': { Bishop newFigure = new Bishop(); Row.Add(newFigure); } break;
                case 's': { Knight newFigure = new Knight(); Row.Add(newFigure); } break;
                case 'q': { Queen newFigure = new Queen(); Row.Add(newFigure); } break;
                case 'k': { King newFigure = new King(); Row.Add(newFigure); } break;
                default: { EmptyField newFigure = new EmptyField(); Row.Add(newFigure); }; break;
            }
            
        }

        private bool CheckColor(char lf, int nf, char col, char col2)
        {
            int i = nf - 1;
            int j = conv(lf) - 1;
            
            if (_boardList[i][j].figure_color == col && col != col2) return true; else return false;
            
        }
        
        static bool TestScope(int Num, char Let)
        {   
            if ((Num >= 1 || Num <= 8) &&
                (  Let == 'A' || Let == 'B' || Let == 'C' || Let == 'D'
                || Let == 'E' || Let == 'F' || Let == 'G' || Let == 'H'
                || Let == 'a' || Let == 'b' || Let == 'c' || Let == 'd'
                || Let == 'e' || Let == 'f' || Let == 'g' || Let == 'h'))
                return true;
            else return false;

        }
        // There are the functions that we can use the console controls.
        //They are almost the same, but I have separated them for convenience
        public void PlayBlack()
        {
            char color = 'b';
            char color2 = 'w';
            int Nf; int Ns;
            char Lf; char Ls;
            turn = false;

            toBegin:
            again:
            try
            {
                Console.WriteLine("What is your move (black)?");
                Nf = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(" | ");
                Lf = Convert.ToChar(Console.ReadLine());
                if (!TestScope(Nf, Lf)) throw new Exception("You want set your pawn out of board");
            }
            catch (Exception)
            {
                Console.WriteLine("You wrote something wrong, try again(scope)");
                goto again;
            }

            again2:
            try
            {
                Console.WriteLine("Where put you this pawn?");
                Ns = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(" | ");
                Ls = Convert.ToChar(Console.ReadLine());
                if (!TestScope(Nf, Lf)) throw new Exception("You want set your pawn out of board");
            }
            catch(Exception)
            {
                Console.WriteLine("You wrote something wrong, try again (scope)");
                goto again2;
            }
            
            if(CheckColor(Lf, Nf, color, color2))
            {
                turn = true;
                int i = Nf - 1;
                int j = conv(Lf) - 1;
                _boardList[i][j].Move(Lf, Ls, Nf, Ns, ref _boardList, ref _memoryList, ref _LostPieces, ref mat, ref turn);
            }
            else
            {
                Console.WriteLine("You wrote something wrong, try again(color)");
                goto toBegin;
            }

        }

        public void PlayWhite()
        {
            char color = 'w';
            char color2 = 'b';
            int Nf; int Ns;
            char Lf; char Ls;
            turn = false;

        toBegin:
        again:
            try
            {
                Console.WriteLine("What is your move (white)?");
                Nf = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(" | ");
                Lf = Convert.ToChar(Console.ReadLine());
                if (!TestScope(Nf, Lf)) throw new Exception("You want set your pawn out of board");
            }
            catch (Exception)
            {
                Console.WriteLine("You wrote something wrong, try again(scope)");
                goto again;
            }

        again2:
            try
            {
                Console.WriteLine("Where put you this pawn?");
                Ns = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(" | ");
                Ls = Convert.ToChar(Console.ReadLine());
                if (!TestScope(Nf, Lf)) throw new Exception("You want set your pawn out of board");
            }
            catch (Exception)
            {
                Console.WriteLine("You wrote something wrong, try again(scope)");
                goto again2;
            }

            if (CheckColor(Lf, Nf, color, color2))
            {
                turn = true;
                int i = Nf - 1;
                int j = conv(Lf) - 1;
                _boardList[i][j].Move(Lf, Ls, Nf, Ns, ref _boardList, ref _memoryList, ref _LostPieces,ref mat,ref turn);
            }
            else
            {
                Console.WriteLine("You wrote something wrong, try again(color)");
                goto toBegin;
            }

        }

        void DisplayLetter()
        {
            List<char> letter = new List<char>() { 'A','B','C','D','E','F','G','H'};
            Console.Write("  ");
            letter.ForEach(ch => Console.Write(ch + " "));
            Console.Write("\n");
        }



        public void Show()
        {
            int n = 1;
            if (_boardList.Count > 0)
            {

                Console.WriteLine("      Black ");
                foreach (var i in _boardList)
                {

                    Console.Write(" {0}", n); n++;
                    foreach (var j in i)
                    {
                        Console.Write(j.figure_name + " ");
                    }
                    Console.Write("\n");
                }
                DisplayLetter();
                Console.WriteLine("      White ");
            }
            else Console.WriteLine("The board is empty");
        }

        public void ShowType(int i, int j)
        {
            Console.WriteLine("The type of object on field {0}, {1} is: " + _boardList[i][j].GetType(), i, j);
        }




    }
}
