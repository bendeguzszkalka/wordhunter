using System.Net.Security;

internal class Program
{
    private static void Main(string[] args)
    {
        string wordsFilePath = @"C:\Users\Bendeguz\Source\Repos\wordhunter\WordHunter\words.wh";

        string[] words = LoadWordsFile(wordsFilePath);

        Console.WriteLine(RandomWord(words));
        TestRandomWord(words);
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
}