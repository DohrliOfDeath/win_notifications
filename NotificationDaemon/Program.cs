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
    class Notification
    {
        public Notification(string head, string body)
        {
            WasShown = false;
            Head = head;
            Body = body;
        }
        bool WasShown { get; set; }
        string Head { get; set; }
        string Body { get; set; }
        public override string ToString()
        {
            return WasShown + "|  " + Head + ": " + Body;
        }
    }
    class Program
    {
        static List<Notification> notifications = new List<Notification>();
        static void Main(string[] args)
        {
            Settings.pythonLocation = @"C:\Program Files\Python39\python.exe"; //if I set this path immediately in Settings, an ? gets written before the path

            //LaunchGetNotifications();

            //read file for new notification
            string[] fileList = File.ReadAllLines(Settings.notificationCacheFileLocation);
            Console.WriteLine(fileList.Length);
            for(int i = 0; i< fileList.Length/2; i+=2)
            {
                notifications.Add(new Notification(fileList[i], fileList[i+1]));
            }
            Console.WriteLine(notifications[0].ToString());
            LaunchNotification(fileList);
            
            Console.ReadKey(true);
        }
        private static void LaunchGetNotifications()
        {
            //StreamReader and ProcessStartInfo only for redirecting output to debugging console of daemon
            Console.WriteLine("starting: " + Settings.getNotificationsEXELocation);
            ProcessStartInfo start = new ProcessStartInfo(Settings.getNotificationsEXELocation);
            start.RedirectStandardOutput = true;
            start.UseShellExecute = false;
            Process getNotifications = Process.Start(start);
            using (StreamReader sr = getNotifications.StandardOutput)
                Console.WriteLine(sr.ReadToEnd());
            getNotifications.WaitForExit();
            Console.WriteLine("finished getting notifications\n\n");
        }
        private static void LaunchNotification(string[] list)
        {
            if (list.Length >= 2) //if file is empty
            {
                Process script = Process.Start(Settings.pythonLocation,
                    Settings.pyqtScriptLocation + " 5 \"" + list[0] + "\" \"" + list[1] + "\"");
                script.WaitForExit();
            }
        }
    }
}
