using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizualizacija_problema_potujočega_potnika
{
    public class Tocka
    {
        public int x, y;

        public Tocka(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Tocka(Tocka tocka)
        {
            this.x = tocka.x;
            this.y = tocka.y;
        }

        public Tocka() { }

        public Tocka NakljucnaTocka(Random r)
        {
            Tocka tocka = new Tocka(r.Next(5, 795), r.Next(5, 647));
            return tocka;
        }

        // metoda za razhroščevanje
        public void IzpisiTocko()
        {
            Console.WriteLine("x: {0}\ty: {1}", this.x, this.y);
        }
    }
}
