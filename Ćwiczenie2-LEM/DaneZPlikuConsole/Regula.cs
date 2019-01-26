using System;
using System.Collections.Generic;
using System.Linq;

namespace DaneZPlikuConsole
{
    class Regula
    {
        private int support, decyzja, numer;
        private List<Deskryptor> deskryptory;
        private List<int> obiekty;
        static int liczba_regul = 0;
        

        public int Decyzja { get => decyzja; set => decyzja = value; }
        public int Numer { get => numer; set => numer = value; }
        public int Support { get => support; set => support = value; }
        public static int Liczba_regul { get => liczba_regul; set => liczba_regul = value; }
        public List<int> Obiekty { get => obiekty; set => obiekty = value; }
        internal List<Deskryptor> Deskryptory { get => deskryptory; set => deskryptory = value; }

        public void Dodaj(Deskryptor deskryptor)//metoda która tworzy deskryptor
        {
            Deskryptory.Add(deskryptor);
        }

        static int StringToInt(string liczba) //metoda konwertująca string na liczbę
        {
            if (!int.TryParse(liczba.Trim(), out int wynik))
                throw new Exception("Nie udało się skonwertować liczby do int");

            return wynik;
        }

        public Regula(int koncept) //konstruktor dla Regula
        {
            Deskryptory = new List<Deskryptor>();
            Obiekty = new List<int>(); //lista obiektów
            Liczba_regul++;//dodanie o 1 wartośc liczby reguł
            Numer = Liczba_regul;//liczba reguł
            Decyzja = koncept; //przypisanie konceptu dla klasy decyzyjnej
            Support = 0;
        }

        public List<int> Pokrywanie_obiektów(string[][] wczytaneDane) //wykonywanie reguły
        {
            List<int> Pokrywanie_obiektow = new List<int>();
            bool sprawdzanie;
            for (int obiekt = 0; obiekt < wczytaneDane.Length; obiekt++)
            {
                sprawdzanie = true;
                foreach (Deskryptor deskryptor in Deskryptory)
                {
                    if (StringToInt(wczytaneDane[obiekt][deskryptor.Argument]) != deskryptor.Wartosc || StringToInt(wczytaneDane[obiekt][wczytaneDane[0].Length - 1]) != Decyzja)
                    {//sprawdzanie obiektów które nie są pokrywane
                        sprawdzanie = false;
                        break;
                    }
                }
                if (sprawdzanie == true) //dodawanie do listy obiektu które są pokrywane
                    Pokrywanie_obiektow.Add(obiekt);
            }
            Support = Pokrywanie_obiektow.Count();//przypisanie liczby supportów
            Obiekty = Pokrywanie_obiektow;//przypisanie pokrywanych obiektów
            return Pokrywanie_obiektow;
        }

        public override string ToString() //metoda do wypisywania reguł
        {
            string napis = "";
            napis +=  Numer + ". reguła "; //wypisywanie numer reguły
            napis += Environment.NewLine;
            foreach (Deskryptor deskryptory in Deskryptory)
            {
                napis += "(a" + (deskryptory.Argument + 1) + "=" + deskryptory.Wartosc + ")"; //wypisywanie np. (a=0) 
                if (deskryptory != this.Deskryptory.Last()) //wypisywanie koniukcji
                    napis += "^";
            }
            napis += "=>(d=" + Decyzja + ")";//wypisanie =>(d=0)

            if (Support > 1) //wypisanie supportu
                napis += "[" + Support + "]";
            return napis;
        }

        ~Regula()
        {

        }
    }
}
