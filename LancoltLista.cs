using System;
using System.Collections;

namespace ZHGyak
{
    delegate void LancoltListaBejarasKezelo(Kutya kutya);

    class LancoltLista<T> : IEnumerable<T> where T : Kutya
    {
        ListaElem fej;

        public T this[int index]
        {
            get
            {
                ListaElem elem = _Kereses(index);
                return elem.tartalom;
            }

            set
            {
                ListaElem elem = _Kereses(index);
                elem.tartalom = value;
            }
        }

        class ListaElem
        {
            public T tartalom;
            public ListaElem kovetkezo;
        }

        class ListaBejaro : IEnumerator<T>
        {
            private ListaElem fej;
            private ListaElem aktualis;

            public T Current { get { return aktualis.tartalom; } }
            object IEnumerator.Current { get { return aktualis.tartalom; } }

            public ListaBejaro(ListaElem fej)
            {
                this.fej = fej;
                aktualis = new ListaElem();
                aktualis = fej.kovetkezo;
            }

            public void Dispose()
            {
                fej = null;
                aktualis = null;
            }

            public bool MoveNext()
            {
                if (aktualis == null) return false;
                aktualis = aktualis.kovetkezo;

                return aktualis != null;
            }

            public void Reset()
            {
                aktualis = new ListaElem();
                aktualis = fej;
            }
        }

        public void Beszuras(T tartalom)
        {
            ListaElem ujElem = new ListaElem();
            ujElem.kovetkezo = fej;
            ujElem.tartalom = tartalom;
            fej = ujElem;
        }

        public void RendezettBeszuras(T tartalom)
        {
            ListaElem ujElem = new ListaElem();
            ujElem.tartalom = tartalom;

            if (fej == null)
            {
                ujElem.kovetkezo = null;
                fej = ujElem;
            }
            else
            {
                if (fej.tartalom.Magassag > tartalom.Magassag)
                {
                    ujElem.kovetkezo = fej;
                    fej = ujElem;
                }
                else
                {
                    ListaElem elem = fej;
                    ListaElem segedElem = null;
                    while (elem != null && elem.tartalom.Magassag < tartalom.Magassag)
                    {
                        segedElem = elem;
                        elem = elem.kovetkezo;
                    }

                    if (elem == null)
                    {
                        ujElem.kovetkezo = null;
                        segedElem.kovetkezo = ujElem;
                    }
                    else
                    {
                        ujElem.kovetkezo = elem;
                        segedElem.kovetkezo = ujElem;
                    }
                }
            }
        }

        public void RendezettBeszurasRovidebb(T tartalom)
        {
            ListaElem ujElem = new ListaElem();
            ujElem.tartalom = tartalom;

            ListaElem elem = fej;
            ListaElem segedElem = null;
            while (elem != null && elem.tartalom.Magassag < tartalom.Magassag)
            {
                segedElem = elem;
                elem = elem.kovetkezo;
            }

            if (segedElem == null)
            {
                ujElem.kovetkezo = fej;
                fej = ujElem;
            }
            else
            {
                ujElem.kovetkezo = elem;
                segedElem.kovetkezo = ujElem;
            }
        }

        public void Bejaras(LancoltListaBejarasKezelo muvelet)
        {
            ListaElem elem = fej;
            while (elem != null)
            {
                muvelet?.Invoke(elem.tartalom);
                elem = elem.kovetkezo;
            }
        }

        public T Kereses(int magassag)
        {
            ListaElem elem = fej;
            while (elem != null)
            {
                if (elem.tartalom.Magassag == magassag)
                    return elem.tartalom;
                elem = elem.kovetkezo;
            }

            throw new ArgumentException();
        }

        public void Kereses(int magassag, LancoltLista<T> lista)
        {
            ListaElem elem = fej;
            while (elem != null)
            {
                if (elem.tartalom.Magassag >= magassag)
                    lista.Beszuras(elem.tartalom);
                elem = elem.kovetkezo;
            }
        }

        private ListaElem _Kereses(int index)
        {
            ListaElem elem = fej;
            int j = 0;
            while (elem != null && j < index)
            {
                elem = elem.kovetkezo;
                j++;
            }

            if (elem != null)
                return elem;

            throw new ArgumentException();
        }

        public void Torles(string nev)
        {
            ListaElem elem = fej;
            ListaElem segedElem = null;

            while (elem != null && elem.tartalom.Nev != nev)
            {
                segedElem = elem;
                elem = elem.kovetkezo;
            }

            if (elem != null)
            {
                if (segedElem == null)
                    fej = elem.kovetkezo;
                else segedElem.kovetkezo = elem.kovetkezo;
            }
            else throw new ArgumentException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListaBejaro(fej);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListaBejaro(fej);
        }
    }
}
