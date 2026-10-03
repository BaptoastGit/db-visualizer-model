using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Model.BO.Service
{
    static public class TableDataService
    {
        static public List<dynamic> SortAndFilterTable(ModelDbContext context, JsonObject payload)
        {   
            string entityName = payload["TableName"]?.ToString()?.TrimEnd('s', 'S') ?? "Accounts";
            int rowCount = string.IsNullOrEmpty(payload["RowCount"]?.ToString()) ? 100: int.Parse(payload["RowCount"]?.ToString());
            var entityType = (context.Model.GetEntityTypes()
                    .FirstOrDefault(e => e.ClrType.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase))?.ClrType) ?? throw new InvalidOperationException($"L'entité '{entityName}' est introuvable dans le DbContext.");


            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)
                     .MakeGenericMethod(entityType);
            IQueryable<dynamic> query = (IQueryable<dynamic>)setMethod.Invoke(context, null)!;

            if (!string.IsNullOrWhiteSpace(payload["Order"]?.ToString()) && entityType.GetProperty(payload["SortBy"]?.ToString()) != null)
            {
                query = AddFiltersToQuery(query, entityType, payload);
                return [.. query.Take(rowCount)];
            }
            else
            {
                return [.. query.Take(rowCount)];
            }

        }

        static private IQueryable<dynamic> AddFiltersToQuery(IQueryable<dynamic> query, Type entityType, JsonObject payload)
        {
            foreach (KeyValuePair<string, JsonNode?> kvp in payload)
            {
                if (kvp.Key != "TableName" && kvp.Key != "SortBy" && kvp.Key != "Order" && kvp.Key != "RowCount")
                {

                    var property = entityType.GetProperty(kvp.Key);
                    var propertyType = property?.PropertyType;
                    Console.WriteLine("key: " + kvp.Key + " value: " + kvp.Value?.ToString() + "property: ");
                    if (!string.IsNullOrEmpty(kvp.Value.ToString()))
                    {
                        if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                        {
                            string startDate = kvp.Value.ToString().Substring(0, 16);
                            string endDate = kvp.Value.ToString().Substring(17, 16);
                            query = query.Where($"{kvp.Key} == null || {kvp.Key} >= @0 && {kvp.Key} <= @1", DateTime.Parse(startDate), DateTime.Parse(endDate));
                        }
                        else if (propertyType == typeof(Guid) && Guid.TryParse(kvp.Value.ToString(), out Guid parsedGuid))
                        {
                            query = query.Where($"{kvp.Key} == @0", parsedGuid);
                        }
                        else if (propertyType == typeof(bool?) || propertyType == typeof(bool))
                        {
                            string filterStr = kvp.Value.ToString().ToLower();
                            string nullCondition = propertyType == typeof(bool?) ? $" || ({kvp.Key} == null && @0.Contains(\"null\"))" : "";
                            query = query.Where($"({kvp.Key} == true && @0.Contains(\"true\")) || ({kvp.Key} == false && @0.Contains(\"false\")){nullCondition}", filterStr);
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(int) || propertyType == typeof(double?))
                        {

                            string operatorSelected = kvp.Value.ToString().Substring(0, 2);
                            if (int.TryParse(kvp.Value.ToString().Substring(3, kvp.Value.ToString().Length - 3), out int parsedInt))
                            {

                                switch (operatorSelected)
                                {
                                    case "gt":
                                        query = query.Where($"{kvp.Key} == null || {kvp.Key} >= @0", parsedInt);
                                        break;
                                    case "eq":
                                        query = query.Where($"{kvp.Key} == null || {kvp.Key} == @0", parsedInt);
                                        break;
                                    case "lt":
                                        query = query.Where($"{kvp.Key} == null || {kvp.Key} <= @0", parsedInt);
                                        break;
                                }
                            }
                        }
                        else if (kvp.Value.ToString().Contains(','))
                        {
                            query = query.Where($"{kvp.Key} != null && @0.Contains({kvp.Key})", kvp.Value.ToString());
                        }
                        else if (propertyType == typeof(string))
                        {
                            query = query.Where($"{kvp.Key} != null && {kvp.Key}.Contains(@0)", kvp.Value.ToString());
                        }
                    }

                }
            }
            var sortProperty = entityType.GetProperty(payload["SortBy"].ToString());
            string orderingExpression = $"{sortProperty.Name} {(payload["Order"].ToString() == "0" ? "descending" : "ascending")}";
            query = query.OrderBy(orderingExpression);
            return query;
        }

    }

}
