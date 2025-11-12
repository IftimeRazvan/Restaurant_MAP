# 🗺️ Restaurant

O aplicație desktop de tip WPF pentru managementul meniului unui restaurant, cu funcționalități atât pentru clienți, cât și pentru angajați: catalog de preparate, filtrare avansată, administrarea comenzilor și configurarea meniurilor.

## 📋 Descriere
Restaurant MAP este o aplicație desktop construită cu .NET 9 și WPF care centralizează într-un singur loc experiența de explorare a meniului și operațiunile interne ale restaurantului. Utilizatorii se pot autentifica, pot salva datele de livrare și pot trimite comenzi, iar angajații au la dispoziție un panou dedicat pentru a gestiona preparatele, categoriile, meniurile și alergenii.

## 🚀 Caracteristici principale
- **Interfață modernă WPF:** Aplicație desktop cu design modern bazat pe Material Design și MVVM.
- **Autentificare și înregistrare:** Gestionarea conturilor clienților și angajaților, cu sesiune stocată la nivelul aplicației.
- **Catalog de preparate:** Listă dinamică de preparate și meniuri, actualizată din baza de date SQL Server.
- **Filtrare avansată:** Căutare după nume, filtrare după categorie, căutare după alergeni și opțiuni de includere/excludere.
- **Coș de cumpărături și reduceri:** Calcul al totalului cu discount-uri configurabile și cost de livrare automat.
- **Panou pentru angajați:** Administrare CRUD pentru categorii, preparate, meniuri și alergeni, plus alerte pentru stoc redus.
- **Configurare prin fișier:** Parametrii comerciali (discount-uri, praguri de livrare, alerte de stoc) sunt controlați din `App.config`.

## 🧩 Structura proiectului
Aplicația este organizată într-un singur proiect WPF (`Restaurant/`), separat în straturi clare pentru prezentare și date.

### Interfața WPF
- `Views/` conține ferestrele principale (`LoginView`, `MenuView`, `EmployeeView`) și ferestrele modale pentru editare (`DishDialogView`, `MenuDialogView`, etc.).
- `ViewModels/` implementează logica de prezentare folosind pattern-ul MVVM, comenzi (`Commands/`) și servicii auxiliare precum `ShoppingCart` și `SettingsHelper`.
- `Converters/` oferă conversii pentru binding (ex. disponibilitate produs, vizibilitate).

### Logica de business
- `Models/BusinessLogicLayer/` expune clase precum `DishBL`, `MenuBL`, `OrderBL` sau `UserBL`, care orchestrează validările și regulile de business (discount-uri, verificări de stoc, istoricul comenzilor).
- `SettingsHelper.cs` citește valorile din `App.config` pentru a unifica toate calculele legate de livrare și reduceri.

### Acces la date
- `Models/DataAccessLayer/` conține DAL-ul pentru SQL Server (`DishDAL`, `MenuDAL`, `OrderDAL`, `UserDAL` etc.), cu ajutorul `DALHelper.Connection` pentru inițierea conexiunilor.
- `Models/EntityLayer/` definește obiectele de transfer (`Dish`, `Menu`, `Category`, `User`, `Allergen`), toate bazate pe `BasePropertyChanged` pentru binding-uri reactive.

## 📊 Fluxuri principale în aplicație
### Experiența clienților
- **Autentificare/Înregistrare:** Utilizatorii își pot crea cont cu date de contact și adresă de livrare din `LoginView`. După autentificare, starea curentă este salvată în `App.CurrentUser`.
- **Explorarea meniului:** `MenuView` oferă tab-uri pentru meniuri, căutare instantă după nume și alergeni, selectarea categoriilor și afișarea disponibilității în timp real.
- **Coș de cumpărături:** Componenta `ShoppingCart` calculează subtotalul, aplică discount-urile din `SettingsHelper` și adaugă automat taxa de livrare dacă este necesar.

### Consola angajaților
- **Managementul catalogului:** Angajații pot adăuga, edita sau șterge categorii, preparate, meniuri și alergeni prin dialoguri dedicate.
- **Monitorizarea stocului:** `EmployeeViewModel` preîncarcă alertele pentru preparatele cu stoc scăzut pe baza pragurilor definite în configurare.
- **Administrarea comenzilor:** `OrderBL` expune operații pentru listarea și actualizarea comenzilor, facilitând procesarea lor în back office.

## ⚙️ Setări importante
`App.config` expune parametri ajustabili pentru business:
- praguri pentru livrare gratuită și cost de livrare (`Free_Delivery_Threshold`, `Delivery_Cost`)
- condiții și procente pentru discount-uri (`Minimum_Order_For_Discount`, `Discount_Percentage` etc.)
- alertă de stoc minim (`Minimum_Stock_Alert`)
- reducere suplimentară pentru meniuri (`Discount_Menu_Percentage`)
