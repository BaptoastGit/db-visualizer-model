using System.Linq.Expressions;
using Model.BO.Data.Model;

namespace Model.BO.Service
{
    static public class OrdersService
    {
        static public List<Order> SortAndFilterOrdersTable(ModelDbContext context, string sortBy, string Order, string RowCount = "", string OrderNumber = "", string CustomerEmail = "", string OrderStatus = "", string PaymentStatus = "", List<string> TotalAmount = null, string Currency = "", string ShippingMethod = "", string TrackingNumber = "", List<string> ItemCount = null, List<string> OrderedOn = null)
        {
            var selectors = new Dictionary<string, Expression<Func<Order, object>>>()
            {
                ["OrderNumber"] = s => s.OrderNumber!,
                ["CustomerEmail"] = s => s.CustomerEmail!,
                ["OrderStatus"] = s => s.OrderStatus!,
                ["PaymentStatus"] = s => s.PaymentStatus!,
                ["TotalAmount"] = s => s.TotalAmount,
                ["Currency"] = s => s.Currency!,
                ["ShippingMethod"] = s => s.ShippingMethod!,
                ["TrackingNumber"] = s => s.TrackingNumber!,
                ["ItemCount"] = s => s.ItemCount,
                ["OrderedOn"] = s => s.OrderedOn
            };

            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<Order> query = context.Orders;


            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                query = query.Where(c =>
                (string.IsNullOrEmpty(OrderNumber) || (c.OrderNumber != null && c.OrderNumber.Contains(OrderNumber))) &&
                (string.IsNullOrEmpty(CustomerEmail) || (c.CustomerEmail != null && c.CustomerEmail.Contains(CustomerEmail))) &&
                (string.IsNullOrEmpty(OrderStatus) || (c.OrderStatus != null && c.OrderStatus.Contains(OrderStatus))) &&
                (string.IsNullOrEmpty(PaymentStatus) || (c.PaymentStatus != null && c.PaymentStatus.Contains(PaymentStatus))) &&
                (TotalAmount == null ||
                    (TotalAmount[0] == "eq" ? c.TotalAmount == decimal.Parse(TotalAmount[1]) :
                    TotalAmount[0] == "gt" ? c.TotalAmount >= decimal.Parse(TotalAmount[1]) :
                    c.TotalAmount <= decimal.Parse(TotalAmount[1]))) &&
                (string.IsNullOrEmpty(Currency) || (c.Currency != null && c.Currency.Contains(Currency))) &&
                (string.IsNullOrEmpty(ShippingMethod) || (c.ShippingMethod != null && c.ShippingMethod.Contains(ShippingMethod))) &&
                (string.IsNullOrEmpty(TrackingNumber) || (c.TrackingNumber != null && c.TrackingNumber.Contains(TrackingNumber))) &&
                (ItemCount == null ||
                    (ItemCount[0] == "eq" ? c.ItemCount == int.Parse(ItemCount[1]) :
                    ItemCount[0] == "gt" ? c.ItemCount >= int.Parse(ItemCount[1]) :
                    c.ItemCount <= int.Parse(ItemCount[1]))) &&
                (OrderedOn == null || (c.OrderedOn >= DateTime.Parse(OrderedOn[0]) && c.OrderedOn <= DateTime.Parse(OrderedOn[1])))
            );
                //Sort
                query = Order == "1" ? query.OrderBy(selector) : query.OrderByDescending(selector);
                query = query.Take(rowCount);
                return [.. query];
            }

            else
            {

                return [.. context.Orders.Take(rowCount)];
            }

        }


        static public List<Product> SortAndFilterProductsTable(ModelDbContext context, string sortBy, string Order, string RowCount = "", string Sku = "", string Title = "", string Category = "", List<string> StockQuantity = null, List<string> IsPublished = null, List<string> WeightKg = null, List<string> ModifiedOn = null)
        {
            var selectors = new Dictionary<string, Expression<Func<Product, object>>>()
            {
                ["Sku"] = s => s.Sku!,
                ["Title"] = s => s.Title!,
                ["Category"] = s => s.Category!,
                ["StockQuantity"] = s => s.StockQuantity,
                ["IsPublished"] = s => s.IsPublished,
                ["WeightKg"] = s => s.WeightKg!,
                ["ModifiedOn"] = s => s.ModifiedOn!
            };
            if (IsPublished != null)
            {
                Console.WriteLine("Ici" + String.Join(",", IsPublished));
                Console.WriteLine(IsPublished.Contains("null"));


            }
            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<Product> query = context.Products;


            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                //Filter
                query = query.Where(c =>
                    (string.IsNullOrEmpty(Sku) || (c.Sku != null && c.Sku.Contains(Sku))) &&
                    (string.IsNullOrEmpty(Title) || (c.Title != null && c.Title.Contains(Title))) &&
                    (string.IsNullOrEmpty(Category) || (c.Category != null && c.Category.Contains(Category))) &&
                    (StockQuantity == null ||
                        (StockQuantity[0] == "eq" ? c.StockQuantity == int.Parse(StockQuantity[1]) :
                        StockQuantity[0] == "gt" ? c.StockQuantity >= int.Parse(StockQuantity[1]) :
                        c.StockQuantity <= int.Parse(StockQuantity[1]))) &&
                    (IsPublished == null || IsPublished.Contains(c.IsPublished.ToString())) &&
                    (WeightKg == null ||
                        (WeightKg[0] == "eq" ? c.WeightKg == double.Parse(WeightKg[1]) :
                        WeightKg[0] == "gt" ? c.WeightKg >= double.Parse(WeightKg[1]) :
                        c.WeightKg <= double.Parse(WeightKg[1]))) &&
                    (ModifiedOn == null || c.ModifiedOn == null || (c.ModifiedOn != null && c.ModifiedOn >= DateTime.Parse(ModifiedOn[0]) && c.ModifiedOn <= DateTime.Parse(ModifiedOn[1])))
                );
                //Sort
                query = Order == "1" ? query.OrderBy(selector) : query.OrderByDescending(selector);
                query = query.Take(rowCount);
                return [.. query];
            }

            else
            {

                return [.. context.Products.Take(rowCount)];
            }

        }



        static public List<Price> SortAndFilterPricesTable(ModelDbContext context, string sortBy, string Order, string RowCount = "", string Sku = "", string PriceListCode = "", List<string> Amount = null, string Currency = "", List<string> ValidFrom = null, List<string> ValidTo = null)
        {
            var selectors = new Dictionary<string, Expression<Func<Price, object>>>()
            {
                ["Sku"] = s => s.Sku!,
                ["PriceListCode"] = s => s.PriceListCode!,
                ["Amount"] = s => s.Amount,
                ["Currency"] = s => s.Currency!,
                ["ValidFrom"] = s => s.ValidFrom!,
                ["ValidTo"] = s => s.ValidTo!
            };

            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<Price> query = context.Prices;


            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                //Filter
                query = query.Where(c =>
                    (string.IsNullOrEmpty(Sku) || (c.Sku != null && c.Sku.Contains(Sku))) &&
                    (string.IsNullOrEmpty(PriceListCode) || (c.PriceListCode != null && c.PriceListCode.Contains(PriceListCode))) &&
                    (Amount == null ||
                        (Amount[0] == "eq" ? c.Amount == decimal.Parse(Amount[1]) :
                        Amount[0] == "gt" ? c.Amount >= decimal.Parse(Amount[1]) :
                        c.Amount <= decimal.Parse(Amount[1]))) &&
                (string.IsNullOrEmpty(Currency) || (c.Currency != null && c.Currency.Contains(Currency))) &&
                (ValidFrom == null || c.ValidTo == null || (c.ValidFrom != null && c.ValidFrom >= DateTime.Parse(ValidFrom[0]) && c.ValidFrom <= DateTime.Parse(ValidFrom[1]))) &&
                (ValidTo == null || c.ValidTo == null || (c.ValidTo != null && c.ValidTo >= DateTime.Parse(ValidTo[0]) && c.ValidTo <= DateTime.Parse(ValidTo[1])))
                );
                //Sort
                query = Order == "1" ? query.OrderBy(selector) : query.OrderByDescending(selector);
                query = query.Take(rowCount);
                return [.. query];
            }

            else
            {

                return [.. context.Prices.Take(rowCount)];
            }

        }
    }
}
