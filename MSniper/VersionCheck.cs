﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSniper
{
    public class NecroWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 10000;
            return w;
        }
    }

    public static class VersionCheck
    {
        public static string VersionUri =
            "https://raw.githubusercontent.com/msx752/MSniper/master/MSniper/Properties/AssemblyInfo.cs";
        public static Version RemoteVersion;
        public static bool IsLatest()
        {
            try
            {
                var regex = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]");
                var match = regex.Match(DownloadServerVersion());

                if (!match.Success)
                    return false;

                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}");
                RemoteVersion = gitVersion;
                if (gitVersion >= Assembly.GetExecutingAssembly().GetName().Version)
                    return false;
            }
            catch (Exception)
            {
                return true; //better than just doing nothing when git server down
            }

            return true;
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new NecroWebClient())
            {
                return wC.DownloadString(VersionUri);
            }
        }
    }
}
