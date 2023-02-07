 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.sys
{
    public class Organization
    {
        public Organization()
        {
            SettlementAccount = new List<OrganizationsSettlementAccount>();
            FunctionalItem = new List<OrganizationsFunctionalItem>();
            Sign = new List<OrganizationSign>();
            DocSettAccount = new List<OrganizationsDocSettAccount>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string INN { get; set; }
        public int OKONHID { get; set; }
        public int FinancingLevelID { get; set; }
        public string FinancingLevelName { get; set; }
        public int TreasuryBranchID { get; set; }
        public string TreasuryDepartmentHeader { get; set; }
        public string TreasuryResPerson { get; set; }
        public int OblastID { get; set; }
        public int RegionID { get; set; }
        public string ZipCode { get; set; }
        public string Adress { get; set; }
        public string ContactInfo { get; set; }
        public string Director { get; set; }
        public string Accounter { get; set; }
        public string Cashier { get; set; }
        public string IncomeNumber { get; set; }
        public DateTime? IncomeDate { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime? DateOfModified { get; set; }
        public int? OrganizationTypeID { get; set; }
        public int StateID { get; set; }
        public bool IsFullBudget { get; set; }
        public int? HeaderOrganizationID { get; set; }
        public string HeaderOrganizationName { get; set; }
        public int ChapterID { get; set; }
        public string ChapterName { get; set; }
        public string ChapterCode { get; set; }
        public int? CentralOrganizationID { get; set; }
        public string CentralOrganizationName { get; set; }
        public int? HeaderStaffListOrganizationID { get; set; }
        public int? BRParentOranizationID { get; set; }
        public string BRParentOrganizationName { get; set; }
        public bool Restriction { get; set; }
        public List<OrganizationsSettlementAccount> SettlementAccount { get; set; }
        public List<OrganizationsFunctionalItem> FunctionalItem { get; set; }
        public List<OrganizationSign> Sign { get; set; }
        public List<OrganizationsDocSettAccount> DocSettAccount { get; set; }
        public class OrganizationsSettlementAccount
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public int BankID { get; set; }
            public string BankCode { get; set; }
            public int? OrganizationFunctionalItemID { get; set; }
            public string OrganizationFunctionalItemCode { get; set; }
            public int? TreasuryBranchID { get; set; }
            public string TreasuryBranchName { get; set; }
            public int? CashSubAccID { get; set; }
            public int? ActualSubAccID { get; set; }
            public string Comment { get; set; }
            public int StateID { get; set; }
            public int OrganizationID { get; set; }
            public int CreatedUserID { get; set; }
            public DateTime? DateOfModified { get; set; }
            public string TreasuryResPersonName { get; set; }
            public int TreasuryResPersonID { get; set; }
            public bool? OutOfBalance { get; set; }
            public DateTime? OpenDate { get; set; }
            public DateTime? CloseDate { get; set; }
            public int? OldID { get; set; }
            public string OldCode { get; set; }
            public DateTime? DateOfCentr { get; set; }
            public bool IsActive { get; set; }
            public int Status { get; set; }
        }
        public class OrganizationsFunctionalItem
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public int FunctionalItemID { get; set; }
            public string FunctionalItemCode { get; set; }
            public int ChapterID { get; set; }
            public string ChapterCode { get; set; }
            public int OrganizationID { get; set; }
            public int CreatedUserID { get; set; }
            public DateTime? DateOfModified { get; set; }
            public int Status { get; set; }
        }
        public class OrganizationSign
        {
            public int ID { get; set; }
            public string FIO { get; set; }
            public int SignNumber { get; set; }
            public int StateID { get; set; }
            public int OrganizationID { get; set; }
            public int CreatedUserID { get; set; }
            public DateTime? DateOfModified { get; set; }
            public string PositionNameRus { get; set; }
            public string PositionNameUzb { get; set; }
            public int Status { get; set; }
        }
        public class OrganizationsDocSettAccount
        {
            public int ID { get; set; }
            public int OwnerID { get; set; }
            public string HeaderOrganizationName { get; set; }
            public int TableID { get; set; }
            public string TableName { get; set; }
            public int OrganizationID { get; set; }
            public string OrganizationName { get; set; }
            public int? OrganizationsSettlementAccountID { get; set; }
            public string OrganizationsSettlementAccountCode { get; set; }
            public int CreatedUserID { get; set; }
            public int ModifiedUserID { get; set; }
            public DateTime DateOfCreated { get; set; }
            public DateTime? DateOfModified { get; set; }
            public int? OrganizationFunctionalItemID { get; set; }
            public string OrganizationFunctionalItemCode { get; set; }
            public string SettleCodeLevel { get; set; }
            public int StateID { get; set; }
            public string State { get; set; }
            public int Status { get; set; }
        }
    }
    public class OrgSign
    {
        public string FIO { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
    }
}
