using System;

namespace ZBC{
	
	public class Bitgame{
		
		// vars //
		
		public string[] players;
		
		public Bitboard[] gamestate;
		public Bitboard[] gamestate_shadow;
		
		public int halfmove_counter;
		public int fullmove_counter{ get{ return Convert.ToInt32(Math.Floor(this.halfmove_counter / 2.0)); } }
		
		public int halfmove_clock;
		public ulong[] castling_rights;
		
		// structors //
		
		public Bitgame(){
			//TODO
		}//end constructor Bitgame

		// utils //
		
		// methods //
		
		public void ReadFEN(){
		}//end ReadFEN
		
		public void WriteFEN(){
		}//end WriteFEN
		
		public void ReadPGN(){
		}//end ReadPGN
		
		public void WritePGN(){
		}//end writePGN
		
	}// end class Bitgame
}// end namespace ZBC