using System;

class Program
{
    static void Main(string[] args)
    {
        display("Hotel Reservation Program");
        Console.ReadLine();
    }
    static void display(string text)
    {
        Console.SetCursorPosition(0, 0);
        Console.Clear();
        string letters = "Logging Off.....";
        for (int i = 0; i < letters.Length; i++)
        {
            for (int j = 1; j < 3; j++)
            {
                Console.SetCursorPosition(5 + i, j);
                Console.Write(letters[i]);
                Console.SetCursorPosition(5 + i, j - 1);
                Console.Write(' ');
                System.Threading.Thread.Sleep(200);

            }
        }
        Environment.Exit(0);
    }
}