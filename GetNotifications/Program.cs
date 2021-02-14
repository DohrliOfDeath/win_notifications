using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.UI.Notifications.Management;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace GetNotifications
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = NowTask();
            task.Wait();
        }
        private static async Task NowTask()
        {
            UserNotificationListener listener = UserNotificationListener.Current;
            UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();
            switch (accessStatus)
            {
                case UserNotificationListenerAccessStatus.Allowed:
                    Console.WriteLine("notifications allowed");
                    break;
                case UserNotificationListenerAccessStatus.Denied:
                    Console.WriteLine("notifications denied");
                    break;
                case UserNotificationListenerAccessStatus.Unspecified:
                    Console.WriteLine("notifications something else");
                    break;
            }
            IReadOnlyList<UserNotification> notifs = await listener.GetNotificationsAsync(NotificationKinds.Toast);

            List<string> combined = new List<string>();
            foreach (var notif in notifs)
            {
                NotificationBinding toastBinding = notif.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);
                if (toastBinding != null)
                {
                    // And then get the text elements from the toast binding
                    IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();

                    // Treat the first text element as the title text
                    string titleText = textElements.FirstOrDefault()?.Text;

                    // We'll treat all subsequent text elements as body text,
                    // joining them together via newlines.
                    string bodyText = string.Join("\n", textElements.Skip(1).Select(t => t.Text));
                    Console.WriteLine("\nhead: " + titleText);
                    Console.WriteLine("body: " + bodyText);
                    
                    combined.Add(titleText);
                    combined.Add(bodyText);
                }
            }
            if (combined.Count > 2)
                throw new NotImplementedException("for now, only one new message is able to be displayed");
            Console.WriteLine("\nWritten to: " + Environment.GetEnvironmentVariable("USERPROFILE") + "\\.notificationCache.txt");
            await File.WriteAllLinesAsync(Environment.GetEnvironmentVariable("USERPROFILE") + "\\.notificationCache.txt", combined);

            //clears windows messages | throws an AggregateException, I have no idea why, nothing from google, too
            //listener.ClearNotifications();
            Environment.Exit(0);
        }
    }
}

