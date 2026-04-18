# 🚗 Autotberelek — .NET MAUI Mobilalkalmazás

Az **Autotberelek** egy autóbérlő mobilalkalmazás, amely kizárólag bérlők számára készült. Az alkalmazás lehetővé teszi az elérhető autók böngészését, szűrését, részletes adatainak megtekintését és a bérlés elvégzését szimulált fizetéssel.

---

## 📱 Funkciók

- **Bejelentkezés / Regisztráció** — JWT token alapú autentikáció
- **Jelszó visszaállítás** — 6 jegyű email kóddal
- **Autólista** — az összes kibérelhető autó megjelenítése képekkel
- **Szűrők** — kategória, üzemanyag, váltó, telephely és elérhetőség alapján
- **Autó részletei** — teljes műszaki adatlap, extra felszereltség, telephely információ
- **Bérlés** — dátumválasztó, árkalkuláció, szimulált fizetés
- **Visszaigazoló email** — bérlés után automatikusan küldve

---

## 🛠️ Technológiák

| Réteg | Technológia |
|---|---|
| Frontend | .NET 10 MAUI |
| Architektúra | MVVM (CommunityToolkit.Mvvm) |
| Navigáció | Shell + NavigationPage |
| HTTP | HttpClient + System.Text.Json |
| Auth | JWT Bearer Token |
| Token tárolás | MAUI Preferences |
| UI komponensek | CommunityToolkit.Maui |

---

## 📋 Követelmények

- Visual Studio 2022+ (MAUI workload telepítve)
- .NET 10 SDK
- Android SDK (fizikai eszköz vagy emulátor)
- A backend fusson lokálisan (`localhost:5128`)

---

## 🚀 Telepítés és futtatás

### 1. Projekt klónozása

```bash
git clone https://github.com/autoberles/Mobil
cd AutoBerlo
```

### 2. NuGet csomagok visszaállítása

Visual Studioban:
```
Build → Restore NuGet Packages
```

Vagy terminálban:
```bash
dotnet restore
```

### 3. Backend indítása

A mobilalkalmazásnak szüksége van a futó backendre. Indítsd el a backendet:

```bash
cd autoberles-backend
dotnet run
```

A backend alapértelmezetten a `http://localhost:5128` címen fut.

---

## 📡 Backend kapcsolat — Android eszköz beállítása

Fizikai Android eszközön teszteléshez **USB kapcsolat** és **adb reverse** szükséges, mivel a telefon nem éri el közvetlenül a fejlesztői gép `localhost`-ját.

### Lépések

**1.** Csatlakoztasd a telefont USB-vel és engedélyezd a **fejlesztői módot** (USB debugging)

**2.** Nyiss terminált Visual Studioban (`View → Terminal`) vagy PowerShellben

**3.** Navigálj az adb mappájába:

```powershell
cd "C:\Program Files (x86)\Android\android-sdk\platform-tools"
```

**4.** Ellenőrizd, hogy a telefon látható-e:

```powershell
.\adb.exe devices
```

Várt kimenet:
```
List of devices attached
XXXXXXXX    device
```

**5.** Állítsd be a port átirányítást:

```powershell
.\adb.exe reverse tcp:5128 tcp:5128
```

Várt kimenet: `5128`

**6.** Ezután már buildelj és telepíts az eszközre — a backend elérhető lesz.

⚠️ **Fontos:** Az `adb reverse`-t minden USB lecsatlakoztatás, telefon újraindítás vagy PC újraindítás után újra kell futtatni!

### Android emulátor esetén

Emulátoron nem kell `adb reverse` — a `MauiProgram.cs`-ben automatikusan `10.0.2.2:5128` címet használ.

---

## 🔑 Teszteléshez


Regisztrálj egy új fiókot a teszteléshez. A jelszónak tartalmaznia kell: kisbetűt, nagybetűt és számot (min. 8 karakter).

Telefonszám formátum: `+36 20 123 4567`

---

## 📦 Függőségek

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.2" />
<PackageReference Include="CommunityToolkit.Maui" Version="9.1.1" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.3.0" />
```

---

## 🔗 Kapcsolódó repók

- **Backend:** [github.com/autoberles/backend](https://github.com/autoberles/backend)
- **Frontend:** [github.com/autoberles/frontend](https://github.com/autoberles/frontend)

---

## 👨‍💻 A projekt készítői

| Név              | Szerep                    |
| ---------------- | ------------------------- |
| Marquetant Zalán | Frontend fejlesztő        |
| Márton Kristóf   | Backend fejlesztő         |
| Szabó Domonkos   | Mobilalkalmazás fejlesztő |

---