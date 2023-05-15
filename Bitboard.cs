using System;

namespace ZBC {
	
	public class Bitboard{
		
		// vars //
		
		/* Compass Rose
			nw      n     ne
			   +7  +8  +9
                 \  |  /
			w -1    0    +1 e
                 /  |  \
               -9  -8  -7
			sw      s     se
		*/
		
		private static char[] piece_symbol = {'p', 'n', 'b', 'r', 'q', 'k'};
		private static string[] piece_name = {"pawn", "knight", "bishop", "rook", "queen", "king"};
		
		// piece_deltas
		
		public ulong[] pieces;
		
		public Bitboard(string serialization_method = ""){
			this.pieces = new ulong[] { // color & piece ; ulongs are 64 bits, 1 bit per square. This represents piece positions.
				0xFFFF000000000000, // white
				0x000000000000FFFF, // black
				0x00FF00000000FF00, // pawn
				0x4200000000000042, // knight
				0x2400000000000024, // bishop
				0x8100000000000081, // rook
				0x1000000000000010, // queen
				0x0800000000000008, // king
			};
		} // end constructor Bitboard
		
		// utilities//		
		
		public static string UlongToString(ulong bitmap){
			string ulong_as_string = Convert.ToString(unchecked((long) bitmap), 2);
			while(ulong_as_string.Length < 64){ // Add leading zeroes until we get to 64 elements
				ulong_as_string = "0" + ulong_as_string; 
			}
			return ulong_as_string;
		} // end UlongToString
		
		public static ulong CoordinateToUlong(string coordinates){
			
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
			
		} // end CoordinateToUlong
		
		public static string DebugBitmap(ulong bitmap){
			string return_string_base = UlongToString(bitmap);
			string return_string = "";
			int string_iterator = 7;
			while(string_iterator >= 0){
				return_string += return_string_base.Substring(string_iterator * 8, 8) + "\n";
				string_iterator--;
			}
			return " " + String.Join(" ", return_string.ToCharArray());
		}
		
		// methods//
		
		public void PieceCreate(int[] piece_type, ulong coordinates){
			// piece_type[0] is either 0 or 1, indicates white or black
			// 2 <= piece_type[1] <= 7, indicates piece class
			if(piece_type[0] < 0 || piece_type [0] > 1){ return; }
			else if(piece_type[1] < 2 || piece_type[1] > this.pieces.Length){ return; }
			this.pieces[piece_type[0]] = this.pieces[piece_type[0]] | coordinates;
			this.pieces[piece_type[1]] = this.pieces[piece_type[1]] | coordinates;
			return;
		} // end PieceCreate
		
		public void PieceRemove(ulong coordinate){
			int pieces_iterator = 0;
			while(pieces_iterator < this.pieces.Length){
				if( (this.pieces[pieces_iterator] & coordinate) != 0x0){
					this.pieces[pieces_iterator] = this.pieces[pieces_iterator] ^ coordinate;
				}
				pieces_iterator++;
			}
			return;
		} // end PieceRemove
		
		public void PieceMove(ulong coordinate_start, ulong coordinate_end){
			int pieces_iterator = 0;
			while(pieces_iterator < this.pieces.Length){
				if( (this.pieces[pieces_iterator] & coordinate_start) != 0x0){
					this.pieces[pieces_iterator] = this.pieces[pieces_iterator] ^ coordinate_start;
					this.pieces[pieces_iterator] = this.pieces[pieces_iterator] | coordinate_end;
				}
				pieces_iterator++;
			}
			return;
		} // end PieceMove
		
		public Bitboard Copy(){
			Bitboard return_bitboard = new Bitboard();
			return_bitboard.pieces = new ulong[this.pieces.Length];
			
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				return_bitboard.pieces[piece_iterator] = this.pieces[piece_iterator];
				piece_iterator++;
			}
			return return_bitboard;
		} // end create_copy
		
		public override string ToString(){
			char[] return_bitboard = UlongToString(0x0).ToCharArray();
			
			int piece_iterator = 2; // start at pawns
			while(piece_iterator < this.pieces.Length){
				
				int color_iterator = 0;
				while(color_iterator < 2){
					
					// Do one color and one piece at a time. Makes it easy to determine the piece symbol
					char[] current_bitmap = UlongToString( this.pieces[piece_iterator] & this.pieces[color_iterator] ).ToCharArray();
					
					// Determine the piece symbol. piece_iterator determins the character, color_iterator determines case.
					char current_piece_symbol = Bitboard.piece_symbol[piece_iterator - 2];
					if(color_iterator == 0){ 
						current_piece_symbol = Char.ToUpper(current_piece_symbol); 
					}
					
					// Replace the appropriate indices with the current character
					int biterator = 0;
					while(biterator < current_bitmap.Length){
						
						if(current_bitmap[biterator] == '1'){
							return_bitboard[biterator] = current_piece_symbol;
						}
						biterator++;
					} // end of biterator loop
					color_iterator++;
				} // end of color iterator loop
				piece_iterator++;
			} // end of piece iterator loop
			
			string return_string_base = String.Join("", return_bitboard); // To let us work with substring
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