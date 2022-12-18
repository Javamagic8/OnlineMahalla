using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model
{
    public class CurrencyRateInfoModel
    {
        public DateTime Date { get; set; }
        public List<CurrencyRateInfoItemModel> Items = new List<CurrencyRateInfoItemModel>();
    }
    public class CurrencyRateInfoItemModel
    {
        public int CurrencyID { get; set; }
        public string Alpha3 { get; set; }
        public decimal Rate { get; set; }
        public decimal? Difference { get; set; }
    }
}
