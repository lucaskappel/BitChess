using System;

namespace ZBC {
	
	public class Bitboard{
		
		// vars //
		
		private static char[] piece_symbol = {'p', 'n', 'b', 'r', 'q', 'k'};
		private static string[] piece_name = {"pawn", "knight", "bishop", "rook", "queen", "king"};
		
		// piece_deltas
		
		public ulong[] pieces;
		
		public Bitboard(){
			//TODO endianess and stuff need to be determined. gotta read more. maybe just do both?
			/* 
			this.pieces = new ulong[] { // ulongs are 64 bits, 1 bit per square. This represents piece positions.
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
			*/
			
		} // end constructor Bitboard
		
		// utilities//
		
		public static string UlongToString(ulong bitmap){
			string ulong_as_string = Convert.ToString(unchecked((long) bitmap), 2);
			while(ulong_as_string.Length < 64){ // Add leading zeroes until we get to 64 elements
				ulong_as_string = "0" + ulong_as_string; 
			}
			return ulong_as_string;
		} // end UlongToString
		
		public static ulong CoordinateToUlong(string coordinate){
			char[] fileIds = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};
			//TODO how to convert depends on mapping method
			
			/* 
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
			*/
			
			return (ulong)0x1;
		} // end CoordinateToUlong
		
		// methods//
		
		public void PieceCreate(int piece_type, ulong coordiante){
			// TODO - piece_type determined by if using compact bitboards or not
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
		
		public static Bitboard ReadFENPosition(string FEN_text){
			//TODO how we read depends on how we record the pieces
			return new Bitboard();
		} // end ReadFENPosition
		
		public string WriteFENPosition(){
			//TODO how we write depends on how we record the pieces
			return "";
		} // end WriteFENPosition
		
		public Bitboard create_copy(){
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
			//TODO debug display
			/*
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
			*/
			return "";
		} // end override ToString
		
	} // end class Bitboard
	
	
	
	
	
	
	
	
	
	/* 

		public static ulong coordinates_to_bitboard(string coordinates){
			
		}
		

	
} // end namespace ZBC