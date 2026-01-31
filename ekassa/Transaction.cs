namespace ekassa;

public class Transaction
{
    public DateTime Date { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }
    public decimal BalanceAfter { get; }

    public Transaction(TransactionType type, decimal amount, decimal balanceAfter)
    {
        Date = DateTime.Now;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
    }

    public override string ToString()
    {
        // məsələn: 2026-01-31 10:20 | Deposit | 50.00 | Balance: 1050.00
        return $"{Date:yyyy-MM-dd HH:mm} | {Type} | {Amount:0.00} | Balance: {BalanceAfter:0.00}";
    }
}