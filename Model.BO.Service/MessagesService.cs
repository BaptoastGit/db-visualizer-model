using Microsoft.Extensions.Configuration;
using Model.BO.Data.Model;
using Renci.SshNet;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;


namespace Model.BO.Service
{
    public class MessagesService(IConfiguration configuration)
    {
        static public List<Message> SortAndFilterMessagesTable(ModelDbContext context, string sortBy, string Order, string RowCount = "", string ErrorMessage = "", List<string> PayloadSize = null, string Priority = "", List<string> ProcessedOn = null, string CorrelationId = "")
        {
            var selectors = new Dictionary<string, Expression<Func<Message, object>>>()
            {
                ["RetryCount"] = s => s.RetryCount,
                ["ErrorMessage"] = s => s.ErrorMessage!,
                ["PayloadSize"] = s => s.PayloadSize!,
                ["Priority"] = s => s.Priority!,
                ["ProcessedOn"] = s => s.ProcessedOn!,
                ["CorrelationId"] = s => s.CorrelationId!
            };

            int rowCount = string.IsNullOrEmpty(RowCount) ? 500 : int.Parse(RowCount);
            IQueryable<Message> query = context.Messages;

            if (!string.IsNullOrWhiteSpace(sortBy) && selectors.TryGetValue(sortBy, out var selector))
            {
                //Filter
                query = query.Where(c =>
                    (string.IsNullOrEmpty(ErrorMessage) || (c.ErrorMessage != null && c.ErrorMessage.Contains(ErrorMessage))) &&
                    (PayloadSize == null ||
                        (PayloadSize[0] == "eq" ? c.PayloadSize == long.Parse(PayloadSize[1]) :
                        PayloadSize[0] == "gt" ? c.PayloadSize >= long.Parse(PayloadSize[1]) :
                        c.PayloadSize <= long.Parse(PayloadSize[1]))) &&
                    (string.IsNullOrEmpty(Priority) || (c.Priority != null && c.Priority.Contains(Priority))) &&
                    (ProcessedOn == null || (c.ProcessedOn != null && c.ProcessedOn >= DateTime.Parse(ProcessedOn[0]) && c.ProcessedOn <= DateTime.Parse(ProcessedOn[1]))) &&
                    (string.IsNullOrEmpty(CorrelationId) || (c.CorrelationId != null && c.CorrelationId.Contains(CorrelationId)))
                );

                //Sort et Take
                query = Order == "1" 
                    ? query.OrderBy(selector).Take(rowCount) 
                    : query.OrderByDescending(selector).Take(rowCount);

                return [.. query];
            }
            else
            {
                return [.. context.Messages.Take(rowCount)];
            }
        }
       

    }
}
