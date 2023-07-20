using System.Net.Security;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Word Hunter";
        Console.WriteLine(@"
 __    __              _                      _            
/ / /\ \ \___  _ __ __| |   /\  /\_   _ _ __ | |_ ___ _ __ 
\ \/  \/ / _ \| '__/ _` |  / /_/ / | | | '_ \| __/ _ \ '__|
 \  /\  / (_) | | | (_| | / __  /| |_| | | | | ||  __/ |   
  \/  \/ \___/|_|  \__,_| \/ /_/  \__,_|_| |_|\__\___|_|   
        ");

        string wordsFilePath = @".\words.wh";
        string failPath = @".\fail.txt";
        string[] words = LoadWordsFile(wordsFilePath);
        string[] failMessages = LoadWordsFile(failPath);

        Console.WriteLine("Welcome to Word Hunter, a simple word guessing game. \nYou will have to guess a valid 5-letter word in 6 tries. \nThe colors of the result indicate how close your guess was.");
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("Green means the letter is in the correct spot.");
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("Yellow means the letter is in the incorrect spot.");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Black means the letter is not in the word. \nAre you ready?");

        Console.ReadLine();
        Console.Clear();

        Word word = new Word(RandomWord(words));
        bool playerWin = false;
        string[] inputs = new string[6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < i; j++)
            {
                DisplayResults(word, inputs[j]);
                Console.WriteLine();
            }
            inputs[i] = Console.ReadLine().ToLower();

            foreach (char result in word.MatchInput(inputs[i]))
            {
                playerWin = true;
                if (result != 'g')
                {
                    playerWin = false;
                    break;
                }
            }

            if (playerWin)
            {
                Console.Clear();
                for (int j = 0; j < i; j++)
                {
                    DisplayResults(word, inputs[j]);
                    Console.WriteLine();
                }
                DisplayResults(word, word.WholeWord);
                Console.WriteLine();
                break;
            }

            if (i != 5) 
            {
                Console.Clear();
            }
        }
        if (playerWin)
        {
            Console.WriteLine("Congratulations! You guessed the word!");
            Console.WriteLine("Would you like to add a new word to the wordlist? (yes/no)");
            switch (Console.ReadLine().ToLower())
            {
                case "yes":
                    AddWordToList(wordsFilePath);
                    break;
                case "y":
                    AddWordToList(wordsFilePath);
                    break;
                case "no":
                    break;
                case "n":
                    break;
                default:
                    break;
            }
            Console.WriteLine("Goodbye!");
        }
        else
        {
            Console.WriteLine(RandomWord(failMessages));
            Console.WriteLine($"The word was {word.WholeWord}.");
        }
        Console.ReadKey();
    }     

    static string RandomWord(string[] words)
    {
        Random random = new Random();
        return words[random.Next(0, words.Length)];
    }

    static void TestRandomWord(string[] words)
    {
        int hello = 0;
        int world = 0;
        for (int i = 1; i < 10001; i++)
        {
            if (RandomWord(words) == "hello")
            {
                hello++;
            }
            else
            {
                world++;
            }
        }
        Console.WriteLine($"h: {hello}\nw: {world}\nsum: {hello + world}");
    }

    static string[] LoadWordsFile(string path)
    {
        while (true)
        {
            try
            {
                string[] words = File.ReadAllLines(path);
                return words;
            }
            catch (IOException exception)
            {
                Console.WriteLine($"Error reading file: {exception.Message}");
                Console.WriteLine($"Please enter a valid path for the words.wh file. Current path: {path}");
                path = Console.ReadLine();
            }
        }
    }

    static void DisplayResults(Word word, string input)
    {
        ConsoleColor bgcolor = Console.BackgroundColor;
        ConsoleColor fgcolor = Console.ForegroundColor;

        for (int i = 0; i < 5; i++)
        {
            switch (word.MatchInput(input)[i])
            {
                case 'g':
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(input[i]);
                    break;

                case 'y':
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(input[i]);
                    break;

                case 'b':
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(input[i]);
                    break;

                default:
                    break;
            }
        }
        Console.BackgroundColor = bgcolor;
        Console.ForegroundColor = fgcolor;
    }

    static void AddWordToList(string path)
    {
        Console.WriteLine("What 5-letter word would you like to add?");
        string newWord = Console.ReadLine();
        while (newWord.Length != 5 && !string.IsNullOrEmpty(newWord))
        {
            Console.WriteLine("Please enter a valid word.");
            newWord = Console.ReadLine();
        }
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine($"\n{newWord.ToLower()}");
        };
    }
}

public class Word
{
    public string WholeWord;
    public char[] ParsedWord;

    public Word(string word)
    {
        WholeWord = word;
        ParsedWord = WholeWord.ToArray();
    }

    public char[] MatchInput(string userInput)
    {
        char[] output = new char[5];
        char[] parsedUserInput = userInput.ToArray();
        bool inWord;
        for (int i = 0; i < 5; i++)
        {
            // Check if char is in word
            /*
             * g = correct spot (green)
             * y = wrong spot (yellow)
             * b = not in word (black)
             */
            inWord = false;
            foreach (char character in ParsedWord)
            {
                if (parsedUserInput[i] == character)
                {
                    inWord = true;
                    if (parsedUserInput[i] == ParsedWord[i]) 
                    {
                        output[i] = 'g';
                    }
                    else
                    {
                        output[i] = 'y';
                    }
                }
            }
            if (!inWord)
            {
                output[i] = 'b';
            }
        }
        return output;
    }
}