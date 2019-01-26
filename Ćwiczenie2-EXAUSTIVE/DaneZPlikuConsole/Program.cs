using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kw.Combinatorics;

namespace DaneZPlikuConsole
{
    class Program
    {
        static string TablicaDoString<T>(T[][] tab)
        {
            string wynik = "";
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab[i].Length; j++)
                {
                    wynik += tab[i][j].ToString() + " ";
                }
                wynik = wynik.Trim() + Environment.NewLine;
            }

            return wynik;
        }

        static double StringToDouble(string liczba)
        {
            double wynik; liczba = liczba.Trim();
            if (!double.TryParse(liczba.Replace(',', '.'), out wynik) && !double.TryParse(liczba.Replace('.', ','), out wynik))
                throw new Exception("Nie udało się skonwertować liczby do double");

            return wynik;
        }


        static int StringToInt(string liczba)
        {
            int wynik;
            if (!int.TryParse(liczba.Trim(), out wynik))
                throw new Exception("Nie udało się skonwertować liczby do int");

            return wynik;
        }

        static string[][] StringToTablica(string sciezkaDoPliku)
        {
            string trescPliku = System.IO.File.ReadAllText(sciezkaDoPliku); // wczytujemy treść pliku do zmiennej
            string[] wiersze = trescPliku.Trim().Split(new char[] { '\n' }); // treść pliku dzielimy wg znaku końca linii, dzięki czemu otrzymamy każdy wiersz w oddzielnej komórce tablicy
            string[][] wczytaneDane = new string[wiersze.Length][];   // Tworzymy zmienną, która będzie przechowywała wczytane dane. Tablica będzie miała tyle wierszy ile wierszy było z wczytanego poliku

            for (int i = 0; i < wiersze.Length; i++)
            {
                string wiersz = wiersze[i].Trim();     // przypisuję i-ty element tablicy do zmiennej wiersz
                string[] cyfry = wiersz.Split(new char[] { ' ' });   // dzielimy wiersz po znaku spacji, dzięki czemu otrzymamy tablicę cyfry, w której każda oddzielna komórka to czyfra z wiersza
                wczytaneDane[i] = new string[cyfry.Length];    // Do tablicy w której będą dane finalne dokładamy wiersz w postaci tablicy integerów tak długą jak długa jest tablica cyfry, czyli tyle ile było cyfr w jednym wierszu
                for (int j = 0; j < cyfry.Length; j++)
                {
                    string cyfra = cyfry[j].Trim(); // przypisuję j-tą cyfrę do zmiennej cyfra
                    wczytaneDane[i][j] = cyfra; 
                }
            }
            return wczytaneDane;
        }

        //FUNCKE //

        bool CzyObiektSpelniaRegule(Regula r, string[] obiekt)
        {
            foreach (var deskryptor in r.deskryptory)
            {
                if (obiekt[deskryptor.Key] != deskryptor.Value)
                    return false;
            }
            return true;
        }


        bool CzyRegulaNieSprzeczna(Regula r, string[][] obiekty)
        {
            foreach (var obiekt in obiekty)
            {
                if (CzyObiektSpelniaRegule(r, obiekt) && obiekt.Last() != r.decyzja)
                    return false;
            }

            return true;
        }

        static Regula UtworzRegule(string[] obiekt, int[] kombinacje)
        {
            Regula r = new Regula();
            r.decyzja = obiekt.Last();
            for (int i = 0; i < kombinacje.Length; i++)
            {
                int NumerAtrybutu = kombinacje[i];
                r.deskryptory.Add(NumerAtrybutu, obiekt[NumerAtrybutu]);
            }
            return r;
        }

        int Support(Regula r, string[][] obiekty)
        {
            r.support = 0;
            foreach (var obiekt in obiekty)
            {
                if (CzyObiektSpelniaRegule(r, obiekt))
                {
                    r.support++;
                }
            }
            return r.support;
        }

        List<int> NrObiektuiSupport(Regula r, string[][] obiekty)
        {
            List<int> obiektySpelniajaceRegule = new List<int>();
            r.support = 0;
            for (int i = 0; i < obiekty.Length; i++)
            {
                if (CzyObiektSpelniaRegule(r, obiekty[i]))
                {
                    obiektySpelniajaceRegule.Add(i);
                    r.support++;
                }
            }
            return obiektySpelniajaceRegule;
        }

        static void Main(string[] args)
        {
            string nazwaPlikuZDanymi = @"SystemDecyzyjny.txt";

            string[][] wczytaneDane = StringToTablica(nazwaPlikuZDanymi);

            Console.WriteLine("Dane systemu");
            string wynik = TablicaDoString(wczytaneDane);
            Console.Write(wynik);

            /****************** Miejsce na rozwiązanie *********************************/
            string napis = "";
            List<int> NumeryObiektow = new List<int>();
            List<Regula> Lista = new List<Regula>();
            Regula r1 = new Regula();
            macierz Macierz = new macierz();
            napis += Environment.NewLine + Environment.NewLine;
            napis += "Metoda Exhaustive :";
            napis += Environment.NewLine + Environment.NewLine;
            string Exhaustive(int nrobiektu, string[][] systemdecyzyjny, int wiersz)
            {
                int[][][] macierz = Macierz.MacierzNieodroznialnosci(wczytaneDane);

                for (int i = 1; i < wczytaneDane[0].Length - 1; i++)
                {
                    for (int k = 0; k < wczytaneDane.Length; k++)
                    {
                        var mc = new Combination(wczytaneDane[k].Length - 1, picks: i);
                        foreach (Combination row in mc.GetRows())
                        {
                            Regula Tworz = UtworzRegule(wczytaneDane[k], row.ToArray());
                            if (!(Macierz.CzyKombinacjawWierszu(macierz[k], row.ToArray())) && !Macierz.CzyRegulaZawieraReguleZListy(Lista, Tworz))
                            {
                                Tworz.Support(Tworz, wczytaneDane);
                                napis += Tworz + Environment.NewLine;
                                Lista.Add(Tworz);
                            }
                        }
                    }
                }
                return napis;
            }

            for (int wiersz = 1; wiersz < wczytaneDane.Length; wiersz++)
            {
                for (int nrobiektu = 0; nrobiektu < wczytaneDane.Length; nrobiektu++)
                {
                    if (!NumeryObiektow.Contains(nrobiektu))
                    {
                        Exhaustive(nrobiektu, wczytaneDane, wiersz);
                    }
                }
                napis += Environment.NewLine;
            }
            napis += "Koniec";
            Console.WriteLine(napis);
            /****************** Koniec miejsca na rozwiązanie ********************************/
            Console.ReadKey();
        }
    }
}
