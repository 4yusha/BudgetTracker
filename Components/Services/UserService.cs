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
    public string PreferredCurrency { get; set; } = "USD"; // Default currency
    private static readonly Dictionary<string, decimal> ConversionRates = new Dictionary<string, decimal>
    {
        { "USD", 1.0m },
        { "NPR", 119m },
        { "INR", 74.5m },
    };

    // Get the conversion rate for a given currency
    public decimal ConvertToPreferredCurrency(decimal amount, string preferredCurrency)
    {
        if (ConversionRates.TryGetValue(preferredCurrency, out var rate))
        {
            return amount * rate;
        }
        return amount; // Default to USD if currency not found
    }
    // Store the currently logged-in user
    private User? _loggedInUser;

    // Set the currently logged-in user
    public void SetLoggedInUser(User user)
    {
        _loggedInUser = user;
    }

    // Get the currently logged-in user
    public User? GetLoggedInUser()
    {
        return _loggedInUser;
    }

    // Check if a user is logged in
    public bool IsUserLoggedIn()
    {
        return _loggedInUser != null;
    }

    // Check if the logged-in user is authorized to access a specific username's data
    public bool IsAuthorizedUser(string username)
    {
        return _loggedInUser?.Username == username;
    }

    // Method to get data of the logged-in user
    public User GetLoggedInUserData()
    {
        var data = DataUpload();

        // Filter the data to only show information for the logged-in user
        var userData = data.Users.FirstOrDefault(u => u.Username == _loggedInUser?.Username);

        if (userData != null)
        {
            return userData;  // Return the data of the logged-in user
        }

        throw new Exception("User data not found.");
    }

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

    // Store debts data
    public void StoreDebts(IEnumerable<Debt> debts)
    {
        var data = DataUpload();
        data.Debts = debts.ToList();
        StoreData(data);
    }

    // Resetting amounts for all transactions and debts
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

    // Saving AppData to JSON file
    public void StoreData(AppData data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(FilePath, json);
    }

    // Load and return the list of users
    public List<User> UsersUpload()
    {
        var appData = DataUpload();
        return appData.Users;
    }

    // Store updated user list
    public void StoreUsers(List<User> users)
    {
        var appData = DataUpload();
        appData.Users = users;
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
}