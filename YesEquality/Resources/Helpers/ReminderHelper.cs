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
                settings.TryGetValue("reminderOne", out enabled);
                return enabled;
            }
            set
            {
                settings["reminderOne"] = value;
                settings.Save();

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
                settings.TryGetValue("reminderTwo", out enabled);
                return enabled;
            }
            set
            {
                settings["reminderTwo"] = value;
                settings.Save();

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
            var day = DateTime.Now.AddSeconds(120);
#else
            var day = new DateTime(2015, 5, 22, 8, 00, 00, 00, DateTimeKind.Utc);
#endif
            // Don't set reminders after date!
            if (DateTime.Now.CompareTo(day) > 0) return;

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
                try
                {
                    ScheduledActionService.Replace(reminder);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception while replacing reminder: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Setting new reminder...");
                try
                {
                    ScheduledActionService.Add(reminder);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception while adding reminder: " + ex.Message);
                }
            }
        }
        private static void addReminderTwo()
        {
#if DEBUG
            var day = DateTime.Now.AddSeconds(20);
#else
            var day = new DateTime(2015, 5, 21, 8, 00, 00, 00, DateTimeKind.Utc);
#endif
            // Don't set reminders after date!
            if (DateTime.Now.CompareTo(day) > 0) return;

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
                try
                {
                    ScheduledActionService.Replace(reminder);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception while replacing reminder: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Setting new reminder...");
                try
                {
                    ScheduledActionService.Add(reminder);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception while adding reminder: " + ex.Message);
                }
            }
        }

        private static void removeReminderOne()
        {
            Debug.WriteLine("Removing reminderOne...");
            
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            if (notifications.Any() && notifications.First().Name == "reminderOne")
            {
                ScheduledActionService.Remove("reminderOne");
            }
            
        }
        private static void removeReminderTwo()
        {
            Debug.WriteLine("Removing reminderTwo...");

            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            if (notifications.Any() && notifications.First().Name == "reminderTwo")
            {
                ScheduledActionService.Remove("reminderTwo");
            }
        }
    }
}
