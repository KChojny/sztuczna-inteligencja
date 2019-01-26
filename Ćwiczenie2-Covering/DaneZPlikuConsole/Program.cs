using Kw.Combinatorics;
using System;
using System.Collections.Generic;
using System.Linq;

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
            liczba = liczba.Trim();
            if (!double.TryParse(liczba.Replace(',', '.'), out double wynik) && !double.TryParse(liczba.Replace('.', ','), out wynik))
                throw new Exception("Nie udało się skonwertować liczby do double");

            return wynik;
        }


        static int StringToInt(string liczba)
        {
            if (!int.TryParse(liczba.Trim(), out int wynik))
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

        //////funkcje///////////////////////////////////////////////////////

        static List<int> numery_obiektow = new List<int>();
        static List<int> odrzucone_obiekty = new List<int>();

        public static List<int> Numery_obiektow { get => numery_obiektow; set => numery_obiektow = value; }
        public static List<int> Odrzucone_obiekty { get => odrzucone_obiekty; set => odrzucone_obiekty = value; }
       

        static string Covering(int numer_obiektu, string[][] system_decycyjny, int wiersz)
        {//wykonywanie metodę covering

            string napis = "";

            var wybor = new Combination(choices: system_decycyjny[numer_obiektu].Length - 1, picks: wiersz);
            //kombinacja dla obiektu i dla atrybutu
            foreach (var wiersz2 in wybor.GetRows())//przechodzenie wartościami między wierszami
            {
                Regula regula = Utworzenie_reguly(system_decycyjny[numer_obiektu], wiersz2.ToArray());
                if (Sprawdzenienie_reguły(regula, system_decycyjny))//sprawdzanie reguły
                {
                    Obiekt_regula(regula, system_decycyjny);//sprawdzanie 

                    napis += regula + Environment.NewLine;//napisanie reguły
                    foreach (var element in Obiekt_regula(regula, system_decycyjny))
                    {
                        if (!Numery_obiektow.Contains(element))//obiekt jest odrzucany
                        {
                            Numery_obiektow.Add(element);
                             napis += "Obiekt " + (element + 1) + " jest odrzucony "+ Environment.NewLine;
                        }
                    }
                    break;
                }
            }
            return napis;
        }

        static bool Sprawdzanie_obiektu(string[] obiekt, Regula r) //funkcja obiekt spełnia regułę
        {
            foreach (var deskryptor in r.Deskryptory)
                if (obiekt[deskryptor.Key] != deskryptor.Value)//sprawdzenie czy obiekt spełnia regułę
                    return false;
    
            return true;
        }

        static bool Sprawdzenienie_reguły(Regula r, string[][] obiekty) //funkcja sprawdza czy reguła jest niesprzeczna
        {
            foreach (var obiekt in obiekty)
                if (Sprawdzanie_obiektu(obiekt, r) && obiekt.Last() != r.Decyzja)//sprawdzanie reguły
                    return false;

            return true;
        }

        static Regula Utworzenie_reguly(string[] obiekt, int[] kombinacje) //funkcja tworząca nową regułę
        {
            Regula r = new Regula { Decyzja = obiekt.Last() };//tworzenie listy reguł
            for (int i = 0; i < kombinacje.Length; i++)
            {
                int NumerAtrybutu = kombinacje[i];
                r.Deskryptory.Add(NumerAtrybutu, obiekt[NumerAtrybutu]);//dodwanie reguły
            }
            return r;
        }

        static int Support(Regula r, string[][] obiekty) //funkcja licząca supporty
        {
            r.Support = 0;
            foreach (var obiekt in obiekty)
                if (Sprawdzanie_obiektu(obiekt, r))
                    r.Support++;

            return r.Support;
        }

        static List<int> Obiekt_regula(Regula r, string[][] obiekty) //funkcja zwracająca obiekty, które spełniają regułę
        {
            r.Support = 0;
            List<int> obiekty_regula = new List<int>();
            for (int i = 0; i < obiekty.Length; i++)
            {
                if (Sprawdzanie_obiektu(obiekty[i], r))
                {
                    obiekty_regula.Add(i);//dodawanie obiektu który spełnia reguły
                    r.Support++;//inkrementacja warość supportu
                }
            }
            return obiekty_regula;
        }

        static void Main(string[] args)
        {
            string nazwaPlikuZDanymi = @"SystemDecyzyjny.txt";

            string[][] wczytaneDane = StringToTablica(nazwaPlikuZDanymi);

            Console.WriteLine("Dane systemu");
            string wynik = TablicaDoString(wczytaneDane);
            Console.Write(wynik);

            /****************** Miejsce na rozwiązanie *********************************/

            Numery_obiektow = new List<int>(); //lista obietków które są sprawdzane
            Odrzucone_obiekty = new List<int>(); //lista obiektów które nie są sprawdzane
            string napis = "";
            Console.WriteLine(Environment.NewLine + "Metoda Covering" + Environment.NewLine + Environment.NewLine);

            for (int rzad = 1; rzad < wczytaneDane[0].Length; rzad++) //wypisywanie reguł
            {
                napis += "Rzad" + rzad + Environment.NewLine; //wypisywanie rządu
                for (int numer_obiektu = 0; numer_obiektu < wczytaneDane.Length; numer_obiektu++)//wypisywanie reguł
                    if (!Numery_obiektow.Contains(numer_obiektu))
                        napis += Covering(numer_obiektu, wczytaneDane, rzad);//wykonywanie meteodę covering

                if (Numery_obiektow.Count == wczytaneDane.Length)
                {
                    napis += Environment.NewLine + "Wszystkie obiekty zostały wyrzucone z rozwazań";
                    break;
                }
                napis += Environment.NewLine;
            }
            Console.Write(napis);

            /****************** Koniec miejsca na rozwiązanie ********************************/
            Console.ReadKey();
        }
    }
}
