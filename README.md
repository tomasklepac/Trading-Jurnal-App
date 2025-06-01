# 📈 TradingJournalApp

TradingJournalApp je moderní mobilní aplikace postavená na .NET MAUI, která umožňuje obchodníkům zaznamenávat své obchody, sledovat statistiky a analyzovat vývoj obchodního účtu pomocí přehledných grafů.

## ✨ Funkce

- ✅ **Zaznamenávání obchodů** – pomocí přehledného formuláře s výběrem směru, confluence, výsledku a dalších údajů.
- 📅 **Přehled obchodních dnů** – seznam obchodních dnů s celkovým PnL a počtem obchodů.
- 📊 **Analytická sekce** – win rate, průměrný zisk/ztráta, podíl Long vs. Short, vývoj účtu v čase.
- 🔍 **Filtrování podle časového období** – aktuální měsíc, rok, nebo vlastní rozsah.
- 💡 **Editace a mazání obchodů** – jednoduchá úprava záznamů pomocí pop-up oken.
- 🌙 **Tmavý režim** – moderní UI s tmavým vzhledem a kontrastními prvky.

## 📱 Použité technologie

- [.NET MAUI](https://learn.microsoft.com/dotnet/maui/) – multiplatformní vývoj pro Android/iOS
- **SQLite** – lokální ukládání obchodů
- **MVVM** – architektura oddělující logiku od UI
- **LiveChartsCore** – grafy s přehledným zobrazením vývoje PnL

## 🏁 Začínáme

1. Klonuj repozitář:
   ```bash
   git clone https://github.com/uzivatel/TradingJournalApp.git
   cd TradingJournalApp
   ```

2. Otevři projekt ve Visual Studiu 2022+ s nainstalovaným .NET MAUI workloadem.

3. Sestav a spusť aplikaci na Android/iOS emulátoru nebo reálném zařízení.

## 📂 Struktura projektu

```
TradingJournalApp/
│
├── Views/                  # .xaml soubory pro jednotlivé obrazovky (Journal, Analytics, Popups)
├── ViewModels/            # Logika (MVVM) pro zobrazení dat a zpracování událostí
├── Models/                # Datové modely jako Trade, TradeDay, ...
├── Data/                  # SQLite repository (TradeRepository.cs)
├── Resources/             # Styly, barvy, fonty
├── App.xaml               # Globální styl aplikace
└── MainPage.xaml          # Navigační taby
```

## 📸 Ukázky

| 📓 Journal Tab | 📈 Analytics Tab |
|----------------|------------------|
| ![journal](screenshots/journal.png) | ![analytics](screenshots/analytics.png) |

## 🛠 TODO

- Export do CSV
- Cloud sync (např. Firebase)
- Pokročilá filtrace (dle instrumentu, výsledku, atd.)

## 👨‍💻 Autor

Tomáš Klepač  
Fakulta aplikovaných věd, Západočeská univerzita v Plzni

---

> "Your trading is only as good as your journaling."
