namespace DaneZPlikuConsole
{
    public class Deskryptor
    {
        private int wartosc; //prywatne atrybuty

        public int Wartosc { get => wartosc; set => wartosc = value; }

        public int Getwartosc() //metoda zwracająca wartosc
        { return Wartosc; }

        public void Setwartosc(int value) // metoda zmieniająca wartosc
        { Wartosc = value; }

        public Deskryptor(int numer, int wartosc) //konstruktor deskryptora
        {
            this.Setwartosc(wartosc);
        }

        ~Deskryptor() //dekonstruktor
        {

        }

    }
}
