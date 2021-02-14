using System;
using System.Collections.Generic;

namespace NotificationDaemon
{
    class Program
    {
        static List<string> bufferedNotifications = new List<string>();
        static void Main(string[] args)
        {
            string[] fileList = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");

            Console.WriteLine("Hello World!");
        }
    }
}
