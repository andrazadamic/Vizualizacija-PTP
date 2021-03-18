using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vizualizacija_problema_potujočega_potnika
{

    public partial class Form1 : Form
    {
        // Deklariramo seznam razreda Tocka
        List<Tocka> tocke = new List<Tocka>();

        double minD;
        static Tocka[] minT = new Tocka[0];

        public Form1()
        {
            InitializeComponent();
        }

        // V seznam "točke" shranimo naključno generirane točke
        private void Form1_Load(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                tocke.Add(new Tocka().NakljucnaTocka(r));
            }
        }

        // Ozadje panel1 pobarvamo v belo barvo in narišemo točke iz seznamu "točke"
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.White;
            NarisiTocke();
        }

        // Algoritem Najbližji sosed (ang. Nearest Neighbour)
        public void NajblizjiSosed()
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Black);

            label1.Text = "Dolžina poti: ";
            label2.Text = "Čas delovanja: ";
            double d = 0;
            double koncniD = 0;
            double minD;
            int minTocka = 0;
            int zacetnaTocka = 0;
            int trenutnaTocka = zacetnaTocka;
            List<int> obiskaneTocke = new List<int>();
            obiskaneTocke.Add(zacetnaTocka);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            while (obiskaneTocke.Count <= tocke.Count)
            {
                minD = 999999999;
                for (int j = 0; j < tocke.Count; j++)
                {
                    if (trenutnaTocka == j || obiskaneTocke.Contains(j))
                        continue;

                    else
                    {
                        d = Math.Sqrt(Math.Pow(tocke[j].x - tocke[trenutnaTocka].x, 2) + Math.Pow(tocke[j].y - tocke[trenutnaTocka].y, 2));

                        if (d < minD)
                        {
                            minD = d;
                            minTocka = j;
                        }
                    }
                }


                g.DrawLine(pen, tocke[trenutnaTocka].x, tocke[trenutnaTocka].y, tocke[minTocka].x, tocke[minTocka].y);
                obiskaneTocke.Add(minTocka);
                koncniD += d;
                trenutnaTocka = minTocka;

            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            label1.Text += koncniD;
            label2.Text += elapsedMs + " ms";
            g.DrawLine(pen, tocke[trenutnaTocka].x, tocke[trenutnaTocka].y, tocke[zacetnaTocka].x, tocke[zacetnaTocka].y);
            g.Dispose();
            pen.Dispose();
        }

        // S klikom na gumb izvedemo algoritem in ga vizualiziramo
        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    NarisiTocke();
                    NajblizjiSosed();
                    break;

                case 1:
                    NarisiTocke();
                    BruteForce();
                    break;

                default:
                    break;
            }

        }

        // S klikom na gumb počistimo črte
        private void button2_Click(object sender, EventArgs e)
        {
            NarisiTocke();
        }

        // Počisti "panel1" in nariše točke iz seznama "točke"
        // To lahko uporabimo da izbrišemo črte ali pa da narišemo nove točke
        public void NarisiTocke()
        {
            Graphics g = panel1.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.Black);
            SolidBrush redBrush = new SolidBrush(Color.Red);

            g.Clear(Color.White);

            for (int i = 0; i < tocke.Count; i++)
            {
                if (i == 0)
                {
                    g.FillEllipse(redBrush, tocke[i].x - 5, tocke[i].y - 5, 10, 10);
                }
                else
                {
                    g.FillEllipse(brush, tocke[i].x - 5, tocke[i].y - 5, 10, 10);
                }
            }
            g.Dispose();
            brush.Dispose();
            redBrush.Dispose();
        }

        // S klikom na gumb naključno generiramo nove točke in jih narišemo
        private void button3_Click(object sender, EventArgs e)
        {
            tocke.Clear();
            Random r = new Random();
            panel1.BackColor = Color.White;

            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                tocke.Add(new Tocka().NakljucnaTocka(r));
            }

            NarisiTocke();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();

            if (f.ShowDialog() == DialogResult.OK)
            {
                tocke = f.nadomestneTocke;
                NarisiTocke();
            }
        }



        public void BruteForce()
        {
            minD = 999999999;
            label1.Text = "Dolžina poti: ";
            label2.Text = "Čas delovanja: ";
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            Tocka[] lt = tocke.ToArray();
            minT = tocke.ToArray();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            Permute(lt, 0, lt.Length - 1);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            for (int i = 1; i < minT.Length; i++)
            {
                g.DrawLine(pen, minT[i - 1].x, minT[i - 1].y, minT[i].x, minT[i].y);
            }
            g.DrawLine(pen, minT[minT.Length - 1].x, minT[minT.Length - 1].y, minT[0].x, minT[0].y);

            label1.Text += minD;
            label2.Text += elapsedMs + " ms";

        }

        public void Swap(ref Tocka t1, ref Tocka t2)
        {
            Tocka temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public void Permute(Tocka[] lt, int recursionDepth, int maxDepth)
        {

            if (recursionDepth == maxDepth)
            {
                Primerjava(lt);
                return;
            }

            for (int i = recursionDepth; i <= maxDepth; i++)
            {
                Swap(ref lt[recursionDepth], ref lt[i]);
                Permute(lt, recursionDepth + 1, maxDepth);

                Swap(ref lt[recursionDepth], ref lt[i]);
            }
        }

        public double Dolzina(Tocka[] t)
        {
            double d = 0;
            for (int i = 1; i < t.Length; i++)
            {
                d += Math.Sqrt(Math.Pow(t[i].x - t[i - 1].x, 2) + Math.Pow(t[i].y - t[i - 1].y, 2));
            }
            d += Math.Sqrt(Math.Pow(t[t.Length - 1].x - t[0].x, 2) + Math.Pow(t[t.Length - 1].y - t[0].y, 2));
            return d;
        }

        public void Primerjava(Tocka[] t)
        {
            double d = Dolzina(t);
            if (d < minD)
            {
                minT = (Tocka[])t.Clone();
                minD = d;

            }
        }
    }
}
