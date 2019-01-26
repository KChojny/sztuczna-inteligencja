using System;
using System.Collections.Generic;
using System.Linq;

namespace DaneZPlikuConsole
{
    class Program
    {
        static Deskryptor Najwiekszy (Deskryptor[] deskryptory)
        {
            Deskryptor wartoscMax = deskryptory[0];
            for (int i = 0; i < deskryptory.Length; i++)
            {
                if (wartoscMax.Wartosc < deskryptory[i].Wartosc)
                    wartoscMax = deskryptory[i];
            }
            return wartoscMax;
        }
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

        //////////////////funkcje/////////////////////////////////////////

        static List<int> Generalna_lista_konceptow(string[][] wczytaneDane, int kolumna_decyzyjna)
        {
            List<int> lista_decyzyjna = new List<int> { StringToInt(wczytaneDane[0][kolumna_decyzyjna]) };
            for (int obiekt = 1; obiekt < wczytaneDane[0].Length; obiekt++)//przechodzenie między obiektami
            {
                if (!lista_decyzyjna.Contains(StringToInt(wczytaneDane[obiekt][kolumna_decyzyjna])))//sprawdzanie listy 
                    lista_decyzyjna.Add(StringToInt(wczytaneDane[obiekt][kolumna_decyzyjna]));
            }
            return lista_decyzyjna;
        }

        static bool Sprawdzanie_reguly(Regula regula, string[][] wczytaneDane, int koncept, int kolumna_decyzyjna)
        {
            bool sprawdzenie = true;
            for (int obiekt = 0; obiekt < wczytaneDane.Length; obiekt++)//przechodznie między obiketami
            {
                sprawdzenie = true;
                foreach (Deskryptor deskryptory in regula.Deskryptory)//przechodzenie między atrybutami
                {
                    if (StringToInt(wczytaneDane[obiekt][deskryptory.Argument]) != deskryptory.Wartosc)//sprawdzanie czy deskryptor nie ma takiej samej wartości
                    {
                        sprawdzenie = false;
                        break;
                    }
                }
                if (sprawdzenie && (StringToInt(wczytaneDane[obiekt][kolumna_decyzyjna]) != koncept))//sprawdzanie czy deksryptor nie jest równy wartości konceptu
                    return true;
            }
            return false;
        }

        static void Sprawdzanie_obiektu(string[][] wczytaneDane, int koncept, List<Regula> lista_regul, List<int> Wykluczone_obiekty, Regula Regula, int kolumna_decyzyjna)
        {
            bool czy_obiekt_spelnia_regule = true;
            for (int obiekt = 0; obiekt < wczytaneDane.Length; obiekt++)//przechodzenie między obiektami 
            {
                czy_obiekt_spelnia_regule = true;
                if (czy_obiekt_spelnia_regule && (StringToInt(wczytaneDane[obiekt][kolumna_decyzyjna]) != koncept))
                {//sprawdzanie warunku zawsze się wykona jak 
                    Wykluczone_obiekty.Add(obiekt);//dodanie obiektu do listy
                    czy_obiekt_spelnia_regule = false;
                }
                if (czy_obiekt_spelnia_regule && Regula.Deskryptory.Count() != 0)//sprawdzanie 
                {
                    foreach (Deskryptor deskryptor in Regula.Deskryptory)//przechodzenie między deskryptoramie
                    {
                        if (StringToInt(wczytaneDane[obiekt][deskryptor.Argument]) != deskryptor.Wartosc)
                        {
                            czy_obiekt_spelnia_regule = false;
                            Wykluczone_obiekty.Add(obiekt);//dodanie obiektu do listy
                            break;
                        }
                    }
                }
                if (czy_obiekt_spelnia_regule == true)//sprawdzanie czy obiekt spełnia regułę z listy
                {
                    foreach (Regula Item in lista_regul)//przechodzenie przez listę reguł
                    {
                        if (Item.Obiekty.Contains(obiekt))
                        {
                            Wykluczone_obiekty.Add(obiekt);//dodanie obiektu do listy
                            czy_obiekt_spelnia_regule = false;
                            break;
                        }
                    }
                }
            }
        }

        static Deskryptor Najczestszy_deskryptor(List<int> atrybut_pominiecia, List<int> obiekt_pominiecia, string[][] wczytaneDane)
        {//szukanie najczęstszy deskryptor
            int support = 0, wartosc = 0;
            Deskryptor najczestszy_deskryptor = null;
            Deskryptor porownywany_deskryptor = null;
            
            for (int argument = 0; argument < wczytaneDane[0].Length - 1; argument++)//przechodzenie między atrybutami
            {
                if (!atrybut_pominiecia.Contains(argument))//sprawdzanie czy atrybut jest pominięty
                {
                    for (int numer_obiekty = 0; numer_obiekty < wczytaneDane.Length; numer_obiekty++)//przechodzenie między obiektami
                    {
                        if (!obiekt_pominiecia.Contains(numer_obiekty))//sprawdzanie czy obiekt jest pominięty
                        {
                            support = 0;
                            wartosc = StringToInt(wczytaneDane[numer_obiekty][argument]);
                            for (int obiekt = 0; obiekt < wczytaneDane.Length; obiekt++)//przechodzenie między obiektami w celu porównania
                                if (!obiekt_pominiecia.Contains(obiekt))
                                    if (StringToInt(wczytaneDane[obiekt][argument]) == wartosc)
                                        support++;//liczenie supportu
                            if (najczestszy_deskryptor == null)//przyisanie pierwszego deskrytora
                                najczestszy_deskryptor = new Deskryptor(argument, numer_obiekty, support, wartosc);
                            else
                            {
                                porownywany_deskryptor = new Deskryptor(argument, numer_obiekty, support, wartosc);
                                if (najczestszy_deskryptor.Pokrycie < porownywany_deskryptor.Pokrycie)
                                {//przypisanie  deskrytora który ma większe pokrycie
                                    najczestszy_deskryptor.Argument = porownywany_deskryptor.Argument;
                                    najczestszy_deskryptor.Obiekt = porownywany_deskryptor.Obiekt;
                                    najczestszy_deskryptor.Pokrycie = porownywany_deskryptor.Pokrycie;
                                    najczestszy_deskryptor.Wartosc = porownywany_deskryptor.Wartosc;
                                }
                            }
                        }
                    }
                }
            }
            return najczestszy_deskryptor;

        }

        public static void Wypisz_reguly(List<Regula> lista_regul)//wypisywanie reguł
        {
            foreach (Regula r in lista_regul)
            {
                Console.WriteLine(r);
                Console.ReadKey();
            }
            
        }

        static void LEM(string[][] wczytaneDane, int kolumna_decyzyjna)//wykonywanie  metody LEM
        {
            List<int> lista_konceptow = Generalna_lista_konceptow(wczytaneDane, kolumna_decyzyjna);
            List<int> atrybut_pominiecia = new List<int>();
            List<int> obiekt_pominiecia = new List<int>();
            List<Regula> lista_regul = new List<Regula>();
            foreach (int koncept in lista_konceptow)//przechodzenie między konceptami
            {
                bool wypelnienie = false; 
                while (!wypelnienie)
                {
                    Regula nowa_regula = new Regula(koncept);//tworzenie klasa reguła
                    atrybut_pominiecia = new List<int>();
                    obiekt_pominiecia = new List<int>();
                    while (Sprawdzanie_reguly(nowa_regula, wczytaneDane, koncept, kolumna_decyzyjna))
                    {
                        Sprawdzanie_obiektu(wczytaneDane, koncept, lista_regul, obiekt_pominiecia, nowa_regula, kolumna_decyzyjna);
                        Deskryptor czesty_deskryptor = Najczestszy_deskryptor(atrybut_pominiecia, obiekt_pominiecia, wczytaneDane);//przypisanie mu wartości
                        nowa_regula.Dodaj(czesty_deskryptor);//dodawanie deskryptora
                        atrybut_pominiecia.Add(czesty_deskryptor.Argument);//dodawanie atrybut który do klasy deskryptora
                    }
                    nowa_regula.Obiekty = nowa_regula.Pokrywanie_obiektów(wczytaneDane);//lista obiektów
                    int suma_obiektu_konceptu = 0;
                    int suma_regul = 0;
                    lista_regul.Add(nowa_regula);//dodawanie do listy reguł nową regułę
                    for (int obiekt = 0; obiekt < wczytaneDane.Length; obiekt++)//przechodzenie między obiektami
                        if (StringToInt(wczytaneDane[obiekt][kolumna_decyzyjna]) == (koncept))
                            suma_obiektu_konceptu++;//dodawanie numer konceptu
                    foreach (Regula r in lista_regul)//przechodzenie między regułami w liście
                        if (r.Decyzja == koncept)
                            suma_regul += r.Support;//liczenie supportu
                    if (suma_obiektu_konceptu == suma_regul) //sprawdzenie czy pętla została prawidłowo wykonana
                        wypelnienie = true;
                }
            }
            Wypisz_reguly(lista_regul);//wypisywanie reguł
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
            napis += Environment.NewLine + "Funkcja LEM" + Environment.NewLine;
            Console.WriteLine(napis);
            int kolumna = (wczytaneDane[0].Length - 1);//długość kolumn
            LEM(wczytaneDane, kolumna);
            //wykonywanie LEMu
            /****************** Koniec miejsca na rozwiązanie ********************************/
            Console.ReadKey();
        }
    }
}
