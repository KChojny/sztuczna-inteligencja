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

        ////////////////////////funkcje/////////////////////////////////////////////////
        static int[] Spr_atr_n(string[][] t) //funkcja zwracająca atrybuty numeryczne
        {
            List<int> lista_atrybutow = new List<int>(); 

            for (int i = 0; i < t.Length; i++)
                if (Kolumna(t, t[0].Length - 1)[i] == "n")
                    lista_atrybutow.Add(i);
            return lista_atrybutow.ToArray();
        }
    
        static double[] TabStringnaTabDouble(string[] s) //funkcja zmieniająca tablice stringa na tablice double
        {
            double[] d = new double[s.Length];

            for (int i = 0; i < s.Length; i++)
                d[i] = StringToDouble(s[i]);
            return d.ToArray();
        }

        static T[] Kolumna<T>(T[][] t, int numer) //listowanie kolumn
        {
            T[] kolumna = new T[t.Length];
            for (int i = 0; i < t.Length; i++)
                kolumna[i] = t[i][numer];
            return kolumna;
        }

        static T[] Kolumna_spr<T>(T[][] t, int numer_kol, int numer_war, T wartosc_war) //sprawdzanie kolumny
        {
            List<T> lista = new List<T>();
            for (int i = 0; i < t.Length; i++)
            {
                T[] wiersz = t[i];
                if (EqualityComparer<T>.Default.Equals(wiersz[numer_war], wartosc_war))
                    lista.Add(wiersz[numer_kol]);
            }
            return lista.ToArray();
        }

        static T[] Unikalne_wartosci<T>(T[] t) //funkcja do wypisywania unikalnych wartości
        {
            List<T> Lista = new List<T> {t[0]};
            for (int i = 1; i < t.Length; i++)
                if (!Lista.Contains(t[i]))
                    Lista.Add(t[i]);
            return Lista.ToArray();
        }

        static Dictionary<T, int> Ilosc<T>(T[] t) //funkcja do liczenia unikalnych wartości
        {
            Dictionary<T, int> wystepowanie = new Dictionary<T, int>{{ t[0], 1 }};
            for(int i = 1; i < t.Length; i++)
            {
                if (wystepowanie.ContainsKey(t[i]))
                        wystepowanie[t[i]]++;
                else
                    wystepowanie.Add(t[i], 1);
            }
            return wystepowanie;
        }

        static double Minimum(double[] t) //funkcja do szukania minimum
        {
            double min = t[0];
            for (int i = 1; i < t.Length; i++)
                if (t[i] < min)
                    min = t[i];
            return min;
        }

        static double Maximum(double[] t) //funkcja do szukania maksymalnej wartości
        {
            double max = t[0];
            for (int i = 1; i < t.Length; i++)
                if (t[i] > max)
                    max = t[i];
            return max;
        }

        static double Srednia(double[] t, int ilosc) //funkcja licząca średnią
        {

            double srednia = 0;

            for (int i = 0; i < t.Length; i++)
                srednia += t[i];
            srednia /= ilosc;

            return srednia;
        }
        static double Wariancja(double[] t, double srednia, int wielkosc) //funkcja do liczenia wariancji
        {
            double wariancja = 0;

            for (int i = 0; i < t.Length; i++)
                wariancja += Math.Pow((t[i] - srednia), 2);
            wariancja /= wielkosc;

            return wariancja;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            string nazwaPlikuZDanymi = @"australian.txt";
            string nazwaPlikuZTypamiAtrybutow = @"australian-type.txt";

            string[][] wczytaneDane = StringToTablica(nazwaPlikuZDanymi);
            string[][] atrType = StringToTablica(nazwaPlikuZTypamiAtrybutow);

            Console.WriteLine("Dane systemu");
            string wynik = TablicaDoString(wczytaneDane);
            Console.Write(wynik);

            Console.WriteLine("");
            Console.WriteLine("Dane pliku z typami");

            string wynikAtrType = TablicaDoString(atrType);
            Console.Write(wynikAtrType);

            /****************** Miejsce na rozwiązanie *********************************/
            Console.ReadKey();
            //podpunkt A 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Klasy decyzyjne"); // wypisywanie klas decyzyjnych
            Console.WriteLine(string.Join(" ", Unikalne_wartosci(Kolumna(wczytaneDane, wczytaneDane[0].Length - 1))) + Environment.NewLine);
            Console.ReadKey();

            Console.WriteLine("Wielkości klas decyzyjnych" + Environment.NewLine); //wypisywanie wielkości klas decyzyjnych
            foreach (var element in Ilosc(Kolumna(wczytaneDane, wczytaneDane[0].Length - 1)))
                Console.WriteLine(element.Key + " - " + element.Value + Environment.NewLine);
            Console.ReadKey();

            Console.WriteLine("Maksymalne i minimalne wartosci na atrybutach" + Environment.NewLine); //wypisywanie maksymalnej i minimalnej wartości
            Console.WriteLine("Minima" + Environment.NewLine); //wypisywanie minimalnych wartości
            for (int i = 0; i < Spr_atr_n(atrType).Length; i++) 
                Console.WriteLine(string.Join(", ", Minimum(TabStringnaTabDouble(Kolumna(wczytaneDane, Spr_atr_n(atrType)[i])))));
            Console.WriteLine("Maksyma" + Environment.NewLine); //wypisywanie maksymalnych wartości
            for (int i = 0; i < Spr_atr_n(atrType).Length; i++)
                Console.WriteLine(string.Join(", ", Maximum(TabStringnaTabDouble(Kolumna(wczytaneDane, Spr_atr_n(atrType)[i])))));
            Console.WriteLine(Environment.NewLine);
            Console.ReadKey();

            //podpunkt B
            Console.WriteLine("Liczba różnych wartości");
            for (int i = 0; i < atrType.Length; i++) //liczenie rodzajów wartości dla atrybutu
                Console.WriteLine(atrType[i][0] + " - " + Unikalne_wartosci(Kolumna(wczytaneDane, i)).Length + Environment.NewLine);
            Console.ReadKey();

            Console.WriteLine("Lista unikalnych wartości");
            Console.WriteLine(Environment.NewLine);
            for (int i = 0; i < atrType.Length; i++) //wypisywanie list wartości które są unikalne
                Console.WriteLine(atrType[i][0] + " - [" + string.Join(", ", Unikalne_wartosci(Kolumna(wczytaneDane, i))) + "]" +  Environment.NewLine);
            Console.ReadKey();

            Console.WriteLine("Odchylenie standardowe");
            for (int i = 0; i < Spr_atr_n(atrType).Length; i++) //pętla przechodzi kolumny gdzie atrybuty są numeryczne
            {

                int ilosc = Kolumna(wczytaneDane, Spr_atr_n(atrType)[i]).Length;
                double[] kolumna = TabStringnaTabDouble(Kolumna(wczytaneDane, Spr_atr_n(atrType)[i]));

                double srednia = Srednia(kolumna, ilosc);  //odwołanie do średniej
                double wariancja = Wariancja(kolumna, srednia, ilosc); //odwoładnie do wariancji
                double odchylenie_standardowe = Math.Sqrt(wariancja); //odchylenie standardowe
                Console.WriteLine(Environment.NewLine + ++Spr_atr_n(atrType)[i] + " - " + odchylenie_standardowe + Environment.NewLine);
                foreach (string unikalne in Unikalne_wartosci(Kolumna(wczytaneDane, wczytaneDane[0].Length - 1))) //liczenie od. std. dla 1 klasy decyzyjne np. 2. atrybutu z wartością decyzyjną 0 => 2[0] 
                {
                    List<double> lista = new List<double>();
                    double[] wartosci_double = TabStringnaTabDouble(Kolumna_spr(wczytaneDane, Spr_atr_n(atrType)[i], wczytaneDane[0].Length - 1, unikalne));//dodawanie wartości dla 1. wartości decyzyjnej 
                    int ilosc_double = Kolumna_spr(wczytaneDane, Spr_atr_n(atrType)[i], wczytaneDane[0].Length - 1, unikalne).Length;//liczenie ilości dla średniej
                    double srednia_double = Srednia(wartosci_double, ilosc_double); //liczenie średniej
                    for (int j = 0; j < wartosci_double.Length; j++)
                        lista.Add(Math.Pow((wartosci_double[j] - srednia_double), 2)); //dodawanie do listy
                    double[] wariancja_double = lista.ToArray();//liczenie wariancji
                    double odchylenie_double = Math.Sqrt(Srednia(wariancja_double, wartosci_double.Length));//liczenie oddchylenia std.
                    Console.Write(++Spr_atr_n(atrType)[i]+ "[" + unikalne + "] - " + odchylenie_double + Environment.NewLine);
                }
                
            }
            /****************** Koniec miejsca na rozwiązanie ********************************/
            Console.ReadKey();
        }

    }
}
