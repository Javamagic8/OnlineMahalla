using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.doc;
using System.Dynamic;

namespace UZASBO.Data.Utility
{
    public static class Extention
    {
        public static PagedDataEx ExtractTotal(this IEnumerable<dynamic> dataList, bool withCalculate = false, string totalColumnPrefix = "Total_")
        {
            if (withCalculate)
                return dataList.ExtractTotalWithCalculate(totalColumnPrefix);

            return dataList.ExtractTotalInternal(totalColumnPrefix);
        }
        private static PagedDataEx ExtractTotalInternal(this IEnumerable<dynamic> dataList, string totalColumnPrefix)
        {
            PagedDataEx data = new PagedDataEx();
            var queryResult = dataList.Cast<IDictionary<string, object>>()
                    .ToArray();
            if (queryResult.Length > 0)
            {
                var totalRow = new ExpandoObject() as IDictionary<string, object>;
                foreach (var key in queryResult[0].Keys.Where(a => a.StartsWith(totalColumnPrefix)))
                    totalRow[key.Substring(totalColumnPrefix.Length)] = queryResult[0][key];
                data.totalRow = totalRow;
                data.total = (data.totalRow != null && data.totalRow.RowsCount != null) ? data.totalRow.RowsCount : 0;
                string[] rowKeys = queryResult[0].Keys.Where(a => !a.StartsWith(totalColumnPrefix)).ToArray();
                data.rows = queryResult.Select(a =>
                {
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var rowKey in rowKeys)
                        row[rowKey] = a[rowKey];
                    return row;
                });
            }
            if (data.rows == null)
                data.rows = new List<dynamic>();
            return data;
        }
        private static PagedDataEx ExtractTotalWithCalculate(this IEnumerable<dynamic> dataList, string totalColumnPrefix)
        {
            PagedDataEx data = new PagedDataEx();
            var queryResult = dataList.Cast<IDictionary<string, object>>()
                    .ToArray();
            if (queryResult.Length > 0)
            {
                var totalRow = new ExpandoObject() as IDictionary<string, object>;
                foreach (var key in queryResult[0].Keys.Where(a => a.StartsWith(totalColumnPrefix)))
                    totalRow[key.Substring(totalColumnPrefix.Length)] = queryResult[0][key];
                data.totalRow = totalRow;
                string[] rowKeys = queryResult[0].Keys.Where(a => !a.StartsWith(totalColumnPrefix)).ToArray();
                data.rows = queryResult.Select(a =>
                {
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var rowKey in rowKeys)
                        row[rowKey] = a[rowKey];
                    foreach (var totalRowKey in totalRow.Keys)
                    {
                        if (a[totalRowKey] != null)
                        {
                            if (totalRow[totalRowKey] == null)
                                totalRow[totalRowKey] = 0;
                            totalRow[totalRowKey] = (dynamic)totalRow[totalRowKey] + (dynamic)a[totalRowKey];
                        }
                    }
                    return row;
                });
                data.totalRow.RowsCount = queryResult.Length;
                data.total = (data.totalRow != null && data.totalRow.RowsCount != null) ? data.totalRow.RowsCount : 0;

            }
            if (data.rows == null)
                data.rows = new List<dynamic>();

            return data;
        }

        public static List<Budget.BudgetTables> ReCalcBudgetTable(List<Budget.BudgetTables> data, List<dynamic> itemlist)
        {
            List<Budget.BudgetTables> result = new List<Budget.BudgetTables>();
            for (int i = 0; i < data.Count; i++)
            {
                result.Add(data[i]);
                AddParentBudgetTable(ref result, itemlist, data[i], data[i]);
            }

            return result;
        }
        private static void AddParentBudgetTable(ref List<Budget.BudgetTables> result, List<dynamic> itemlist, Budget.BudgetTables currow, Budget.BudgetTables addrow)
        {
            var curitem = itemlist.FirstOrDefault(x => x.ID == currow.ItemOfExpensesID);
            System.Reflection.PropertyInfo[] pinfos = typeof(Budget.BudgetTables).GetProperties();
            if (curitem.ParentID != null && curitem.ParentID > 0)
            {
                var parentitem = itemlist.FirstOrDefault(x => x.ID == curitem.ParentID);
                var parentrow = result.FirstOrDefault(x => x.ItemOfExpensesID == parentitem.ID);
                if (parentrow == null)
                {
                    parentrow = new Budget.BudgetTables()
                    {
                        ItemOfExpensesID = parentitem.ID,
                        ItemOfExpensesCode = parentitem.Code,
                        ItemOfExpensesName = parentitem.Name,
                        NumberOfGroup = parentitem.NumberOfGroup,
                        IsGroup = parentitem.IsGroup,
                        Status = 0,
                    };
                    result.Add(parentrow);
                }
                for (int i = 1; i < 13; i++)
                {
                    decimal parentvalue = (decimal)pinfos.First(x => x.Name == "Month" + i).GetValue(parentrow);
                    decimal curvalue = (decimal)pinfos.First(x => x.Name == "Month" + i).GetValue(addrow);
                    pinfos.First(x => x.Name == "Month" + i).SetValue(parentrow, parentvalue + curvalue, null);

                }
                AddParentBudgetTable(ref result, itemlist, parentrow, addrow);
            }
        }

    }
}
