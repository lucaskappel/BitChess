using System;
using System.IO;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			Bitboard a = new Bitboard();
			Console.WriteLine(a.ToString());
			string[] coordinates = new string[2];
			while(true){
				coordinates[0] = Console.ReadLine();
				coordinates[1] = Console.ReadLine();
				a.move(coordinates);
				Console.WriteLine(a.ToString());
			}
			
        } // end method Main
		
    } // end class Engine
} // end namespace ZC