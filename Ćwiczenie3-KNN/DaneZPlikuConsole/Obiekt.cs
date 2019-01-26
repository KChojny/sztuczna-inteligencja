using System;
using System.Collections.Generic;
using System.Linq;

namespace DaneZPlikuConsole
{
    public class Obiekt
    {
        private string decyzja; //prywatne atrybutów

        private List<Deskryptor> deskryptory = new List<Deskryptor>(); //lista deskryptorów


        public string Decyzja //upubliczenie atrybutu
        {
            get => decyzja; set => decyzja = value; 
        }

        public List<Deskryptor> Deskryptory { get => deskryptory; set => deskryptory = value; }
                
        public Obiekt(string s) //konstruktor Obiekt
        {
            string[] t = s.Trim().Split();
            for (int i = 0; i < t.Length - 1; i++)
            {
                Deskryptory.Add(new Deskryptor(i + 1, Int32.Parse(t[i])));
            }
            this.Decyzja = t.Last();
        }

        public override string ToString() //metoda do nadpisywania
        {
            string napis = "";
            foreach (var element in Deskryptory)
                napis += element.Getwartosc() + " ";
            napis += Decyzja;
            return napis;
        }

        ~Obiekt()
        {

        }
    }
}

