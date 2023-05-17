using System;
using System.IO;
using System.Collections.Generic;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			DebuggingMenu();
			
			
				
				
				/*
				
				
				
				
				else if(input[0].Equals("debug")){
					history.Add(history[history.Count - 1].Copy());
					ulong temp = history[history.Count - 1].pieces[0] & history[history.Count - 1].pieces[2];
					temp = Bitboard.FillNorth(temp);
					history[history.Count - 1].pieces[color] |= temp;
					history[history.Count - 1].pieces[piece] |= temp;
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
				"coordinate",
				"board",
			};
			
			string input;
			
			while(true){
				// Menu loop
				Console.WriteLine("Main Menu");
				input = Console.ReadLine();
				
				if(input.Equals("exit")){ break; }
				else if(input.Equals("coordinate")){ DebugCoordinates(); }
				else if(input.Equals("board")){ DebugBoard(); }
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
		
		public static void DebugBits(){
			Console.WriteLine("Bit playground");
			string[] input;
			while(true){
				input = Console.ReadLine().Split(' ');
				if(input[0].Equals("exit")){ break; }
				Console.WriteLine("");
			}
		} // end DebugBits
		
		public static void DebugBoard(){
			Console.WriteLine("Board testing");
			
			string[] input;
			List<Bitboard> history = new List<Bitboard>();
			history.Add(new Bitboard());
			
			while(true){
				Console.WriteLine(history.Count.ToString() + "\n" + history[history.Count - 1].ToString());
				
				input = Console.ReadLine().Split(' ');
				if(input[0].Equals("exit")){ break; }
				
				history.Add(history[history.Count - 1].Copy()); // Copy the latest gamestate; it is from here we make our next move.
				
				if(input[0].Equals("undo")){
					history.RemoveAt(history.Count - 1);
					history.RemoveAt(history.Count - 1);
				}
				else if(input[0].Equals("create")){
					history[history.Count - 1].PieceCreate(input[1], input[2], input[3]);
					Console.WriteLine((0x8100000000000081).ToString());
				}
				else if(input[0].Equals("remove")){
					history[history.Count - 1].PieceRemove(input[1]);
				}
				else if(input[0].Equals("move")){
					history[history.Count - 1].PieceMove(input[1], input[2]);
				}
				else if(input[0].Equals("clear")){
					history[history.Count - 1].ClearBoard();
				}
			}
		} // end DebugBoard
		
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
				
				ulong BitmapCoordinate = Bitboard.SquareIndexToUlong(SquareIndexRecalculated);
				Console.WriteLine("As a ulong bitmap: " + BitmapCoordinate.ToString());
				
				string BitmapVisualized = new string(Bitboard.UlongToVisualBoardOrder(BitmapCoordinate));
				Console.WriteLine("Ulong bitmap, visualized: \n");
				for(int i = 0; i < 8; i++){
					Console.WriteLine(BitmapVisualized.Substring(i*8, 8));
				}
				Console.WriteLine("--------");
			}
		} // end DebugCoordinates
		
    } // end class Engine
} // end namespace ZC