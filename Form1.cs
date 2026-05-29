using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aplikacija_za_finansiije
{
    public partial class Form1 : Form
    {
        // Colors
        private readonly Color ColorPrimary = Color.FromArgb(30, 136, 160);
        private readonly Color ColorBg = Color.FromArgb(245, 248, 252);
        private readonly Color ColorCard = Color.White;
        private readonly Color ColorText = Color.FromArgb(30, 40, 60);
        private readonly Color ColorMuted = Color.FromArgb(120, 140, 165);
        private readonly Color ColorGreen = Color.FromArgb(34, 197, 94);
        private readonly Color ColorRed = Color.FromArgb(239, 68, 68);
        private readonly Color ColorGold = Color.FromArgb(234, 179, 8);

        private string[] meseci = { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Avg", "Sep", "Okt", "Nov", "Dec" };
        private TextBox[] tbPrihodi = new TextBox[12];
        private TextBox[] tbRashodi = new TextBox[12];

        // Extra labels
        private TextBox[] tbDodatniOpis = new TextBox[5];
        private TextBox[] tbDodatniIznos = new TextBox[5];
        private ComboBox[] cbDodatniTip = new ComboBox[5];

        private TextBox tbIme, tbPrezime;
        private Panel panelGrafikon;
        private Label lblUkupnoPrihodi, lblUkupnoRashodi, lblPorez, lblNeto, lblProsek;

        private TabControl tabControl;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Finansijski Menadžment";
            this.Size = new Size(1200, 850);
            this.MinimumSize = new Size(1000, 700);
            this.BackColor = ColorBg;
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;

            BuildUI();
        }

        private void BuildUI()
        {
            // Header
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = ColorPrimary
            };
            Label lblTitle = new Label
            {
                Text = "??  Finansijski Menadžment",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 18)
            };
            header.Controls.Add(lblTitle);
            this.Controls.Add(header);

            // Tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10f),
                Padding = new Point(20, 8)
            };
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl_DrawItem;

            TabPage tabUnos = new TabPage("  Unos podataka  ") { BackColor = ColorBg };
            TabPage tabAnaliza = new TabPage("  Analiza & Grafikon  ") { BackColor = ColorBg };

            BuildUnosTab(tabUnos);
            BuildAnalizaTab(tabAnaliza);

            tabControl.TabPages.Add(tabUnos);
            tabControl.TabPages.Add(tabAnaliza);
            this.Controls.Add(tabControl);
            tabControl.BringToFront();
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabControl.TabPages[e.Index];
            Rectangle rect = tabControl.GetTabRect(e.Index);
            bool selected = (tabControl.SelectedIndex == e.Index);

            using (SolidBrush bg = new SolidBrush(selected ? ColorPrimary : Color.FromArgb(220, 230, 240)))
                e.Graphics.FillRectangle(bg, rect);

            using (SolidBrush fg = new SolidBrush(selected ? Color.White : ColorMuted))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                e.Graphics.DrawString(page.Text, new Font("Segoe UI", 10f, selected ? FontStyle.Bold : FontStyle.Regular), fg, rect, sf);
        }

        private void BuildUnosTab(TabPage tab)
        {
            Panel scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(15) };

            int y = 15;

            // Li?ni podaci
            Panel cardLicni = CreateCard(scroll, "??  Li?ni podaci", ref y, 100);
            AddLabelInCard(cardLicni, "Ime:", 10, 40);
            tbIme = AddTextBoxInCard(cardLicni, 80, 40, 200);
            AddLabelInCard(cardLicni, "Prezime:", 300, 40);
            tbPrezime = AddTextBoxInCard(cardLicni, 380, 40, 200);

            // Mese?ni prihodi i rashodi
            Panel cardMeseci = CreateCard(scroll, "??  Mese?ni prihodi i rashodi (€)", ref y, 320);
            BuildMesecniGrid(cardMeseci);

            // Dodatni stavke
            Panel cardDodatni = CreateCard(scroll, "?  Dodatne stavke (prihodi / rashodi)", ref y, 230);
            BuildDodatneStavke(cardDodatni);

            // Dugme Izra?unaj
            Button btnCalc = new Button
            {
                Text = "??  IZRA?UNAJ ANALIZU",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                BackColor = ColorPrimary,
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

        private Panel CreateCard(Panel parent, string title, ref int y, int height)
        {
            Panel card = new Panel
            {
                Location = new Point(15, y),
                Size = new Size(parent.Width - 50, height),
                BackColor = ColorCard,
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
                ForeColor = ColorPrimary,
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
            Label l = new Label { Text = text, AutoSize = true, Location = new Point(x, y), ForeColor = ColorText, Font = new Font("Segoe UI", 9f) };
            card.Controls.Add(l);
            return l;
        }

        private TextBox AddTextBoxInCard(Panel card, int x, int y, int width)
        {
            TextBox tb = new TextBox { Location = new Point(x, y), Size = new Size(width, 26), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle };
            card.Controls.Add(tb);
            return tb;
        }

        private void BuildMesecniGrid(Panel card)
        {
            int startX = 12, startY = 42;
            int colW = 75, rowH = 28, labelH = 18;
            int prihodiY = startY + labelH + 4;
            int rashodiY = prihodiY + rowH + 8;

            // Header row labels
            Label lblPrihodiLbl = new Label { Text = "Prihodi (€):", ForeColor = ColorGreen, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(startX, prihodiY + 5) };
            Label lblRashodiLbl = new Label { Text = "Rashodi (€):", ForeColor = ColorRed, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(startX, rashodiY + 5) };
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
                    ForeColor = ColorPrimary,
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
                Text = "* Porez 10% se automatski obra?unava na ukupne prihode",
                ForeColor = ColorMuted,
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

            Label h1 = new Label { Text = "Opis", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = ColorMuted, AutoSize = true, Location = new Point(12, startY - 18) };
            Label h2 = new Label { Text = "Iznos (€)", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = ColorMuted, AutoSize = true, Location = new Point(230, startY - 18) };
            Label h3 = new Label { Text = "Tip", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = ColorMuted, AutoSize = true, Location = new Point(340, startY - 18) };
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
                    ForeColor = ColorMuted,
                    Text = opisPlaceholder[i]
                };
                int idx = i;
                tbDodatniOpis[i].GotFocus += (s, e) => { if (tbDodatniOpis[idx].ForeColor == ColorMuted) { tbDodatniOpis[idx].Text = ""; tbDodatniOpis[idx].ForeColor = ColorText; } };
                tbDodatniOpis[i].LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tbDodatniOpis[idx].Text)) { tbDodatniOpis[idx].Text = opisPlaceholder[idx]; tbDodatniOpis[idx].ForeColor = ColorMuted; } };
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

        private void BuildAnalizaTab(TabPage tab)
        {
            Panel main = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            // Summary cards row
            Panel summaryRow = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(tab.Width - 40, 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            lblUkupnoPrihodi = BuildSummaryCard(summaryRow, "Ukupni prihodi", "€0", ColorGreen, 0);
            lblUkupnoRashodi = BuildSummaryCard(summaryRow, "Ukupni rashodi", "€0", ColorRed, 1);
            lblPorez = BuildSummaryCard(summaryRow, "Porez (10%)", "€0", ColorGold, 2);
            lblNeto = BuildSummaryCard(summaryRow, "Neto prihod", "€0", ColorPrimary, 3);
            lblProsek = BuildSummaryCard(summaryRow, "Prosek/mesec", "€0", Color.FromArgb(147, 51, 234), 4);

            main.Controls.Add(summaryRow);

            // Chart panel
            Panel chartCard = new Panel
            {
                Location = new Point(15, 140),
                Size = new Size(tab.Width - 40, 420),
                BackColor = ColorCard,
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
                RebuildSummaryCardSizes(summaryRow);
                chartCard.Invalidate();
            };
        }

        private void RebuildSummaryCardSizes(Panel row)
        {
            int count = row.Controls.Count;
            if (count == 0) return;
            int w = (row.Width - (count - 1) * 8) / count;
            for (int i = 0; i < count; i++)
                row.Controls[i].SetBounds(i * (w + 8), 0, w, row.Height);
        }

        private Label BuildSummaryCard(Panel row, string title, string value, Color accent, int index)
        {
            Panel card = new Panel { BackColor = ColorCard };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, accent.R, accent.G, accent.B)))
                    e.Graphics.FillRectangle(sb, 0, card.Height - 5, card.Width, 5);
                using (Pen p = new Pen(Color.FromArgb(220, 230, 245)))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };
            row.Controls.Add(card);

            Label lblTitle = new Label
            {
                Text = title.ToUpper(),
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = ColorMuted,
                AutoSize = false,
                Dock = DockStyle.None,
                Location = new Point(10, 14),
                Size = new Size(200, 16)
            };
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = accent,
                AutoSize = false,
                Location = new Point(8, 34),
                Size = new Size(200, 40)
            };
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            // Auto-resize labels with card
            card.Resize += (s, e) =>
            {
                lblTitle.Width = card.Width - 16;
                lblValue.Width = card.Width - 16;
            };

            return lblValue;
        }

        // ==================== Grafikon ====================

        private double[] _chartPrihodi = new double[12];
        private double[] _chartRashodi = new double[12];
        private string _chartIme = "";

        private void ChartCard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Panel panel = (Panel)sender;
            int w = panel.Width;
            int h = panel.Height;
            int padL = 65, padR = 20, padT = 50, padB = 60;
            int chartW = w - padL - padR;
            int chartH = h - padT - padB;

            // Title
            using (Font fTitle = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (SolidBrush br = new SolidBrush(ColorText))
                g.DrawString($"Mese?na analiza  {_chartIme}", fTitle, br, padL, 14);

            // Background grid
            double maxVal = 0;
            for (int i = 0; i < 12; i++) maxVal = Math.Max(maxVal, Math.Max(_chartPrihodi[i], _chartRashodi[i]));
            if (maxVal == 0) maxVal = 1000;
            maxVal = Math.Ceiling(maxVal / 200.0) * 200 + 200;

            int gridLines = 5;
            using (Pen gridPen = new Pen(Color.FromArgb(235, 240, 248), 1))
            using (Font fGrid = new Font("Segoe UI", 7.5f))
            using (SolidBrush gridBr = new SolidBrush(ColorMuted))
            {
                for (int i = 0; i <= gridLines; i++)
                {
                    double val = maxVal * i / gridLines;
                    int yGrid = padT + chartH - (int)(chartH * i / gridLines);
                    g.DrawLine(gridPen, padL, yGrid, padL + chartW, yGrid);
                    string label = "€" + ((int)val).ToString("N0");
                    g.DrawString(label, fGrid, gridBr, 2, yGrid - 8);
                }
            }

            // X axis
            using (Pen axisPen = new Pen(Color.FromArgb(200, 215, 230), 1))
                g.DrawLine(axisPen, padL, padT + chartH, padL + chartW, padT + chartH);

            // Bars
            double barGroupW = (double)chartW / 12;
            double barW = barGroupW * 0.35;
            double gap = barGroupW * 0.05;

            using (Font fLbl = new Font("Segoe UI", 7.5f))
            using (Font fMonth = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (SolidBrush monthBr = new SolidBrush(ColorText))
            using (StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center })
            {
                for (int i = 0; i < 12; i++)
                {
                    double groupX = padL + i * barGroupW + barGroupW * 0.07;
                    int xPrihod = (int)(groupX);
                    int xRashod = (int)(groupX + barW + gap);

                    // Prihod bar
                    int hPrihod = _chartPrihodi[i] > 0 ? (int)(chartH * _chartPrihodi[i] / maxVal) : 0;
                    int yPrihod = padT + chartH - hPrihod;
                    Rectangle rP = new Rectangle(xPrihod, yPrihod, (int)barW, hPrihod);
                    if (hPrihod > 0)
                    {
                        using (LinearGradientBrush lgb = new LinearGradientBrush(
                            new Point(rP.X, rP.Y), new Point(rP.X, rP.Bottom),
                            Color.FromArgb(255, 52, 211, 153), Color.FromArgb(200, 34, 197, 94)))
                            g.FillRectangle(lgb, rP);

                        // Value label
                        string valStr = "€" + ((int)_chartPrihodi[i]).ToString("N0");
                        using (SolidBrush vbr = new SolidBrush(ColorGreen))
                            g.DrawString(valStr, fLbl, vbr, xPrihod + (int)barW / 2, yPrihod - 14, sfCenter);
                    }

                    // Rashod bar
                    int hRashod = _chartRashodi[i] > 0 ? (int)(chartH * _chartRashodi[i] / maxVal) : 0;
                    int yRashod = padT + chartH - hRashod;
                    Rectangle rR = new Rectangle(xRashod, yRashod, (int)barW, hRashod);
                    if (hRashod > 0)
                    {
                        using (LinearGradientBrush lgb = new LinearGradientBrush(
                            new Point(rR.X, rR.Y), new Point(rR.X, rR.Bottom),
                            Color.FromArgb(255, 252, 100, 100), Color.FromArgb(200, 239, 68, 68)))
                            g.FillRectangle(lgb, rR);

                        string valStr = "€" + ((int)_chartRashodi[i]).ToString("N0");
                        using (SolidBrush vbr = new SolidBrush(ColorRed))
                            g.DrawString(valStr, fLbl, vbr, xRashod + (int)barW / 2, yRashod - 14, sfCenter);
                    }

                    // Month label
                    g.DrawString(meseci[i], fMonth, monthBr, (int)(groupX + barW), padT + chartH + 8, sfCenter);
                }
            }

            // Legend
            int legX = padL + 10, legY = 14;
            DrawLegendItem(g, legX + 180, legY, ColorGreen, "Prihodi");
            DrawLegendItem(g, legX + 300, legY, ColorRed, "Rashodi");
        }

        private void DrawLegendItem(Graphics g, int x, int y, Color color, string text)
        {
            using (SolidBrush sb = new SolidBrush(color))
                g.FillRectangle(sb, x, y + 2, 14, 14);
            using (Font f = new Font("Segoe UI", 9f))
            using (SolidBrush tb = new SolidBrush(ColorText))
                g.DrawString(text, f, tb, x + 18, y);
        }

        // ==================== Izra?unaj ====================

        private void BtnCalc_Click(object sender, EventArgs e)
        {
            string ime = tbIme.Text.Trim();
            string prezime = tbPrezime.Text.Trim();
            if (string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(prezime))
            {
                MessageBox.Show("Molimo unesite ime i prezime.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double ukupnoPrihodi = 0, ukupnoRashodi = 0;
            double[] prihodiMes = new double[12];
            double[] rashodiMes = new double[12];

            for (int i = 0; i < 12; i++)
            {
                double.TryParse(tbPrihodi[i].Text.Replace(",", "."), out prihodiMes[i]);
                double.TryParse(tbRashodi[i].Text.Replace(",", "."), out rashodiMes[i]);
                ukupnoPrihodi += prihodiMes[i];
                ukupnoRashodi += rashodiMes[i];
            }

            // Dodatne stavke
            double dodatniPrihodi = 0, dodatniRashodi = 0;
            for (int i = 0; i < 5; i++)
            {
                double iznos;
                double.TryParse(tbDodatniIznos[i].Text.Replace(",", "."), out iznos);
                if (cbDodatniTip[i].SelectedItem?.ToString() == "Prihod")
                    dodatniPrihodi += iznos;
                else
                    dodatniRashodi += iznos;
            }

            ukupnoPrihodi += dodatniPrihodi;
            ukupnoRashodi += dodatniRashodi;

            double porez = ukupnoPrihodi * 0.10;
            double neto = ukupnoPrihodi - porez - ukupnoRashodi;
            double prosek = (ukupnoPrihodi - porez) / 12.0;

            // Ažuriraj labele
            lblUkupnoPrihodi.Text = "€" + ukupnoPrihodi.ToString("N0");
            lblUkupnoRashodi.Text = "€" + ukupnoRashodi.ToString("N0");
            lblPorez.Text = "€" + porez.ToString("N0");
            lblNeto.Text = "€" + neto.ToString("N0");
            lblNeto.ForeColor = neto >= 0 ? ColorGreen : ColorRed;
            lblProsek.Text = "€" + prosek.ToString("N0");

            // Grafikon
            _chartPrihodi = prihodiMes;
            _chartRashodi = rashodiMes;
            _chartIme = $"— {ime} {prezime}";
            panelGrafikon?.Invalidate();

            // Prebaci na analizu
            tabControl.SelectedIndex = 1;
        }
    }
}

