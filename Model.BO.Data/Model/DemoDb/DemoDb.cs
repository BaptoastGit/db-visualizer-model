using Model.BO.Data.Model;
using System.Text.Json.Nodes;

namespace Model.BO.Data.Model.DemoDb
{
    public class DemoDb
    {
        public static void GenerateFakeDb(ModelDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Set<User>().Any())
            {
                var rand = new Random();

                string[] firstNames = { "Alice", "Bob", "Charlie", "Diane", "Eric", "Fiona", "Gaston", "Hélène", "Ian", "Julie" };
                string[] lastNames = { "Dupont", "Martin", "Durand", "Lefebvre", "Moreau", "Simon", "Laurent", "Michel", "Garcia", "David" };
                string[] roles = { "Admin", "User", "Manager", "Guest", "Auditor" };
                string[] statuses = { "Active", "Inactive", "Suspended", "Pending" };
                string[] locales = { "fr-FR", "en-US", "es-ES", "de-DE" };
                string[] categories = { "Accessoires", "Moniteurs", "Audio", "Composants", "Réseau" };
                string[] currencies = { "EUR", "USD", "GBP", "CHF" };
                string[] shippingMethods = { "Standard", "Express", "Relay", "Drone" };

                var users = new List<User>();
                for (int i = 1; i <= 1000; i++)
                {
                    string fName = firstNames[rand.Next(firstNames.Length)];
                    string lName = lastNames[rand.Next(lastNames.Length)];
                    bool hasLogged = rand.Next(10) > 2;

                    users.Add(new User
                    {
                        Id = Guid.NewGuid(),
                        Username = $"{fName.ToLower()}_{lName.ToLower()}_{i}",
                        Email = $"{fName.ToLower()}.{lName.ToLower()}{i}@example.com",
                        Role = roles[rand.Next(roles.Length)],
                        Status = statuses[rand.Next(statuses.Length)],
                        FirstName = fName,
                        LastName = lName,
                        PhoneNumber = rand.Next(10) > 3 ? $"+33 6 {rand.Next(10, 99)} {rand.Next(10, 99)} {rand.Next(10, 99)} {rand.Next(10, 99)}" : null,
                        IsEmailConfirmed = rand.Next(2) == 1,
                        IsPhoneConfirmed = rand.Next(10) > 5,
                        Locale = locales[rand.Next(locales.Length)],
                        TimeZone = "Europe/Paris",
                        LastLoginIp = hasLogged ? $"192.168.{rand.Next(0, 255)}.{rand.Next(1, 255)}" : null,
                        FailedLoginAttempts = rand.Next(10) > 8 ? rand.Next(1, 5) : 0,
                        IsTwoFactorEnabled = rand.Next(10) > 7,
                        CreatedOn = DateTime.UtcNow.AddDays(-rand.Next(100, 1000)),
                        LastLoginOn = hasLogged ? DateTime.UtcNow.AddDays(-rand.Next(0, 50)) : null,
                        LastPasswordChangedOn = DateTime.UtcNow.AddDays(-rand.Next(50, 300)),
                        RequiresPasswordChange = rand.Next(10) > 8,
                        LockoutEnd = rand.Next(100) > 95 ? DateTimeOffset.UtcNow.AddDays(rand.Next(1, 7)) : null,
                        GdprConsentOn = rand.Next(10) > 2 ? DateTime.UtcNow.AddDays(-rand.Next(50, 900)) : null,
                        MarketingOptIn = rand.Next(2) == 1,
                        InternalNotes = rand.Next(10) > 7 ? "Utilisateur VIP" : null,
                        CreatedBy = "System"
                    });
                }
                context.Set<User>().AddRange(users);

                var accounts = new List<Account>();
                string[] accountTypes = { "Personal", "Business", "Enterprise", "NonProfit" };
                string[] tiers = { "Basic", "Standard", "Gold", "Platinum" };
                for (int i = 1; i <= 300; i++)
                {
                    accounts.Add(new Account
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Compte {accountTypes[rand.Next(accountTypes.Length)]} {i}",
                        Type = accountTypes[rand.Next(accountTypes.Length)],
                        Status = statuses[rand.Next(statuses.Length)],
                        Tier = tiers[rand.Next(tiers.Length)],
                        Balance = Math.Round((decimal)(rand.NextDouble() * 50000), 2), // Solde entre -5000 et 45000
                        IsVatExempt = rand.Next(10) > 8,
                        CreatedOn = DateTime.UtcNow.AddDays(-rand.Next(10, 2000))
                    });
                }
                context.Set<Account>().AddRange(accounts);

                var products = new List<Product>();
                for (int i = 1; i <= 100; i++)
                {
                    products.Add(new Product
                    {
                        Sku = $"SKU-{rand.Next(100, 999)}-{i:D4}",
                        Title = $"Produit Générique {i}",
                        Category = categories[rand.Next(categories.Length)],
                        StockQuantity = rand.Next(0, 500),
                        IsPublished = rand.Next(10) > 2, // 80% publiés
                        WeightKg = rand.Next(10) > 1 ? Math.Round(rand.NextDouble() * 10, 2) : null,
                        ModifiedOn = rand.Next(2) == 1 ? DateTime.UtcNow.AddDays(-rand.Next(1, 100)) : null
                    });
                }
                context.Set<Product>().AddRange(products);

                var prices = new List<Price>();
                foreach (var product in products)
                {
                    bool hasValidTo = rand.Next(10) > 7;
                    prices.Add(new Price
                    {
                        Sku = product.Sku,
                        PriceListCode = rand.Next(10) > 2 ? "DEFAULT" : "PROMO2026",
                        Amount = Math.Round((decimal)(rand.NextDouble() * 990 + 10), 2),
                        Currency = currencies[rand.Next(currencies.Length)],
                        ValidFrom = DateTime.UtcNow.AddDays(-rand.Next(10, 300)),
                        ValidTo = hasValidTo ? DateTime.UtcNow.AddDays(rand.Next(10, 100)) : null
                    });
                }
                context.Set<Price>().AddRange(prices);

                var orders = new List<Order>();
                string[] orderStatuses = { "Pending", "Processing", "Completed", "Cancelled", "Refunded" };
                string[] paymentStatuses = { "Unpaid", "Paid", "Refunded", "Failed" };
                for (int i = 1; i <= 400; i++)
                {
                    orders.Add(new Order
                    {
                        OrderNumber = $"ORD-2026-{i:D6}",
                        CustomerEmail = $"client{i}@example.com",
                        OrderStatus = orderStatuses[rand.Next(orderStatuses.Length)],
                        PaymentStatus = paymentStatuses[rand.Next(paymentStatuses.Length)],
                        TotalAmount = Math.Round((decimal)(rand.NextDouble() * 2000 + 15), 2),
                        Currency = currencies[rand.Next(currencies.Length)],
                        ShippingMethod = shippingMethods[rand.Next(shippingMethods.Length)],
                        TrackingNumber = rand.Next(10) > 3 ? $"TRK{rand.Next(10000000, 99999999)}FR" : null,
                        ItemCount = rand.Next(1, 15),
                        OrderedOn = DateTime.UtcNow.AddDays(-rand.Next(1, 365))
                    });
                }
                context.Set<Order>().AddRange(orders);

                // 6. Génération des Messages (1000 entités)
                var messages = new List<Message>();
                string[] priorities = { "Low", "Normal", "High", "Critical" };
                string[] errors = { "Timeout service tier", "Échec JSON", "Connexion base de données perdue", "Erreur réseau" };
                for (int i = 1; i <= 200; i++)
                {
                    int retry = rand.Next(0, 5);
                    messages.Add(new Message
                    {
                        CorrelationId = Guid.NewGuid().ToString(),
                        RetryCount = retry,
                        ErrorMessage = retry > 0 ? errors[rand.Next(errors.Length)] : null,
                        PayloadSize = rand.Next(128, 8192),
                        Priority = priorities[rand.Next(priorities.Length)],
                        ProcessedOn = retry == 0 ? DateTime.UtcNow.AddMinutes(-rand.Next(1, 1000)) : null
                    });
                }
                context.Set<Message>().AddRange(messages);

                context.SaveChanges();
            }
        }
    }
}
     