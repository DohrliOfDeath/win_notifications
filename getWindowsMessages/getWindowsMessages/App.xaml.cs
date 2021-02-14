using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications.Management;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace getWindowsMessages
{
    sealed partial class App : Application
    {
        private List<string> heads = new List<string>();
        private List<string> bodies = new List<string>();

        //public App() => this.InitializeComponent();
        
        protected override void OnLaunched(LaunchActivatedEventArgs e) => _ = NowTask();

        private async Task NowTask()
        {
            UserNotificationListener listener = UserNotificationListener.Current;
            UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();
            switch (accessStatus)
            {
                case UserNotificationListenerAccessStatus.Allowed:
                    Debug.WriteLine("notifications allowed");
                    break;
                case UserNotificationListenerAccessStatus.Denied:
                    Debug.WriteLine("notifications denied");
                    break;
                case UserNotificationListenerAccessStatus.Unspecified:
                    Debug.WriteLine("notifications something else");
                    break;
            }
            IReadOnlyList<UserNotification> notifs = await listener.GetNotificationsAsync(NotificationKinds.Toast);
            foreach(var notif in notifs)
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
                    Debug.WriteLine("realhead: " + titleText);
                    Debug.WriteLine("body: " + bodyText);
                    heads.Add(titleText);
                    bodies.Add(bodyText);
                }
            }
            Current.Exit();
        }
    }
}
