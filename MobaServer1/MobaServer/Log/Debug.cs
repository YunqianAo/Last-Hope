using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;



class Debug
{
    public static void Log(string log)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(log);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("**************************");
    }
    public static void LogError(string log)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(log);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("**************************");
    }
    public static void Log(int messageID,IMessage message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nprotoID{messageID}\n{JsonHelper.SerializeObject(message)}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("**************************");
    }
}

