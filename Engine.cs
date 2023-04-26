using System;
using System.IO;

namespace ZBC{
    public class Engine{
		
        public static void Main(string[] args){
			
			Bitboard a = new Bitboard();
			Console.WriteLine(a.ToString());
			
			while(true){
				string[] input = (Console.ReadLine()).Split(' ');
				if(input[0].Equals("move") & input.Length == 3){
					a.Move( new string[] { input[1], input[2] } );
				}
				else if(input[0].Equals("spawn") & input.Length ==4){
					a.Spawn(input[1], input[2], input[3]);
				}
				else if(input[0].Equals("capture") & input.Length==2){
					a.Capture(input[1]);
				}
				Console.WriteLine(a.ToString());
			}
			
        } // end method Main
		
    } // end class Engine
} // end namespace ZC