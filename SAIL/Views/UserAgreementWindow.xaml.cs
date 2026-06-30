using System.Windows;
using System.Windows.Controls;
using SAIL.Helpers;

namespace SAIL.Views;

public partial class UserAgreementWindow : Window
{
    public bool Accepted { get; private set; }

    public UserAgreementWindow(bool reviewOnly = false)
    {
        InitializeComponent();
        AgreementText.Text = GetAgreementText();

        if (reviewOnly)
        {
            SetupReviewMode();
            return;
        }

        AgreeTerms.Checked += (_, _) => UpdateAcceptButton();
        AgreeTerms.Unchecked += (_, _) => UpdateAcceptButton();
        AgreeAge.Checked += (_, _) => UpdateAcceptButton();
        AgreeAge.Unchecked += (_, _) => UpdateAcceptButton();
        AgreePrivacy.Checked += (_, _) => UpdateAcceptButton();
        AgreePrivacy.Unchecked += (_, _) => UpdateAcceptButton();
    }

    private void UpdateAcceptButton() =>
        AcceptButton.IsEnabled = AgreeTerms.IsChecked == true
                                 && AgreeAge.IsChecked == true
                                 && AgreePrivacy.IsChecked == true;

    private void SetupReviewMode()
    {
        AgreeTerms.Visibility = Visibility.Collapsed;
        AgreeAge.Visibility = Visibility.Collapsed;
        AgreePrivacy.Visibility = Visibility.Collapsed;

        var declineButton = (Button)FindName("DeclineButton");
        if (declineButton is not null)
            declineButton.Visibility = Visibility.Collapsed;

        AcceptButton.Content = "Закрыть";
        AcceptButton.IsEnabled = true;
        AcceptButton.Click -= Accept_Click;
        AcceptButton.Click += (_, _) => Close();

        var record = UserAgreementStorage.GetRecord();
        if (record is not null)
        {
            AgreementText.Text += $"\n\n────────\n✅ Принято: {record.AcceptedAt:dd.MM.yyyy HH:mm}";
        }
    }

    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        UserAgreementStorage.SaveAcceptance();
        Accepted = true;
        SoundHelper.PlayAction(SoundAction.AgreementAccept);
        DialogResult = true;
        Close();
    }

    private void Decline_Click(object sender, RoutedEventArgs e)
    {
        var game = new RejectContractGameWindow { Owner = this };
        game.ShowDialog();

        if (game.EscapeSuccessful)
        {
            Accepted = false;
            DialogResult = false;
            Close();
            return;
        }

        MessageBox.Show(
            "ЭТО НЕ ВЫХОД\n\nНе сбежал от 15% — соглашение не отклонено.\nПрочитай и нажми «Принимаю» 🔒",
            "SAIL PROJECT",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    private static string GetAgreementText() =>
        """
        ПОЛЬЗОВАТЕЛЬСКОЕ СОГЛАШЕНИЕ
        SAIL PROJECT / SAIL Launcher
        Версия: bito 0.0.0.1
        Издатель: dolbaeb productions

        ────────────────────────────────────────

        1. ОБЩИЕ ПОЛОЖЕНИЯ

        1.1. Настоящее Пользовательское соглашение («Соглашение») регулирует использование программы SAIL Launcher («Программа»), являющейся частью проекта SAIL PROJECT.

        1.2. Запуская Программу, вы подтверждаете, что прочитали, поняли и принимаете все условия настоящего Соглашения.

        1.3. Если вы не согласны с условиями — не используйте Программу.

        ────────────────────────────────────────

        2. НАЗНАЧЕНИЕ ПРОГРАММЫ

        2.1. SAIL Launcher — программный лаунчер для управления продажей игровых аккаунтов Roblox и связанными документами (в том числе договором о комиссии 15%).

        2.2. Программа предоставляется «как есть» (as is) без каких-либо гарантий работоспособности, прибыльности или юридической силы встроенных шаблонов документов.

        2.3. Встроенный «договор 15%» носит развлекательный характер и не является юридически обязывающим документом, если стороны не оформили его отдельно по закону.

        ────────────────────────────────────────

        3. ОБЯЗАННОСТИ ПОЛЬЗОВАТЕЛЯ

        3.1. Вы обязуетесь использовать Программу только в рамках действующего законодательства вашей страны.

        3.2. Вы несёте полную ответственность за продажу, покупку и передачу игровых аккаунтов Roblox, включая соблюдение правил платформы Roblox.

        3.3. Запрещается использовать Программу для мошенничества, обмана других пользователей или незаконной деятельности.

        3.4. Запрещается декомпилировать, модифицировать или распространять Программу без разрешения dolbaeb productions.

        ────────────────────────────────────────

        4. ПОЛИТИКА КОНФИДЕНЦИАЛЬНОСТИ

        4.1. Программа НЕ собирает и НЕ передаёт ваши персональные данные на внешние серверы без вашего явного согласия.

        4.2. Локально на вашем компьютере могут сохраняться:
             • факт принятия настоящего Соглашения;
             • журнал ошибок (для диагностики сбоев);
             • данные, которые вы вводите сами (имена в договоре и т.п.).

        4.3. Данные хранятся в папке:
             %LocalAppData%\SAIL PROJECT\

        4.4. Издатель не имеет доступа к вашему компьютеру и не получает ваши данные автоматически.

        ────────────────────────────────────────

        5. ОГРАНИЧЕНИЕ ОТВЕТСТВЕННОСТИ

        5.1. dolbaeb productions не несёт ответственности за:
             • блокировку аккаунтов Roblox;
             • финансовые потери при продаже/покупке аккаунтов;
             • споры между пользователями по поводу «15%» или иных условий;
             • любой ущерб, возникший в результате использования Программы.

        5.2. Максимальная ответственность Издателя ограничена суммой, которую вы фактически заплатили за Программу (если применимо).

        ────────────────────────────────────────

        6. ИНТЕЛЛЕКТУАЛЬНАЯ СОБСТВЕННОСТЬ

        6.1. Все права на SAIL PROJECT, SAIL Launcher, дизайн, код и материалы принадлежат dolbaeb productions / bito.

        6.2. Цифровая подпись: dolbaeb productions.

        ────────────────────────────────────────

        7. ИЗМЕНЕНИЕ СОГЛАШЕНИЯ

        7.1. Издатель вправе обновить настоящее Соглашение. При обновлении версии Соглашения вам будет предложено принять его заново.

        ────────────────────────────────────────

        8. ЗАКЛЮЧИТЕЛЬНЫЕ ПОЛОЖЕНИЯ

        8.1. Настоящее Соглашение действует с момента нажатия кнопки «Принимаю».

        8.2. Применимое право — законодательство страны проживания пользователя, если иное не установлено императивными нормами.

        8.3. Контакт издателя: dolbaeb productions · SAIL PROJECT · bito 0.0.0.1

        ────────────────────────────────────────
        Дата редакции: 30 июня 2026 г.
        """;
}
