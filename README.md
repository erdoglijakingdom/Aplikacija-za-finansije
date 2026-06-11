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




;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
void BuildMesecniGrid(Panel card)
{
    int startX = 12, startY = 42;
    int colW = 75, rowH = 28, labelH = 18;
    int prihodiY = startY + labelH + 4;
    int rashodiY = prihodiY + rowH + 8;

    // Header row labels
    Label lblPrihodiLbl = new Label { Text = "Prihodi (€):", ForeColor = Zelena, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(startX, prihodiY + 5) };
    Label lblRashodiLbl = new Label { Text = "Rashodi (€):", ForeColor = Crvena, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(startX, rashodiY + 5) };
    card.Controls.Add(lblPrihodiLbl);
    card.Controls.Add(lblRashodiLbl);

    int offsetX = 95;

    for (int i = 0; i < 12; i++)
    {
        int x = offsetX + i * (colW + 4);

        // Month label
        Label lblMesec = new Label
        {
            Text = meseci[i],
            Font = new Font("Segoe UI", 8f, FontStyle.Bold),
            ForeColor = Primarna_boja,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(colW, labelH),
            Location = new Point(x, startY)
        };
        card.Controls.Add(lblMesec);

        // Prihodi
        tbPrihodi[i] = new TextBox
        {
            Size = new Size(colW, rowH),
            Location = new Point(x, prihodiY),
            Font = new Font("Segoe UI", 9f),
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = HorizontalAlignment.Right,
            Text = "0"
        };
        tbPrihodi[i].GotFocus += (s, e) => { if (((TextBox)s).Text == "0") ((TextBox)s).Clear(); };
        card.Controls.Add(tbPrihodi[i]);

        // Rashodi
        tbRashodi[i] = new TextBox
        {
            Size = new Size(colW, rowH),
            Location = new Point(x, rashodiY),
            Font = new Font("Segoe UI", 9f),
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = HorizontalAlignment.Right,
            Text = "0"
        };
        tbRashodi[i].GotFocus += (s, e) => { if (((TextBox)s).Text == "0") ((TextBox)s).Clear(); };
        card.Controls.Add(tbRashodi[i]);
    }

    // Porez napomena
    Label lblPorezNote = new Label
    {
        Text = "* Porez 10% se automatski obracunava na ukupne prihode",
        ForeColor = Svetlo_plava,
        Font = new Font("Segoe UI", 8f, FontStyle.Italic),
        AutoSize = true,
        Location = new Point(startX, rashodiY + rowH + 12)
    };
    card.Controls.Add(lblPorezNote);
}

private void BuildDodatneStavke(Panel card)
{
    string[] opisPlaceholder = { "Npr. Bonus", "Npr. Kirija", "Npr. Freelance", "Npr. Osiguranje", "Npr. Investicija" };
    int startY = 42;
    int rowH = 30;

    Label h1 = new Label { Text = "Opis", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Svetlo_plava, AutoSize = true, Location = new Point(12, startY - 18) };
    Label h2 = new Label { Text = "Iznos (€)", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Svetlo_plava, AutoSize = true, Location = new Point(230, startY - 18) };
    Label h3 = new Label { Text = "Tip", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Svetlo_plava, AutoSize = true, Location = new Point(340, startY - 18) };
    card.Controls.Add(h1); card.Controls.Add(h2); card.Controls.Add(h3);

    for (int i = 0; i < 5; i++)
    {
        int y = startY + i * (rowH + 6);

        tbDodatniOpis[i] = new TextBox
        {
            Location = new Point(12, y),
            Size = new Size(200, 26),
            Font = new Font("Segoe UI", 9f),
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Svetlo_plava,
            Text = opisPlaceholder[i]
        };
        int idx = i;
        tbDodatniOpis[i].GotFocus += (s, e) => { if (tbDodatniOpis[idx].ForeColor == Svetlo_plava) { tbDodatniOpis[idx].Text = ""; tbDodatniOpis[idx].ForeColor = Tekst_boja; } };
        tbDodatniOpis[i].LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tbDodatniOpis[idx].Text)) { tbDodatniOpis[idx].Text = opisPlaceholder[idx]; tbDodatniOpis[idx].ForeColor = Svetlo_plava; } };
        card.Controls.Add(tbDodatniOpis[i]);

        tbDodatniIznos[i] = new TextBox
        {
            Location = new Point(230, y),
            Size = new Size(95, 26),
            Font = new Font("Segoe UI", 9f),
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = HorizontalAlignment.Right,
            Text = "0"
        };
        tbDodatniIznos[i].GotFocus += (s, e) => { if (tbDodatniIznos[idx].Text == "0") tbDodatniIznos[idx].Clear(); };
        card.Controls.Add(tbDodatniIznos[i]);

        cbDodatniTip[i] = new ComboBox
        {
            Location = new Point(340, y),
            Size = new Size(130, 26),
            Font = new Font("Segoe UI", 9f),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cbDodatniTip[i].Items.AddRange(new object[] { "Prihod", "Rashod" });
        cbDodatniTip[i].SelectedIndex = (i % 2 == 0) ? 0 : 1;
        card.Controls.Add(cbDodatniTip[i]);
    }
}

void AnalizaTab(TabPage tab)
{
    Panel main = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

    // Summary cards row
    Panel summaryRow = new Panel
    {
        Location = new Point(15, 15),
        Size = new Size(tab.Width - 40, 110),
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
    };

    lblUkupnoPrihodi = Build_5_kartica(summaryRow, "Ukupni prihodi", "€0", Zelena, 0);
    lblUkupnoRashodi = Build_5_kartica(summaryRow, "Ukupni rashodi", "€0", Crvena, 1);
    lblPorez = Build_5_kartica(summaryRow, "Porez (10%)", "€0", Zlatna, 2);
    lblNeto = Build_5_kartica(summaryRow, "Neto prihod", "€0", Primarna_boja, 3);
    lblProsek = Build_5_kartica(summaryRow, "Prosek/mesec", "€0", Color.FromArgb(147, 51, 234), 4);

    main.Controls.Add(summaryRow);

    // Chart panel
    Panel chartCard = new Panel
    {
        Location = new Point(15, 140),
        Size = new Size(tab.Width - 40, 420),
        BackColor = Bela,
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
    };
    chartCard.Paint += ChartCard_Paint;
    panelGrafikon = chartCard;
    main.Controls.Add(chartCard);

    tab.Controls.Add(main);
    tab.Resize += (s, e) =>
    {
        summaryRow.Width = tab.Width - 40;
        chartCard.Width = tab.Width - 40;
        Rasporedjivanje_kartica(summaryRow);
        chartCard.Invalidate();
    };
}

void Rasporedjivanje_kartica(Panel red)
{
    int br = red.Controls.Count;
    if (br == 0) return;
    int w = (red.Width - (br - 1) * 8) / br;
    for (int i = 0; i < br; i++)
        red.Controls[i].SetBounds(i * (w + 8), 0, w, red.Height);
}







            
            
        }
    }
}
