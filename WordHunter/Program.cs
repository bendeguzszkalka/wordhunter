internal class Program
{
    private static void Main(string[] args)
    {
        string wordsFilePath = @"enter path of words.wh";
		try
		{
            string[] words = File.ReadAllLines(wordsFilePath);
            Console.WriteLine(randomWord(words));
        }
		catch (IOException exception)
		{
            Console.WriteLine($"Error reading file: {exception.Message}");
        }
    }

    static string randomWord(string[] words)
    {
        Random random = new Random();
        return words[random.Next(0, words.Length)];
    }
}