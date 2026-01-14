using System;
using System.Windows.Forms;
using KillerDex.Infrastructure.Services;

namespace KillerDex
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize language settings before any form is created
            LanguageService.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}