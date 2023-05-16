using System;
using System.IO;
using System.Collections.Generic;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			List<Bitboard> history = new List<Bitboard>();
			history.Add(new Bitboard());
			
			
			while(true){
				Console.WriteLine(history.ToString());
				Console.WriteLine(history.Count);
				Console.WriteLine(history[history.Count - 1].ToString());
				
				string[] input = Console.ReadLine().Split(' ');
				int color = 0;
				int piece = 2;
				if(input[0].Equals("create") && Int32.TryParse(input[1], out color) && Int32.TryParse(input[2], out piece) ){
					history.Add(history[history.Count - 1].Copy());
					history[history.Count - 1].PieceCreate(new int[] {color, piece}, Bitboard.CoordinateToUlong(input[3]));
				}
				else if(input[0].Equals("remove")){
					history.Add(history[history.Count - 1].Copy());
					history[history.Count - 1].PieceRemove(Bitboard.CoordinateToUlong(input[1]));
				}
				else if(input[0].Equals("move")){
					history.Add(history[history.Count - 1].Copy());
					history[history.Count - 1].PieceMove(Bitboard.CoordinateToUlong(input[1]), Bitboard.CoordinateToUlong(input[2]));
				}
				else if(input[0].Equals("dbc")){
					Console.WriteLine(Bitboard.CoordinateToUlong(input[1]));
				}
				else if(input[0].Equals("clear")){
					history.Add(history[history.Count - 1].Copy());
					history[history.Count - 1].ClearBoard();
				}
				else if(input[0].Equals("undo")){
					history.RemoveAt(history.Count - 1);
				}
				else if(input[0].Equals("end")){
					break;
				}
			}

        } // end method Main
		
    } // end class Engine
} // end namespace ZC