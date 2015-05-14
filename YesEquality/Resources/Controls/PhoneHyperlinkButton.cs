// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using System.Text.RegularExpressions;

namespace YesEquality.Resources
{
    /// <summary>
    /// An extended HyperlinkButton control that uses the Tag property to
    /// open the web browser, compose an e-mail, text message, or make a call.
    /// </summary>
    public class PhoneHyperlinkButton : HyperlinkButton
    {
        /// <summary>
        /// Handles the click event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            Debug.Assert(Tag is string, "You need to set the Tag property!");
            string tag = Tag as string;

            if (tag == null)
            {
                return;
            }

            // Review
            if (tag.Contains("review"))
            {
                Review();
                return;
            }

            // List all apps in the store
            var pattern = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b";
            var match = Regex.Match(tag, pattern, RegexOptions.IgnoreCase);
            if (tag.StartsWith("store:"))
            {
                var terms = tag.Split(':');

                if (terms[1] == null)
                {
                    return;
                }

                Search(terms[1]);
                return;
            }

            if (tag.StartsWith("mailto:"))
            {
                // Email
                Email(tag.Substring(7));
                return;
            }
            
            if (tag.StartsWith("http://"))
            {
                // Browser
                WebBrowserTask wbt = new WebBrowserTask { Uri = new Uri(tag) };

                try
                {
                    wbt.Show();
                }
                catch (Exception) { }
                return;
            }
        }

        private void Review()
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            try
            {
                marketplaceReviewTask.Show();
            }
            catch(Exception) {};
        }

        private void Search(string terms)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
            marketplaceSearchTask.ContentType = MarketplaceContentType.Applications;
            marketplaceSearchTask.SearchTerms = terms;

            try
            {
                marketplaceSearchTask.Show();
            }
            catch (Exception) {};
        }

        private void Email(string s)
        {
            IDictionary<string, string> d;
            string to = GetAddress(s, out d);

            EmailComposeTask ect = new EmailComposeTask
            {
                To = to,
            };

            string cc;
            if (d.TryGetValue("cc", out cc))
            {
                ect.Cc = cc;
            }

            string subject;
            if (d.TryGetValue("subject", out subject))
            {
                ect.Subject = subject;
            }

            string body;
            if (d.TryGetValue("body", out body))
            {
                ect.Body = body;
            }

            try
            {
                ect.Show();
            }
            catch (InvalidOperationException e) 
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
            
        }

        private static string GetAddress(string input, out IDictionary<string, string> query)
        {
            query = new Dictionary<string, string>(StringComparer.Ordinal);
            int q = input.IndexOf('?');
            string address = input;
            if (q >= 0)
            {
                address = input.Substring(0, q);
                ParseQueryStringToDictionary(input.Substring(q + 1), query);
            }
            return address;
        }

        private static void ParseQueryStringToDictionary(string queryString, IDictionary<string, string> dictionary)
        {
            foreach (string str in queryString.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                int index = str.IndexOf("=", StringComparison.Ordinal);
                if (index == -1)
                {
                    dictionary.Add(HttpUtility.UrlDecode(str), string.Empty);
                }
                else
                {
                    dictionary.Add(HttpUtility.UrlDecode(str.Substring(0, index)), HttpUtility.UrlDecode(str.Substring(index + 1)));
                }
            }
        }
    }
}