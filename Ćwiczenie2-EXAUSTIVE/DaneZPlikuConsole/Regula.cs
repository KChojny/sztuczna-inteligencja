using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaneZPlikuConsole
{
    class Regula
    {
        public Dictionary<int, string> deskryptory = new Dictionary<int, string>();
        public string decyzja;
        public int support = 0;
        string napis = "";
        public override string ToString()
        {
            int i = deskryptory.Count;
            foreach (var element in deskryptory)
            {
                switch (i)
                {
                    case 1:
                        napis += "(a" + (element.Key + 1) + " = " + element.Value + ")" + " => " + "(d = " + decyzja + ")";
                        if (support > 1)
                            napis += "  [" + support + "]";
                        break;
                    default:
                        napis += "(a" + (element.Key + 1) + " = " + element.Value + ")" + " ^ ";
                        i--;
                        break;
                }
            }
            return napis;
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
