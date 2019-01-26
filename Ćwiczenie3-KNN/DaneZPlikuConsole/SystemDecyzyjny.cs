using System;
using System.Collections.Generic;
using System.Linq;

namespace DaneZPlikuConsole
{
    public class SystemDecyzyjny
    {
        private double acc0;
        private double acc1;
        private double cov0;
        private double cov1;

        private List<Obiekt> obiekty = new List<Obiekt>();

        private double[,] macierz_predykcji = new double[3, 5];

        public double Acc0 { get => acc0; set => acc0 = value; }
        public double Acc1 { get => acc1; set => acc1 = value; }
        public double Cov0 { get => cov0; set => cov0 = value; }
        public double Cov1 { get => cov1; set => cov1 = value; }

        public List<Obiekt> Obiekty { get => obiekty; set => obiekty = value; }

        public double[,] Macierz_predykcji { get => macierz_predykcji; set => macierz_predykcji = value;  }

        public delegate double funkcjaDelegowana(Obiekt obiekt_testowy, Obiekt obiekt_treningowy); //delegat obiektów testowych i treningowych

        public SystemDecyzyjny(string lokalizacja) //konstruktor
        {
            string[] linie = System.IO.File.ReadAllLines(lokalizacja); //pobieranie wartości

            foreach (string l in linie)
                Obiekty.Add(new Obiekt(l));
        }

        public string[] Unikalne_decyzje() //wypisywanie niepowtarzających decyzji
        {
            List<string> unikalne_decyzje = new List<string>();

            foreach (var obiekt in Obiekty)
                if (!unikalne_decyzje.Contains(obiekt.Decyzja))
                    unikalne_decyzje.Add(obiekt.Decyzja);

            return unikalne_decyzje.ToArray();
        }

        public int Minimalna_liczebnosc_klas() //funkcja wypisująca minimalne k w Program
        {
            int min = Wystepowanie_klas().First().Value;

            foreach (var czestosc in Wystepowanie_klas())
                if (min > czestosc.Value)
                        min = czestosc.Value;

            return min;
        }

        public Dictionary<string,int> Wystepowanie_klas() //wypisywanie klas decyzyjnych
        {
            Dictionary<string, int> lista = new Dictionary<string, int>();

            foreach (var obiekt in Obiekty)
            {
                if (lista.ContainsKey(obiekt.Decyzja))
                    lista[obiekt.Decyzja]++;
                else
                    lista.Add(obiekt.Decyzja, 1);
            }
            return lista;
        }

        public double Euklides(Obiekt obiekt_testowy, Obiekt obiekt_treningowy) //metryka Euklidesa
        {
            double suma = 0;
            for (int i = 0; i < obiekt_testowy.Deskryptory.Count; i++)
                suma += (double)(obiekt_testowy.Deskryptory[i].Getwartosc() - obiekt_treningowy.Deskryptory[i].Getwartosc()) * (obiekt_testowy.Deskryptory[i].Getwartosc() - obiekt_treningowy.Deskryptory[i].Getwartosc());
            return Math.Sqrt(suma);

        }

        public double Canaber(Obiekt obiekt_testowy, Obiekt obiekt_treningowy) //metryka Canabera
        {
            double suma = 0;
            for (int i = 0; i < obiekt_testowy.Deskryptory.Count; i++)
                suma += Math.Abs((double)(obiekt_testowy.Deskryptory[i].Getwartosc() - obiekt_treningowy.Deskryptory[i].Getwartosc()) / (obiekt_testowy.Deskryptory[i].Getwartosc() + obiekt_treningowy.Deskryptory[i].Getwartosc()));
            return suma;
        }

        public double Czybyszew(Obiekt obiekt_testowy, Obiekt obiekt_treningowy) //metryka Czybyszewa
        {
            List<int> liczby = new List<int>();
            for (int i = 0; i < obiekt_testowy.Deskryptory.Count; i++)
                liczby.Add(Math.Abs((obiekt_testowy.Deskryptory[i].Getwartosc() - obiekt_treningowy.Deskryptory[i].Getwartosc())));
            return liczby.Max(x => x);
        }

        public double Manhattan(Obiekt obiekt_testowy, Obiekt obiekt_treningowy) //metryka Manhattana
        {
            double wynik = 0;
            for (int i = 0; i < obiekt_testowy.Deskryptory.Count; i++)
                wynik += Math.Abs(obiekt_testowy.Deskryptory[i].Getwartosc() - obiekt_treningowy.Deskryptory[i].Getwartosc());
            return wynik;
        }

        public Dictionary<string, Dictionary<string, int>> Wykonanie_metryki(string[] klasy_testowe, string[] klasy_treningowe, SystemDecyzyjny system_treningowy, int k, funkcjaDelegowana obliczenie_odleglosci)
        {//wykonywanie określonej metryki
            Dictionary<string, Dictionary<string, int>> wynik = new Dictionary<string, Dictionary<string, int>>();

            foreach (var tst in klasy_testowe)
            {
                wynik.Add(tst, new Dictionary<string, int>());
                foreach (var trn in klasy_treningowe)
                    wynik[tst].Add(trn, 0);
            }

            foreach (Obiekt obiekt_testowy in Obiekty)
            {
                string decyzja_nadania = Klasyfikacja(obiekt_testowy, k, system_treningowy, obliczenie_odleglosci);
                string decyzja_obiekt = obiekt_testowy.Decyzja;
                if (decyzja_nadania == null)
                    continue;
                wynik[decyzja_obiekt][decyzja_nadania]++;
            }
            Wypelnienie_macierzy(wynik, klasy_treningowe);
            Wykonanie_metryki(klasy_treningowe);
            return wynik;

        }

        public void Wypelnienie_macierzy(Dictionary<string, Dictionary<string, int>> wynik, string[] klasy_treningowe)
        {//wypełnianie macierzy perdykacji
            int i = 0, j = 0;
            foreach (var k in klasy_treningowe)
            {
                j = 0;
                foreach (var w in klasy_treningowe)
                {
                    Macierz_predykcji[i, j] = wynik[k][w];
                    j++;
                }
                i++;
            }
        }

        string Klasyfikacja(Obiekt obiekt_testowy, int k, SystemDecyzyjny system_treningowy, funkcjaDelegowana obliczenie_odleglosc)
        { //klasyfikowanie 
            string klasyfikacja = null;
            Dictionary<string, Dictionary<string, double>> metryka = Metryka(obiekt_testowy, system_treningowy, obliczenie_odleglosc);
            Dictionary<string, double> najmniesze_odleglosci_suma = new Dictionary<string, double>();
            foreach (var m in metryka)
            {
                List<double> wartosci = new List<double>(m.Value.Values);
                najmniesze_odleglosci_suma.Add(m.Key, wartosci.OrderBy(x => x).Take(k).Sum());//sumowanie odległości
            }

            //sprawdzanie sum
            if (najmniesze_odleglosci_suma.First().Value > najmniesze_odleglosci_suma.Last().Value)
                klasyfikacja = najmniesze_odleglosci_suma.Last().Key;

            if (najmniesze_odleglosci_suma.First().Value < najmniesze_odleglosci_suma.Last().Value)
                klasyfikacja = najmniesze_odleglosci_suma.First().Key;


            return klasyfikacja;
        }

        public Dictionary<string, Dictionary<string, double>> Metryka(Obiekt obiekt_treningowy, SystemDecyzyjny system_treningowy, funkcjaDelegowana obliczenie_odleglosc)
        {
            Dictionary<string, Dictionary<string, double>> metryka = new Dictionary<string, Dictionary<string, double>>();
            int i = 0;
            foreach (Obiekt obiekt in system_treningowy.Obiekty)
            {//listowanie wartości obliczoną określoną metryką
                Dictionary<string, double> wynik = new Dictionary<string, double> { { i.ToString(), obliczenie_odleglosc(obiekt_treningowy, obiekt) } };
                if (!metryka.ContainsKey(obiekt.Decyzja))
                {
                    metryka.Add(obiekt.Decyzja, wynik); //dodawanie do listy wartości
                }
                else
                {
                    foreach (var element in wynik)
                        metryka[obiekt.Decyzja].Add(element.Key, element.Value);
                }
                i++;
            }
            return metryka;
        }

        public void Wykonanie_metryki(string[] klasy_treningowe) //tworzenie wyników do wyświetlenia
        {

            Macierz_predykcji[0, 2] = Obiekty.FindAll(x => x.Decyzja == klasy_treningowe[0]).Count(); //liczba skwalifikowanych przez system 2
            Macierz_predykcji[1, 2] = Obiekty.FindAll(x => x.Decyzja == klasy_treningowe[1]).Count(); //liczba skwalifikowanych przez system 4

            Cov0 = (Macierz_predykcji[0, 0] + Macierz_predykcji[0, 1]) / Macierz_predykcji[0, 2]; //cov dla 2
            Cov1 = (Macierz_predykcji[1, 0] + Macierz_predykcji[1, 1]) / Macierz_predykcji[1, 2]; //cov dla 4

            if (Macierz_predykcji[0, 0] + Macierz_predykcji[0, 1] == 0) //acc dla 2
                Acc0 = 0;
            else
                Acc0 = Macierz_predykcji[0, 0] / (Macierz_predykcji[0, 0] + Macierz_predykcji[0, 1]);

            if (Macierz_predykcji[1, 0] + Macierz_predykcji[1, 1] == 0) //acc dla 4
                Acc1 = 0;
            else
                Acc1 = Macierz_predykcji[1, 1] / (Macierz_predykcji[1, 0] + Macierz_predykcji[1, 1]);
            // przypisanie do macierzy predykacji
            Macierz_predykcji[0, 4] = Cov0;
            Macierz_predykcji[1, 4] = Cov1;
            Macierz_predykcji[0, 3] = Acc0;
            Macierz_predykcji[1, 3] = Acc1;

            if (Macierz_predykcji[0, 0] + Macierz_predykcji[1, 0] == 0) //stosunek skwalifikowanych do dobrze skwalifikowanych 2
                Macierz_predykcji[2, 0] = 0;
            else
                Macierz_predykcji[2, 0] = Macierz_predykcji[0, 0] / (Macierz_predykcji[0, 0] + Macierz_predykcji[1, 0]);

            if (Macierz_predykcji[0, 1] + Macierz_predykcji[1, 1] == 0) //stosunek skwalifikowanych do dobrze skwalifikowanych 2
                Macierz_predykcji[2, 1] = 0;
            else
                Macierz_predykcji[2, 1] = Macierz_predykcji[1, 1] / (Macierz_predykcji[0, 1] + Macierz_predykcji[1, 1]);
        }

        public override string ToString() //funkcja do nadpisywania
        {
            string napis = "";
            foreach (var obiekt in Obiekty)
                napis += obiekt + Environment.NewLine;
            return napis;
        }

        ~SystemDecyzyjny()
        {

        }

    }

}
