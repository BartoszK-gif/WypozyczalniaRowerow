# System Zarządzania Wypożyczalnią Rowerów (C#)

## Opis
Aplikacja służy do zarządzania rezerwacjami i wynajmem pojazdów w wypożyczalni rowerów. System pozwala na dodawanie rowerów do bazy, zapisywanie danych klientów oraz przypisywanie wolnych jednośladów do konkretnych osób na określony czas. Program pozwala też na płacenie za usługę na różne sposoby.

---

## Lista klas

* **Wypozyczalnia**
    * **Odpowiedzialność:** Główna klasa w programie. Trzyma listy rowerów oraz aktywnych wypożyczeń i pilnuje, żeby wszystko trafiało na swoje miejsce (Prezentacja "Podstawy OOP", Slajd 15).
    * **Właściwości:** `_nazwa`, `_rowery`, `_wypozyczenia`.
    * **Metody:** `DodajRower()`, `StworzWypozyczenie()`, `WyswietlWypozyczenia()`.
* **Rower**
    * **Odpowiedzialność:** Reprezentuje konkretny rower w bazie i pilnuje, czy ktoś aktualnie na nim jeździ.
    * **Właściwości:** `_numer`, `_typ`, `_czyWypozyczony`.
    * **Metody:** `CzyJestWolny()`, `Wypozycz()`, `Zwroc()`.
* **Klient**
    * **Odpowiedzialność:** Trzyma dane klienta, który wypożycza sprzęt.
    * **Właściwości:** `_identyfikator`, `_imie`, `_telefon`.
    * **Metody:** `PobierzImie()`.
* **Wypozyczenie**
    * **Odpowiedzialność:** Łączy klienta z konkretnym rowerem oraz deklarowanym czasem wypożyczenia.
    * **Właściwości:** `_identyfikator`, `_klient`, `_rower`, `_czasGodziny`.
    * **Metody:** `WyswietlSzczegoly()`.
* **Oplata**
    * **Odpowiedzialność:** Ogólny szablon dla płatności za wypożyczenie.
    * **Właściwości:** `_identyfikator`, `_kwota`.
    * **Metody:** `Rozlicz()`.
* **OplataKarta**
    * **Odpowiedzialność:** Obsługuje transakcje bezgotówkowe za pomocą karty płatniczej.
    * **Właściwości:** Wszystko z klasy Oplata oraz `_numerKarty`.
    * **Metody:** `Rozlicz()`.
* **OplataGotowka**
    * **Odpowiedzialność:** Obsługuje płatności tradycyjne banknotami w kasie.
    * **Właściwości:** Przejęte z klasy Oplata.
    * **Metody:** `Rozlicz()`.

---

## Opis relacji między klasami

Klasy łączą się ze sobą na podstawie schematów ze **Slajdu 15** i **Slajdu 17** z "Relacje między klasami":

1. **Kolekcja / Agregacja (Slajd 15):** Klasa `Wypozyczalnia` trzyma w środku generyczne listy `List<Rower>` oraz `List<Wypozyczenie>`. Rowery to osobne obiekty i mogą fizycznie istnieć w magazynie, nawet jeśli zlikwidujemy punkt wypożyczalni.
2. **Przekazanie obiektu jako parametr metody (Slajd 15):** Metoda `DodajRower(Rower rower)` dostaje z zewnątrz gotowy obiekt roweru i po prostu dorzuca go do wewnętrznej listy.
3. **Właściwość (Slajd 15):** Klasa `Wypozyczenie` zapisuje w swoich polach `_klient` i `_rower` bezpośrednie linki do tych obiektów, żeby trwale powiązać osobę ze sprzętem.
4. **Dziedziczenie (Slajd 17):** Klasy `OplataKarta` i `OplataGotowka` dziedziczą po klasie `Oplata` przy użyciu dwukropka (`:`), bo każda z nich "jest rodzajem" transakcji finansowej.

---

## Cztery zasady OOP (4 Filary)

1. **Enkapsulacja (Prezentacja "Cztery zasady OOP", Slajd 3, Slajd 32)**
   * **Gdzie jest:** W klasie `Rower`.
   * **Jak to działa:** Wszystkie zmienne mają modyfikator dostępu `private` (np. `private bool _czyWypozyczony`), co oznacza, że są schowane przed bezpośrednim dostępem z zewnątrz. Zgodnie z analogią bankomatu ze slajdu 3, nikt nie może ręcznie zmienić statusu roweru. Trzeba wywołać metodę `Wypozycz()`, która sama sprawdza reguły biznesowe i bezpiecznie blokuje rower.

2. **Dziedziczenie (Prezentacja "Cztery zasady OOP", Slajd 11 / Prezentacja "Kompozycja vs dziedziczenie", Slajd 13)**
   * **Gdzie jest:** W klasach `OplataKarta`, `OplataGotowka` i `Oplata`.
   * **Jak to działa:** Zastosowałem zasadę „jest rodzajem” ze slajdu 13. Płatność kartą i gotówką to są rodzaje opłat. Dzięki dziedziczeniu podklasy dostały wspólne pola (`_identyfikator`, `_kwota`) automatycznie z klasy nadrzędnej przez konstruktor bazowy `: base(identyfikator, kwota)`.

3. **Polimorfizm (Prezentacja "Cztery zasady OOP", Slajd 11)**
   * **Gdzie jest:** W klasie `Program` w menu przy uruchamianiu płatności.
   * **Jak to działa:** Tworzę zmienną ogólnego typu `Oplata oplata`. Trafia do niej obiekt `OplataKarta` albo `OplataGotowka` (w zależności od wyboru użytkownika). Wywołanie polecenia `oplata.Rozlicz()` powoduje, że program automatycznie uruchamia odpowiednią, nadpisaną wersję metody (słowo kluczowe `override`), co dokładnie odpowiada przykładowi ze slajdu 11.

4. **Abstrakcja (Prezentacja "AWSB_OOP", Slajd 2)**
   * **Gdzie jest:** W klasie `Oplata` i metodzie `Rozlicz()`.
   * **Jak to działa:** Klasa `Oplata` oraz metoda `Rozlicz()` zostały oznaczone słowem kluczowym `abstract`, dokładnie tak jak na wzorze ze **Slajdu 2 w pliku AWSB_OOP.pdf**. Uniemożliwia to stworzenie bezpośredniego obiektu `new Oplata()`. Działa to jak sztywny kontrakt – zmusza klasy pochodne do wdrożenia własnej, szczegółowej implementacji płatności.

---

## Wykryte i poprawione błędy

Podczas pisania kodu udało mi się uniknąć typowych błędów z prezentacji:

* **Błąd 1:** Na początku mogłem zmienić stan roweru pisząc w programie `rower._czyWypozyczony = true`. Poprawiłem to zgodnie ze **Slajdem 3 ("Cztery zasady OOP")** – pola są prywatne, a stan zmienia bezpieczna metoda.
* **Błąd 2:** Myślałem, żeby klasa `Rower` dziedziczyła po klasie `Wypozyczalnia`. Według **Slajdów 12 i 13 ("Kompozycja vs dziedziczenie")** to błąd, bo rower nie jest rodzajem wypożyczalni. Zamiast tego wypożyczalnia ma w sobie listę obiektów `Rower`.
* **Błąd 3:** Na początku planowałem stworzyć jedną wielką klasę obsługującą wszystko. Zgodnie ze **Slajdem 15 w "Podstawy OOP"** podzieliłem system na mniejsze, wyspecjalizowane klasy.