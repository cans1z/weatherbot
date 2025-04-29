namespace weatherbot;

internal class Host
{
    private static void Main(string[] args)
    {
        var weatherbot = new Management("7814611141:AAGGDBntR2aPGqQ7AJcvxnzgPbMX-9JSaDk");
        //weatherbot.AddUser(new Models.User { TgId = 1231321312});
        weatherbot.Start();
        Console.ReadLine();
    }
}