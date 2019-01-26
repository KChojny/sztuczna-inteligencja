using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaneZPlikuConsole
{
    class macierz
    {
        int[] KomórkaMacierzy(string[] ob1, string[] ob2)
        {
            List<int> Komórka = new List<int>();

            if (ob1.Last() == ob2.Last())
                return Komórka.ToArray();
            for (int i = 0; i < ob1.Length - 1; i++)
                if (ob1[i] == ob2[i])
                    Komórka.Add(i);
            return Komórka.ToArray();
        }
        public int[][][] MacierzNieodroznialnosci(string[][] obiekty)
        {
            int[][][] macierz = new int[obiekty.Length][][];
            for (int i = 0; i < obiekty.Length; i++)
            {
                macierz[i] = new int[obiekty.Length][];
                for (int j = 0; j < obiekty.Length; j++)
                {
                    macierz[i][j] = KomórkaMacierzy(obiekty[i], obiekty[j]);
                }
            }
            return macierz;
        }
        bool CzyKombinacjaWKomórce(int[] komórka, int[] kombinacje)
        {
            foreach (var komb in kombinacje)
            {
                if (!(komórka.Contains(komb)))
                    return false;

            }
            return true;
        }
        public bool CzyKombinacjawWierszu(int[][] wiersz, int[] kombinacje)
        {
            for (int i = 0; i < wiersz.Length; i++)
            {
                if (CzyKombinacjaWKomórce(wiersz[i], kombinacje))
                    return true;

            }
            return false;
        }
        public bool CzyRegulaZawieraRegule(Regula r1, Regula r2)
        {
            foreach (var desk in r2.deskryptory)
            {
                if (!r1.deskryptory.ContainsKey(desk.Key) || r1.deskryptory[desk.Key] != desk.Value)
                    return false;
            }
            return true;
        }
        public bool CzyRegulaZawieraReguleZListy(List<Regula> lista, Regula r)
        {
            foreach (var rlisty in lista)
            {
                if (CzyRegulaZawieraRegule(r, rlisty))
                    return true;
            }
            return false;
        }
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
        public int Support(Regula r, string[][] obiekty)
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
    }
}
