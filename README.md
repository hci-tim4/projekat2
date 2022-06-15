# projekat2

Projekat iz predmeta Interakcija čovek računar, jun 2022. godina

Aplikacija - Železnice Srbije

Tim 4:

  * Violeta Erdelji, SW 32/2019 

  * Anastasija Samčović, SW 44/2019

  * Milica Petrović, SW 46/2019

Koraci za pokretanje aplikacije "ŽELEZNICE SRBIJE"

1. Preuzimanje sa sajta github opcijom download zip ili klonovanjem korišćenjem komande git clone https://github.com/hci-tim4/projekat2.git
2. Podešavanje putanja foldera icon i imeges u railway.csproj fajlu. Konfigurisati apsolutnu putanju do prethodno navedenih foldera u ItemGroup tagu, Resource podtagu.
3. Kopirati folder Help u bin/Debug/netcoreapp3.1 ili u slučaju ako se pokreće u Release režimu onda kopirati Help u bin/Release/netcoreapp3.1.
4. Otvoriti aplikaciju pokretanjem railway.sln
5. U fajlu MainWindow.xaml.cs iskomentarisati deo koji popunjava bazu podacima i generiše buduće redove vožnje.
6. Pokrenite aplikaciju klikom na F5 ili u slučaju ako se pokreće u Release režimu CTRL + F5.
7. Logovanje u ulozi običnog korisnika: username: miki, password: 123
8. Logovanje u ulozi menadžera: username: djole, password: 123


Napomena: Uputstvo je pisano za Microsoft Visual Studio okruženje. 
