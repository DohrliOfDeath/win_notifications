using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
        public bool WasShown { get; set; }

        string _head;
        string _body;
        public string Head { get 
            {
                return _head;
            }
            set { _head = value; } }
        public string Body { 
            get
            {
                return _body; 
            } 
            set { _body = value; } }

        public override string ToString()
        {
            return WasShown + " |  " + Head + ": " + Body;
        }
    }
    class Program
    {
        static List<Notification> notifications = new List<Notification>();
        static void Main(string[] args)
        {
            Settings.pythonLocation = @"C:\Program Files\Python39\python.exe"; //if I set this path immediately in Settings, an ? gets written before the path

            while (true)
            {
                LaunchGetNotifications(); //listens for all current notifications and writes them into the cache file
                ReadFromFile();

                int offsetY = -55;
                for (int i = 0; i < notifications.Count; i++)
                {
                    Console.WriteLine(notifications[i]);
                    if (!notifications[i].WasShown)
                        LaunchNotification(i, offsetY+=55);
                    notifications[i].WasShown = true;
                }
                Thread.Sleep(5000);
            }
        }
        private static void LaunchGetNotifications()
        {
            //StreamReader and ProcessStartInfo only for redirecting output to debugging console of daemon
            Console.WriteLine("starting: " + Settings.getNotificationsEXELocation);
            ProcessStartInfo start = new ProcessStartInfo(Settings.getNotificationsEXELocation);
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            Process getNotifications = new Process();
            getNotifications.StartInfo = start;
            getNotifications.EnableRaisingEvents = true;
            getNotifications.Start();
            using (StreamReader sr = getNotifications.StandardOutput)
                Console.WriteLine(sr.ReadToEnd());
            getNotifications.WaitForExit();
            Console.WriteLine("finished getting notifications\n\n");
        }
        private static void LaunchNotification(int number, int offset)
        {
            if (notifications.Count > 0) //if notifications is empty
            {
                ProcessStartInfo start = new ProcessStartInfo(Settings.pythonLocation,
                    Settings.pyqtScriptLocation + " " + offset + " 5 \"" + notifications[number].Head + "\" \"" + notifications[number].Body + "\"");
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                Process script = new Process();
                script.StartInfo = start;
                script.EnableRaisingEvents = true;
                script.Start();
                //script.WaitForExit();
            }
        }
        private static void ReadFromFile()
        {
            //read file for new notification
            string[] fileList = File.ReadAllLines(Settings.notificationCacheFileLocation);
            for (int i = 0; i < fileList.Length; i += 2) //write into notifications array
            {
                bool isAlreadyInNotifications = false;
                foreach (var notif in notifications)
                    if (notif.Head == fileList[i] && notif.Body == fileList[i + 1])
                        isAlreadyInNotifications = true;

                if (!isAlreadyInNotifications)
                    notifications.Add(new Notification(fileList[i], fileList[i + 1]));
            }
        }
    }
}
