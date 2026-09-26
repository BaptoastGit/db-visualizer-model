using Model.BO.Data.Model;
using System.Data;
using System.Linq.Expressions;
using System.Net.NetworkInformation;

namespace Model.BO.Service
{
    static public class UsersDataService
    {
        static public List<Account> SortAndFilterAccountsTable(ModelDbContext context, string sortBy, string Order, string RowCount = "", string Id = "", string Name = "", string Type = "", string Status = "", string Tier = "", List<string> Balance = null, List<string> IsVatExempt = null, List<string> CreatedOn = null)
        {

            var selectors = new Dictionary<string, Expression<Func<Account, object>>>()
            {
                ["Id"] = s => s.Id,
                ["Name"] = s => s.Name!,
                ["Type"] = s => s.Type!,
                ["Status"] = s => s.Status!,
                ["Tier"] = s => s.Tier!,
                ["Balance"] = s => s.Balance,
                ["IsVatExempt"] = s => s.IsVatExempt,
                ["CreatedOn"] = s => s.CreatedOn
            };

            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<Account> query = context.Accounts;

            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                //Filter
                query = query.Where(c =>
                    (string.IsNullOrEmpty(Id) || c.Id == Guid.Parse(Id)) &&
                    (string.IsNullOrEmpty(Name) || (c.Name != null && c.Name.Contains(Name))) &&
                    (string.IsNullOrEmpty(Type) || (c.Type != null && c.Type.Contains(Type))) &&
                    (string.IsNullOrEmpty(Status) || (c.Status != null && c.Status.Contains(Status))) &&
                    (string.IsNullOrEmpty(Tier) || (c.Tier != null && c.Tier.Contains(Tier))) &&
                    (Balance == null ||
                        (Balance[0] == "eq" ? c.Balance == decimal.Parse(Balance[1]) :
                        Balance[0] == "gt" ? c.Balance >= decimal.Parse(Balance[1]) :
                        c.Balance <= decimal.Parse(Balance[1]))) &&
                    (IsVatExempt == null || (IsVatExempt.Contains("null") && (c.IsVatExempt == null)) || (c.IsVatExempt != null && IsVatExempt.Contains(c.IsVatExempt.ToString()))) &&
                    (CreatedOn == null || (c.CreatedOn >= DateTime.Parse(CreatedOn[0]) && c.CreatedOn <= DateTime.Parse(CreatedOn[1])))
                );

                //Sort et Take
                query = Order == "1"
                    ? query.OrderBy(selector).Take(rowCount)
                    : query.OrderByDescending(selector).Take(rowCount);

                return [.. query];
            }
            else
            {
                return [.. context.Accounts.OrderByDescending(c => c.CreatedOn).Take(rowCount)];
            }
        }


        static public List<User> SortAndFilterUsersTable(ModelDbContext context, string sortBy, string Order, string RowCount, string Id = "", string Email = "", string Username = "", List<string> CreatedOn = null, List<string> FailedLoginAttempts = null, string PhoneNumber = "", string InternalNotes = "")
        {

            var selectors = new Dictionary<string, Expression<Func<User, object>>>()
            {
                ["Id"] = s => s.Id,
                ["Email"] = s => s.Email!,
                ["Username"] = s => s.Username!,
                ["Role"] = s => s.Role!,
                ["Status"] = s => s.Status!,
                ["LastLoginOn"] = s => s.LastLoginOn!,
                ["FailedLoginAttempts"] = s => s.FailedLoginAttempts,
                ["IsTwoFactorEnabled"] = s => s.IsTwoFactorEnabled,
                ["CreatedOn"] = s => s.CreatedOn,
                ["PhoneNumber"] = s => s.PhoneNumber!,
                ["InternalNotes"] = s => s.InternalNotes
            };

            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<User> query = context.Users;

            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                //Filter
                query = query.Where(c =>
                  (Id == null || c.Id == Guid.Parse(Id)) &&
                  (string.IsNullOrEmpty(Email) || (c.Email != null && c.Email.Contains(Email))) &&
                  (string.IsNullOrEmpty(Username) || (c.Username != null && c.Username.Contains(Username))) &&
                  (CreatedOn == null || (c.CreatedOn >= DateTime.Parse(CreatedOn[0]) && c.CreatedOn <= DateTime.Parse(CreatedOn[1])))&&
                  (FailedLoginAttempts == null ||
                      (FailedLoginAttempts[0] == "eq" ? c.FailedLoginAttempts == int.Parse(FailedLoginAttempts[1]) :
                      FailedLoginAttempts[0] == "gt" ? c.FailedLoginAttempts >= int.Parse(FailedLoginAttempts[1]) :
                      c.FailedLoginAttempts <= int.Parse(FailedLoginAttempts[1]))) &&
                  (string.IsNullOrEmpty(PhoneNumber) || (c.PhoneNumber != null && c.PhoneNumber.Contains(PhoneNumber))) &&
                  (string.IsNullOrEmpty(InternalNotes) || (c.InternalNotes != null && c.InternalNotes.Contains(InternalNotes))) 
                  
              );

                //Sort et Take
                query = Order == "1"
                    ? query.OrderBy(selector).Take(rowCount)
                    : query.OrderByDescending(selector).Take(rowCount);

                return [.. query];
            }
            else
            {
                return [.. context.Users.Take(rowCount)];
            }
        }
    }

}
