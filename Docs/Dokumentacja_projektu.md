# Dokumentacja projektu: System zarządzania hotelem

## 1. Temat projektu

**System zarządzania hotelem w języku C# z wykorzystaniem WPF oraz Entity Framework Core**

## 2. Cel projektu

Celem projektu jest przygotowanie aplikacji desktopowej wspierającej podstawową pracę recepcji hotelowej. Aplikacja pozwala na obsługę gości, pokoi, rezerwacji, płatności i usług dodatkowych. System przechowuje dane w relacyjnej bazie SQLite obsługiwanej przez Entity Framework Core.

## 3. Wymagania funkcjonalne

1. Użytkownik może przeglądać listę gości.
2. Użytkownik może dodać nowego gościa.
3. Użytkownik może przeglądać listę pokoi i ich status.
4. Użytkownik może sprawdzić dostępność pokoi w wybranym terminie.
5. Użytkownik może utworzyć nową rezerwację.
6. Użytkownik może przeglądać listę rezerwacji.
7. Użytkownik może dodać płatność do rezerwacji.
8. Użytkownik może przeglądać dostępne usługi dodatkowe, w tym ich dodawanie i edycję.
9. System pokazuje podstawowe statystyki na panelu głównym.

## 4. Wymagania niefunkcjonalne

- aplikacja desktopowa wykonana w WPF;
- baza danych SQLite;
- dostęp do danych przez Entity Framework Core;
- podział na warstwy;
- obsługa błędów;
- walidacja podstawowych danych;
- czytelny i spójny interfejs użytkownika.

## 5. Architektura aplikacji

Projekt wykorzystuje wzorzec **MVVM**:

- **Models** — klasy encji bazy danych;
- **Views** — interfejs użytkownika w XAML;
- **ViewModels** — logika prezentacji i komendy;
- **Services** — logika biznesowa;
- **Repositories** — warstwa pośrednia do pracy z danymi;
- **Data** — konfiguracja Entity Framework Core i inicjalizacja danych.

Dodatkowo zastosowano **Dependency Injection**, dzięki czemu klasy są luźno powiązane i łatwiejsze do testowania oraz rozbudowy.

## 6. Opis bazy danych

Baza danych została znormalizowana i składa się z kilku powiązanych tabel. Zastosowano klucze główne, klucze obce oraz ograniczenia unikalności, np. unikalny adres e-mail gościa i unikalny numer pokoju.

### Tabele

| Tabela | Opis |
|---|---|
| Guests | Dane gości hotelowych |
| RoomTypes | Typy pokoi, pojemność i cena za noc |
| Rooms | Pokoje hotelowe |
| Reservations | Rezerwacje pokoi |
| Payments | Płatności za rezerwacje |
| Employees | Pracownicy obsługujący rezerwacje |
| Services | Usługi dodatkowe |
| ReservationServices | Tabela łącząca rezerwacje z usługami |

## 7. Integralność danych

System wykorzystuje relacje:

- `Guest 1..N Reservation`
- `Room 1..N Reservation`
- `RoomType 1..N Room`
- `Reservation 1..N Payment`
- `Reservation N..M Service` przez tabelę `ReservationServices`
- `Employee 1..N Reservation`

Zastosowano także ograniczenia usuwania, np. rezerwacje nie usuwają automatycznie gości ani pokoi.

## 8. Instrukcja uruchomienia

1. Zainstalować Visual Studio 2022 z obsługą .NET Desktop Development.
2. Otworzyć plik `HotelManagementApp.csproj`.
3. Poczekać na przywrócenie pakietów NuGet.
4. Uruchomić aplikację.
5. Baza danych `hotel.db` zostanie utworzona automatycznie.

## 9. Możliwe rozszerzenia

- edycja i usuwanie rekordów;
- logowanie użytkowników;
- generowanie faktur PDF;
- eksport raportów do pliku CSV lub Excel;
- kalendarz rezerwacji;
- obsługa zdjęć pokoi.


## Aktualizacja interfejsu

Panel główny został zmieniony na ekran statystyczny. Zawiera kafelki z najważniejszymi liczbami, listę ostatnich rezerwacji oraz szybki podgląd meldunków i wartości rezerwacji. Zakładka „Usługi” umożliwia dodawanie, edycję i usuwanie usług, a zakładka „Pracownicy” pozwala dodawać oraz usuwać pracowników.
