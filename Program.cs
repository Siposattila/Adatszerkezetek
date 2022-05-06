using System;

namespace ZHGyak
{
    class Pogram
    {
        static Random random = new Random();
        static string[] kutyaNevek = new string[10]
        {
            "Bodri",
            "Málna",
            "Apacs",
            "Buksi",
            "Tudor",
            "Gondor",
            "Zeusz",
            "Hádész",
            "Gombóc",
            "Tötye"
        };

        static void Main(string[] args)
        {
            #region LancoltLista
            Console.WriteLine("Bináris Keresőfa *****************");
            LancoltLista<Kutya> simaBeszurassalkutyak = new LancoltLista<Kutya>();
            LancoltLista<Kutya> rendezettBeszurassalkutyak = new LancoltLista<Kutya>(); // magassag alapjan van
            LancoltListaFeltoltesBeszuras(simaBeszurassalkutyak);
            LancoltListaFeltoltesRendezettBeszuras(rendezettBeszurassalkutyak);

            foreach (Kutya kutya in simaBeszurassalkutyak)
            {
                Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
            }

            Console.WriteLine();
            foreach (Kutya kutya in rendezettBeszurassalkutyak)
            {
                Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
            }

            Console.WriteLine();
            Console.WriteLine($"A 3. kutya lekérése lehetséges index alapján (mert implementálva van lásd LancoltLista 11. sor): {simaBeszurassalkutyak[2].Nev}");

            Console.WriteLine();
            Console.WriteLine("Az első kutya törlése után");
            simaBeszurassalkutyak.Torles(simaBeszurassalkutyak[0].Nev);
            foreach (Kutya kutya in simaBeszurassalkutyak)
            {
                Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
            }

            Console.WriteLine();
            Console.WriteLine("Keressük meg azt a kutyát aki 100 magas (ha van olyan).");
            try
            {
                Console.WriteLine($"{simaBeszurassalkutyak.Kereses(100).Nev}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Nincs olyan kutya aki 100 magas...");
            }

            Console.WriteLine();
            Console.WriteLine("Keressük meg azokat a kutyákat akik legalább 100 magasak.");
            LancoltLista<Kutya> keresettKutyak = new LancoltLista<Kutya>();
            simaBeszurassalkutyak.Kereses(100, keresettKutyak);
            foreach (Kutya kutya in keresettKutyak)
            {
                Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
            }
            #endregion

            #region BinarisKeresofa
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Bináris Keresőfa *****************");
            BinarisKeresofa<Kutya> kutyaFa = new BinarisKeresofa<Kutya>();
            BinarisKeresofaFeltoltesBeszuras(kutyaFa);

            kutyaFa.PreorderBejaras(BinarisKeresofaBejaroKezelo);

            Console.WriteLine();
            kutyaFa.InorderBejaras(BinarisKeresofaBejaroKezelo);

            Console.WriteLine();
            kutyaFa.PostorderBejaras(BinarisKeresofaBejaroKezelo);

            Console.WriteLine();
            Console.WriteLine("Kiválogatjuk azokat a kutyákat akik legalább 50 magasak.");
            LancoltLista<Kutya> valogatottKutyak = kutyaFa.Kivalogat(LegalabbOtvenMagasE);
            foreach (Kutya kutya in valogatottKutyak)
            {
                Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
            }
            #endregion

            #region Graf
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Gráf *****************");
            List<Kutya> kutyaLista = KutyaCsucsokLetrehozasGraf();
            Graf<Kutya> kutyaGraf = new Graf<Kutya>(kutyaLista);
            kutyaGraf.ElHozzaadasEsemeny += GrafElHozzadasErtesito;

            Console.WriteLine();
            kutyaGraf.ElHozzaadas(kutyaLista[0], kutyaLista[1]);
            kutyaGraf.ElHozzaadas(kutyaLista[1], kutyaLista[2]);
            kutyaGraf.ElHozzaadas(kutyaLista[3], kutyaLista[4]);
            kutyaGraf.ElHozzaadas(kutyaLista[4], kutyaLista[5]);
            kutyaGraf.ElHozzaadas(kutyaLista[6], kutyaLista[7]);
            kutyaGraf.ElHozzaadas(kutyaLista[7], kutyaLista[8]);
            kutyaGraf.ElHozzaadas(kutyaLista[8], kutyaLista[9]);

            Console.WriteLine();
            kutyaGraf.MelysegiBejaras(kutyaLista[0], new List<Kutya>(), GrafBejarasKiiro);

            Console.WriteLine();
            kutyaGraf.SzelessegiBejaras(kutyaLista[0], GrafBejarasKiiro);
            #endregion

            #region HasitoTabla
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Hasító táblázat *****************");
            HasitoTabla<int, Kutya> kutyaHasitoTabla = new HasitoTabla<int, Kutya>(10);
            HasitoTablaFeltoltesBeszuras(kutyaHasitoTabla);
            #endregion
        }

        #region LancoltLista seged
        static void LancoltListaFeltoltesBeszuras(LancoltLista<Kutya> kutyak)
        {
            for (int i = 0; i < 10; i++)
            {
                kutyak.Beszuras(new Kutya(kutyaNevek[random.Next(0, 10)], random.Next(20, 211)));
            }
        }

        static void LancoltListaFeltoltesRendezettBeszuras(LancoltLista<Kutya> kutyak)
        {
            for (int i = 0; i < 10; i++)
            {
                kutyak.RendezettBeszurasRovidebb(new Kutya(kutyaNevek[random.Next(0, 10)], random.Next(20, 211)));
            }
        }
        #endregion

        #region BinarisKeresofa seged
        static void BinarisKeresofaFeltoltesBeszuras(BinarisKeresofa<Kutya> fa)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    fa.Beszuras(new Kutya(kutyaNevek[random.Next(0, 10)], random.Next(20, 211)));
                }
                catch (MarVanIlyenElemException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void BinarisKeresofaBejaroKezelo(Kutya kutya)
        {
            Console.WriteLine($"{kutya.Nev} {kutya.Magassag}");
        }

        static bool LegalabbOtvenMagasE(Kutya kutya)
        {
            return kutya.Magassag >= 50;
        }
        #endregion

        #region Graf seged
        static List<Kutya> KutyaCsucsokLetrehozasGraf()
        {
            List<Kutya> kutyak = new List<Kutya>();
            for (int i = 0; i < 10; i++)
            {
                kutyak.Add(new Kutya(kutyaNevek[random.Next(0, 10)], random.Next(20, 211)));
            }

            return kutyak;
        }

        static void GrafElHozzadasErtesito(object forras, GrafEsemenyParameterek<Kutya> parameterek)
        {
            Console.WriteLine($"{forras} {parameterek.A.Nev} <---> {parameterek.B.Nev}");
        }

        static void GrafBejarasKiiro(Kutya kutya)
        {
            Console.WriteLine($"{kutya.Nev}");
        }
        #endregion

        #region HasitoTabla seged
        static void HasitoTablaFeltoltesBeszuras(HasitoTabla<int, Kutya> kutyak)
        {
            for (int i = 0; i < 10; i++)
            {
                kutyak.Beszuras(i, new Kutya(kutyaNevek[random.Next(0, 10)], random.Next(20, 211)));
            }
        }
        #endregion
    }
}
