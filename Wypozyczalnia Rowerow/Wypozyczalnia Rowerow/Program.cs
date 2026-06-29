using System;
using System.Collections.Generic;

namespace WypozyczalniaRowerowApp
{
    public abstract class Oplata
    {
        protected string _identyfikator;
        protected double _kwota;

        public Oplata(string identyfikator, double kwota)
        {
            _identyfikator = identyfikator;
            _kwota = kwota;
        }

        public abstract void Rozlicz();
    }

    public class OplataKarta : Oplata
    {
        private string _numerKarty;

        public OplataKarta(string identyfikator, double kwota, string numerKarty)
            : base(identyfikator, kwota)
        {
            _numerKarty = numerKarty;
        }

        public override void Rozlicz()
        {
            Console.WriteLine($"[TERMINAL] Pobrano {_kwota} zl z karty o numerze {_numerKarty}.");
        }
    }

    public class OplataGotowka : Oplata
    {
        public OplataGotowka(string identyfikator, double kwota)
            : base(identyfikator, kwota)
        {
        }

        public override void Rozlicz()
        {
            Console.WriteLine($"[KASA] Przyjeto gotowke w kwocie {_kwota} zl do kasy wypozyczalni.");
        }
    }

    public class Klient
    {
        private string _identyfikator;
        private string _imie;
        private string _telefon;

        public Klient(string identyfikator, string imie, string telefon)
        {
            _identyfikator = identyfikator;
            _imie = imie;
            _telefon = telefon;
        }

        public string PobierzImie()
        {
            return _imie;
        }
    }

    public class Rower
    {
        private int _numer;
        private string _typ;
        private bool _czyWypozyczony;

        public Rower(int numer, string typ)
        {
            _numer = numer;
            _typ = typ;
            _czyWypozyczony = false;
        }

        public int PobierzNumer()
        {
            return _numer;
        }

        public string PobierzTyp()
        {
            return _typ;
        }

        public bool CzyJestWolny()
        {
            return !_czyWypozyczony;
        }

        public bool Wypozycz()
        {
            if (_czyWypozyczony) return false;
            _czyWypozyczony = true;
            return true;
        }

        public void Zwroc()
        {
            _czyWypozyczony = false;
        }
    }

    public class Wypozyczenie
    {
        private string _identyfikator;
        private Klient _klient;
        private Rower _rower;
        private int _czasGodziny;

        public Wypozyczenie(string identyfikator, Klient klient, Rower rower, int czasGodziny)
        {
            _identyfikator = identyfikator;
            _klient = klient;
            _rower = rower;
            _czasGodziny = czasGodziny;
        }

        public void WyswietlSzczegoly()
        {
            Console.WriteLine($"  -> Wypozyczenie {_identyfikator}: Rower nr {_rower.PobierzNumer()} dla {_klient.PobierzImie()} na czas: {_czasGodziny}h.");
        }
    }

    public class Wypozyczalnia
    {
        private string _nazwa;
        private List<Rower> _rowery = new List<Rower>();
        private List<Wypozyczenie> _wypozyczenia = new List<Wypozyczenie>();

        public Wypozyczalnia(string nazwa)
        {
            _nazwa = nazwa;
        }

        public void DodajRower(Rower rower)
        {
            _rowery.Add(rower);
        }

        public bool StworzWypozyczenie(string identyfikatorWypozyczenia, Klient klient, int numerRoweru, int czasGodziny)
        {
            Rower wybranyRower = null;
            foreach (var rower in _rowery)
            {
                if (rower.PobierzNumer() == numerRoweru)
                {
                    wybranyRower = rower;
                    break;
                }
            }

            if (wybranyRower != null && wybranyRower.CzyJestWolny())
            {
                wybranyRower.Wypozycz();
                Wypozyczenie noweWypozyczenie = new Wypozyczenie(identyfikatorWypozyczenia, klient, wybranyRower, czasGodziny);
                _wypozyczenia.Add(noweWypozyczenie);
                return true;
            }
            return false;
        }

        public void WyswietlWypozyczenia()
        {
            Console.WriteLine($"\n--- Aktywne wypozyczenia w: {_nazwa} ---");
            if (_wypozyczenia.Count == 0)
            {
                Console.WriteLine("  Brak aktywnych wypozyczen.");
            }
            foreach (var wypozyczenie in _wypozyczenia)
            {
                wypozyczenie.WyswietlSzczegoly();
            }
        }

        public void WyswietlRowery()
        {
            Console.WriteLine($"\n--- Stan rowerow w wypozyczalni ---");
            foreach (var rower in _rowery)
            {
                string status = rower.CzyJestWolny() ? "DOSTEPNY" : "WYPOZYCZONY";
                Console.WriteLine($"  Rower nr {rower.PobierzNumer()} ({rower.PobierzTyp()}) - Status: {status}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Wypozyczalnia mojaWypozyczalnia = new Wypozyczalnia("Eko-Bike");

            mojaWypozyczalnia.DodajRower(new Rower(1, "Gorski"));
            mojaWypozyczalnia.DodajRower(new Rower(2, "Miejski"));
            mojaWypozyczalnia.DodajRower(new Rower(3, "E-Bike"));

            int licznik = 1;

            while (true)
            {
                Console.WriteLine("\n==============================");
                Console.WriteLine("  WYPOZYCZALNIA ROWEROW - MENU");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Pokaz dostepne rowery");
                Console.WriteLine("2. Wypozycz rower");
                Console.WriteLine("3. Pokaz aktualne wypozyczenia");
                Console.WriteLine("4. Rozlicz oplate (Polimorfizm)");
                Console.WriteLine("5. Wyjdz z programu");
                Console.Write("Wybierz opcje (1-5): ");

                string wybor = Console.ReadLine();

                if (wybor == "1")
                {
                    mojaWypozyczalnia.WyswietlRowery();
                }
                else if (wybor == "2")
                {
                    Console.WriteLine("\n--- NOWE WYPOŻYCZENIE ---");
                    Console.Write("Podaj imie i nazwisko klienta: ");
                    string imie = Console.ReadLine();
                    Console.Write("Podaj telefon: ");
                    string telefon = Console.ReadLine();

                    mojaWypozyczalnia.WyswietlRowery();

                    Console.Write("Wybierz numer roweru: ");
                    if (int.TryParse(Console.ReadLine(), out int nrRoweru))
                    {
                        Console.Write("Na ile godzin? ");
                        if (int.TryParse(Console.ReadLine(), out int czas))
                        {
                            Klient klient = new Klient($"K{licznik}", imie, telefon);
                            string idWyp = $"W{licznik}";

                            if (mojaWypozyczalnia.StworzWypozyczenie(idWyp, klient, nrRoweru, czas))
                            {
                                Console.WriteLine($"\n[SUKCES] Rower zostal wypozyczony! ID transakcji: {idWyp}");
                                licznik++;
                            }
                            else
                            {
                                Console.WriteLine("\n[BŁĄD] Ten rower jest niedostepny lub nie istnieje!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n[BŁĄD] Wprowadzono niepoprawna liczbe godzin!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n[BŁĄD] Wprowadzono niepoprawny numer roweru!");
                    }
                }
                else if (wybor == "3")
                {
                    mojaWypozyczalnia.WyswietlWypozyczenia();
                }
                else if (wybor == "4")
                {
                    Console.WriteLine("\n--- ROZLICZENIE (POLIMORFIZM) ---");
                    Console.Write("Podaj kwote do zaplaty: ");
                    if (double.TryParse(Console.ReadLine(), out double kwota))
                    {
                        Console.WriteLine("Wybierz forme platnosci:");
                        Console.WriteLine("1. Karta");
                        Console.WriteLine("2. Gotowka");
                        Console.Write("Wybor: ");
                        string typPlatnosci = Console.ReadLine();

                        Oplata oplata = null;

                        if (typPlatnosci == "1")
                        {
                            Console.Write("Podaj numer karty: ");
                            string nrKarty = Console.ReadLine();
                            oplata = new OplataKarta($"O{licznik}", kwota, nrKarty);
                        }
                        else if (typPlatnosci == "2")
                        {
                            oplata = new OplataGotowka($"O{licznik}", kwota);
                        }
                        else
                        {
                            Console.WriteLine("Niepoprawny wybor.");
                            continue;
                        }

                        Console.WriteLine();
                        oplata.Rozlicz();
                    }
                    else
                    {
                        Console.WriteLine("\n[BŁĄD] Niepoprawna kwota!");
                    }
                }
                else if (wybor == "5")
                {
                    Console.WriteLine("\nZamykanie systemu. Do widzenia!");
                    break;
                }
                else
                {
                    Console.WriteLine("\nNiepoprawna opcja! Wybierz od 1 do 5.");
                }
            }
        }
    }
}