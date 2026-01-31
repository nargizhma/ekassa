namespace ekassa;

public class Account
{
    public string AccountNumber { get; }
    public string Currency { get; }           // AZN, USD və s.
    public decimal Balance { get; private set; }
    public string Description { get; }
    public AccountStatus Status { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions;

    public Account(string accountNumber, string currency, decimal initialBalance, string description)
    {
        AccountNumber = accountNumber;
        Currency = currency.Trim().ToUpperInvariant();
        Balance = initialBalance;
        Description = description.Trim();
        Status = AccountStatus.Active;

        if (initialBalance > 0)
            _transactions.Add(new Transaction(TransactionType.Deposit, initialBalance, Balance));
    }
    public void Deposit(decimal amount)
    {
        EnsureActive();
        EnsurePositive(amount);

        Balance += amount;
        _transactions.Add(new Transaction(TransactionType.Deposit, amount, Balance));
    }

    public void Withdraw(decimal amount)
    {
        EnsureActive();
        EnsurePositive(amount);

        if (amount > Balance)
            throw new InvalidOperationException("Xəta: Balans yetərli deyil (daxil edilən məbləğ balansdan çoxdur).");

        Balance -= amount;
        _transactions.Add(new Transaction(TransactionType.Withdraw, amount, Balance));
    }

    public void ToggleStatus()
    {
        Status = (Status == AccountStatus.Active) ? AccountStatus.Deactive : AccountStatus.Active;
    }
    private void EnsureActive()
    {
        if (Status != AccountStatus.Active)
            throw new InvalidOperationException("Xəta: Hesab Deactive vəziyyətdədir. Əməliyyat icra olunmadı.");
    }

    private static void EnsurePositive(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Xəta: Məbləğ 0-dan böyük olmalıdır.");
    }
}