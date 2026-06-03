# Aplikacija-za-finansije
using Aplikacija_za_finansije;
using System;
using System.Windows.Forms;

namespace Aplikacija_za_finansije
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }
}
namespace Aplikacija_za_finansije
{
    partial class Form1
    {
        

        private void InitializeComponent()
        {
            SuspendLayout();
            ClientSize = new Size(811, 403);
            Name = "Form1";
            ResumeLayout(false);

}}
readonly Color Primarna_boja = Color.FromArgb(30, 136, 160);
readonly Color Pozadina_boja = Color.FromArgb(245, 248, 252);
readonly Color ColorCard = Color.White;
readonly Color Tekst_boja = Color.FromArgb(30, 40, 60);
readonly Color ColorMuted = Color.FromArgb(120, 140, 165);
readonly Color Zelena = Color.FromArgb(34, 197, 94);
readonly Color Crvena = Color.FromArgb(239, 68, 68);
readonly Color Zlatna = Color.FromArgb(234, 179, 8);









            
            
        }
    }
}
