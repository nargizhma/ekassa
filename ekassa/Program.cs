using System.Globalization;

namespace ekassa
{
    public static class Program
    {
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.Write("E-Kassa sahibi adınız: ");
            string owner = Console.ReadLine() ?? "User";
            var ekassa = new EKassa(owner);

            while (true)
            {
                Console.WriteLine("\n=== ƏSAS MENYU ===");
                Console.WriteLine("1. E-Kassa yarat");
                Console.WriteLine("2. E-Kassaya bağlı hesab yarat");
                Console.WriteLine("3. E-kassaya bağlı bütün hesabları göstər");
                Console.WriteLine("4. Hesab nömrəsinə görə hesabı görüntüləmək");
                Console.WriteLine("5. Çıxış");

                int choice = ReadInt("Seçim: ", 1, 5);

                try
                {
                    switch (choice)
                    {
                        case 1:
                            ekassa.Create();
                            Console.WriteLine("E-Kassa yaradıldı.");
                            break;

                        case 2:
                            CreateAccountFlow(ekassa);
                            break;

                        case 3:
                            ShowAllAccountsFlow(ekassa);
                            break;

                        case 4:
                            ViewAccountFlow(ekassa);
                            break;

                        case 5:
                            Console.WriteLine("Çıxış edildi.");
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private static void CreateAccountFlow(EKassa ekassa)
        {
            while (true)
            {
                Console.WriteLine("\n--- Yeni hesab yarat ---");
                Console.Write("Açıqlama (məs: Maaş hesabı): ");
                string description = Console.ReadLine() ?? "";

                Console.Write("Pul vahidi (məs: AZN / USD): ");
                string currency = Console.ReadLine() ?? "";

                decimal initial = ReadDecimal("Başlanğıc balans: ");

                var acc = ekassa.CreateAccount(currency, initial, description);

                Console.WriteLine($"Hesab yaradıldı. Hesab NO: {acc.AccountNumber}");
                Console.Write("Başqa hesab yaratmaq istəyirsiniz? (B/X): ");
                string more = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

                if (more != "B")
                    break;
            }
        }

        private static void ShowAllAccountsFlow(EKassa ekassa)
        {
            var accounts = ekassa.GetAllAccounts();

            Console.WriteLine("\nHesab NO   | Balans   | Valyuta | Açıqlama        | Status");
            Console.WriteLine("--------------------------------------------------------------");

            foreach (var a in accounts)
            {
                Console.WriteLine(
                    $"{a.AccountNumber,-9} | {a.Balance,7:0.00} | {a.Currency,-6} | {TrimTo(a.Description,14),-14} | {a.Status}");
            }

            if (accounts.Count == 0)
                Console.WriteLine("(Hələ hesab yoxdur)");
        }

        private static void ViewAccountFlow(EKassa ekassa)
        {
            Console.Write("\nHesab nömrəsi daxil edin: ");
            string no = Console.ReadLine() ?? "";
            var acc = ekassa.GetAccountByNumber(no);

            if (acc == null)
            {
                Console.WriteLine("Belə hesab tapılmadı.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n=== HESAB ===");
                PrintSingleAccount(acc);

                Console.WriteLine("a) Hesaba pul yatır");
                Console.WriteLine("b) Hesabdan pul çəkmək");
                Console.WriteLine("c) Hesab hərəkətlərini göstər");
                Console.WriteLine("d) Hesabı Active / Deactive et");
                Console.WriteLine("e) Əsas menyuya geri dön");

                Console.Write("Seçim (a/b/c/d/e): ");
                string ch = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

                try
                {
                    switch (ch)
                    {
                        case "a":
                            {
                                decimal amt = ReadDecimal("Yatırılacaq məbləğ: ");
                                acc.Deposit(amt);
                                Console.WriteLine("Pul yatırıldı.");
                                break;
                            }
                        case "b":
                            {
                                decimal amt = ReadDecimal("Çəkiləcək məbləğ: ");
                                acc.Withdraw(amt);
                                Console.WriteLine("Pul çəkildi.");
                                break;
                            }
                        case "c":
                            ShowTransactions(acc);
                            break;

                        case "d":
                            acc.ToggleStatus();
                            Console.WriteLine($"Yeni status: {acc.Status}");
                            break;

                        case "e":
                            return;

                        default:
                            Console.WriteLine("Yanlış seçim.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private static void ShowTransactions(Account acc)
        {
            Console.WriteLine("\n--- Hərəkətlər ---");
            if (acc.Transactions.Count == 0)
            {
                Console.WriteLine("(Hərəkət yoxdur)");
                return;
            }

            foreach (var t in acc.Transactions)
                Console.WriteLine(t);
        }

        private static void PrintSingleAccount(Account a)
        {
            Console.WriteLine($"Hesab NO : {a.AccountNumber}");
            Console.WriteLine($"Valyuta  : {a.Currency}");
            Console.WriteLine($"Balans   : {a.Balance:0.00}");
            Console.WriteLine($"Açıqlama : {a.Description}");
            Console.WriteLine($"Status   : {a.Status}");
            Console.WriteLine();
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine() ?? "";
                if (int.TryParse(s, out int x) && x >= min && x <= max)
                    return x;

                Console.WriteLine($"Düzgün ədəd daxil edin ({min}-{max}).");
            }
        }

        private static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = (Console.ReadLine() ?? "").Trim();

                // həm 10.5, həm 10,5 qəbul etsin deyə:
                s = s.Replace(',', '.');

                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val))
                    return val;

                Console.WriteLine("Düzgün məbləğ daxil edin (məs: 10.50).");
            }
        }

        private static string TrimTo(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Length <= max ? s : s.Substring(0, max - 1) + "…";
        }
    }
}