using System;
using System.IO;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			Bitboard a = new Bitboard();
			Console.WriteLine(a.ToString());
			
			while(true){
				string[] input = Console.ReadLine().Split(' ');
				int color = 0;
				int piece = 2;
				if(input[0].Equals("create") && Int32.TryParse(input[1], out color) && Int32.TryParse(input[2], out piece) ){
					a.PieceCreate(new int[] {color, piece}, Bitboard.CoordinateToUlong(input[3]));
				}
				else if(input[0].Equals("remove")){
					a.PieceRemove(Bitboard.CoordinateToUlong(input[1]));
				}
				else if(input[0].Equals("move")){
					a.PieceMove(Bitboard.CoordinateToUlong(input[1]), Bitboard.CoordinateToUlong(input[2]));
				}
				else if(input[0].Equals("dbc")){
					Console.WriteLine(Bitboard.CoordinateToUlong(input[1]));
				}
				Console.WriteLine(a.ToString());
			}

        } // end method Main
		
    } // end class Engine
} // end namespace ZC