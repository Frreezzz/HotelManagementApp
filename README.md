# System zarządzania hotelem

Projekt zaliczeniowy wykonany w języku **C#** jako aplikacja desktopowa **WPF** z wykorzystaniem **Entity Framework Core** oraz bazy danych **SQLite**.

## Cel projektu

Celem projektu jest stworzenie aplikacji umożliwiającej podstawowe zarządzanie pracą hotelu: obsługę gości, pokoi, rezerwacji, płatności oraz usług dodatkowych. System został zaprojektowany tak, aby spełniał wymagania dotyczące struktury bazy danych, podziału aplikacji na warstwy oraz użycia nowoczesnych technologii dostępu do danych.

## Technologie

- C#
- WPF
- .NET 8
- Entity Framework Core
- SQLite
- MVVM
- Dependency Injection
- Repository Pattern
- Service Layer
- Migracje EF Core

## Funkcjonalności

- lista gości hotelowych;
- dodawanie nowego gościa;
- lista pokoi z typem, ceną i statusem;
- sprawdzanie dostępności pokoi w podanym terminie;
- tworzenie rezerwacji;
- lista rezerwacji z gościem, pokojem, statusem i ceną;
- dodawanie płatności do rezerwacji;
- lista usług dodatkowych;
- panel główny ze statystykami: aktywne rezerwacje, dostępne pokoje, suma płatności;
- obsługa błędów i walidacja podstawowych danych.

## Struktura projektu

```text
HotelManagementApp/
├── Commands/              # RelayCommand dla MVVM
├── Data/                  # DbContext oraz dane startowe
├── Migrations/            # Migracja początkowa bazy danych
├── Models/                # Encje bazy danych
├── Repositories/          # Generyczne repozytorium
├── Services/              # Logika biznesowa aplikacji
├── ViewModels/            # MainViewModel i ObservableObject
├── Docs/                  # Dokumentacja projektu i ERD
├── App.xaml
├── MainWindow.xaml
└── HotelManagementApp.csproj
```

## Model danych

Główne tabele:

- `Guests` — goście hotelowi;
- `Rooms` — pokoje;
- `RoomTypes` — typy pokoi i cena za noc;
- `Reservations` — rezerwacje;
- `Payments` — płatności;
- `Employees` — pracownicy;
- `Services` — usługi dodatkowe, w tym ich dodawanie i edycję;
- `ReservationServices` — tabela łącząca rezerwacje i usługi.

## Relacje

- jeden gość może mieć wiele rezerwacji;
- jeden pokój może mieć wiele rezerwacji w różnych terminach;
- jeden typ pokoju może być przypisany do wielu pokoi;
- jedna rezerwacja może mieć wiele płatności;
- jedna rezerwacja może mieć wiele usług dodatkowych;
- jeden pracownik może obsługiwać wiele rezerwacji.

## Uruchomienie projektu

1. Otwórz folder projektu w **Visual Studio 2022**.
2. Otwórz plik `HotelManagementApp.csproj`.
3. Przywróć pakiety NuGet, jeśli Visual Studio nie zrobi tego automatycznie.
4. Uruchom projekt przyciskiem **Start**.
5. Przy pierwszym uruchomieniu zostanie utworzona lokalna baza danych `hotel.db` oraz dane przykładowe.

## Komendy EF Core

W razie potrzeby można użyć komend:

```bash
dotnet ef database update
```

Nowa migracja:

```bash
dotnet ef migrations add NazwaMigracji
```

## Proponowany opis na obronę

Projekt został wykonany w architekturze MVVM. Warstwa widoku znajduje się w pliku `MainWindow.xaml`, logika prezentacji w `MainViewModel`, natomiast logika biznesowa w `HotelService`. Dostęp do danych jest realizowany przez Entity Framework Core oraz wzorzec Repository. Baza danych została zaprojektowana z uwzględnieniem kluczy głównych, kluczy obcych, relacji jeden-do-wielu oraz relacji wiele-do-wielu dla usług przypisanych do rezerwacji.


## Aktualizacja interfejsu

Panel główny został rozbudowany o kafelki statystyczne, listę ostatnich rezerwacji oraz krótkie podsumowanie modułów systemu. Zakładka „Usługi” umożliwia dodawanie nowych usług oraz edycję istniejących pozycji po zaznaczeniu rekordu w tabeli.
