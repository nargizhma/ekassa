namespace ekassa;

public class EKassa
{
    private readonly Dictionary<string, Account> _accounts = new();
    private readonly Random _random = new();

    public string OwnerName { get; }
    public bool IsCreated { get; private set; }

    public EKassa(string ownerName)
    {
        OwnerName = ownerName.Trim();
    }

    public void Create()
    {
        IsCreated = true;
    }
    public Account CreateAccount(string currency, decimal initialBalance, string description)
    {
        EnsureKassaCreated();

        string no = GenerateUnique8DigitNumber();
        var account = new Account(no, currency, initialBalance, description);
        _accounts.Add(no, account);
        return account;
    }

    public IReadOnlyCollection<Account> GetAllAccounts()
    {
        EnsureKassaCreated();
        return _accounts.Values;
    }

    public Account? GetAccountByNumber(string accountNumber)
    {
        EnsureKassaCreated();

        accountNumber = accountNumber.Trim();
        return _accounts.TryGetValue(accountNumber, out var acc) ? acc : null;
    }
    private string GenerateUnique8DigitNumber()
    {
        // 8 rəqəmli: 10000000 - 99999999
        while (true)
        {
            int n = _random.Next(10_000_000, 100_000_000);
            string no = n.ToString();
            if (!_accounts.ContainsKey(no))
                return no;
        }
    }

    private void EnsureKassaCreated()
    {
        if (!IsCreated)
            throw new InvalidOperationException("Əvvəlcə E-Kassa yaratmalısınız (Menyu 1).");
    }
}