using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BudgetTracker.Components.Model; 
public class UserService
{
    // Paths for storing application data
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string FilePath = Path.Combine(FolderPath, "appdata.json");

    // Loading AppData from JSON file
    public AppData DataUpload()
    {
        if (!File.Exists(FilePath))
        {
            return new AppData();
        }

        try
        {
            // Reading JSON content from file
            var json = File.ReadAllText(FilePath);

            // Deserializing JSON into AppData object
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        }
        catch (JsonException)
        {
            // Handling corrupted JSON files gracefully.
            return new AppData();
        }
        catch (Exception)
        {
            return new AppData();
        }



    }

    private AppData _data;

    public void StoreDebts(IEnumerable<Debt> debts)
    {
        // Logic to saving the updated debts data 

        _data.Debts = debts.ToList();

        // Saving _data to persistent storage

        StoreData();
    }

    private void StoreData()
    {
    }


    //Resetting amout
    public void AdjustAmounts()
    {
        var data = DataUpload(); 

        // Reset all transactions
        foreach (var transaction in data.Transactions)
        {
            transaction.Credit = 0;
            transaction.Debit = 0;
        }

        // Reset all debts
        foreach (var debt in data.Debts)
        {
            debt.Amount = 0;
            debt.PaidAmount = 0;
            debt.RemainingAmount = 0;
        }

        StoreData(data); // Saving the updated data back to storage
    }
    public IEnumerable<Debt> GetPendingDebts()
    {
        var data = DataUpload(); // Load current data

        // Returning debts 

        return data.Debts.Where(d => d.RemainingAmount > 0);
    }

    // Saving AppData to JSON file
    public void StoreData(AppData data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(FilePath, json);
    }


    public List<User> UsersUpload()
    {
        // Load AppData and return the Users list
        var appData = DataUpload();
        return appData.Users;
    }

    public void StoreUsers(List<User> users)
    {
        // Loading the current AppData
        var appData = DataUpload();

        // Updating the Users list
        appData.Users = users;
        // Save the updating AppData back to the file
        StoreData(appData);
    }

    // Hashing a password securely
    public string PasswordHashing(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashing = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashing);
    }

    // Validating a password against a stored hash
    public bool AuthenticatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = PasswordHashing(inputPassword);
        return hashedInputPassword == storedPassword;
    }

    private void CertifyFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
    }
}
