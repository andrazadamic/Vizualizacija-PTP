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
    public partial class Form2 : Form
    {
        public List<Tocka> nadomestneTocke = new List<Tocka>();
        public Form2()
        {
            InitializeComponent();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            nadomestneTocke.Add(new Tocka(e.X, e.Y));

            Graphics g = panel1.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.Black);


            g.FillEllipse(brush, e.X - 5, e.Y - 5, 10, 10);
                
            g.Dispose();
            brush.Dispose();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
        }
    }
}
