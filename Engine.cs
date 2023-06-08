using System;
using System.IO;
using System.Collections.Generic;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			DebuggingMenu();
        } // end Main
		
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
			}
		} // end DebuggingMenu
		
		public static void DebugTransforms(){
			Bitboard playground = new Bitboard("r");
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
		} // end DebugTransforms
	
		public static void DebugBits(){
			Console.WriteLine("Bit playground");
			
			string[] ops = new string[]{"&", "^", "|", "~"};
			Bitboard playground = new Bitboard("r");
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
				Console.WriteLine("Turn " + history.Count.ToString() + "\n" + history[history.Count - 1].ToString());
				
				input = Console.ReadLine().Split(' ');
				if(input[0].Equals("exit")){ break; }
				
				history.Add(history[history.Count - 1].Copy()); // Copy the latest gamestate; it is from here we make our next move.
				
				if(input[0].Equals("undo")){
					history.RemoveAt(history.Count - 1);
					history.RemoveAt(history.Count - 1);
				}
				else if(input[0].Equals("create")){
					history[history.Count - 1].PieceCreate(input[1], input[2], input[3]);
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
				else if(input[0].Equals("reset")){
					if(input.Length > 1){ history[history.Count-1] = new Bitboard(input[1]); }
					else{ history[history.Count-1] = new Bitboard(); }
				}
				else if(input[0].Equals("fill")){
					if(input.Length != 4){ history.RemoveAt(history.Count - 1); }
					else{
						int index1 = Array.IndexOf(Bitboard.piece_name, input[1]);
						int index2 = Array.IndexOf(Bitboard.piece_name, input[2]);
						if(index1 < 0 || index2 < 0){ Console.WriteLine("Invalid piece at fill"); }
						else if(Array.IndexOf(Bitboard.compass_rose_index, input[3]) < 0) { Console.WriteLine("Invalid location at fill"); }
						else{
						
							ulong working_set = history[history.Count-1].pieces[index1] & history[history.Count-1].pieces[index2];
							
							int fill_direction = Bitboard.compass_rose[Array.IndexOf(Bitboard.compass_rose_index, input[3])];
							
							history[history.Count-1].pieces[index1] |= Bitboard.DumbFill(working_set, fill_direction);
							history[history.Count-1].pieces[index2] = history[history.Count-1].pieces[index1];
						}
					}
				}
				else if(input[0].Equals("block")){
					if(input.Length != 4){ history.RemoveAt(history.Count - 1); }
					else{
						int index1 = Array.IndexOf(Bitboard.piece_name, input[1]);
						int index2 = Array.IndexOf(Bitboard.piece_name, input[2]);
						if(index1 < 0 || index2 < 0){ Console.WriteLine("Invalid piece at fill"); }
						else if(Array.IndexOf(Bitboard.compass_rose_index, input[3]) < 0) { Console.WriteLine("Invalid location at fill"); }
						else{
						
							ulong working_set = history[history.Count-1].pieces[index1] & history[history.Count-1].pieces[index2];
							
							int block_direction = Bitboard.compass_rose[Array.IndexOf(Bitboard.compass_rose_index, input[3])];
							
							history[history.Count-1].pieces[index1] = Bitboard.DumbBlock(working_set, block_direction);
							history[history.Count-1].pieces[index2] = history[history.Count-1].pieces[index1];
						}
					}
				}
				else if(input[0].Equals("span")){
					if(input.Length != 4){ history.RemoveAt(history.Count - 1); }
					else{
						int index1 = Array.IndexOf(Bitboard.piece_name, input[1]);
						int index2 = Array.IndexOf(Bitboard.piece_name, input[2]);
						if(index1 < 0 || index2 < 0){ Console.WriteLine("Invalid piece at fill"); }
						else if(Array.IndexOf(Bitboard.compass_rose_index, input[3]) < 0) { Console.WriteLine("Invalid location at fill"); }
						else{
						
							ulong working_set = history[history.Count-1].pieces[index1] & history[history.Count-1].pieces[index2];
							
							int span_direction = Bitboard.compass_rose[Array.IndexOf(Bitboard.compass_rose_index, input[3])];
							
							history[history.Count-1].pieces[index1] = Bitboard.DumbSpan(working_set, span_direction);
							history[history.Count-1].pieces[index2] = history[history.Count-1].pieces[index1];
						}
					}
				}
				else if(input[0].Equals("slide")){
					if(input.Length != 4){ history.RemoveAt(history.Count - 1); }
					else{
						int index1 = Array.IndexOf(Bitboard.piece_name, input[1]);
						int index2 = Array.IndexOf(Bitboard.piece_name, input[2]);
						if(index1 < 0 || index2 < 0){ Console.WriteLine("Invalid piece at slide"); }
						else if(Array.IndexOf(Bitboard.compass_rose_index, input[3]) < 0) { Console.WriteLine("Invalid location at slide"); }
						else{
						
							ulong working_set = history[history.Count-1].pieces[index1] & history[history.Count-1].pieces[index2];
							int slide_direction = Bitboard.compass_rose[Array.IndexOf(Bitboard.compass_rose_index, input[3])];
							
							history[history.Count-1].pieces[index2] = history[history.Count-1].DumbSlidingAttack(working_set, slide_direction);
							history[history.Count-1].pieces[index1] |= history[history.Count-1].pieces[index2];
							
							if(index1 == 0){ history[history.Count-1].pieces[1] &= ~history[history.Count-1].pieces[index2]; }
							else if(index1 == 1){ history[history.Count-1].pieces[0] &= ~history[history.Count-1].pieces[index2]; }
							
							for(int i=2; i<history[history.Count-1].pieces.Length; i++){
								if(i != index2){ history[history.Count-1].pieces[i] &= ~history[history.Count-1].pieces[index2]; }
							}
						}
					}
				}
			} // end while loop
			
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