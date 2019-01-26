using System;
using System.Collections.Generic;
using System.Linq;

namespace DaneZPlikuConsole
{
    class Program
    {
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

        static T[] Kolumna<T>(T[][] t, int numer) //funkcja zwracająca wartości kolumny
        {
            T[] kolumna = new T[t.Length];
            for (int i = 0; i < t.Length; i++)
                kolumna[i] = t[i][numer];
            return kolumna;
        }

        static Dictionary<T, int> Wystepowanie<T>(T[] t) //funkcja listująca unikalne wartości deczyjne
        {
            Dictionary<T, int> lista = new Dictionary<T, int> { { t[0], 1 } };
            for (int i = 1; i < t.Length; i++)
            {
                if (lista.ContainsKey(t[i])) //srawdzanie czy jest dana wartość decyzyjna
                    lista[t[i]]++;
                else
                    lista.Add(t[i], 1); //dodawanie wartości decyzyjnej
            }
            return lista;
        }

        static double Minimum(double[] t) //funkcja szukająca minimum w tabeli
        {
            double min = t[0];
            for (int i = 1; i < t.Length; i++)
                if (t[i] < min)
                    min = t[i];
            return min;
        }

        static int Maksymalne_K(string[][] dane) //funkcja szukająca maksymalne k
        {
            string[] decyzja = Kolumna(dane, dane.Length - 1);
            Dictionary<string, int> unikalne = Wystepowanie(decyzja);
            var Slownik = unikalne.OrderByDescending(x => x.Value); //sortowanie słownika od najmniejszego elementu
            var wynik = Slownik.Last(); //wyszukanie największego elementu 

            int maksymalne_k = wynik.Value;
            return maksymalne_k;
        }

        static string Tablica_do_string<T>(T[][] t)
        {
            string ciag = "";
            for (int i = 0; i < t.Length; i++)
            {
                for (int j = 0; j < t[i].Length; j++)
                {
                    ciag += t[i][j].ToString() + " ";
                }
                ciag = ciag.Trim() + Environment.NewLine;
            }
            return ciag;
        }

        static void Main(string[] args)
        {
            string sciezkaDoSystemuTestowego = @"SystemTestowy.txt";
            string sciezkaDoSystemuTreningowego = @"SystemTreningowy.txt";



            SystemDecyzyjny systemTreningowy = new SystemDecyzyjny(sciezkaDoSystemuTreningowego);
            SystemDecyzyjny systemTestowy = new SystemDecyzyjny(sciezkaDoSystemuTestowego);


            Console.WriteLine("System testowy");
            Console.Write(systemTestowy);
            Console.WriteLine("");

            Console.WriteLine("System treningowy");
            Console.Write(systemTreningowy);
            Console.WriteLine("");

            string liczba;
            // Przykład konwertowania string do double
            liczba = "1.4";
            double dliczba = StringToDouble(liczba);


            // Przykład konwertowania string do int
            liczba = "1";
            int iLiczba = StringToInt(liczba);

            /****************** Miejsce na rozwiązanie *********************************/

            int k, metryka;

            while (true) //wybór wartości k
                try
                {
                    Console.WriteLine("Podaj k od 1 do " + systemTreningowy.Minimalna_liczebnosc_klas());
                    k = int.Parse(Console.ReadLine());
                    if (k < 1 || k > systemTreningowy.Minimalna_liczebnosc_klas())
                        throw new Exception("Wybierz liczbę od 1 do " + systemTreningowy.Minimalna_liczebnosc_klas() + " !");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wybierz liczbę od 1 do " + systemTreningowy.Minimalna_liczebnosc_klas() + " !");
                }

            Console.WriteLine();

            while (true) //wybór metryki
                try
                {

                    Console.WriteLine("Metryka");
                    Console.WriteLine("1 Euklides");
                    Console.WriteLine("2 Canabera");
                    Console.WriteLine("3 Czebyszew");
                    Console.WriteLine("4 Manhattan");
                    metryka = int.Parse(Console.ReadLine());
                    if (metryka < 1 || metryka > 4)
                        throw new Exception("Wybierz liczbę od 1 do 4!");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wybierz liczbę od 1 do 4!");
                }

            KNN(systemTestowy.Unikalne_decyzje(), systemTreningowy.Unikalne_decyzje(), metryka, systemTreningowy, systemTestowy, k);
            //wykonywanie obliczeń w k w określonej metryce
            /****************** Koniec miejsca na rozwiązanie ********************************/
            Console.ReadKey();
        }


        static private void KNN(string[] klasy_testowe, string[] klasy_treningowe, int typ, SystemDecyzyjny system_treningowy, SystemDecyzyjny system_testowy, int KNN)
        { //wykonanie z któryś metryk

            if (typ == 1)//Euklides
                system_testowy.Wykonanie_metryki(klasy_testowe, klasy_treningowe, system_treningowy, KNN, system_treningowy.Euklides);
            
            if (typ == 2)//Canabera
                system_testowy.Wykonanie_metryki(klasy_testowe, klasy_treningowe, system_treningowy, KNN, system_treningowy.Canaber);

            if (typ == 3)//Czebyszew
                system_testowy.Wykonanie_metryki(klasy_testowe, klasy_treningowe, system_treningowy, KNN, system_treningowy.Czybyszew);

            if (typ == 4)//Manhattan
                system_testowy.Wykonanie_metryki(klasy_testowe, klasy_treningowe, system_treningowy, KNN, system_treningowy.Manhattan);

            //wypisywanie macierzy predykacji
            Console.WriteLine();
            Console.Write("     " + klasy_treningowe[0] + "   "); // 2
            Console.Write(klasy_treningowe[1] + "   "); // 4
            Console.Write("No. of obj   ");
            Console.Write("Accuracy   ");
            Console.WriteLine("Coverage   ");
            Console.Write(" " + klasy_treningowe[0]); //2
            Console.Write("   " + system_testowy.Macierz_predykcji[0, 0].ToString() + "   "); //prawidłowo skwalifikowane 2
            Console.Write(system_testowy.Macierz_predykcji[0, 1].ToString() + "        "); //4 skwalifikowane jako 2
            Console.Write(system_testowy.Macierz_predykcji[0, 2].ToString() + "         ");//ilość 2 w systemie testowym
            Console.Write(system_testowy.Acc0.ToString() + "         ");//skuteczność skwalifikowania 2
            Console.WriteLine(system_testowy.Cov0.ToString());//skuteczność chwycenia 2
            Console.Write(" " + klasy_treningowe[1]);//4
            Console.Write("   " + system_testowy.Macierz_predykcji[1, 0].ToString() + "   ");//2 skwalifiokowano jako 4
            Console.Write(system_testowy.Macierz_predykcji[1, 1].ToString() + "        ");//prawidłowo skwalifikowanie 4
            Console.Write(system_testowy.Macierz_predykcji[1, 2].ToString() + "         ");//ilość 4 w systemie testowym
            Console.Write(system_testowy.Acc1.ToString() + "         ");//skuteczność skwalifikowania 4
            Console.WriteLine(system_testowy.Cov1.ToString());//skuteczność chycenia 4
            Console.Write("TPR");
            Console.Write("  " + system_testowy.Macierz_predykcji[2, 0].ToString() + "   ");//stosunek skwalifkowanych prawidłowo 2 do skalifikowanych 2
            Console.WriteLine(system_testowy.Macierz_predykcji[2, 1].ToString());////stosunek skwalifkowanych prawidłowo 4 do skalifikowanych 4
        }

    }
}
