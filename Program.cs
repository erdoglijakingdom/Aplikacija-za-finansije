using Aplikacija_za_finansiije;
using System;
using System.Windows.Forms;

namespace Aplikacija_za_finansiije
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}