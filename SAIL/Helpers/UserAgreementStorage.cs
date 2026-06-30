using System.IO;
using System.Text.Json;

namespace SAIL.Helpers;

public static class UserAgreementStorage
{
    private const string AgreementVersion = "1.0";
    private static readonly string StorageDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SAIL PROJECT");

    private static readonly string StoragePath = Path.Combine(StorageDir, "user-agreement.json");

    public record AcceptanceRecord(string Version, DateTime AcceptedAt, string MachineName);

    public static bool IsAccepted =>
        TryLoad(out var record) && record.Version == AgreementVersion;

    public static void SaveAcceptance()
    {
        Directory.CreateDirectory(StorageDir);
        var record = new AcceptanceRecord(
            AgreementVersion,
            DateTime.Now,
            Environment.MachineName);

        File.WriteAllText(StoragePath, JsonSerializer.Serialize(record, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    public static AcceptanceRecord? GetRecord() =>
        TryLoad(out var record) ? record : null;

    private static bool TryLoad(out AcceptanceRecord record)
    {
        record = null!;
        if (!File.Exists(StoragePath)) return false;

        try
        {
            var data = JsonSerializer.Deserialize<AcceptanceRecord>(File.ReadAllText(StoragePath));
            if (data is null) return false;
            record = data;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
