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
        static public int conv(char l)
        {

           // if (l >= 97 && l <= 122)
              //  l = l - 32;
           
            switch (l)
            {
                case 'A': return 1; break;
                case 'B': return 2; break;
                case 'C': return 3; break;
                case 'D': return 4; break;
                case 'E': return 5; break;
                case 'F': return 6; break;
                case 'G': return 7; break;
                case 'H': return 8; break;
                case 'a': return 1; break;
                case 'b': return 2; break;
                case 'c': return 3; break;
                case 'd': return 4; break;
                case 'e': return 5; break;
                case 'f': return 6; break;
                case 'g': return 7; break;
                case 'h': return 8; break;
                default: return 0;
            }
        }


        public struct Transf
        {
            //public int Sf = 0; Ss = 0, iS = 0, jS = 0, iF = 0, jF = 0, llf = 0, lls = 0;
            public int Sf, Ss, iS, jS, iF, jF, llf, lls;
            public Transf(char lf, char ls, int nf, int ns) {
                this.Sf = (nf * 8) - 8 + Board.conv(lf);
                this.Ss = (ns * 8) - 8 + conv(ls);
                this.iF = nf - 1;
                this.jF = conv(lf) - 1;
                this.iS = ns - 1;
                this.jS = conv(ls) - 1;
                this.llf = conv(lf);
                this.lls = conv(ls);

            }
        }

        // this algorithms prevent a situation in which another pawn is between the starting and ending square:
        // this is the version for rook:
        static bool RookCheck(int lf, int ls, int nf, int ns, BoardList board, int iF, int jF)
        {
            //iF++; jF++;
            int t = 0;
            bool d = true, c = true;
            if (lf != ls) { t = (lf - ls); d = true; if (t > 0) t--; else t++; }
            else if (nf != ns) { t = (nf - ns); d = false; if (t > 0) t--; else t++; }
            if (d == true)
            {
                while (c != false || t != 0)
                {
                    if (t > 0) { iF--; t--; }
                    else { iF++; t++; }

                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }
            else if (d == false)
            {
                while (c != false || t != 0)
                {
                    if (t > 0) { jF--; t--; }
                    else { jF++; t++; }

                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }

            return c;
        }

        // this is the version for bishop:
        static bool BishopCheck(int lf, int ls, int nf, int ns, BoardList board, int iF, int jF)
        {
            int t, u;
            bool c = true;
            t = lf - ls; u = nf - ns;
            if (t > 0 && u > 0)
            {
                t--; u--;
                while (c != false || t != 0 || u != 0)
                {
                    iF--; t--; jF--; u--;
                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }
            else if (t > 0 && u < 0)
            {
                t--; u++;
                while (c != false || t != 0 || u != 0)
                {
                    iF--; t--; jF++; u++;
                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }
            else if (t < 0 && u > 0)
            {
                t++; u--;
                while (c != false || t != 0 || u != 0)
                {
                    iF++; t++; jF--; u--;
                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }
            else if (t < 0 && u < 0)
            {
                t++; u++;
                while (c != false || t != 0 || u != 0)
                {
                    iF++; t++; jF++; u++;
                    if (board[iF][jF].figure_name == 'x') c = true; else c = false;
                }
            }
            return c;
        }

        // this is the combined version for the queen:
        static bool QueenCheck(int lf, int ls, int nf, int ns, BoardList board, int iF, int jF)
        {
            if (lf == ls || nf == ns) return RookCheck(lf, ls, nf, ns, board, iF, jF);
            else if ((Math.Abs(ns - nf) == Math.Abs(ls - lf))) return BishopCheck(lf, ls, nf, ns, board, iF, jF);
            else return false;
        }

        ///////////////////////////////////////////////////////////////////////////// Class figure with child classes /////////////////////////////////////////////////////////////
        internal abstract class Figure
        {

            
            public char figure_name;
            public char figure_color;

            public int m = 0;
            public int counter() { return m++; }

            public Figure() { }


            public virtual Figure CopyFigure()
            {
                return this;
            }

            public virtual void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
                ref bool mat, ref bool turn) { }

        }

        internal class EmptyField : Figure
        {
            public EmptyField() : base() { }


            public new char figure_name = 'x';
            public new char figure_color = 'x';

        }

        internal class Pawn : Figure
        {
            public new char figure_name = 'p';

            public Pawn() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                 var Tr = new Transf(lf, ls, nf, ns);


                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;

                if ((Nf == 'p' && Ns != 'x') && ((Math.Abs(nf - ns) == 1) && (Math.Abs(Tr.llf - Tr.lls) == 1)))
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);
                    
                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if ((Nf == 'p' && Ns == 'x') && ((Math.Abs(Tr.Sf - Tr.Ss) == 8) || ((Math.Abs(Tr.Sf - Tr.Ss) == 16) && board[Tr.iS][Tr.jS].m == 0)))
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    
                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);
                   
                }
                else { Console.WriteLine("Your move is incorrect!"); turn = false; }
            }

        }

        internal class Rook : Figure
        {
            public new char  figure_name = 'w';
            
            public Rook() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                var Tr = new Transf(lf, ls, nf, ns);

                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;
                /*
                if(!(lf == ls || nf == ns) && ((Tr.iF - Tr.iS == 1 || Tr.iF - Tr.iS == -1) || (Tr.jF - Tr.jS == 1 || Tr.jF - Tr.jS == -1)))
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }*/
                if (!(lf == ls || nf == ns) || (!((Tr.iF - Tr.iS == 1 || Tr.iF - Tr.iS == -1) || (Tr.jF - Tr.jS == 1 || Tr.jF - Tr.jS == -1)) && (RookCheck(Tr.llf, Tr.lls, nf, ns, board, Tr.iF, Tr.jF) == false)))
                {
                    Console.WriteLine("Your move is incorrect(check)!"); turn = false;
                }
                else if (Nf == 'w' && Ns != 'x')
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if (Nf == 'w')
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;

                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);

                }
                else { Console.WriteLine("Your move is incorrect! (cond)"); turn = false; }
            }
        }

        internal class Bishop : Figure 
        {
            public new char figure_name = 'h';

            public Bishop() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                var Tr = new Transf(lf, ls, nf, ns);

                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;

                if (!(Math.Abs(ns - nf) == Math.Abs(Tr.lls - Tr.llf)) || (BishopCheck(Tr.llf, Tr.lls, nf, ns, board, Tr.iF, Tr.jF) == false))
                {
                    Console.WriteLine("Your move is incorrect!"); turn = false;
                }
                else if (Nf == 'h' && Ns != 'x')
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if (Nf == 'h' && Ns == 'x')
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;

                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);

                }
                else { Console.WriteLine("Your move is incorrect!"); turn = false; }
            }
        }

        internal class Knight : Figure
        {
            public new char figure_name = 'k';

            public Knight() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                var Tr = new Transf(lf, ls, nf, ns);

                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;


                if ((Nf == 's' && Ns != 'x') && (((nf == (ns + 2) || nf == (ns - 2)) &&
                    (Tr.llf == (Tr.lls + 1) || Tr.llf == (Tr.lls - 1))) || ((Tr.llf == (Tr.lls + 2) || Tr.llf == (Tr.lls - 2)) &&
                    (nf == (ns + 1) || (nf == (ns - 1))))))
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if (Nf == 's' && (((nf == (ns + 2) || nf == (ns - 2)) &&
                    (Tr.llf == (Tr.lls + 1) || Tr.llf == (Tr.lls - 1))) || ((Tr.llf == (Tr.lls + 2) || Tr.llf == (Tr.lls - 2)) &&
                    (nf == (ns + 1) || (nf == (ns - 1))))))
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;

                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);

                }
                else { Console.WriteLine("Your move is incorrect!"); turn = false; }
            }
        }


        internal class Queen : Figure
        {
            public new char figure_name = 'q';

            public Queen() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                var Tr = new Transf(lf, ls, nf, ns);

                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;

                if ((Nf == 'q' && Ns != 'x') && ((Math.Abs(ns - nf) == Math.Abs(Tr.lls - Tr.llf) 
                    || (Nf == 'q' && (Tr.llf == Tr.lls || nf == ns)) 
                    || (QueenCheck(Tr.llf, Tr.lls, nf, ns, board, Tr.iF, Tr.jF) == false))))
                {
                    Console.WriteLine("Your move is incorrect!"); turn = false;
                }
                else if (Nf == 'q' && Ns != 'x')
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if (Nf == 'q')
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;

                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);

                }
                else { Console.WriteLine("Your move is incorrect!"); turn = false; }
            }
        }

        internal class King : Figure 
        {
            public new char figure_name = 'k';

            public King() : base() { }

            public override void Move(char lf, char ls, int nf, int ns,
            ref BoardList board,
            ref MemoryList memory_of_game,
            ref FigureDictionary beaten,
            ref bool mat, ref bool turn)
            {
                var Tr = new Transf(lf, ls, nf, ns);

                char Nf = board[Tr.iF][Tr.jF].figure_name;
                char Ns = board[Tr.iS][Tr.jS].figure_name;


                if ((Nf == 'k' && Ns != 'x') && ((Math.Abs(ns - nf) == 1) || (Math.Abs(Tr.lls - Tr.llf) == 1) || (Math.Abs(Tr.lls - Tr.llf) / 9 == 1)))
                {
                    Console.WriteLine("Player {0} loses {1}", board[Tr.iS][Tr.jS].figure_color, board[Tr.iS][Tr.jS].figure_name);

                    SetBeaten(board[Tr.iS][Tr.jS], ref beaten);
                    if (Ns == 'k') MatCheck(board[Tr.iF][Tr.jF].figure_color, ref mat);
                    Figure temp = new EmptyField();
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;
                    //board[Tr.iF][Tr.jF].figure_name = 'x';

                    SaveInMemory(board, memory_of_game);
                }
                else if (Nf == 'k' && ((Math.Abs(ns - nf) == 1) || (Math.Abs(Tr.lls - Tr.llf) == 1) || (Math.Abs(Tr.lls - Tr.llf) / 9 == 1)))
                {

                    Figure temp = new EmptyField();
                    temp = board[Tr.iS][Tr.jS];
                    board[Tr.iS][Tr.jS] = board[Tr.iF][Tr.jF];
                    board[Tr.iF][Tr.jF] = temp;

                    board[Tr.iF][Tr.jF].figure_color = 'x';
                    board[Tr.iS][Tr.jS].counter();
                    SaveInMemory(board, memory_of_game);

                }
                else { Console.WriteLine("Your move is incorrect!"); turn = false; }
            }
        }
    }
}
