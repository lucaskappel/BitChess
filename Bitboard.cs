using System;

namespace ZBC {
	
	public class Bitboard{
		
		// vars //
		
/* Compass Rose

nw    n    ne
  +7  +8  +9
    \  |  /
w  -1  0  +1  e
    /  |  \
  -9  -8  -7
sw    s    se

*/
		
		private static char[] piece_symbol = {'W', 'b', 'p', 'n', 'b', 'r', 'q', 'k'};
		private static string[] piece_name = {"White", "black", "pawn", "knight", "bishop", "rook", "queen", "king"};
		private static string[] compass_rose_index_book = {"n", "ne", "e", "se", "s", "sw", "w", "nw"};
		private static int[] compass_rose = {8, 9, 1, -7, -8, -9, -1, 7};
		
		// piece_deltas
		
		public ulong[] pieces;
		
		public Bitboard(){
			this.pieces = new ulong[] { // little-endian rank-file mapping
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
		} // end static DebugBitmap
		
		// fills
		
		public static ulong Fill(ulong bitmap, string direction){
			int compass_rose_index = Array.IndexOf(compass_rose_index_book, direction);
			if(compass_rose_index < 0){ 
				Console.WriteLine("compass rose direction was invalid: " + direction);
				return 0x0; 
			}
			int direction_shift = compass_rose[compass_rose_index];
			Console.WriteLine(DebugBitmap(bitmap));
			bitmap |= (bitmap << direction_shift * 1);
			Console.WriteLine(DebugBitmap(bitmap));
			bitmap |= (bitmap << direction_shift * 2);
			Console.WriteLine(DebugBitmap(bitmap));
			bitmap |= (bitmap << direction_shift * 4);
			Console.WriteLine(DebugBitmap(bitmap));
			return bitmap;
		} // end static Fill
		
		public static ulong FillNorth(ulong bitmap){
			bitmap |= (bitmap <<  8);
			bitmap |= (bitmap << 16);
			bitmap |= (bitmap << 32);
			return bitmap;
		} // end static FillNorth
		
		public static ulong FillSouth(ulong bitmap){
			bitmap |= (bitmap >>  8);
			bitmap |= (bitmap >> 16);
			bitmap |= (bitmap >> 32);
			return bitmap;
		} // end static FillSouth
		
		// transforms (3 digit number is a reference to degrees)
		
		public static ulong TransformMirror000(ulong bitmap){
			/* 
			mirror bitmap about the center ranks, along the X axis
			Parallel prefix approach
			reverses all eight bytes
			performs little <-> big endian transformation
			*/
			const ulong k1 = 0x00FF00FF00FF00FF;
			const ulong k2 = 0x0000FFFF0000FFFF;
			bitmap = ( (bitmap >> 8 ) & k1) | ( (bitmap & k1) << 8 );
			bitmap = ( (bitmap >> 16) & k2) | ( (bitmap & k2) << 16);
			bitmap = ( (bitmap >> 32)	  )	| ( (bitmap     ) << 32);
			return bitmap;
		} // end static TransformMirror000
		
		public static ulong TransformMirror090(ulong bitmap){
			/* 
			mirror bitmap about the center files, along the Y axis
			parallel prefix approach
			reverses the bits of each byte
			*/
			const ulong k1 = 0x5555555555555555;
			const ulong k2 = 0x3333333333333333;
			const ulong k4 = 0x0f0f0f0f0f0f0f0f;
			bitmap = ( ( bitmap >> 1) & k1) | ( (bitmap & k1) << 1);
			bitmap = ( ( bitmap >> 2) & k2) | ( (bitmap & k2) << 2);
			bitmap = ( ( bitmap >> 4) & k4) | ( (bitmap & k4) << 4);
			return bitmap;
		} // end static TransformMirror090
		
		public static ulong TransformMirror045(ulong bitmap){
			/* 
			mirror bitmap the diagnonal (45 degree axis), a1<->h8
			*/
			ulong temp;
			const ulong k1 = 0x5500550055005500;
			const ulong k2 = 0x3333000033330000;
			const ulong k4 = 0x0f0f0f0f00000000;
			temp    = k4 & (bitmap ^ (bitmap << 28) );
			bitmap ^=      (temp   ^ (temp   >> 28) );
			temp    = k2 & (bitmap ^ (bitmap << 14) );
			bitmap ^=      (temp   ^ (temp   >> 14) );
			temp    = k1 & (bitmap ^ (bitmap <<  7) );
			bitmap ^=      (temp   ^ (temp   >>  7) );
			return bitmap;
		} // end static TransformMirror045
		
		public static ulong TransformMirror135(ulong bitmap){
			/* 
			mirror bitmap the anti-diagnonal (135 degree axis), a8<->h1
			*/
			ulong temp;
			const ulong k1 = 0xaa00aa00aa00aa00;
			const ulong k2 = 0xcccc0000cccc0000;
			const ulong k4 = 0x0f0f0f0f0f0f0f0f;
			temp    =      (bitmap ^ (bitmap << 36) );
			bitmap ^= k4 & (  temp ^ (bitmap >> 36) );
			temp    = k2 & (bitmap ^ (bitmap << 18) );
			bitmap ^=      (temp   ^ (temp   >> 18) );
			temp    = k1 & (bitmap ^ (bitmap <<  9) );
			bitmap ^=      (temp   ^ (temp   >>  9) );
			return bitmap;
		} // end static TransformMirror135
		
		public static ulong TransformRotate180(ulong bitmap){
			return TransformMirror090( TransformMirror000(bitmap) );
		} // end static TransformRotate180
		
		public static ulong TransformRotate270(ulong bitmap){
			return TransformMirror000( TransformMirror045(bitmap) );
		} // end static TransformRotate270
		
		public static ulong TransformRotate090(ulong bitmap){
			return TransformMirror045( TransformMirror000(bitmap) );
		} // end static TransformRotate090
		
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
	
		public void ClearBoard(){
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				this.pieces[piece_iterator] = 0x0;
			}
		} // end ClearBoard
	
		public Bitboard Copy(){
			Bitboard return_bitboard = new Bitboard();
			return_bitboard.pieces = new ulong[this.pieces.Length];
			
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				return_bitboard.pieces[piece_iterator] = this.pieces[piece_iterator];
				piece_iterator++;
			}
			return return_bitboard;
		} // end Copy()
		
		public override string ToString(){
			char[] return_bitboard = UlongToString(0x0).ToCharArray();
			
			int piece_iterator = 2; // start at pawns
			while(piece_iterator < this.pieces.Length){
				
				int color_iterator = 0;
				while(color_iterator < 2){
					
					// Do one color and one piece at a time. Makes it easy to determine the piece symbol
					char[] current_bitmap = UlongToString( this.pieces[piece_iterator] & this.pieces[color_iterator] ).ToCharArray();
					
					// Determine the piece symbol. piece_iterator determins the character, color_iterator determines case.
					char current_piece_symbol = Bitboard.piece_symbol[piece_iterator];
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