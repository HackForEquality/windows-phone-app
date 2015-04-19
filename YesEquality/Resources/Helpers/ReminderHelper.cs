using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesEquality.Helpers
{
    public static class ReminderHelper
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public static bool IsSetup
        {
            get 
            {
                if (!settings.Contains("reminderOne") || !settings.Contains("reminderTwo"))
                {
                    return false;
                }

                Debug.WriteLine("Reminders already setup");
                return true;
            }
        }

        public static void Setup()
        {
            Debug.WriteLine("Adding settings and new reminders");

            settings.Add("reminderOne", true);
            settings.Add("reminderTwo", true);
            settings.Save();

            addReminderOne();
            addReminderTwo();
        }
        
        public static bool ReminderOneEnabled
        {
            get 
            {
                bool enabled = true;
                settings.TryGetValue("ReminderOne", out enabled);
                return enabled;
            }
            set
            {
                settings["ReminderOne"] = value;

                if (value == true)
                {
                    addReminderOne();
                }
                else
                {
                    removeReminderOne();
                }
            }
        }

        public static bool ReminderTwoEnabled
        {
            get
            {
                bool enabled = true;
                settings.TryGetValue("ReminderTwo", out enabled);
                return enabled;
            }
            set
            {
                settings["ReminderTwo"] = value;

                if (value == true)
                {
                    addReminderTwo();
                }
                else
                {
                    removeReminderTwo();
                }
            }
        }

        private static void addReminderOne()
        {
#if DEBUG
            var day = DateTime.Now.AddSeconds(40);
#else
            var day = new DateTime(2015, 05, 22, 7, 58, 00, 00, DateTimeKind.Utc);
#endif

            Microsoft.Phone.Scheduler.Reminder reminder = new Microsoft.Phone.Scheduler.Reminder("reminderOne");
            reminder.Title = "Today is the day";
            reminder.Content = "Don't forget to vote!";
            reminder.BeginTime = day;
            reminder.ExpirationTime = day.AddDays(1);
            reminder.RecurrenceType = RecurrenceInterval.None;
            reminder.NavigationUri = new Uri("/Views/MainView.xaml", UriKind.Relative);

            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            if (notifications.Any() && notifications.First().Name == reminder.Name)
            {

                Debug.WriteLine("Replacing existing reminder...");
                ScheduledActionService.Replace(reminder);
            }
            else
            {
                Debug.WriteLine("Setting new reminder...");
                ScheduledActionService.Add(reminder);
            }
        }
        private static void addReminderTwo()
        {
#if DEBUG
            var day = DateTime.Now.AddSeconds(20);
#else
            var day = new DateTime(2015, 05, 21, 7, 58, 00, 00, DateTimeKind.Utc);
#endif

            Microsoft.Phone.Scheduler.Reminder reminder = new Microsoft.Phone.Scheduler.Reminder("reminderTwo");
            reminder.Title = "Tomorrow is a big day";
            reminder.Content = "Remind your friends to vote YES!";
            reminder.BeginTime = day;
            reminder.ExpirationTime = day.AddDays(1);
            reminder.RecurrenceType = RecurrenceInterval.None;
            reminder.NavigationUri = new Uri("/Views/MainView.xaml", UriKind.Relative);

            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            if (notifications.Any() && notifications.First().Name == reminder.Name)
            {

                Debug.WriteLine("Replacing existing reminder...");
                ScheduledActionService.Replace(reminder);
            }
            else
            {
                Debug.WriteLine("Setting new reminder...");
                ScheduledActionService.Add(reminder);
            }
        }

        private static void removeReminderOne()
        {
            Debug.WriteLine("Removing reminder...");
            ScheduledActionService.Remove("reminderOne");
        }
        private static void removeReminderTwo()
        {
            Debug.WriteLine("Removing reminder...");
            ScheduledActionService.Remove("reminderTwo");
        }
    }
}
