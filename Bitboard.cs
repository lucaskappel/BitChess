using System;

namespace ZBC {
	
	public class Bitboard{ // All code designed for Little-Endian Rank-File Mapping
		
		// vars //
		
		public static string[] piece_name = {"white", "black", "pawn", "knight", "bishop", "rook", "queen", "king"};
		public static char[] piece_symbol = {'w', 'b', 'p', 'n', 'b', 'r', 'q', 'k'};
		
		public static char[] file_index = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};
		public static char[] rank_index = {'1', '2', '3', '4', '5', '6', '7', '8'};
		
		public static ulong file_a = 0x0101010101010101;
		public static ulong file_h = 0x8080808080808080;
		public static ulong rank_1 = 0x00000000000000FF;
		public static ulong rank_8 = 0xFF00000000000000;
		public static ulong diagonal_a1h8 = 0x8040201008040201;
		public static ulong diagonal_h1a8 = 0x0102040810204080;
		public static ulong squares_light = 0x55AA55AA55AA55AA;
		public static ulong squares_dark  = 0x0AA55AA55AA55AA5;
		
		public static string[] compass_rose_index = {"n", "ne", "e", "se", "s", "sw", "w", "nw"};
		public static int[] compass_rose = {8, 9, 1, -7, -8, -9, -1, 7};
		
		/*
			   nw    n   ne
				+7  +8  +9
			   	  \  |  /
			  w  -1  0  +1  e
				  /  |  \
			    -9  -8  -7
			   sw    s    se
		*/
		
		public ulong[] pieces;
		
		// structors //
		
		public Bitboard(){
			this.pieces = new ulong[] { // little-endian rank-file mapping
				0x000000000000ffff, // white
				0xffff000000000000, // black
				0x00ff00000000ff00, // pawn
				0x4200000000000042, // knight
				0x2400000000000024, // bishop
				0x8100000000000081, // rook
				0x0800000000000008, // queen
				0x1000000000000010, // king
			};
		} // end constructor Bitboard
		
		public Bitboard(string configuration){
			
			this.pieces = new ulong[] { // little-endian rank-file mapping
					0x0000000000000000, // white
					0x0000000000000000, // black
					0x0000000000000000, // pawn
					0x0000000000000000, // knight
					0x0000000000000000, // bishop
					0x0000000000000000, // rook
					0x0000000000000000, // queen
					0x0000000000000000, // king
				}; 
			
			if(configuration.Equals("r")){
				this.pieces[0] = 0x7844444870504844;
				this.pieces[2] = this.pieces[0];
			}
			
			else if(configuration.Equals("centerknights")){
				this.pieces[0] = 0x0000001008000000;
				this.pieces[1] = 0x0000000810000000;
				this.pieces[3] = this.pieces[0] & this.pieces[1];
			}
			
			else if(configuration.Equals("pawns")){
				this.pieces[0] = 0x000000000000ff00;
				this.pieces[1] = 0x00ff000000000000;
				this.pieces[2] = this.pieces[0] & this.pieces[1];
			}
			
			else if(configuration.Equals("random")){
				this.pieces[0] = RandomUlong() & RandomUlong(); // & is to help to reduce piece count, only 1 makes too many
				this.pieces[2] = this.pieces[0];
			}
			
			return;
		} // end constructor Bitboard(string)
		
		// indexing //
		
		public static int SquareIndex(int fileIndex, int rankIndex){
			/* translates the rank and file indicies to the square's bit index
			a1 is the least significant bit at index 0 (2^0)
			h8 is the most significant bit at index 63 (2^63)
			
			8  56 57 58 59 60 61 62 63
			7  48 49 50 51 52 53 54 55
			6  40 41 42 43 44 45 46 47
			5  32 33 34 35 36 37 38 39
			4  24 25 26 27 28 29 30 31
			3  16 17 18 19 20 21 22 23
			2  08 09 10 11 12 13 14 15
			1  00 01 02 03 04 05 06 07
			
			   a  b  c  d  e  f  g  h
			   
			*/
			if(fileIndex < 0 || rankIndex < 0){
				Console.WriteLine("Invalid rank or file\nrank: " + rankIndex.ToString() + "\nfile: " + fileIndex.ToString());
				return -1;
			}
			return 8 * rankIndex + fileIndex;
		} // end static SquareIndex(int, int)
		
		public static int SquareIndex(string coordinate){
			char[] coordinateArray;
			try{ 
				coordinateArray = coordinate.ToCharArray(); 
			}
			catch(Exception e){ 
				Console.WriteLine(e.Message + "\nError trying to convert coordinate to SquareIndex: " + coordinate);
				return -1;	
			}
			if(coordinateArray.Length != 2){ 
				Console.WriteLine("Coordinate was not properly formatted: " + coordinate);
				return -1; 
			}
			
			int fileIndex = Array.IndexOf(file_index, coordinateArray[0]);
			int rankIndex = Array.IndexOf(rank_index, coordinateArray[1]);
			
			return SquareIndex(fileIndex, rankIndex);
		} // end static SquareIndex(string)
		
		public static int FileIndex(int squareIndex){
			/* Translates the square index into the file index.
			FileIndex = squareIndex % 8 = squareIndex & 8
			
			7 == 00000111
			
			a1 = 00000000 & 00000111 = 00000000 = 0
			b2 = 00001001 & 00000111 = 00000001 = 1
			c3 = 00010010 & 00000111 = 00000010 = 2
			d4 = 00011011 & 00000111 = 00000011 = 3
			e5 = 00100100 & 00000111 = 00000100 = 4
			f6 = 00101101 & 00000111 = 00000101 = 5
			g7 = 00110110 & 00000111 = 00000110 = 6
			h8 = 00111111 & 00000111 = 00000111 = 7
			*/
			return squareIndex & 7;
		} // end static FileIndex
		
		public static int RankIndex(int squareIndex){
			/* Translates the square index into the rank index.
			RankIndex = squareIndex div 8 = squareIndex >> 3
			
			a1 = 00000000 >> 3 = 00000000 = 0
			b2 = 00001001 >> 3 = 00000001 = 1
			c3 = 00010010 >> 3 = 00000010 = 2
			d4 = 00011011 >> 3 = 00000011 = 3
			e5 = 00100100 >> 3 = 00000100 = 4
			f6 = 00101101 >> 3 = 00000101 = 5
			g7 = 00110110 >> 3 = 00000110 = 6
			h8 = 00111111 >> 3 = 00000111 = 7
			*/
			return squareIndex >> 3;
		} // end static RankIndex

		public static ulong SquareIndexToUlong(int squareIndex){
			return (ulong)Math.Pow(2, squareIndex);
		} // end static SquareIndexToUlong

		public static ulong CoordinateToUlong(string coordinate){
			int squareIndex = SquareIndex(coordinate);
			return SquareIndexToUlong(squareIndex);
		} // end static CoordinateToUlong

		// display //		
		
		public static char[] UlongToVisualBoardOrder(ulong bitmap){
			// converts to char[64] and orders the chars to the bitboard's visual indexing
			string ulong_as_string = UlongToString(bitmap);
			string mappedString = "";
			char[] workbench;
			while(mappedString.Length < 64){
				workbench = ulong_as_string.Substring(mappedString.Length, 8).ToCharArray();
				Array.Reverse(workbench);
				mappedString += new string(workbench);
			}
			return mappedString.ToCharArray();
		}
		
		public static string UlongToString(ulong bitmap){
			string ulong_as_string = Convert.ToString(unchecked((long) bitmap), 2);
			while(ulong_as_string.Length < 64){ // Add leading zeroes until we get to 64 elements
				ulong_as_string = "0" + ulong_as_string; 
			}
			return ulong_as_string;
		} // end UlongToString
		
		public override string ToString(){
			char[] workArray = new char[64];
			for(int i=0; i < workArray.Length; i++){ workArray[i] = '-'; }
			
			for(int piece_iterator = 2; piece_iterator < this.pieces.Length; piece_iterator++){
				for(int color_iterator = 0; color_iterator < 2; color_iterator++){
					// get the array of characters representing the right set of pieces
					ulong currentBitmap = this.pieces[color_iterator] & this.pieces[piece_iterator];
					char[] currentBitmapChars = UlongToVisualBoardOrder(currentBitmap);
					
					// set the character to represent the current set of pieces
					char currentPieceSymbol = piece_symbol[piece_iterator];
					if(color_iterator == 0){ currentPieceSymbol = Char.ToUpper(currentPieceSymbol); }
					
					// Replace the appropriate indices with the current character
					int bit_index = Array.IndexOf(currentBitmapChars, '1');
					while(bit_index >= 0){
						workArray[bit_index] = currentPieceSymbol;
						currentBitmapChars[bit_index] = currentPieceSymbol;
						bit_index = Array.IndexOf(currentBitmapChars, '1');
					} // end bit loop
				} // end color_iterator loop
			} // end piece_iterator loop
			
			string returnString = " " + String.Join(" ", workArray);
			for(int stringIndex = 2*(64-8); stringIndex > 0; stringIndex -= 8*2){ 
				returnString = returnString.Insert(stringIndex, "\n"); 
			}
			return returnString;
		} // end override ToString
		
		// spans //
		
		public static ulong DumbSpan(ulong bitmap, int direction){
			int compassRoseIndex = Array.IndexOf(compass_rose, direction);
			if(compassRoseIndex < 0){ 
				Console.WriteLine("compass rose direction was invalid: " + direction);
				return 0x0; 
			}
			else if(compassRoseIndex == 0){
				return Span_N(bitmap);
			}
			else if(compassRoseIndex == 1){
				return Span_NE(bitmap);
			}
			else if(compassRoseIndex == 2){
				return Span_E(bitmap);
			}
			else if(compassRoseIndex == 3){
				return Span_SE(bitmap);
			}
			else if(compassRoseIndex == 4){
				return Span_S(bitmap);
			}
			else if(compassRoseIndex == 5){
				return Span_SW(bitmap);
			}
			else if(compassRoseIndex == 6){
				return Span_W(bitmap);
			}
			else if(compassRoseIndex == 7){
				return Span_NW(bitmap);
			}
			return bitmap;
		}//DumbSpan
		
		public static ulong Span_N(ulong bitmap){
			ulong span = bitmap << 8;
			span |= (span | bitmap) << 16;
			span |= (span | bitmap) << 32;
			return span;
		}//Span_N
		
		public static ulong Span_NE(ulong bitmap){
			ulong span = (bitmap <<  9) & TransformRotate270(TransformRotate090(bitmap) <<  7);
			span |= ((span | bitmap) << 18) & TransformRotate270(TransformRotate090(bitmap | span) <<  14);
			span |= ((span | bitmap) << 36) & TransformRotate270(TransformRotate090(bitmap | span) <<  28);
			return span;
		}//Span_NE
		
		public static ulong Span_E(ulong bitmap){
			return TransformRotate270(Span_N(TransformRotate090(bitmap)));
		}//Span_E
		
		public static ulong Span_SE(ulong bitmap){
			ulong span = (bitmap >>  7) & TransformRotate090(TransformRotate270(bitmap) >>  9);
			span |= ((span | bitmap) >> 14) & TransformRotate090(TransformRotate270(bitmap | span) >> 18);
			span |= ((span | bitmap) >> 28) & TransformRotate090(TransformRotate270(bitmap | span) >> 36);
			return span;
		}//Span_SE
		
		public static ulong Span_S(ulong bitmap){
			ulong span = bitmap >> 8;
			span |= (span | bitmap) >> 16;
			span |= (span | bitmap) >> 32;
			return span;
		}//Span_S
		
		public static ulong Span_SW(ulong bitmap){
			ulong span = (bitmap >>  9) & TransformRotate270(TransformRotate090(bitmap) >>  7);
			span |= ((span | bitmap) >> 18) & TransformRotate270(TransformRotate090(span | bitmap) >> 14);
			span |= ((span | bitmap) >> 36) & TransformRotate270(TransformRotate090(span | bitmap) >> 28);
			return span;
		}//Span_SW
		
		public static ulong Span_W(ulong bitmap){
			return TransformRotate270(Span_S(TransformRotate090(bitmap)));
		}//Span_W
		
		public static ulong Span_NW(ulong bitmap){
			ulong span = (bitmap <<  7) & TransformRotate090(TransformRotate270(bitmap) <<  9);
			span |= ((span | bitmap) << 14) & TransformRotate090(TransformRotate270((span | bitmap)) << 18);
			span |= ((span | bitmap) << 28) & TransformRotate090(TransformRotate270((span | bitmap)) << 36);
			return span;
		}//Span_NW
		
		// fills //
		
		public static ulong DumbFill(ulong bitmap, int direction){
			int compassRoseIndex = Array.IndexOf(compass_rose, direction);
			if(compassRoseIndex < 0){ 
				Console.WriteLine("compass rose direction was invalid: " + direction);
				return 0x0; 
			}
			
			// not a great way to do this but it will work for now
			if(compassRoseIndex == 0){
				return Fill_N(bitmap);
			}
			else if(compassRoseIndex == 1){
				return Fill_NE(bitmap);
			}
			else if(compassRoseIndex == 2){
				return Fill_E(bitmap);
			}
			else if(compassRoseIndex == 3){
				return Fill_SE(bitmap);
			}
			else if(compassRoseIndex == 4){
				return Fill_S(bitmap);
			}
			else if(compassRoseIndex == 5){
				return Fill_SW(bitmap);
			}
			else if(compassRoseIndex == 6){
				return Fill_W(bitmap);
			}
			else if(compassRoseIndex == 7){
				return Fill_NW(bitmap);
			}
			return bitmap;
		} // end static DumbFill
		
		public static ulong Fill_N(ulong bitmap){
			bitmap |= (bitmap <<  8);
			bitmap |= (bitmap << 16);
			bitmap |= (bitmap << 32);
			return bitmap;
		} // end static Fill_N
		
		public static ulong Fill_S(ulong bitmap){
			bitmap |= (bitmap >>  8);
			bitmap |= (bitmap >> 16);
			bitmap |= (bitmap >> 32);
			return bitmap;
		} // end static Fill_S
		
		public static ulong Fill_E(ulong bitmap){
			bitmap = TransformRotate090(bitmap);
			bitmap = Fill_N(bitmap);
			bitmap = TransformRotate270(bitmap);
			return bitmap;
		} // end static Fill_E
		
		public static ulong Fill_W(ulong bitmap){
			bitmap = TransformRotate270(bitmap);
			bitmap = Fill_N(bitmap);
			bitmap = TransformRotate090(bitmap);
			return bitmap;
		} // end static Fill_W
		
		public static ulong Fill_NE(ulong bitmap){
			bitmap |= (bitmap <<  9) & TransformRotate270(TransformRotate090(bitmap) <<  7);
			bitmap |= (bitmap << 18) & TransformRotate270(TransformRotate090(bitmap) << 14);
			bitmap |= (bitmap << 36) & TransformRotate270(TransformRotate090(bitmap) << 28);
			return bitmap;
		} // end static fill_NE
		
		public static ulong Fill_NW(ulong bitmap){
			bitmap |= (bitmap <<  7) & TransformRotate090(TransformRotate270(bitmap) <<  9);
			bitmap |= (bitmap << 14) & TransformRotate090(TransformRotate270(bitmap) << 18);
			bitmap |= (bitmap << 28) & TransformRotate090(TransformRotate270(bitmap) << 36);
			return bitmap;
		} // end static fill_NW
		
		public static ulong Fill_SE(ulong bitmap){
			bitmap |= (bitmap >>  7) & TransformRotate090(TransformRotate270(bitmap) >>  9);
			bitmap |= (bitmap >> 14) & TransformRotate090(TransformRotate270(bitmap) >> 18);
			bitmap |= (bitmap >> 28) & TransformRotate090(TransformRotate270(bitmap) >> 36);
			return bitmap;
		} // end static fill_SW
		
		public static ulong Fill_SW(ulong bitmap){
			bitmap |= (bitmap >>  9) & TransformRotate270(TransformRotate090(bitmap) >>  7);
			bitmap |= (bitmap >> 18) & TransformRotate270(TransformRotate090(bitmap) >> 14);
			bitmap |= (bitmap >> 36) & TransformRotate270(TransformRotate090(bitmap) >> 28);
			return bitmap;
		} // end static fill_SE
	
		public static ulong Fill_NE_test(ulong bitmap){
			// rotate by 45 deg then north fill, then rotate back.
			// I don't think this is how this works...
			bitmap = TransformRotate045(bitmap);
			bitmap = Fill_N(bitmap);
			bitmap = TransformRotate315(bitmap);
			return bitmap;
		}
		
		// blocks //
	
		public static ulong DumbBlock(ulong bitmap, int direction){
			ulong span = 0x0;
			
			int compassRoseIndex = Array.IndexOf(compass_rose, direction);
			if(compassRoseIndex < 0){ 
				Console.WriteLine("compass rose direction was invalid: " + direction);
				return span; 
			}
			else if(compassRoseIndex == 0){
				return Blocker_N(bitmap);
			}
			else if(compassRoseIndex == 1){
				return Blocker_NE(bitmap);
			}
			else if(compassRoseIndex == 2){
				return Blocker_E(bitmap);
			}
			else if(compassRoseIndex == 3){
				return Blocker_SE(bitmap);
			}
			else if(compassRoseIndex == 4){
				return Blocker_S(bitmap);
			}
			else if(compassRoseIndex == 5){
				return Blocker_SW(bitmap);
			}
			else if(compassRoseIndex == 6){
				return Blocker_W(bitmap);
			}
			else if(compassRoseIndex == 7){
				return Blocker_NW(bitmap);
			}
			return bitmap;
		} // end static DumbFill
	
		public static ulong Blocker_N(ulong bitmap){
			ulong span = Fill_N(bitmap << 8);
			return bitmap & (~span);
		} // end static Blocker_N
		
		public static ulong Blocker_S(ulong bitmap){
			ulong span = Fill_S(bitmap >> 8);
			return bitmap & (~span);
		} // end static Blocker_S
		
		public static ulong Blocker_E(ulong bitmap){
			bitmap = TransformRotate090(bitmap);
			ulong blockers = Blocker_N(bitmap);
			blockers = TransformRotate270(blockers);
			return blockers;
		} // end static Blocker_E
		
		public static ulong Blocker_W(ulong bitmap){
			bitmap = TransformRotate270(bitmap);
			ulong blockers = Blocker_N(bitmap);
			blockers = TransformRotate090(blockers);
			return blockers;
		} // end static Blocker_W
		
		public static ulong Blocker_NE(ulong bitmap){
			ulong span = Fill_NE(bitmap <<  9);
			return bitmap & (~span);
		} // end static Blocker_NE
		
		public static ulong Blocker_NW(ulong bitmap){
			ulong span = Fill_NW(bitmap <<  7);
			return bitmap & (~span);
		} // end static Blocker_NW
		
		public static ulong Blocker_SE(ulong bitmap){
			ulong span = Fill_SE(bitmap >>  7);
			return bitmap & (~span);
		} // end static Blocker_SE
		
		public static ulong Blocker_SW(ulong bitmap){
			ulong span = Fill_SW(bitmap >>  9);
			return bitmap & (~span);
		} // end static Blocker_SW
		
		// transforms //
		
		public static ulong TransformMirrorVertical(ulong bitmap){
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
		} // end static TransformMirrorVertical
		
		public static ulong TransformMirrorHorizontal(ulong bitmap){
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
		} // end static TransformMirrorHorizontal
		
		public static ulong TransformMirrorDiagonal(ulong bitmap){
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
		} // end static TransformMirrorDiagonal
		
		public static ulong TransformMirrorAntidiagonal(ulong bitmap){
			/* 
			mirror bitmap the anti-diagnonal (135 degree axis), a8<->h1
			*/
			ulong temp;
			const ulong k1 = 0xaa00aa00aa00aa00;
			const ulong k2 = 0xcccc0000cccc0000;
			const ulong k4 = 0xf0f0f0f00f0f0f0f;
			temp    =       bitmap ^ (bitmap << 36)  ;
			bitmap ^= k4 & ( temp  ^ (bitmap >> 36) );
			temp    = k2 & (bitmap ^ (bitmap << 18) );
			bitmap ^=        temp  ^ (temp   >> 18)  ;
			temp    = k1 & (bitmap ^ (bitmap <<  9) );
			bitmap ^=        temp  ^ (temp   >>  9)  ;
			return bitmap;
		} // end static TransformMirrorAntidiagonal
		
		public static ulong TransformRotate180(ulong bitmap){
			return TransformMirrorHorizontal( TransformMirrorVertical(bitmap) );
		} // end static TransformRotate180
		
		public static ulong TransformRotate270(ulong bitmap){
			return TransformMirrorVertical( TransformMirrorDiagonal(bitmap) );
		} // end static TransformRotate270
		
		public static ulong TransformRotate090(ulong bitmap){
			return TransformMirrorDiagonal( TransformMirrorVertical(bitmap) );
		} // end static TransformRotate090
		
		public static ulong TransformRotate315(ulong bitmap){
			ulong k1 = 0xaaaaaaaaaaaaaaaa;
			ulong k2 = 0xcccccccccccccccc;
			ulong k4 = 0xf0f0f0f0f0f0f0f0;
			
			bitmap ^= k1 & (bitmap ^ BitRotateRight(bitmap,  8));
			bitmap ^= k2 & (bitmap ^ BitRotateRight(bitmap, 16));
			bitmap ^= k4 & (bitmap ^ BitRotateRight(bitmap, 32));
			return bitmap;
		} // end TransformRotate045
		
		public static ulong TransformRotate045(ulong bitmap){
			ulong k1 = 0x5555555555555555;
			ulong k2 = 0x3333333333333333;
			ulong k4 = 0x0f0f0f0f0f0f0f0f;
			
			bitmap ^= k1 & (bitmap ^ BitRotateRight(bitmap,  8));
			bitmap ^= k2 & (bitmap ^ BitRotateRight(bitmap, 16));
			bitmap ^= k4 & (bitmap ^ BitRotateRight(bitmap, 32));
			return bitmap;
		} // end TransformRotate315
		
		// board interaction //
		
		public void PieceCreate(int[] piece_id, ulong coordinate){
			// piece_id[0] is either 0 or 1, indicates white or black
			// 2 <= piece_id[1] <= 7, indicates piece class
			if(piece_id[0] < 0 || piece_id [0] > 1){ return; }
			else if(piece_id[1] < 2 || piece_id[1] > this.pieces.Length){ return; }
			this.pieces[piece_id[0]] = this.pieces[piece_id[0]] | coordinate;
			this.pieces[piece_id[1]] = this.pieces[piece_id[1]] | coordinate;
			return;
		} // end PieceCreate
		
		public void PieceCreate(string piece_color, string piece_role, string coordinate){
			if(Array.IndexOf(piece_name, piece_color) < 0 || Array.IndexOf(piece_name, piece_role) < 0){ 
				Console.WriteLine("Piece name or type was not valid:");
				Console.WriteLine("color: " + piece_color + " : " + Array.IndexOf(piece_name, piece_color).ToString());
				Console.WriteLine("role: " + piece_role + " : " + Array.IndexOf(piece_name, piece_role).ToString());
				return; 
			}
			this.PieceCreate(
				new int[] {
					Array.IndexOf(piece_name, piece_color), 
					Array.IndexOf(piece_name, piece_role )},
				CoordinateToUlong(coordinate)
			);
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
		} // end PieceRemove(ulong)
		
		public void PieceRemove(string coordinate){
			this.PieceRemove(CoordinateToUlong(coordinate));
		} // end PieceRemove(string)
		
		
		public void PieceMove(ulong[] coordinates){
			int pieces_iterator = 0;
			while(pieces_iterator < this.pieces.Length){
				if( (this.pieces[pieces_iterator] & coordinates[0]) != 0x0){
					this.pieces[pieces_iterator] = this.pieces[pieces_iterator] ^ coordinates[0];
					this.pieces[pieces_iterator] = this.pieces[pieces_iterator] | coordinates[1];
				}
				pieces_iterator++;
			}
			return;
		} // end PieceMove(ulong[])
	
		public void PieceMove(string coordinate_start, string coordinate_end){
			this.PieceMove(new ulong[] {
				CoordinateToUlong(coordinate_start),
				CoordinateToUlong(coordinate_end)
			});
		} // end PieceMove(string, string)
	
	
		public void ClearBoard(){
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				this.pieces[piece_iterator] = 0x0;
				piece_iterator++;
			}
		} // end ClearBoard
	
		// util //
	
		public Bitboard Copy(){
			Bitboard return_bitboard = new Bitboard();
			return_bitboard.pieces = new ulong[this.pieces.Length];
			
			int piece_iterator = 0;
			while(piece_iterator < this.pieces.Length){
				return_bitboard.pieces[piece_iterator] = this.pieces[piece_iterator];
				piece_iterator++;
			}
			return return_bitboard;
		} // end Copy
		
		private static ulong RandomUlong(){
			//Working with ulong so that modulo works correctly with values > long.MaxValue
			ulong uRange = 0xffffffffffffffff;
			Random random = new Random(Guid.NewGuid().GetHashCode());

			//Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
			//for more information.
			//In the worst case, the expected number of calls is 2 (though usually it's
			//much closer to 1) so this loop doesn't really hurt performance at all.
			ulong ulongRand;
			do
			{
				byte[] buf = new byte[8];
				random.NextBytes(buf);
				ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
			} while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return ulongRand % uRange;
		} // end static RandomUlong
		
		private static ulong BitRotateLeft(ulong bitmap, int shift){
			return (bitmap << shift) | (bitmap >> 64 - shift);
		} // end BitRotateLeft
		
		private static ulong BitRotateRight(ulong bitmap, int shift){
			return (bitmap >> shift) | (bitmap << 64 - shift);
		} // end BitRotateRight
		
	} // end class Bitboard

} // end namespace ZBC