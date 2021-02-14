using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

static class Settings
{
    public static string notificationCacheFileLocation { get; } = Environment.GetEnvironmentVariable("USERPROFILE") + "\\.notificationCache.txt";
    public static string pythonLocation { get; set; }
    public static string pyqtScriptLocation { get; } = @"C:\Users\Michi\source\repos\win_notifications\pyqt-notification.py";
    public static string getNotificationsEXELocation { get; } = @"C:\Users\Michi\source\repos\win_notifications\GetNotifications\bin\Debug\netcoreapp3.1\GetNotifications.exe";
}

namespace NotificationDaemon
{
    
    class Program
    {
        static List<string> bufferedNotifications = new List<string>();
        static void Main(string[] args)
        {
            Settings.pythonLocation = @"C:\Program Files\Python39\python.exe"; //if I set this path immediately in Settings, an ? gets written before the path
            Console.WriteLine("starting: " + Settings.getNotificationsEXELocation);
            Process getNotifications = Process.Start(Settings.getNotificationsEXELocation);
            Console.WriteLine("waiting for exit");
            getNotifications.WaitForExit();
            Console.WriteLine("finished getting notifications");

            //read file for new notifications
            string[] fileList = File.ReadAllLines(Settings.notificationCacheFileLocation);



            //launch notification
            ProcessStartInfo start = new ProcessStartInfo
                (Settings.pythonLocation, Settings.pyqtScriptLocation + " 5 \"" + fileList[0] + "\" \"" + fileList[1] + "\"");
            Process script = Process.Start(start);
            script.WaitForExit();


            Console.ReadKey(true);
        }
    }
}
