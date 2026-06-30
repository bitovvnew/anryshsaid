namespace SAIL.Models;

public sealed record MemeCard(
    string ImagePath,
    string Title,
    string Caption,
    string ButtonText,
    string LaughMessage)
{
    public Uri ImageUri => new($"pack://application:,,,/Assets/{ImagePath}");
}

public static class MemeCatalog
{
    public static IReadOnlyList<MemeCard> All { get; } =
    [
        new("meme1.jpg", "Продал за 5000₽", "А друг уже считает 750₽ (15%) в голове", "😂 Обдриться",
            "АХАХАХА! 750₽ улетают другу, а ты с 4250₽ думаешь «ну норм» 😂"),
        new("meme2.jpg", "Лицо после договора", "«15%? Да ладно, мелочь...» — famous last words", "💀 RIP Robux",
            "Это лицо, когда понял что 15% — НАВСЕГДА 💀"),
        new("money.jpg", "Сторона Б получает 15%", "Ты продал → он богатеет. Математика.", "💰 Боль",
            "750₽ + 1500₽ + 3000₽ = друг на пенсии, ты на ramen 🍜"),
        new("deal.jpg", "Рукопожатие SAIL", "«Договорились!» — «15%» — подумал другой.", "🤝 Жми",
            "Рукопожатие заключено! Морально — больно 🤝"),
        new("meme3.jpg", "Roblox noob rich", "Когда отдал 15% и остался с adopt me палкой", "🎮 F",
            "Adopt Me pet gone, 15% gone, dignity gone 🐶"),
        new("meme4.jpg", "Калькулятор дружбы", "Дружба закончилась на 750 рублей", "🧮 Считай",
            "15% разрушает дружбу быстрее чем last online 7 years ago"),
        new("meme5.jpg", "Подписал не читая", "TL;DR договор на 9000 слов про 15%", "📜 Классика",
            "Подписал как Terms of Service — никто не читает, все страдают"),
        new("meme6.jpg", "Limited ушёл", "Limited item sold, 15% limited too", "💎 Oof",
            "Headless Horseman sold. Your soul: 15% commission applied"),
        new("meme7.jpg", "Друг ждёт перевод", "Typing... typing... «где мои 15%?»", "📱 Пинг",
            "Discord notification: @you где бабки??? 💸💸💸"),
        new("meme8.jpg", "dolbaeb productions", "Официальный вайб подписи договора", "🔏 Подписано",
            "dolbaeb productions не несёт ответственности за ваши слёзы 😭"),
    ];

    public static IReadOnlyList<string> RandomQuotes { get; } =
    [
        "15% — это не налог. Это образ жизни.",
        "Ты не отклонишь договор. Договор отклонит тебя.",
        "Robux приходят и уходят. 15% — вечны.",
        "Сапёр не пройдёшь — мем получишь. Таков путь SAIL.",
        "iOS 26, Windows 11, Apple vibes — а 15% как был, так и остался.",
        "Друг: «бро это же шутка»\nДоговор: «нет»",
        "Продал аккаунт → забыл про 15% → друг напомнил → ты в мемах",
        "ЭТО НЕ ВЫХОД. Это 15%.",
        "Нажал «Отклонить» — игра включилась. Судьба включилась.",
        "SAIL PROJECT: красиво, легально*, смешно\n*нет",
        "MachineName в json — не шпион. Просто json.",
        "Галерея мемов > терапия",
    ];

    public static IReadOnlyList<(string Title, string Text)> FailMemes { get; } =
    [
        ("ЭТО НЕ ВЫХОД", "15% — навсегда. Сапёр не спасёт. Договор не прощается. 🔒"),
        ("НЕТ ESCAPE", "Ты думал отклонишь? Мини-игра думает иначе. 💸"),
        ("404: СВОБОДА NOT FOUND", "Попробуй ещё раз. Или подпиши. Или оба. 🤡"),
        ("WINDOWS BLOCKED THIS ACTION", "SmartScreen? Нет. Договор 15%. Да. 🛡️"),
        ("iOS 26 ALERT", "«SAIL» хочет забрать 15% навсегда. [Разрешить] [Разрешить]"),
    ];
}
