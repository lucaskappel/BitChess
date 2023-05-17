using System;
using System.IO;
using System.Collections.Generic;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			DebuggingMenu();
			
			
				
				
				/* Console.WriteLine(history.ToString());
				Console.WriteLine(history.Count);
				Console.WriteLine(history[history.Count - 1].ToString());
				
				int color = 0;
				int piece = 2;
				string[] input = Console.ReadLine().Split(' ');
				
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
				else if(input[0].Equals("debug")){
					history.Add(history[history.Count - 1].Copy());
					ulong temp = history[history.Count - 1].pieces[0] & history[history.Count - 1].pieces[2];
					temp = Bitboard.FillNorth(temp);
					history[history.Count - 1].pieces[color] |= temp;
					history[history.Count - 1].pieces[piece] |= temp;
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
				else if(input[0].Equals("fill") && Int32.TryParse(input[1], out color) && Int32.TryParse(input[2], out piece)){
					history.Add(history[history.Count - 1].Copy());
					ulong temp = history[history.Count - 1].pieces[color] & history[history.Count - 1].pieces[piece];
					temp = Bitboard.Fill(temp, input[3]);
					history[history.Count - 1].pieces[color] |= temp;
					history[history.Count - 1].pieces[piece] |= temp;
				}
			}*/

        } // end method Main
		
		// Debugging methods
		
		public static void DebuggingMenu(){
			string[] menu_options = {
				"coordinate test",
			};
			
			List<Bitboard> history = new List<Bitboard>();
			history.Add(new Bitboard());
			string input;
			
			while(true){
				// Menu loop
				Console.WriteLine("Main Menu. 'exit' to quit");
				input = Console.ReadLine();
				
				if(input.Equals("exit")){ break; }
				else if(input.Equals("coordinate test")){ DebugCoordinates(); }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
			}
		}
		
		public static void DebugCoordinates(){
			while(true){
				
				Console.WriteLine("--------\nCoordinate Debugging Menu. 'exit' to return\n--------");
				
				string input = Console.ReadLine();
				
				if(input.Equals("exit")){ break; }
				
				Console.WriteLine("--------");
				Console.WriteLine("Coordinate: " + input);
				
				int SquareIndex = Bitboard.SquareIndex(input);
				Console.WriteLine("Square Index: " + SquareIndex.ToString());
				
				int FileIndex = Bitboard.FileIndex(SquareIndex);
				Console.WriteLine("File Index: " + FileIndex.ToString());
				
				int RankIndex = Bitboard.RankIndex(SquareIndex);
				Console.WriteLine("Rank Index: " + RankIndex.ToString());
				
				int SquareIndexRecalculated = Bitboard.SquareIndex(FileIndex, RankIndex);
				Console.WriteLine("Square Index, recalculated: " + SquareIndexRecalculated.ToString());
				
				ulong BitmapCoordinate = Bitboard.SquareIndexToBitmap(SquareIndexRecalculated);
				Console.WriteLine("As a ulong bitmap: " + BitmapCoordinate.ToString());
				
				string BitmapVisualized = new string(Bitboard.UlongToVisualizedChars(BitmapCoordinate));
				Console.WriteLine("Ulong bitmap, visualized: \n");
				for(int i = 0; i < 8; i++){
					Console.WriteLine(BitmapVisualized.Substring(i*8, 8));
				}
				Console.WriteLine("--------");
			}
		}
		
    } // end class Engine
} // end namespace ZC