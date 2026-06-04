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




;;;;;;;;;
/*private void Nacrtaj(object sender, DrawItemEventArgs e)
{
    TabPage page = tabControl.TabPages[e.Index];
    Rectangle rect = tabControl.GetTabRect(e.Index);
    bool selected = (tabControl.SelectedIndex == e.Index);

    using (SolidBrush bg = new SolidBrush(selected ? Primarna_boja : Color.FromArgb(220, 230, 240)))
        e.Graphics.FillRectangle(bg, rect);

    using (SolidBrush fg = new SolidBrush(selected ? Color.White : Svetlo_plava))
    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        e.Graphics.DrawString(page.Text, new Font("Segoe UI", 10f, selected ? FontStyle.Bold : FontStyle.Regular), fg, rect, sf);
}*/



private void UnosTab(TabPage tab)
{
    Panel scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(15) };

    int y = 15;

    // Li?ni podaci
    Panel cardLicni = CreateCard(scroll, "Licni podaci", ref y, 100);
    AddLabelInCard(cardLicni, "Ime:", 10, 40);
    tbIme = AddTextBoxInCard(cardLicni, 80, 40, 200);
    AddLabelInCard(cardLicni, "Prezime:", 300, 40);
    tbPrezime = AddTextBoxInCard(cardLicni, 380, 40, 200);

    // Mese?ni prihodi i rashodi
    Panel cardMeseci = CreateCard(scroll, "Mesecni prihodi i rashodi (€)", ref y, 320);
    BuildMesecniGrid(cardMeseci);

    // Dodatni stavke
    Panel cardDodatni = CreateCard(scroll, "Dodatne stavke (prihodi / rashodi)", ref y, 230);
    BuildDodatneStavke(cardDodatni);

    // Dugme Izra?unaj
    Button btnCalc = new Button
    {
        Text = "IZRACUNAJ ANALIZU",
        Font = new Font("Segoe UI", 12f, FontStyle.Bold),
        BackColor = Primarna_boja,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Size = new Size(260, 46),
        Location = new Point(20, y + 10),
        Cursor = Cursors.Hand
    };
    btnCalc.FlatAppearance.BorderSize = 0;
    btnCalc.Click += BtnCalc_Click;
    scroll.Controls.Add(btnCalc);

    tab.Controls.Add(scroll);
}

Panel CreateCard(Panel parent, string title, ref int y, int height)
{
    Panel card = new Panel
    {
        Location = new Point(15, y),
        Size = new Size(parent.Width - 50, height),
        BackColor = Bela,
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
    };
    card.Paint += (s, e) =>
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen p = new Pen(Color.FromArgb(220, 230, 245), 1))
            e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
    };

    Label lbl = new Label
    {
        Text = title,
        Font = new Font("Segoe UI", 10f, FontStyle.Bold),
        ForeColor = Primarna_boja,
        AutoSize = true,
        Location = new Point(12, 10)
    };
    Panel divider = new Panel
    {
        Location = new Point(12, 30),
        Size = new Size(card.Width - 24, 2),
        BackColor = Color.FromArgb(230, 238, 248)
    };
    card.Controls.Add(lbl);
    card.Controls.Add(divider);
    parent.Controls.Add(card);
    y += height + 15;
    return card;
}

private Label AddLabelInCard(Panel card, string text, int x, int y)
{
    Label l = new Label { Text = text, AutoSize = true, Location = new Point(x, y), ForeColor = Tekst_boja, Font = new Font("Segoe UI", 9f) };
    card.Controls.Add(l);
    return l;
}

private TextBox AddTextBoxInCard(Panel card, int x, int y, int width)
{
    TextBox tb = new TextBox { Location = new Point(x, y), Size = new Size(width, 26), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle };
    card.Controls.Add(tb);
    return tb;
}







            
            
        }
    }
}
