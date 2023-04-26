using System;

namespace ZBC {
	
	public class Bitboard {
		
		// class variables //
		public ulong[] pieces;
		private char[] piece_symbol = {'p', 'b', 'n', 'r', 'q', 'k'};
		
		// structors //
		public Bitboard(){
			this.pieces = new ulong[] {
				0xFF000000000000, // white pawn
				0x2400000000000000, // white bishop
				0x4200000000000000, // white knight
				0x8100000000000000, // white rook
				0x1000000000000000, // white queen
				0x800000000000000, // white king
				0xFF00, // black pawn
				0x24, // black bishop
				0x42, // black knight
				0x81, // black rook
				0x10, // black queen
				0x8 // black king
			};
		} // end constructor Bitboard
		
		// utilities //
		public static ulong coordinates_to_bitboard(string coordinates){
			char[] fileIds = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};
			char[] coordinate_chararray;
			
			try{ 
				coordinate_chararray = coordinates.ToCharArray(); 
			}
			catch(Exception e){ 
				Console.WriteLine(e.Message + "\nError trying to convert coordinates to bitboard: " + coordinates);
				return 0x0;	
			}
			if(coordinate_chararray.Length != 2){ return 0x0; }
			
			// convert the characters to rank/file coordinates, and verify the number is in the correct range
			int[] rank_file = { 
				(int)coordinate_chararray[0] - 97, // 97 is the char id for 'a'
				(int)coordinate_chararray[1] - 49  // 49 is the char id for '0'
			};
			if(rank_file[0] < 0 | rank_file[0] > 7 | rank_file[1] < 0 | rank_file[1] > 7 ){ return 0x0; }
			
			// Now convert these coordinates to a bitboard.
			return (ulong)0x1 << - rank_file[0] + (8 - rank_file[1]) * 8 - 1;
		}
		
		public void move(string[] coordinates_string){
			if(coordinates_string.Length != 2){ return; }
			ulong[] coordinates = {
				coordinates_to_bitboard(coordinates_string[0]),
				coordinates_to_bitboard(coordinates_string[1])
			};
			
			// look for the intersection of the start coordinate and a piece
			int pieces_iterator = 0;
			while(pieces_iterator < this.pieces.Length){
				// if you find the intersection, delete the piece and then move it to the end destination.
				if((pieces[pieces_iterator] & coordinates[0]) != 0x0){
					pieces[pieces_iterator] = pieces[pieces_iterator] ^ coordinates[0];
					pieces[pieces_iterator] = pieces[pieces_iterator] ^ coordinates[1];
					return;
				}
				pieces_iterator++;
			}
		} // end move
		
		// display //
		private char[] ulong_to_chars(ulong bitmap){
			string bitmap_as_string = Convert.ToString(unchecked((long) bitmap), 2);
			
			// Add leading zeroes until we get to 64 elements
			while(bitmap_as_string.Length < 64){ 
				bitmap_as_string = "0" + bitmap_as_string; 
			}
			
			return bitmap_as_string.ToCharArray();
		} // end ulong_to_chars
		
		public override string ToString(){
			char[] return_bitboard = ulong_to_chars(0x0);
			
			// iterate over each piece's bitmap
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				
				// get the piece's symbol. capitalize if its white's
				char current_piece_symbol = this.piece_symbol[piece_iterator % 6];
				if(piece_iterator < 6){ current_piece_symbol = Char.ToUpper(current_piece_symbol); }
				
				// loop over the bits for the current bitmap and enter the symbols where there are 1's
				int biterator = 0;
				char[] current_bitmap = ulong_to_chars( this.pieces[piece_iterator] );
				while(biterator < current_bitmap.Length){
					if(current_bitmap[biterator] == '1'){
						return_bitboard[biterator] = current_piece_symbol;
					}
					biterator++;
				}
				piece_iterator++;
			}
			
			string return_string_base = String.Join("", return_bitboard);
			string return_string = "";

			int string_iterator = 7;
			while(string_iterator >= 0){
				return_string += return_string_base.Substring(string_iterator * 8, 8) + "\n";
				string_iterator--;
			}
			return " " + String.Join(" ", return_string.ToCharArray()).Replace('0', '-');
		} // end override ToString
		
	} // end class Bitboard
} // end namespace ZBC