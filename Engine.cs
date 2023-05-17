using System;
using System.IO;
using System.Collections.Generic;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			DebuggingMenu();
        } // end method Main
		
		// Debugging methods
		
		public static void DebuggingMenu(){

			string input;
			
			while(true){
				// Menu loop
				Console.WriteLine("Main Menu");
				input = Console.ReadLine();
				
				if(input.Equals("exit")){ break; }
				else if(input.Equals("coordinate")){ DebugCoordinates(); }
				else if(input.Equals("board")){ DebugBoard(); }
				else if(input.Equals("bits")){ DebugBits(); }
				else if(input.Equals("transform")){ DebugTransforms(); }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
				else if(input.Equals("")){  }
			}
		}
		
		public static void DebugTransforms(){
			Bitboard playground = new Bitboard("test config");
			Console.WriteLine(playground.ToString());
			
			string input;
			while(true){
				
				input = Console.ReadLine();
				if(input.Equals("exit")){ break; }
				else if(input.Equals("mv")){
					playground.pieces[0] = Bitboard.TransformMirrorVertical(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformMirrorVertical(playground.pieces[2]);
				}
				else if(input.Equals("mh")){
					playground.pieces[0] = Bitboard.TransformMirrorHorizontal(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformMirrorHorizontal(playground.pieces[2]);
				}
				else if(input.Equals("md")){
					playground.pieces[0] = Bitboard.TransformMirrorDiagonal(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformMirrorDiagonal(playground.pieces[2]);
				}
				else if(input.Equals("ma")){
					playground.pieces[0] = Bitboard.TransformMirrorAntidiagonal(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformMirrorAntidiagonal(playground.pieces[2]);
				}
				else if(input.Equals("rl")){
					playground.pieces[0] = Bitboard.TransformRotate090(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformRotate090(playground.pieces[2]);
				}
				else if(input.Equals("rr")){
					playground.pieces[0] = Bitboard.TransformRotate270(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformRotate270(playground.pieces[2]);
				}
				else if(input.Equals("rs")){
					playground.pieces[0] = Bitboard.TransformRotate180(playground.pieces[0]);
					playground.pieces[2] = Bitboard.TransformRotate180(playground.pieces[2]);
				}
				Console.WriteLine(playground.ToString());
			}
		}
	
		public static void DebugBits(){
			Console.WriteLine("Bit playground");
			
			string[] ops = new string[]{"&", "^", "|", "~"};
			Bitboard playground = new Bitboard("test config");
			Console.WriteLine(playground.ToString());
			
			string[] input;
			while(true){
				
				input = Console.ReadLine().Split(' ');
				if(input[0].Equals("exit")){ break; }
				
				UInt64 arg2;
				ulong arg;
				if(input.Length > 1 && UInt64.TryParse(input[1], out arg2)){
					arg = (ulong)arg2;
					
					if(input[0].Equals("&")){
						Console.WriteLine("wah");
						playground.pieces[0] = playground.pieces[0] & arg;
						playground.pieces[2] = playground.pieces[2] & arg;
						Console.WriteLine(playground.ToString());
					}
					else if(input[0].Equals("^")){
						playground.pieces[0] = playground.pieces[0] ^ arg;
						playground.pieces[2] = playground.pieces[2] ^ arg;
						Console.WriteLine(playground.ToString());
					}
					else if(input[0].Equals("|")){
						playground.pieces[0] = playground.pieces[0] | arg;
						playground.pieces[2] = playground.pieces[2] | arg;
						Console.WriteLine(playground.ToString());
					}
					else if(input[0].Equals(">>")){
						playground.pieces[0] = playground.pieces[0] >> (int)arg;
						playground.pieces[2] = playground.pieces[2] >> (int)arg;
						Console.WriteLine(playground.ToString());
					}
					else if(input[0].Equals("<<")){
						playground.pieces[0] = playground.pieces[0] << (int)arg;
						playground.pieces[2] = playground.pieces[2] << (int)arg;
						Console.WriteLine(playground.ToString());
					}
				}
				else if(input[0].Equals("~")){
					playground.pieces[0] = ~playground.pieces[0];
					playground.pieces[2] = ~playground.pieces[2];
					Console.WriteLine(playground.ToString());
				}
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