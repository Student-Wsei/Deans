Autor: Paweł Polakiewicz (15590)

URL: https://github.com/Student-Wsei/Deans

Uruchomienie projektu:

- dotnet tool install --global dotnet-ef --version 10.0.0
- dotnet ef database update
- dotnet run --project Api

Logowanie:
Admin: admin@wsei.edu.pl | Admin123!
Pracownik Dziekanatu: dean@wsei.edu.pl | Dean1234!

AUTENTYKACJA (/api/auth):
POST login - Zaloguj się i otrzymaj Access Token + Refresh Token
POST refresh - Odśwież Access Token gdy wygaśnie
POST revoke - Odwołaj Refresh Token (wylogowanie)
GET me - Sprawdź swoje dane i role

STUDENCI (/api/students):
GET - Lista wszystkich studentów ze stronicowaniem (AdminOnly)
GET {id} - Szczegóły konkretnego studenta
POST - Dodaj nowego studenta
PUT {id} - Zaktualizuj dane studenta
PATCH {id}/status - Zmień status studenta
POST {studentId}/grades - Dodaj ocenę studentowi
GET {studentId}/grades - Pobierz oceny studenta
PUT {studentId}/grades/{gradeId} - Zaktualizuj ocenę

KIERUNKI STUDIÓW (/api/degree-programs):
POST - Dodaj nowy kierunek studiów
GET {id}/report - Pobierz raport o kierunku

Załaczona kolekcja do postmana pozwoli łatwo sobie poklikać po applikacji
