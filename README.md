# CaYaChat

**TR:** Oyun ve uygulamaların üzerinde, her zaman en üstte duran küçük bir Windows sohbet uygulaması. Mesajlar MongoDB üzerinden paylaşılır; içinde komutlarla yönetilen bir sohbet botu ve lisans anahtarı sistemi vardır.
**EN:** A small always-on-top Windows chat overlay for games and apps. Messages are shared through MongoDB, with a command-driven chat bot and a licence-key system.

---

## Türkçe

### Özellikler
- Oyunun/uygulamanın üzerinde duran sohbet penceresi (oyun tam ekran değil, pencereli veya kenarlıksız pencereli olmalı).
- Genel kısayol tuşu (**Ctrl+Alt+T**) ve saydamlık açma/kapama.
- MongoDB tabanlı ortak mesaj akışı (son 15 mesaj tutulur).
- Sohbet botu: `/addResponse "mesaj" "yanıt"`, `/removeResponse`, `/botON`, `/botOFF` (bot tetikleyicileri küçük harf olmalı).
- Moderasyon ve araçlar: `/ban`, `/unban`, `/clear`, `/timer` (alarm sesli), `/help`, tek seferlik duyuru mesajı.
- Özelleştirilebilir filigran (watermark) metinleri.
- Lisans anahtarı ile giriş ve yöneticiler için lisans yönetim paneli (ekle, sil, yasakla).
- Kullanıcı ayarları `%AppData%\CaYaChatMessage` altında saklanır.

### Gereksinimler
- Windows, .NET 6 (WinForms) — .NET 8 SDK ile de derlenir.
- Kendi MongoDB veritabanınız (ör. MongoDB Atlas). Kullanılan koleksiyonlar: `messages`, `BOT`, `BotDataBaseResponse`, `BotName`, `BotResponseDefault`, `Fligran`, `LicenceSystem`, `OneTime`.

### Kurulum
1. MongoDB Atlas'ta bir küme ve kullanıcı oluşturun.
2. Bağlantı bilgisini ortam değişkeni olarak ayarlayın (`set-env.example.ps1` dosyasını `set-env.ps1` olarak kopyalayıp doldurabilirsiniz, bu dosya git'e eklenmez):
   - `CAYACHAT_MONGO` = `mongodb+srv://<user>:<password>@<cluster>/<db>`
   - `CAYACHAT_MONGO_DB` = veritabanı adı (ör. `CaYaChatSystem`)
3. `LicenceSystem` koleksiyonuna kendinize bir lisans kaydı ekleyin.
4. Derleyip çalıştırın:
   ```
   dotnet build CaYaChat/CaYaChat.csproj -c Release
   ```

### Proje yapısı
- `LoginForm`, `StartupLicenceCheck` — giriş ve lisans kontrolü
- `ChatApp` — ana sohbet penceresi, komutlar, kısayollar
- `MongoDBService` — tüm veritabanı işlemleri
- `MongoDBAddress` — bağlantı bilgisinin tutulduğu yer
- `BotSettings`, `CustomFligran`, `SettingsForm`, `Help`, `Blocked`, `StartupLicenceManagement` — ayar ve yönetim pencereleri

### Güvenlik notu
Orijinal sürümde bağlantı bilgisi derlenmiş exe'nin sonuna eklenen bir DLL'den okunuyordu. Yayın için kodda hiçbir bağlantı bilgisi ya da şifre bırakılmadı; `CAYACHAT_MONGO` ayarlıysa bağlantı oradan okunur. Eski derlenmiş sürümler bu yüzden yayımlanmadı.

### Bilinen sorunlar
- .NET 6 desteği bitti (NETSDK1138 uyarısı).
- Erişim kontrolü sadece lisans anahtarına dayanır; veritabanı bağlantısı istemcide olduğu için gerçek bir sunucu tarafı yetkilendirme yoktur.
- `FirebaseService.cs` yorum satırına alınmış, kullanılmıyor.
- `.csproj` içindeki paket simgesi (`PackageIcon`) repoda olmayan yerel bir yolu gösteriyor.

### Proje tarihleri
- Oluşturma: 8 Ağustos 2024
- Son güncelleme: 13 Ağustos 2024

### Lisans
Belirtilmemiş.

---

## English

### Features
- Always-on-top chat overlay for games/apps (the game must run windowed or borderless windowed).
- Global hotkey (**Ctrl+Alt+T**) and opacity toggle.
- Shared MongoDB message feed (keeps the latest 15 messages).
- Chat bot: `/addResponse "message" "reply"`, `/removeResponse`, `/botON`, `/botOFF` (triggers must be lowercase).
- Moderation and tools: `/ban`, `/unban`, `/clear`, `/timer` (with alarm sound), `/help`, one-time announcement message.
- Customisable watermark texts.
- Licence-key login and an admin licence management panel (add, delete, ban).
- User settings stored under `%AppData%\CaYaChatMessage`.

### Requirements
- Windows, .NET 6 (WinForms); also builds with the .NET 8 SDK.
- Your own MongoDB (e.g. Atlas). Collections: `messages`, `BOT`, `BotDataBaseResponse`, `BotName`, `BotResponseDefault`, `Fligran`, `LicenceSystem`, `OneTime`.

### Setup
1. Create a MongoDB Atlas cluster and user.
2. Set environment variables (copy `set-env.example.ps1` to the git-ignored `set-env.ps1`):
   - `CAYACHAT_MONGO` = `mongodb+srv://<user>:<password>@<cluster>/<db>`
   - `CAYACHAT_MONGO_DB` = database name (e.g. `CaYaChatSystem`)
3. Add a licence document for yourself to `LicenceSystem`.
4. Build and run: `dotnet build CaYaChat/CaYaChat.csproj -c Release`

### Security note
The original build read its connection string from a DLL appended to the compiled exe. For publication, no credentials remain in the code; when `CAYACHAT_MONGO` is set, the connection is read from it. Old compiled binaries were intentionally not published.

### Known issues
- .NET 6 is end-of-life (NETSDK1138 warning).
- Access control relies only on licence keys; the client connects to the database directly, so there is no real server-side authorisation.
- `FirebaseService.cs` is commented out and unused.
- The `PackageIcon` entry in the `.csproj` points to a local path that is not in the repo.

### Project dates
- Created: 8 August 2024
- Last updated: 13 August 2024

### License
Not specified.