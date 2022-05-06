using System;

namespace ZHGyak
{
    // ezt kell, hogy mas osztaly is lassa
    class GrafEsemenyParameterek<T> : EventArgs
    {
        public T A { get; }
        public T B { get; }

        public GrafEsemenyParameterek(T a, T b)
        {
            A = a;
            B = b;
        }
    }

    delegate void GrafBejarasKezelo(Kutya kutya);

    class Graf<T> where T : Kutya
    {
        List<T> elek;
        List<int>[] szomszedok;
        private int n;
        public delegate void GrafEsemenyKezelo<T>(object forras, GrafEsemenyParameterek<T> parameterek);
        public event GrafEsemenyKezelo<T> ElHozzaadasEsemeny;

        public Graf(List<T> elek)
        {
            n = elek.Count;
            szomszedok = new List<int>[n];
            this.elek = elek;

            for (int i = 0; i < n; i++)
            {
                szomszedok[i] = new List<int>();
            }
        }

        public List<T> Elek()
        {
            return elek;
        }

        public void CsucsBeszuras(T csucs)
        {
            elek.Add(csucs);
            szomszedok[elek.Count-1] = new List<int>();
            for (int i = 0; i < szomszedok.Length; i++)
            {
                szomszedok[i].Add(0);
            }
        }

        public void ElHozzaadas(T honnan, T hova)
        {
            // iranyitatlan
            szomszedok[elek.IndexOf(honnan)].Add(elek.IndexOf(hova));
            szomszedok[elek.IndexOf(hova)].Add(elek.IndexOf(honnan));
            // ha kernenek esetleg, hogy jelezd, hogy lett e hozzadva akkor itt egy pelda
            ElHozzaadasEsemeny?.Invoke(this, new GrafEsemenyParameterek<T>(honnan, hova));
        }

        public bool VanEleE(T honnan, T hova)
        {
            return szomszedok[elek.IndexOf(honnan)].Contains(elek.IndexOf(hova));
        }

        public List<T> Szomszedok(T csucs)
        {
            List<T> lista = new List<T>();
            foreach (T masikCsucs in Elek())
            {
                if (VanEleE(csucs, masikCsucs))
                    lista.Add(masikCsucs);
            }

            return lista;
        }

        public void MelysegiBejaras(T kezdoCsucs, List<T> F, GrafBejarasKezelo muvelet)
        {
            F.Add(kezdoCsucs);
            muvelet?.Invoke(kezdoCsucs);
            List<T> szomszedok = Szomszedok(kezdoCsucs);
            foreach (T x in szomszedok)
            {
                if (!F.Contains(x))
                    MelysegiBejaras(x, F, muvelet);
            }
        }

        public void SzelessegiBejaras(T kezdoCsucs, GrafBejarasKezelo muvelet)
        {
            GrafBejarasKezelo _muvelet = muvelet;

            Queue<T> S = new Queue<T>();
            List<T> F = new List<T>();
            S.Enqueue(kezdoCsucs);
            F.Add(kezdoCsucs);
            while (S.Count != 0)
            {
                T k = S.Dequeue();
                _muvelet?.Invoke(k);
                foreach (T x in Szomszedok(k))
                {
                    if (!F.Contains(x))
                    {
                        S.Enqueue(x);
                        F.Add(x);
                    }
                }
            }
        }
    }
}
