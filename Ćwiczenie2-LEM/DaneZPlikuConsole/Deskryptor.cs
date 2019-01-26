namespace DaneZPlikuConsole
{
    class Deskryptor
    {
        private int argument; //numer argumentu
        private int obiekt; //numer obiektu
        private int pokrycie;//liczba pokryć
        private int wartosc;// warotść obiektu

        public int Argument { get => argument; set => argument = value; }
        public int Obiekt { get => obiekt; set => obiekt = value; }
        public int Pokrycie { get => pokrycie; set => pokrycie = value; }
        public int Wartosc { get => wartosc; set => wartosc = value; }

        public Deskryptor(int argument, int obiekt, int pokrycie, int wartosc)//konstruktor klasy
        {
            Argument = argument;
            Obiekt = obiekt;
            Pokrycie = pokrycie;
            Wartosc = wartosc;
        }

        ~Deskryptor()
        {

        }
    }
}
