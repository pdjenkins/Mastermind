
using System;

/*
* Mastermind Rules
* 6 colors to choose
* 10 Rows/guesses
* Code must be 4 colors
* Each guess is complemented with two numbers, X/Y (commonly referred to as code pegs)
* Black Pegs ==> Number of colors correct AND NOT in the correct position.
* White Pegs ==> Number of colors correct AND in the correct position.
* 
* The number or position of pegs has no correlation or indication of which position is correct. 
* It is purely a summary of correct colors and/or positions.
*/

namespace Mastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Mastermind!");
            Console.WriteLine("Enter 'exit' or 'x' to exit the program.");
            Console.WriteLine("Enter 'new game' or 'new code' to start a new game.");
            Console.WriteLine("Enter 'colors' to see the color options");
            int numGuesses = 0;
            bool play = true;
            bool command = false;
            string? input = "";
            String[] scode = new String[4];
            String[] colors = new String[] {"BLUE","RED","YELLOW","GREEN","PURPLE","ORANGE" };
            int[] pegs = {0,0};
            scode = GetSecretCode(colors);
            while(play){
                command = false;
                
                //Get user input
                try
                {
                input = Console.ReadLine();

                //Make a decision based on the input
                if(input is not null){
                    input = input.ToUpper();
                    
                    /*
                    * Check for exit
                    */
                    if((input.Equals("X")) || (input.Equals("EXIT"))){
                        play = false;
                        command = true;
                    }

                    /*
                    * Create a secret code or start a new game
                    */
                    if( ((input.Equals("NC")) || (input.Equals("NEW CODE"))) || ((input.Equals("NEW GAME") || (input.Equals("NG")))) )
                    {
                        command = true;
                        scode = GetSecretCode(colors);
                        numGuesses = 0;
                        Console.WriteLine("====================");
                        Console.WriteLine("Game restart!");
                        Console.WriteLine("====================");
                    }

                    /*
                    * Validate Input
                    */
                     if(input.Equals("COLORS")){
                        command = true;
                        Console.WriteLine("Color choices: BLUE, RED, YELLOW, GREEN, PURPLE, ORANGE");
                    }

                    //TEST
                    if(input.Equals("TEST"))
                    {
                        command = true;
                        Console.WriteLine("TEST: input detected.");
                        Console.WriteLine("TEST: SCODE is ");
                        foreach(var color in scode){
                            Console.WriteLine(color);
                        }
                    }

                    /*
                    * Guess attempt
                    */
                    //validate user input is a series of colors, no greater than 4
                    if( ((command == false) && (validateInput(input, colors))) && (numGuesses < 10)){
                        numGuesses++;
                        pegs = Guess(input, scode);

                        //display results;
                        Console.WriteLine("White pegs: " + pegs[0]);
                        Console.WriteLine("Black pegs: " + pegs[1]);
                        Console.WriteLine("Attempts remaining: " + numGuesses);
                    }

                    /*
                    * Check for win condition
                    */
                    if(pegs[0] == 4){
                        Console.WriteLine("====================\nCongrats! You are a Mastermind!\n====================");
                        Instructions();
                    }
                    /*
                    * Check for lose condition
                    */
                    if((pegs[0] != 4) && numGuesses >= 10){
                        Console.WriteLine("GAME OVER: You ran out of attempts!");
                        Console.Write("Secret code was: ");
                        foreach(var color in scode){
                            Console.WriteLine(" | " + color);
                        }
                        Instructions();
                    }
                }else{
                        Console.WriteLine("Input cannot be null. Please try again.");
                    }
                }catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Invalid input. Please try again");
                }
            
            }
        }
        static String[] GetSecretCode(String[] colors){
            String[] scode = new String[4];
            Random rnd = new Random();
            for (int i = 0; i < scode.Length; i++)
            {
                scode[i] = colors[rnd.Next(0,5)];
            }
            //return code
            return scode;
        }
        static int[] Guess(String input, String[] scode){

            //pegs representing correct guesses
            //0 => No matches.
            //1 => Color & position match
            //2 => Color match. Wrong position.
            
            int[] pegs = {0,0,0,0};
            int[] pegSums = {0,0}; //White/black
            //seperate the input
            String[] inputArray = input.Split(' ');
            //compare the guess/input to the secret code

            //white peg guess
            for(int g = 0; g < inputArray.Length; g++)//position tracker
            {
                //if position & color match the secret code, its a white peg for that position.
                if(inputArray[g].Equals(scode[g])){
                    pegs[g] = 1;
                }
            }
            //TEST
            /*Console.WriteLine("Before black pegs are added");
            for(int p = 0; p < pegs.Length; p++){
                Console.WriteLine("Peg at position {0} is {1}",p,pegs[p]);
            }*/
            //black peg guess
            for(int k = 0; k < inputArray.Length; k++)
            {
                //count the occurances of the color in the secret code
                int colorCount = 0;
                for(int c = 0; c < scode.Length; c++){
                    if(inputArray[k].Equals(scode[c])){
                        //Console.WriteLine("Color "+ inputArray[k] + " was found in the secret code " );
                        colorCount++;//color match
                        if((pegs[c] == 1)){//remove duplicates
                            colorCount--;
                        }
                        //Console.WriteLine("Color count for {0} at position {1} is {2}", inputArray[k], k, colorCount);
                    }

                }
                if((pegs[k] == 0) && colorCount >= 1){
                    pegs[k] = 2;//mark as black peg
                }
                //TEST
                /*Console.WriteLine("After black pegs are added");
                for(int p = 0; p < pegs.Length; p++){
                    Console.WriteLine("Peg at position {0} is {1}",p,pegs[p]);
                }*/
            }

            //count results
            foreach (int peg in pegs){
                if(peg == 1){
                   pegSums[0] = pegSums[0] + 1;//white
                   }
                if(peg == 2){
                    pegSums[1] = pegSums[1] + 1;//black
                }
            }
            return pegSums;
        }

        static bool validateInput(String input, String[] colors){
            String[] inputArray = input.Split(' ');
            bool colorMatch = false;
            
            //Check for correct input size
            if(inputArray.Length != 4)
            {
                BadInput();
                return false;
            }

            //compare each word of input to the list of colors to verify input is valid.
            foreach (var word in inputArray)
            {
                word.ToUpper();
                colorMatch = false;

                //check if this word is a valid color choice
                foreach (var color in colors){
                    color.ToUpper();

                    if(word.Equals(color)){
                        colorMatch = true; //
                    }
                }
                //if the input was not a valid color, exit the function
                if(colorMatch == false)
                {
                    BadInput();
                    return false;
                }
                
            }
            //Since the previous loops will exit early on bad input, it stands that the loop will exit with colorMatch being true, ONLY IF all previous inputs are true.
            //Thus, we can return the last state of the boolean.
            return colorMatch;
        }
        static void BadInput(){
                Console.WriteLine("Invalid guess. Your guess must consist of 4 colors seperated by spaces. Guesses are NOT case sensitive.");
                Console.WriteLine("Color choices: BLUE, RED, YELLOW, GREEN, PURPLE, ORANGE");
        }
        static void Instructions(){
                        Console.WriteLine("Enter 'NG' or 'New Game' to start another game.");
                        Console.WriteLine("Enter 'exit' or 'x' to exit the program.");
        }
    }
}








