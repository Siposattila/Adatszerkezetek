using System;

namespace ZHGyak
{
    delegate bool Feltetel(Kutya kutya);
    delegate void BinarisBejarasKezelo(Kutya kutya);

    // binariskeresofahoz is lehet implementalni az IEnumerable<T> interfeszt (az nem biztos, hogy kerik)
    class BinarisKeresofa<T> where T : Kutya
    {
        FaElem gyoker;

        class FaElem
        {
            public FaElem bal, jobb;
            public T tartalom;

            public FaElem(T tartalom)
            {
                this.tartalom = tartalom;
                bal = null;
                jobb = null;
            }
        }

        public void Beszuras(T tartalom)
        {
            _Beszuras(ref gyoker, tartalom);
        }

        // nem csak itt de itt is ervenyes nem muszaj az _ a szignaturaba mert overloading-gal is lehet
        // a ref (cimszerint) kulcsszo akkor kell ha tudod, hogy modosulni fog a fa!!!
        private void _Beszuras(ref FaElem elem, T tartalom)
        {
            if (elem == null)
                elem = new FaElem(tartalom);
            else
            {
                // lehet, hogy kerni fogjak, hogy IComparable interfeszt implementaljanak
                // akkor megkoteskent megkell adnod! where T : IComparable
                // emellett a CompareTo-t kell hasznalnod az osszehasonlitasra (1 0 -1)
                // tehat akkor elem.tartalom.CompareTo(tartalom) < 0 (jobbra)
                // elem.tartalom.CompareTo(tartalom) > 0 (balra)
                // mas esetben nulla tehat egyenlo vagyis ilyen mar van
                if (elem.tartalom.Magassag < tartalom.Magassag)
                    _Beszuras(ref elem.jobb, tartalom);
                else if (elem.tartalom.Magassag > tartalom.Magassag)
                    _Beszuras(ref elem.bal, tartalom);
                else throw new MarVanIlyenElemException($"Már szerepel ilyen magasságú kutya a fában! {tartalom.Magassag}");
            }
        }

        // itt jelenleg a kulcs az a kutya magassaga nezd a Beszurast!
        public T Kereses(int magassag)
        {
            return _Kereses(gyoker, magassag);
        }

        private T _Kereses(FaElem elem, int magassag)
        {
            if (elem != null)
            {
                if (elem.tartalom.Magassag > magassag)
                    return _Kereses(elem.bal, magassag);
                else
                {
                    if (elem.tartalom.Magassag < magassag)
                        return _Kereses(elem.jobb, magassag);
                    else return elem.tartalom;
                }
            }

            throw new ArgumentException();
        }

        public LancoltLista<T> Kivalogat(Feltetel muvelet)
        {
            if (muvelet != null)
            {
                LancoltLista<T> lista = new LancoltLista<T>();
                _Kivalogat(gyoker, muvelet, lista);

                return lista;
            }

            throw new ArgumentNullException();
        }

        private void _Kivalogat(FaElem elem, Feltetel muvelet, LancoltLista<T> lista)
        {
            if (elem != null)
            {
                // Itt az, hogy milyen a bejaras ugyanugy mukodik mint a sima bejarasok Inorder stb. (ez most egy Inorder)
                _Kivalogat(elem.jobb, muvelet, lista);
                if (muvelet(elem.tartalom))
                    lista.Beszuras(elem.tartalom);
                _Kivalogat(elem.bal, muvelet, lista);
            }
        }

        public void PreorderBejaras(BinarisBejarasKezelo muvelet)
        {
            _PreorderBejaras(gyoker, muvelet);
        }

        private void _PreorderBejaras(FaElem elem, BinarisBejarasKezelo muvelet)
        {
            if (elem != null)
            {
                muvelet?.Invoke(elem.tartalom);
                _PreorderBejaras(elem.bal, muvelet);
                _PreorderBejaras(elem.jobb, muvelet);
            }
        }

        public void InorderBejaras(BinarisBejarasKezelo muvelet)
        {
            _InorderBejaras(gyoker, muvelet);
        }

        private void _InorderBejaras(FaElem elem, BinarisBejarasKezelo muvelet)
        {
            if (elem != null)
            {
                _InorderBejaras(elem.bal, muvelet);
                muvelet?.Invoke(elem.tartalom);
                _InorderBejaras(elem.jobb, muvelet);
            }
        }

        public void PostorderBejaras(BinarisBejarasKezelo muvelet)
        {
            _PostorderBejaras(gyoker, muvelet);
        }

        private void _PostorderBejaras(FaElem elem, BinarisBejarasKezelo muvelet)
        {
            if (elem != null)
            {
                _PostorderBejaras(elem.bal, muvelet);
                _PostorderBejaras(elem.jobb, muvelet);
                muvelet?.Invoke(elem.tartalom);
            }
        }
    }
}
