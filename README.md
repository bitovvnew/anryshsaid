# SAIL PROJECT 🚀

**SAIL Launcher** · версия **bito 0.0.0.1**  
**Цифровая подпись:** dolbaeb productions  
**Язык:** 🇷🇺 Русский · 🇺🇸 English (флаги в интерфейсе)

Красивый WPF-лаунчер в стиле **Apple / Windows 11** с договором на **15% с продажи Roblox-аккаунта** и галереей мемов.

## Автоматическая сборка (GitHub Actions)

Проект настроен для автоматической компиляции в `.exe` при push в `main`/`master`.

### Как подключить GitHub

```powershell
cd "d:\Bot For Selling\Launcher SaIL"
git init
git add .
git commit -m "SAIL PROJECT — bito 0.0.0.1"
git branch -M main
git remote add origin https://github.com/ВАШ_ЮЗЕР/SAIL-PROJECT.git
git push -u origin main
```

После push:
1. Открой **Actions** на GitHub
2. Дождись зелёной галочки у workflow **«SAIL PROJECT — Сборка»**
3. Скачай артефакт **SAIL-PROJECT-bito-0.0.0.1-win-x64** — там лежит `.exe`

### Release по тегу

```powershell
git tag v0.0.0.1
git push origin v0.0.0.1
```

GitHub автоматически создаст Release с прикреплённым `.exe`.

## Локальная сборка

### Требования

- Windows 10/11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Запуск

```powershell
cd SAIL
dotnet run
```

### Сборка portable .exe

```powershell
cd SAIL
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../publish
```

Готовый файл: `publish/SAIL.exe`

## Что внутри

| Раздел | Описание |
|--------|----------|
| **Главная** | Дашборд SAIL PROJECT с карточками |
| **Договор 15%** | Смешной договор о 15% с продажи Roblox-аккаунта |
| **Мемы** | Галерея картинок + калькулятор 15% |
| **Запустить SAIL** | Кнопка запуска в сайдбаре |

## Метаданные сборки

| Поле | Значение |
|------|----------|
| Продукт | SAIL PROJECT |
| Версия | bito 0.0.0.1 |
| Автор | bito |
| Подпись | dolbaeb productions |

---

*15% — навсегда. По договору. (шутка)*
