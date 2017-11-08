using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordTicker
{
    class Program
    {
        class KeywordCount
        {
            public string Word { get; set; }
            public int Count { get; set; }
            public string Key { get; set; }
        };

        static void Main(string[] args)
        {
            List<KeywordCount> keywords = new List<KeywordCount>();

            // Assign each number key to a particular user-defined word
            while (true) // Put this intro in a while loop so if the user doesn't provide proper input we can cycle back through
            {
                Console.WriteLine("Enter in the keywords you want to bind to each number key (up to 0)");
                Console.WriteLine("Press Esc to continue to the next step");
                Console.WriteLine();

                // Iterate over the number keys. When we get to i = 10 we'll replace "10" with "0" for the zero key.
                // We want to make all of the keys go in order so don't start with 0.
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine("Binding for key " + i.ToString().Replace("10", "0")); // Replace "10" with "0"

                    string input = ReadLineWithCancel();

                    if (input != null)
                    {
                        KeywordCount item = new KeywordCount();
                        item.Word = input;
                        item.Count = 0; // Set the initial count to 0
                        item.Key = i.ToString().Replace("10", "0"); // Replace "10" with "0"
                        keywords.Add(item);
                        Console.WriteLine();
                    }
                    else
                    {
                        break;
                    }
                }

                Console.Clear();

                if (keywords.Count() > 0) // See if the user gave any input
                {
                    break;
                }
                else // If the user just pressed Esc with no input repeat the above process
                {
                    Console.WriteLine();
                    Console.WriteLine("You didn't provide any words, please try again");
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }

            // Now we're ready to start counting. Let the user know which keys are associated with which word
            WriteRules(keywords, true); // Write the rules initially

            // Keep prompting the user for input until they press Q
            while (true)
            {
                // TODO: Make the rules "stick" to the top of the screen even when the user enters enough info to scroll past it
                //WriteRules(keywords, false);

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine(" Quit");
                    Console.WriteLine();
                    break;
                }

                KeywordCount match = keywords.Where(x => x.Key == key.KeyChar.ToString()).FirstOrDefault();
                if (match != null)
                {
                    match.Count++;
                    Console.WriteLine(" " + match.Word);
                }
                else // No word is associated with the key that was pressed
                {
                    Console.WriteLine(" ERROR! No match found");
                }
            }

            // Print the count results
            Console.WriteLine("Results");
            foreach (var keyword in keywords)
            {
                Console.WriteLine("[" + keyword.Key + "] " + keyword.Word + ": " + keyword.Count);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Listens to user input with the typical ReadLine() functionality (i.e. reads until "Enter" is pressed),
        /// unless "Esc" is pressed in which case an event is triggered immediately and a null string is returned.
        /// </summary>
        /// <returns></returns>
        private static string ReadLineWithCancel()
        {
            string result = null;

            StringBuilder buffer = new StringBuilder();

            // The key is read passing true for the intercept argument to prevent
            // any characters from displaying when the Escape key is pressed.
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            }

            // Mimic the functionality of ReadLine() by registering "Enter" as a submission.
            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }

            return result;
        }

        /// <summary>
        /// Writes which keys are associated with which words at the top of the console screen.
        /// 
        /// TODO: Make the rules "stick" to the top of the screen even when the user enters enough info to scroll past it
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="firstRun"></param>
        private static void WriteRules(List<KeywordCount> keywords, bool firstRun)
        {            
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            //Console.CursorTop = Console.WindowTop;// + Console.WindowHeight - 1; // TODO

            Console.WriteLine("Press the following numbers to record an instance of the associated word.");

            foreach (var keyword in keywords)
            {
                Console.WriteLine(keyword.Key + " = " + keyword.Word);
            }

            Console.WriteLine("*** Q = Quit & summarize ***");
            Console.WriteLine();

            if (!firstRun)
            {
                //Console.SetCursorPosition(currentLeft, currentTop);
            }
            //Console.SetCursorPosition(currentLeft, currentTop); // TODO
        }
    }
}