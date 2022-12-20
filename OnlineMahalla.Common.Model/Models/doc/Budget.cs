using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.doc
{
    public class Budget
    {
        public Budget()
        {
            this.Date = DateTime.Today;
            this.FinanceYear = 2022;
            //this.DateOfStart = DateTime.Today;
            //this.DateOfEnd = new DateTime(DateTime.Today.Year, 12, 31);
            this.TableByMonths = new List<BudgetTable>();
            this.Tables = new List<BudgetTables>();
            this.TableByCalc = new List<BudgetCalc>();
            this.TableByCalcGSM = new List<BudgetCalcGSM>();
            this.TableByCalcFM = new List<BudgetCalcFM>();
            this.TableByCalcFood = new List<BudgetCalcFood>();
            this.TableByCalcFMSub = new List<BudgetCalcFMSub>();
            this.TablesReceived = new List<BudgetTableOranization>();
            this.TablesNotReceived = new List<BudgetTableOranization>();
            //this.ElementsTables = new List<BudgetTables>();
            //this.OrganizationsTables = new List<BudgetTables>();
            //this.FinanceYear = DateTime.Today.Year;
            this.ToSent = false;
        }
        public long ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public int? OrganizationsSettlementAccountID { get; set; }
        public string OrganizationAccountCode { get; set; }
        public int BudgetTypeID { get; set; }
        public int TreasBudgetTypeID { get; set; }
        public int StatusID { get; set; }
        public int OrganizationID { get; set; }
        public int? TreasStatusID { get; set; }
        [JsonIgnore]
        public int? CreatedUserID { get; set; }
        [JsonIgnore]
        public int TableID { get; set; }
        public int? FinanceYear { get; set; }
        public long? BudgetRegisteryID { get; set; }
        public int? HeaderOrganizationID { get; set; }
        public string HeaderOrganizationName { get; set; }
        public int? OrganizationFunctionalItemID { get; set; }
        public string OrganizationFunctionalItemCode { get; set; }
        public string SettleCodeLevel { get; set; }
        public long? HeaderBudgetRegisteryID { get; set; }
        public bool ToSent { get; set; }
        public long? StaffListID { get; set; }
        public int? MonthlyAllocationID { get; set; }
        public decimal? MonthlyAllocationPercentage { get; set; }
        public string Detail { get; set; }
        public long? TreasBudgetID { get; set; }
        public bool CanSave { get; set; } = true;
        public string RegistrationName1 { get; set; }
        public string RegistrationName2 { get; set; }
        public List<BudgetTable> TableByMonths { get; set; }
        public List<BudgetTables> Tables { get; set; }
        //public List<BudgetTables> ElementsTables { get; set; }
        //public List<BudgetTables> OrganizationsTables { get; set; }
        public List<BudgetCalc> TableByCalc { get; set; }
        public List<BudgetCalcGSM> TableByCalcGSM { get; set; }
        public List<BudgetCalcFM> TableByCalcFM { get; set; }
        public List<BudgetCalcFood> TableByCalcFood { get; set; }
        public List<BudgetCalcFMSub> TableByCalcFMSub { get; set; }
        public class BudgetCalc
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public string Name { get; set; }
            public int ItemOfExpensesID { get; set; }
            public string ItemOfExpensesCode { get; set; }
            public int UnitsOfMeasureID { get; set; }
            public string UnitsOfMeasureName { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
            public decimal QuantityCalc { get; set; }
            public decimal PriceCalc { get; set; }
            public decimal SumCalc { get; set; }
            public decimal QuantitySep { get; set; }
            public decimal PriceSep { get; set; }
            public decimal SumSep { get; set; }
        }
        public class BudgetCalcGSM
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public string Name { get; set; }
            public string CountryNumber { get; set; }
            public decimal Coefficient { get; set; }
            public string WorkShift { get; set; }
            public int FuelTypeID { get; set; }
            public string FuelTypeCode { get; set; }
            public int UnitsOfMeasureID { get; set; }
            public string UnitsOfMeasureName { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
            public decimal Annual { get; set; }
            public decimal Monthly { get; set; }
            public decimal FuelConsumption { get; set; }
            public decimal MonthlyFuelConsumptionOld { get; set; }
            public decimal SoInAYearOld { get; set; }
            public decimal MonthlyFuelConsumption { get; set; }
            public decimal SoInAYear { get; set; }
            public decimal MonthlyFuelConsumptionLitr { get; set; }
            public decimal FuelPrices { get; set; }
            public decimal OneYearAmount { get; set; }
        }
        public class BudgetCalcFM
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public int? DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public bool IsFood { get; set; }
            public bool CanEdit { get; set; }
            public string Name { get; set; }
            public int UnitsOfMeasureID { get; set; }
            public string UnitsOfMeasureName { get; set; }
            public decimal NumberOfSeatsOld { get; set; }
            public decimal PlaceOfWork { get; set; }
            public decimal DaysWorkedOneYearOld { get; set; }
            public decimal ActualAmountOfAnnualProduct { get; set; }
            public decimal AveragePriceOfProducts { get; set; }
            public decimal AmountOfConsumedProducts { get; set; }
            public decimal AverageDailyRealCost { get; set; }
            public decimal NumberOfSeats { get; set; }
            public decimal AnnualWorkingDay { get; set; }
            public decimal DaysWorkedOneYear { get; set; }
            public decimal StandardOfTheProduct { get; set; }
            public decimal PriceOfTheProduct { get; set; }
            public decimal TotalRequiredFinance { get; set; }
            public decimal AverageDailyCost { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
        }
        public class BudgetCalcFood
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public int? DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public bool IsFood { get; set; }
            public bool CanEdit { get; set; }
            public string Name { get; set; }
            public int UnitsOfMeasureID { get; set; }
            public string UnitsOfMeasureName { get; set; }
            public decimal NumberOfSeatsOld { get; set; }
            public decimal PlaceOfWork { get; set; }
            public decimal DaysWorkedOneYearOld { get; set; }
            public decimal ActualAmountOfAnnualProduct { get; set; }
            public decimal AveragePriceOfProducts { get; set; }
            public decimal AmountOfConsumedProducts { get; set; }
            public decimal AverageDailyRealCost { get; set; }
            public decimal NumberOfSeats { get; set; }
            public decimal AnnualWorkingDay { get; set; }
            public decimal DaysWorkedOneYear { get; set; }
            public decimal StandardOfTheProduct { get; set; }
            public decimal PriceOfTheProduct { get; set; }
            public decimal TotalRequiredFinance { get; set; }
            public decimal AverageDailyCost { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
        }
        public class BudgetCalcFMSub
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public int? DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public bool IsFood { get; set; }
            public bool CanEdit { get; set; }
            public string Name { get; set; }
            public int UnitsOfMeasureID { get; set; }
            public string UnitsOfMeasureName { get; set; }
            public decimal NumberOfSeatsOld { get; set; }
            public decimal PlaceOfWork { get; set; }
            public decimal DaysWorkedOneYearOld { get; set; }
            public decimal ActualAmountOfAnnualProduct { get; set; }
            public decimal AveragePriceOfProducts { get; set; }
            public decimal AmountOfConsumedProducts { get; set; }
            public decimal AverageDailyRealCost { get; set; }
            public decimal NumberOfSeats { get; set; }
            public decimal AnnualWorkingDay { get; set; }
            public decimal DaysWorkedOneYear { get; set; }
            public decimal StandardOfTheProduct { get; set; }
            public decimal PriceOfTheProduct { get; set; }
            public decimal TotalRequiredFinance { get; set; }
            public decimal AverageDailyCost { get; set; }
            public bool? IsSub { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
        }
        public class BudgetTable
        {
            public long ID { get; set; }
            public long OwnerID { get; set; }
            public int ItemOfExpensesID { get; set; }
            public int Month { get; set; }
            public decimal Sum { get; set; }
            public string ItemOfExpensesCode { get; set; }
            public string ItemOfExpensesName { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted   
            public int OrganizationID { get; set; }
            public string OrganizationName { get; set; }
            public int ParentID { get; set; }
            public bool IsGroup { get; set; }
            public decimal ChangeLastYearSum { get; set; }
            public decimal LastYearSum { get; set; }
            public int NumberOfGroup { get; set; }

        }
        public class BudgetTables
        {
            public long ID { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted
            public long OwnerID { get; set; }
            public int ItemOfExpensesID { get; set; }
            public string ItemOfExpensesCode { get; set; }
            public int OrganizationID { get; set; }
            public string OrganizationName { get; set; }
            public string OrganizationINN { get; set; }
            public string OrganizationSettlementAccount { get; set; }
            public long ParentID { get; set; }
            public int BudgetMonth { get; set; }
            public int BudgetTypeID { get; set; }
            public string ItemOfExpensesName { get; set; }
            public int NumberOfGroup { get; set; }
            public bool IsGroup { get; set; }
            public decimal ChangeLastYearSum { get; set; }
            public decimal LastYearSum { get; set; }

            /// <summary>
            /// Квартал-1
            /// </summary>
            public decimal Quarter1
            {
                get { return Month1 + Month2 + Month3; }
            }
            public decimal Month1 { get; set; }
            public decimal Month2 { get; set; }
            public decimal Month3 { get; set; }
            /// <summary>
            /// Квартал-2
            /// </summary>
            public decimal Quarter2
            {
                get { return Month4 + Month5 + Month6; }
            }
            public decimal Month4 { get; set; }
            public decimal Month5 { get; set; }
            public decimal Month6 { get; set; }
            /// <summary>
            /// Квартал-4
            /// </summary>
            public decimal Quarter3
            {
                get { return Month7 + Month8 + Month9; }
            }
            public decimal Month7 { get; set; }
            public decimal Month8 { get; set; }
            public decimal Month9 { get; set; }
            /// <summary>
            /// Квартал-4
            /// </summary>
            public decimal Quarter4
            {
                get { return Month10 + Month11 + Month12; }
            }
            public decimal Month10 { get; set; }
            public decimal Month11 { get; set; }
            public decimal Month12 { get; set; }
            public decimal Yearly
            {
                get { return Quarter1 + Quarter2 + Quarter3 + Quarter4; }
            }

            /// <summary>
            /// Годовой
            /// </summary>
            public decimal Sum
            {
                get { return Month1 + Month2 + Month3 + Month4 + Month5 + Month6 + Month7 + Month8 + Month9 + Month10 + Month11 + Month12; }
            }
        }
        public List<BudgetTableOranization> TablesReceived { get; set; }
        public List<BudgetTableOranization> TablesNotReceived { get; set; }
        public class BudgetTableOranization
        {
            public long ID { get; set; }
            public int Number { get; set; }
            public string Organization { get; set; }
            public string INN { get; set; }
            public string ContactInfo { get; set; }
            public DateTime Date { get; set; }
            public int FinanceYear { get; set; }
            public decimal Sum { get; set; }
            public string SettlementAccountCode { get; set; }
            public string BudgetType { get; set; }
            public string OrganizationFunctionalItemCode { get; set; }
            public string SettleCodeLevel { get; set; }
            public int Status { get; set; }//1-Created,2-Modified,3-Deleted 

        }
    }
}
