using System;

namespace ZHGyak
{
    // nem kell ettol nagyon megijedni ez lenyegeben csak egy lassu lancolt lista sorozat
    class HasitoTabla<K, T>
    {
        private int meret;
        private HasitoElem[] fejek;

        class HasitoElem
        {
            public K kulcs;
            public T tartalom;
            public HasitoElem kovetkezo;
        }

        public HasitoTabla(int meret)
        {
            this.meret = meret;
            fejek = new HasitoElem[meret];
        }

        private int Hash(K kulcs)
        {
            return Math.Abs(kulcs.GetHashCode()) % meret;
        }

        public void Beszuras(K kulcs, T tartalom)
        {
            HasitoElem ujElem = new HasitoElem();
            ujElem.kulcs = kulcs;
            ujElem.tartalom = tartalom;
            ujElem.kovetkezo = fejek[Hash(kulcs)];
            fejek[Hash(kulcs)] = ujElem;
        }

        // a kereses es a torles nem szerepelt abban, amit leirt a tanci bacsi de azert itt hagyom, hogy arra is legyen pelda
        public T Kereses(K kulcs)
        {
            HasitoElem elem = fejek[Hash(kulcs)];
            while (elem != null && !elem.kulcs.Equals(kulcs))
            {
                elem = elem.kovetkezo;
            }

            if (elem != null)
                return elem.tartalom;

            throw new KeyNotFoundException();
        }

        public void Torles(K kulcs)
        {
            HasitoElem elem = fejek[Hash(kulcs)];
            HasitoElem segedElem = null;
            while (elem != null && !elem.kulcs.Equals(kulcs))
            {
                segedElem = elem;
                elem = elem.kovetkezo;
            }

            if (elem != null)
            {
                if (elem == null)
                    fejek[Hash(kulcs)] = elem.kovetkezo;
                else segedElem.kovetkezo = elem.kovetkezo;
            }

            throw new KeyNotFoundException();
        }
    }
}
