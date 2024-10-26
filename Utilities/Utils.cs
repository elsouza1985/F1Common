using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FC1_CC.Common.Utilities
{
    public class Utils
    {
        public static bool TryParseJson<T>(string @this, out T result)
        {
            try
            {


                bool success = true;
                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        success = false;
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error
                };
                result = JsonConvert.DeserializeObject<T>(@this, settings);
                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = default(T);
                return false;
            }
        }
        public static string FormatStringAsURL(string input)
        {
            if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
            {
                return input;
            }

            string urlFriendly = Regex.Replace(input, @"[^A-Za-z0-9-]", "").Replace(" ", "-").ToLower();

            if (!urlFriendly.StartsWith("http://") && !urlFriendly.StartsWith("https://"))
            {
                urlFriendly = "http://" + urlFriendly;
            }

            return urlFriendly;
        }
    }
}
