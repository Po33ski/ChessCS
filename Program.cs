using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCS
{

    //var b = new Board();


    class Program
    {
      
        static void Main(string[] args)
        {
            

            Console.WriteLine("Welcome in the new Chess Game");

            Console.WriteLine("Do you want to start the game?");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            var MyGame = new Board();

			MyGame.Show();
			// Here is the loop of the game:

			MyGame.ShowType(1, 1);
			MyGame.ShowType(0, 0);
			MyGame.ShowType(5, 5);
			while (MyGame.mat != true)
			{
				Console.WriteLine("first enter a number and then a character ");
				while (MyGame.turn != true)
				{
					MyGame.PlayWhite();
				}
				MyGame.turn = false;
				while (MyGame.turn != true)
				{
					MyGame.PlayBlack();
				}
				MyGame.turn = false;

				MyGame.Show();
			}

		}
    }
}


