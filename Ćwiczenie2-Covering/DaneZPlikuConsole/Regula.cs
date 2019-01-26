using System.Collections.Generic;


namespace DaneZPlikuConsole
{
    class Regula
    {
        private string decyzja; //numer decyzji w stringu
        private int support = 0; //wartość suppartu
        private Dictionary<int, string> deskryptory = new Dictionary<int, string>(); //w nim jest numer atrybutu i jego wartość 

        public string Decyzja { get => decyzja; set => decyzja = value; }
        public int Support { get => support; set => support = value; }
        public Dictionary<int, string> Deskryptory { get => deskryptory; set => deskryptory = value; }

        public override string ToString() //metoda do wypisania reguły
        {
            string ciag = "";
            foreach (var a in Deskryptory)
                ciag += "(a" + (a.Key + 1) + " = " + a.Value + ")^";
            ciag = ciag.TrimEnd('^');
            ciag += " => (d = " + Decyzja + ") ";
            if (Support > 1) ciag += "[" + Support + "]";

            return ciag;
        }

        ~Regula()
        {

        }

    }
}