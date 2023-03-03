using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GetAdminNeighborhoodList(int ID, string INN, string Name, string Region, string District, int OrganizationType, string Search, string Sort, string Order, int Offset, int Limit)
        {
            string sql = "SELECT UserDistrict.DistrictID FROM sys_UserDistrict UserDistrict WHERE UserDistrict.UserID=8";
            var userregionlist = _databaseExt.GetDataFromSql(sql,
                new string[] { "@UserID" },
                new object[] { UserID }, System.Data.CommandType.Text).ToList();
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT 
                                        Neighborhood.ID,
                                        Neighborhood.Name,
                                        Neighborhood.ChairmanName,
                                        Neighborhood.CountFamily,
                                        Region.Name RegionName,
                                        District.Name DistrictName,
                                        Neighborhood.CountHome,
                                        Neighborhood.Address,
                                        Neighborhood.PhoneNumber,
                                        Neighborhood.INN,
                                        OrganizationType.Name OrganizationType,
                                        DistrictType.DisplayName DistrictType ";
            string sqlfrom = @" FROM info_Neighborhood Neighborhood
                                        JOIN info_Region Region ON Region.ID = Neighborhood.RegionID
                                        JOIN info_District District ON District.ID = Neighborhood.DistrictID
                                        join enum_DistrictType DistrictType ON DistrictType.ID = Neighborhood.DistrictTypeID
                                        JOIN enum_OrganizationType OrganizationType ON OrganizationType.ID = Neighborhood.TypeOrganizationID ";
            
            string sqlwhere = " WHERE 1=1";
            string UserDistrictID = "";
            switch (UserID)
            {

                case 28877: UserDistrictID = "6"; break;
                
                default:
                    UserDistrictID = "0";
                    break;
            }
            bool check = false;
            if (UserDistrictID == "0" && UserIsInRole("FinancialAuthority"))
            {
                var list = _databaseExt.GetDataFromSql(@"SELECT ID FROM sys_UserDistrict where UserID = @UserID", new string[] { "@UserID" }, new object[] { UserID }).ToList();
                if (list.Count > 0)
                {
                    UserDistrictID = String.Join(",", list.Select(x => x.DistrictID).ToList());
                    sqlwhere += " AND Neighborhood.DistrictID in(" + UserDistrictID + ")";
                }
                else
                    sqlwhere += " AND Neighborhood.DistrictID in(" + 0 + ")";
                check = true;

            }

            if ((UserDistrictID.Length != 4 && UserDistrictID != "0") && !check)
                sqlwhere += " AND Neighborhood.RegionID in( " + UserDistrictID + ")";



            if (userregionlist.Count() != 0)
            {
                sqlfrom += "JOIN sys_UserDistrict UserDistrict ON UserDistrict.DistrictID=District.ID ";
                sqlwhere += " AND usr.UserID=@UserID ";
                sqlparamas.Add("@UserID", UserID);
            }
            if (ID > 0)
            {
                sqlwhere += " AND Neighborhood.ID=@ID";
                sqlparamas.Add("@ID", ID);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Neighborhood.Name LIKE '%' + @Name + '%'";
                sqlparamas.Add("@Name", Name);
            }
            if (!String.IsNullOrEmpty(INN) && INN.Length == 9)
            {
                sqlwhere += " AND Neighborhood.INN = @INN";
                sqlparamas.Add("@INN", INN);
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sqlwhere += " AND Region.Name LIKE '%' + @Region + '%'";
                sqlparamas.Add("@District", District);
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sqlwhere += " AND District.Name LIKE '%' + @District + '%'";
                sqlparamas.Add("@District", Region);
            }

            string sqlcount = "SELECT Count(Neighborhood.ID) " + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Neighborhood.ID DESC";
            if (Offset == 1)
                Offset = 0;
            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }

        public Organization GetAdminNeighborhood(int ID, bool? IsClone = false)
        {
            if (ID == 0)
                ID = OrganizationID;
            var data = _databaseExt.GetDataFromSql("SELECT org.*, (SELECT top 1 Name FROM info_Organization WHERE BRParentOranizationID=org.ID) as BRParentOrganizationName FROM info_Organization org WHERE ID=@ID", new string[] { "@ID" }, new object[] { ID }).First();
            Organization Organization = new Organization();
            if (!IsClone.Value)
            {
                Organization.ID = data.ID;
                Organization.Name = data.Name;
                Organization.FullName = data.FullName;
                Organization.INN = data.INN;
                Organization.OKONHID = data.OKONHID;
                Organization.FinancingLevelID = data.FinancingLevelID;
                Organization.TreasuryBranchID = data.TreasuryBranchID;
                Organization.TreasuryDepartmentHeader = data.TreasuryDepartmentHeader;
                Organization.TreasuryResPerson = data.TreasuryResPerson;
                Organization.OblastID = data.OblastID;
                Organization.RegionID = data.RegionID;
                Organization.ZipCode = data.ZipCode;
                Organization.Adress = data.Adress;
                Organization.ContactInfo = data.ContactInfo;
                Organization.Director = data.Director;
                Organization.Accounter = data.Accounter;
                Organization.Cashier = data.Cashier;
                Organization.IncomeNumber = data.IncomeNumber;
                Organization.IncomeDate = DateTimeUtility.ToNullable(data.IncomeDate);
                Organization.CreatedUserID = data.CreatedUserID;
                Organization.OrganizationTypeID = data.OrganizationTypeID;
                Organization.StateID = data.StateID;
                Organization.IsFullBudget = data.IsFullBudget;
                Organization.HeaderOrganizationID = data.HeaderOrganizationID;
                Organization.ChapterID = data.ChapterID;
                Organization.CentralOrganizationID = data.CentralOrganizationID;
                Organization.CentralOrganizationName = data.CentralOrganizationName;
                Organization.BRParentOranizationID = data.BRParentOranizationID;
                Organization.BRParentOrganizationName = data.BRParentOrganizationName;
                Organization.HeaderStaffListOrganizationID = null;
                Organization.SettlementAccount = new List<Organization.OrganizationsSettlementAccount>();
                Organization.FunctionalItem = new List<Organization.OrganizationsFunctionalItem>();
                Organization.Sign = new List<Organization.OrganizationSign>();
            }

            else if (IsClone.Value)
            {
                Organization.ID = 0;
                Organization.Name = data.Name;
                Organization.FullName = data.FullName;
                Organization.INN = data.INN;
                Organization.OKONHID = data.OKONHID;
                Organization.FinancingLevelID = data.FinancingLevelID;
                Organization.TreasuryBranchID = data.TreasuryBranchID;
                Organization.TreasuryDepartmentHeader = data.TreasuryDepartmentHeader;
                Organization.TreasuryResPerson = data.TreasuryResPerson;
                Organization.OblastID = data.OblastID;
                Organization.RegionID = data.RegionID;
                Organization.ZipCode = data.ZipCode;
                Organization.Adress = data.Adress;
                Organization.ContactInfo = data.ContactInfo;
                Organization.Director = data.Director;
                Organization.Accounter = data.Accounter;
                Organization.Cashier = data.Cashier;
                Organization.IncomeNumber = data.IncomeNumber;
                Organization.IncomeDate = DateTime.Now;
                Organization.CreatedUserID = data.CreatedUserID;
                Organization.OrganizationTypeID = data.OrganizationTypeID;
                Organization.StateID = data.StateID;
                Organization.IsFullBudget = data.IsFullBudget;
                Organization.HeaderOrganizationID = null;
                Organization.ChapterID = data.ChapterID;
                Organization.CentralOrganizationID = null;
                Organization.CentralOrganizationName = null;
                Organization.BRParentOranizationID = data.BRParentOranizationID;
                Organization.BRParentOrganizationName = data.BRParentOrganizationName;
                Organization.HeaderStaffListOrganizationID = null;
                Organization.SettlementAccount = new List<Organization.OrganizationsSettlementAccount>();
                Organization.FunctionalItem = new List<Organization.OrganizationsFunctionalItem>();
                Organization.Sign = new List<Organization.OrganizationSign>();
                return Organization;
            }
            var organizationssettlementaccount = _databaseExt.GetDataFromSql("SELECT osa.ID, osa.Name, osa.Code, osa.BankID, bank.Code [BankCode], osa.OrganizationFunctionalItemID, (SELECT Code FROM info_OrganizationsFunctionalItem WHERE ID=osa.OrganizationFunctionalItemID) [OrganizationFunctionalItemCode], (SELECT Name FROM info_TreasuryBranch WHERE ID=osa.TreasuryBranchID) [TreasuryBranchName], osa.TreasuryBranchID, osa.CashSubAccID, osa.ActualSubAccID, osa.Comment, osa.StateID, osa.OrganizationID, osa.CreatedUserID, osa.DateOfModified, osa.TreasuryResPersonName, osa.TreasuryResPersonID, osa.OutOfBalance, osa.IsActive FROM info_OrganizationsSettlementAccount osa, info_Bank bank WHERE osa.BankID=bank.ID AND osa.OrganizationID=@ID ORDER BY ID", new string[] { "@ID" }, new object[] { ID }).ToList();
            organizationssettlementaccount.ForEach((table1) =>
            {
                var OrganizationsSettlementAccount = new Organization.OrganizationsSettlementAccount() //L/S
                {
                    ID = table1.ID,
                    Name = table1.Name,
                    Code = table1.Code,
                    BankID = table1.BankID,
                    BankCode = table1.BankCode,
                    OrganizationFunctionalItemID = table1.OrganizationFunctionalItemID,
                    OrganizationFunctionalItemCode = table1.OrganizationFunctionalItemCode,
                    TreasuryBranchID = table1.TreasuryBranchID,
                    TreasuryBranchName = table1.TreasuryBranchName,
                    CashSubAccID = table1.CashSubAccID,
                    ActualSubAccID = table1.ActualSubAccID,
                    Comment = table1.Comment,
                    StateID = table1.StateID,
                    OrganizationID = table1.OrganizationID,
                    CreatedUserID = table1.CreatedUserID,
                    DateOfModified = table1.DateOfModified,
                    TreasuryResPersonName = table1.TreasuryResPersonName,
                    TreasuryResPersonID = table1.TreasuryResPersonID == null ? 0 : table1.TreasuryResPersonID,
                    OutOfBalance = BoolUtility.ToNullable(table1.OutOfBalance),
                    IsActive = table1.IsActive,
                    Status = 0


                };
                Organization.SettlementAccount.Add(OrganizationsSettlementAccount);
            });
            var organizationsfunctionalitem = _databaseExt.GetDataFromSql("SELECT ofi.ID, ofi.Code, FunctionalItemID, ChapterID,fioe.Code [FunctionalItemCode], chap.Code [ChapterCode] FROM info_OrganizationsFunctionalItem ofi, info_Chapter chap, info_FunctionalItemOfExpense fioe WHERE ofi.ChapterID=chap.ID AND ofi.FunctionalItemID=fioe.ID and ofi.OrganizationID=@ID", new string[] { "@ID" }, new object[] { ID }).ToList();
            organizationsfunctionalitem.ForEach((table2) =>
            {
                var OrganizationsFunctionalItem = new Organization.OrganizationsFunctionalItem()
                {
                    ID = table2.ID,
                    Code = table2.Code,
                    FunctionalItemID = table2.FunctionalItemID,
                    ChapterID = table2.ChapterID,
                    FunctionalItemCode = table2.FunctionalItemCode,
                    ChapterCode = table2.ChapterCode,
                    Status = 0
                };
                Organization.FunctionalItem.Add(OrganizationsFunctionalItem);
            });
            var organizationsign = _databaseExt.GetDataFromSql("SELECT * FROM info_OrganizationSign WHERE StateID=1 AND OrganizationID=@ID", new string[] { "@ID" }, new object[] { ID }).ToList();
            organizationsign.ForEach((table3) =>
            {
                var OrganizationSign = new Organization.OrganizationSign()
                {
                    ID = table3.ID,
                    FIO = table3.FIO,
                    SignNumber = table3.SignNumber,
                    PositionNameRus = table3.PositionNameRus,
                    PositionNameUzb = table3.PositionNameUzb,
                    Status = 0
                };
                Organization.Sign.Add(OrganizationSign);
            });
            //var organizationDocSettAccount = _databaseExt.GetDataFromSql(@"SELECT doc.*, 
            //                                                  tab.DisplayName TableName, 
            //                                                  org.Name OrganizationName, 
            //                                                  orgset.Code OrganizationsSettlementAccountCode 

            //                                                  FROM info_OrganizationsDocSettAccount doc 
            //                                                  LEFT JOIN info_Organization org ON org.ID = doc.OrganizationID
            //                                                  LEFT JOIN sys_Table tab ON tab.ID = doc.TableID
            //                                                  LEFT JOIN info_OrganizationsSettlementAccount orgset ON orgset.ID = doc.OrganizationsSettlementAccountID


            //                                                  WHERE OwnerID=@ID",
            //                                                  new string[] { "@ID" },
            //                                                  new object[] { ID }).ToList();
            //organizationDocSettAccount.ForEach((table4) =>
            //{
            //    var orgDocSettAccount = new Organization.OrganizationsDocSettAccount()
            //    {
            //        ID = table4.ID,
            //        OwnerID = table4.OwnerID,
            //        TableID = table4.TableID,
            //        TableName = table4.TableName,
            //        OrganizationID = table4.OrganizationID,
            //        OrganizationName = table4.OrganizationName,
            //        OrganizationsSettlementAccountID = table4.OrganizationsSettlementAccountID != 0 ? table4.OrganizationsSettlementAccountID : null,
            //        OrganizationsSettlementAccountCode = table4.OrganizationsSettlementAccountCode,
            //        OrganizationFunctionalItemID = table4.OrganizationFunctionalItemID,
            //        OrganizationFunctionalItemCode = table4.OrganizationFunctionalItemCode,
            //        SettleCodeLevel = table4.SettleCodeLevel,
            //        StateID = table4.StateID,
            //        State = table4.StateID == 1 ? "Актив" : "Пассив",
            //        Status = 0
            //    };
            //    Organization.DocSettAccount.Add(orgDocSettAccount);
            //});


            return Organization;
        }

        public dynamic GetAdminNeighborhoodRecalc(int ID)
        {
            if (ID == 0)
                ID = OrganizationID;
            return _databaseExt.GetFirstDataFromSql("SELECT IsRecalcNeed,EndDate Date FROM WB_sys_Should_ReCalc_Org WHERE OrganizationID=@OrganizationID", new string[] { "@OrganizationID" }, new object[] { ID });
        }

        public dynamic GetAllNeighborhoodForIndicator()
        {
            string sql = "SELECT ";
            sql += " ID,";
            sql += " Name";
            sql += @" From info_Organization WHERE ID in (SELECT OrganizationID FROM info_MinistryOrganization Where StateID=1)";

            var data = _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { }).ToList();
            return data;
        }

        public Organization UpdateAdminNeighborhood(Organization organization)
        {
            if (!UserIsInRole("OrganizationEdit"))
                throw new Exception("Нет доступа.");

            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    CheckNeighborhood(organization.INN);
                    CheckNeighborhoodINN(organization.INN, organization.ID);

                    string sql = "";

                    if (organization.ID > 0)
                    {

                        var oldinfo = GetOrganization(organization.ID);
                        if (oldinfo.Name != organization.Name)
                            LogDataHistory(InfoStruct.Organization, organization.ID, "Name", organization.Name, OrganizationID, UserID);

                        //if (oldinfo.Director != organization.Director)
                        //    LogDataHistory(InfoStruct.Organization, organization.ID, "Director", organization.Director, OrganizationID, UserID);

                        // if (oldinfo.Accounter != organization.Accounter)
                        //     LogDataHistory(InfoStruct.Organization, organization.ID, "Accounter", organization.Accounter, OrganizationID, UserID);
                        //
                        // if (oldinfo.INN != organization.INN)
                        //   LogDataHistory(InfoStruct.Organization, organization.ID, "INN", organization.INN, OrganizationID, UserID);
                    }

                    if (organization.ID == 0)
                    {

                        sql = "SELECT INN FROM info_Organization WHERE Name=@Name AND FullName=@FullName";
                        var inn = _databaseExt.ExecuteScalar(sql,
                            new string[] { "@Name", "@FullName" },
                            new object[] { organization.Name.Trim(), organization.FullName.Trim() }, System.Data.CommandType.Text);
                        if (inn != null)
                            throw new Exception("Организация с этим " + organization.FullName + " уже существует.");

                        organization.StateID = 1;
                        organization.IsFullBudget = true;

                        sql = "INSERT INTO [info_Organization] (Name,FullName,INN,OKONHID,FinancingLevelID,TreasuryBranchID,TreasuryDepartmentHeader,TreasuryResPerson,OblastID,RegionID,ZipCode,Adress,ContactInfo,Director,Accounter,Cashier,IncomeNumber,IncomeDate,CreatedUserID,OrganizationTypeID,StateID,IsFullBudget,HeaderOrganizationID,ChapterID,CentralOrganizationID,CentralOrganizationName,BRParentOranizationID,HeaderStaffListOrganizationID) OUTPUT INSERTED.ID VALUES (@Name,@FullName,@INN,@OKONHID,@FinancingLevelID,@TreasuryBranchID,@TreasuryDepartmentHeader,@TreasuryResPerson,@OblastID,@RegionID,@ZipCode,@Adress,@ContactInfo,@Director,@Accounter,@Cashier,@IncomeNumber,@IncomeDate,@CreatedUserID,@OrganizationTypeID,@StateID,@IsFullBudget,@HeaderOrganizationID,@ChapterID,@CentralOrganizationID,@CentralOrganizationName,@BRParentOranizationID,@HeaderStaffListOrganizationID)";
                        var NewID = _databaseExt.ExecuteScalar(sql,
                            new string[] { "@Name", "@FullName", "@INN", "@OKONHID", "@FinancingLevelID", "@TreasuryBranchID", "@TreasuryDepartmentHeader", "@TreasuryResPerson", "@OblastID", "@RegionID", "@ZipCode", "@Adress", "@ContactInfo", "@Director", "@Accounter", "@Cashier", "@IncomeNumber", "@IncomeDate", "@CreatedUserID", "@OrganizationTypeID", "@StateID", "@IsFullBudget", "@HeaderOrganizationID", "@ChapterID", "@CentralOrganizationID", "@CentralOrganizationName", "@BRParentOranizationID", "@HeaderStaffListOrganizationID" },
                            new object[] { organization.Name, organization.FullName, organization.INN, organization.OKONHID, organization.FinancingLevelID, organization.TreasuryBranchID, organization.TreasuryDepartmentHeader, organization.TreasuryResPerson, organization.OblastID, organization.RegionID, organization.ZipCode, organization.Adress, organization.ContactInfo, organization.Director, organization.Accounter, organization.Cashier, organization.IncomeNumber, organization.IncomeDate, UserID, organization.OrganizationTypeID, organization.StateID, organization.IsFullBudget, organization.HeaderOrganizationID, organization.ChapterID, organization.CentralOrganizationID, organization.CentralOrganizationName, organization.BRParentOranizationID, organization.HeaderStaffListOrganizationID }, System.Data.CommandType.Text, ts);
                        organization.ID = Convert.ToInt32(NewID);

                        sql = "INSERT INTO hl_SubAcc(Code, Name, IsCurrency, AccID, OrganizationID, CreatedUserID) SELECT Code, Name, IsCurrency, ID,@OrganizationID,@UserID FROM info_Acc";
                        _databaseExt.ExecuteScalar(sql,
                            new string[] { "@OrganizationID", "UserID" },
                            new object[] { organization.ID, UserID }, System.Data.CommandType.Text, ts);

                        List<MoneyMovement> moneyList = new List<MoneyMovement>();
                        MoneyMovement moneyName = new MoneyMovement()
                        {
                            Name = "Поступление",
                            Code = 1,
                            MoneyMeansMovementsKindID = 3
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Расход денежных средств",
                            Code = 3,
                            MoneyMeansMovementsKindID = 2
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Возврат",
                            Code = 4,
                            MoneyMeansMovementsKindID = 1
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Экономия денежных средств",
                            Code = 5,
                            MoneyMeansMovementsKindID = 5
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Централизованная передача",
                            Code = 6,
                            MoneyMeansMovementsKindID = 6
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Возврат Прошлый год",
                            Code = 7,
                            MoneyMeansMovementsKindID = 7
                        };
                        moneyList.Add(moneyName);

                        moneyName = new MoneyMovement()
                        {
                            Name = "Поступления прошлого года",
                            Code = 8,
                            MoneyMeansMovementsKindID = 8
                        };
                        moneyList.Add(moneyName);

                        for (int i = 0; i < moneyList.Count(); i++)
                        {
                            sql = "INSERT INTO info_MoneyMeansMovement(Name, Code, MoneyMeansMovementsKindID, OrganizationID, CreatedUserID) VALUES (@Name, @Code, @MoneyMeansMovementsKindID, @OrganizationID, @CreatedUserID)";
                            _databaseExt.ExecuteScalar(sql,
                                new string[] { "@Name", "@Code", "@MoneyMeansMovementsKindID", "@OrganizationID", "@CreatedUserID" },
                                new object[] { moneyList[i].Name, moneyList[i].Code, moneyList[i].MoneyMeansMovementsKindID, organization.ID, UserID }, System.Data.CommandType.Text, ts);
                        }
                    }
                    else
                    {
                        sql = "UPDATE [info_Organization] SET [Name]=@Name, [FullName]=@FullName, [INN]=@INN, [OKONHID]=@OKONHID, [FinancingLevelID]=@FinancingLevelID, [TreasuryBranchID]=@TreasuryBranchID, [TreasuryDepartmentHeader]=@TreasuryDepartmentHeader, [OblastID]=@OblastID, [RegionID]=@RegionID, [ZipCode]=@ZipCode, [Adress]=@Adress, [ContactInfo]=@ContactInfo, [Director]=@Director, [Accounter]=@Accounter, [Cashier]=@Cashier, [IncomeNumber]=@IncomeNumber, [IncomeDate]=@IncomeDate, [DateOfModified]=GETDATE(), [OrganizationTypeID]=@OrganizationTypeID, [HeaderOrganizationID]=@HeaderOrganizationID, [ChapterID]=@ChapterID, [CentralOrganizationID]=@CentralOrganizationID, [CentralOrganizationName]=@CentralOrganizationName, [BRParentOranizationID]=@BRParentOranizationID, [HeaderStaffListOrganizationID]=@HeaderStaffListOrganizationID, [TreasuryResPerson]=@TreasuryResPerson,[StateID]=@StateID WHERE ID=@ID ";
                        _databaseExt.ExecuteNonQuery(sql,
                            new string[] { "@Name", "@FullName", "@INN", "@OKONHID", "@FinancingLevelID", "@TreasuryBranchID", "@TreasuryDepartmentHeader", "@OblastID", "@RegionID", "@ZipCode", "@Adress", "@ContactInfo", "@Director", "Accounter", "@Cashier", "@IncomeNumber", "@IncomeDate", "@OrganizationTypeID", "@HeaderOrganizationID", "@ChapterID", "@CentralOrganizationID", "@CentralOrganizationName", "@HeaderStaffListOrganizationID", "@BRParentOranizationID", "@TreasuryResPerson", "@StateID", "@ID" },
                            new object[] { organization.Name, organization.FullName, organization.INN, organization.OKONHID, organization.FinancingLevelID, organization.TreasuryBranchID, organization.TreasuryDepartmentHeader, organization.OblastID, organization.RegionID, organization.ZipCode, organization.Adress, organization.ContactInfo, organization.Director, organization.Accounter, organization.Cashier, organization.IncomeNumber, organization.IncomeDate, organization.OrganizationTypeID, organization.HeaderOrganizationID, organization.ChapterID, organization.CentralOrganizationID > 0 ? organization.CentralOrganizationID : null, organization.CentralOrganizationName, organization.HeaderStaffListOrganizationID, organization.BRParentOranizationID > 0 ? organization.BRParentOranizationID : null, organization.TreasuryResPerson, organization.StateID, organization.ID }, System.Data.CommandType.Text, ts);
                    }

                    organization.SettlementAccount.ForEach((table) =>
                    {
                        if (table.Status == 3 && table.ID > 0)
                        {
                            sql = "DELETE FROM [info_OrganizationsSettlementAccount] WHERE ID=@ID";
                            _databaseExt.ExecuteNonQuery(sql,
                            new string[] { "@ID" },
                            new object[] { table.ID }, System.Data.CommandType.Text, ts);
                        }
                        else if (table.Status != 0)
                        {
                            if (!(table.Code.Length == 20 || table.Code.Length == 27 || table.Code.Length == 25))
                                throw new Exception("Количество символов в счете должно составлять 20, 25 или 27 символов");
                            string sql = "SELECT INN FROM info_Organization WHERE ID=@OrganizationID";
                            var inn = _databaseExt.ExecuteScalar(sql,
                                new string[] { "@OrganizationID" },
                                new object[] { table.OrganizationID }, System.Data.CommandType.Text, ts);

                            sql = "SELECT SettlementAccountCode FROM treas_Organization WHERE INN=@INN AND State='A'";
                            var checkexists = _databaseExt.GetDataFromSql(sql,
                                new string[] { "@INN" },
                                new object[] { inn }, System.Data.CommandType.Text, ts).ToList();
                            string[] checkExists = new string[checkexists.Count()];
                            for (int i = 0; i < checkexists.Count(); i++)
                            {
                                checkExists[i] = Convert.ToString(checkexists[i].SettlementAccountCode);
                            }
                            if (!checkExists.Contains(table.Code) && (organization.INN != inn.ToString()))
                                throw new Exception("Неправильные ИНН, Л/С " + table.Code);
                            if (table.Code.Length == 20) // 20 talik x/r i borligini 2 marta kiritmaslik uchun tekshirish
                            {
                                sql = "SELECT ID FROM info_OrganizationsSettlementAccount WHERE ID<>@ID AND Code=@Code AND BankID=@BankID";
                                var code = _databaseExt.ExecuteScalar(sql,
                                    new string[] { "@ID", "@Code", "@BankID" },
                                    new object[] { table.ID, table.Code, table.BankID }, System.Data.CommandType.Text, ts);
                                if (code != null)
                                    throw new Exception("Р/С с этим кодом (" + table.Code + ") уже существует.");
                            }

                            if (table.Status == 1)
                            {
                                sql = "SELECT ID FROM [info_OrganizationsSettlementAccount] WHERE StateID=1 and Code=@Code";
                                var checkaccount = _databaseExt.ExecuteScalar(sql,
                                    new string[] { "@Code" },
                                    new object[] { table.Code }, System.Data.CommandType.Text, ts);
                                if (checkaccount != null && checkaccount != DBNull.Value)
                                    throw new Exception("Эти " + table.Code + " данные по КЛС имеются в базе ПК UzASBO.  Ушбу " + table.Code + " ШҒҲ UzASBO ДМ маълумотлар базасида мавжуд.");

                                table.StateID = 1;
                                sql = "INSERT INTO [info_OrganizationsSettlementAccount] ([Name],[Code],[BankID],[OrganizationFunctionalItemID],[TreasuryBranchID],[IsActive],[StateID],[OrganizationID],[CreatedUserID], [OutOfBalance]) VALUES (@Name,@Code,@BankID,@OrganizationFunctionalItemID,@TreasuryBranchID,@IsActive,@StateID,@OrganizationID,@CreatedUserID,@OutOfBalance)";
                                _databaseExt.ExecuteNonQuery(sql,
                                    new string[] { "@Name", "@Code", "@BankID", "@OrganizationFunctionalItemID", "@TreasuryBranchID", "@IsActive", "@StateID", "@OrganizationID", "@CreatedUserID", "@OutOfBalance" },
                                    new object[] { table.Name, table.Code, table.BankID, table.OrganizationFunctionalItemID, table.TreasuryBranchID, table.IsActive, table.StateID, table.OrganizationID, UserID, table.OutOfBalance }, System.Data.CommandType.Text, ts);
                            }
                            else if (table.Status == 2)
                            {
                                if (!UserIsInRole("DepartmentOfBudget") && !(table.Code.StartsWith("20") || table.Code.StartsWith("30") || table.Code.StartsWith("39")))
                                {
                                    sql = "SELECT OutOfBalance FROM info_OrganizationsSettlementAccount WHERE ID=@ID AND OutOfBalance = 0 ";
                                    var x = Convert.ToString(_databaseExt.ExecuteScalar(sql,
                                        new string[] { "@ID" },
                                        new object[] { table.ID }, System.Data.CommandType.Text, ts));
                                    if (x != "True" && !string.IsNullOrEmpty(x) && table.OutOfBalance != null && table.OutOfBalance == true)
                                        throw new Exception("Нет доступа к изменению внебаланса");
                                }

                                if (!UserIsInRole("DepartmentOfBudget") && !(table.Code.StartsWith("20") || table.Code.StartsWith("30")))
                                {
                                    sql = "SELECT OutOfBalance FROM info_OrganizationsSettlementAccount WHERE ID=@ID AND OutOfBalance = 1 ";
                                    var x = Convert.ToString(_databaseExt.ExecuteScalar(sql,
                                        new string[] { "@ID" },
                                        new object[] { table.ID }, System.Data.CommandType.Text, ts));
                                    if (x != "False" && !string.IsNullOrEmpty(x) && table.OutOfBalance != null && table.OutOfBalance == false)
                                        throw new Exception("Нет доступа к изменению внебаланса");
                                }
                                if (organization.ID != table.OrganizationID)
                                {

                                    var FuncID = Convert.ToInt32(_databaseExt.GetDataFromSql(@"select ID from info_OrganizationsFunctionalItem 
                                                                        where OrganizationID=@OrganizationID and Code=@Code", new string[] { "@OrganizationID", "@Code" }, new object[] { table.OrganizationID, table.OrganizationFunctionalItemCode }).Select(x => x.ID).FirstOrDefault());



                                    sql = "UPDATE [info_OrganizationsSettlementAccount] SET [Name]=@Name, [BankID]=@BankID, [OrganizationFunctionalItemID]=@OrganizationFunctionalItemID, [TreasuryBranchID]=@TreasuryBranchID, [IsActive]=@IsActive, [StateID]=@StateID, [OrganizationID]=@OrganizationID, [OutOfBalance]=@OutOfBalance, [CashSubAccID]=null, [ActualSubAccID]=null,[DateOfModified]=getdate() WHERE ID=@ID";
                                    _databaseExt.ExecuteNonQuery(sql,
                                        new string[] { "@Name", "@Code", "@BankID", "@OrganizationFunctionalItemID", "@TreasuryBranchID", "@IsActive", "@StateID", "@OrganizationID", "@OutOfBalance", "@ID" },
                                        new object[] { table.Name, table.Code, table.BankID, FuncID == 0 ? null : FuncID, table.TreasuryBranchID, table.IsActive, table.StateID, table.OrganizationID, table.OutOfBalance, table.ID }, System.Data.CommandType.Text, ts);

                                }
                                else
                                {
                                    sql = "UPDATE [info_OrganizationsSettlementAccount] SET [Name]=@Name, [BankID]=@BankID, [OrganizationFunctionalItemID]=@OrganizationFunctionalItemID, [TreasuryBranchID]=@TreasuryBranchID, [IsActive]=@IsActive, [StateID]=@StateID, [OrganizationID]=@OrganizationID, [OutOfBalance]=@OutOfBalance,[DateOfModified]=getdate() WHERE ID=@ID";
                                    _databaseExt.ExecuteNonQuery(sql,
                                        new string[] { "@Name", "@Code", "@BankID", "@OrganizationFunctionalItemID", "@TreasuryBranchID", "@IsActive", "@StateID", "@OrganizationID", "@OutOfBalance", "@ID" },
                                        new object[] { table.Name, table.Code, table.BankID, table.OrganizationFunctionalItemID, table.TreasuryBranchID, table.IsActive, table.StateID, table.OrganizationID, table.OutOfBalance, table.ID }, System.Data.CommandType.Text, ts);
                                }
                            }
                        }
                    });

                    organization.FunctionalItem.ForEach((table) =>
                    {
                        if (table.Status == 3 && table.ID > 0)
                        {
                            sql = "DELETE FROM [info_OrganizationsFunctionalItem] WHERE ID=@ID";
                            _databaseExt.ExecuteNonQuery(sql,
                            new string[] { "@ID" },
                            new object[] { table.ID }, System.Data.CommandType.Text, ts);
                        }
                        else if (table.Status != 0)
                        {
                            sql = "SELECT Code FROM [info_Chapter] WHERE StateID=1 and ID=@ChapterID";
                            var chapterCode = _databaseExt.ExecuteScalar(sql,
                                new string[] { "@ChapterID" },
                                new object[] { table.ChapterID }, System.Data.CommandType.Text, ts);


                            if (((table.FunctionalItemCode).Substring(0, 7) + (chapterCode)) != table.Code)
                                throw new Exception("Код не соответствует к функциональной классификации расходов и глав " + table.FunctionalItemCode);

                            if (table.Status == 1)
                            {
                                sql = "INSERT INTO info_OrganizationsFunctionalItem (Code,FunctionalItemID,ChapterID,OrganizationID,CreatedUserID) VALUES (@Code,@FunctionalItemID,@ChapterID,@OrganizationID,@CreatedUserID)";
                                _databaseExt.ExecuteNonQuery(sql,
                                    new string[] { "@Code", "@FunctionalItemID", "@ChapterID", "@OrganizationID", "@CreatedUserID" },
                                    new object[] { table.Code, table.FunctionalItemID, table.ChapterID, table.OrganizationID, UserID }, System.Data.CommandType.Text, ts);
                            }
                            else if (table.Status == 2)
                            {
                                sql = "UPDATE info_OrganizationsFunctionalItem Set Code=@Code,FunctionalItemID=@FunctionalItemID,ChapterID=@ChapterID,DateOfModified=GETDATE() WHERE ID=@ID";
                                _databaseExt.ExecuteNonQuery(sql,
                                    new string[] { "@Code", "@FunctionalItemID", "@ChapterID", "@ID" },
                                    new object[] { table.Code, table.FunctionalItemID, table.ChapterID, table.ID }, System.Data.CommandType.Text, ts);
                            }
                        }
                    });

                    organization.Sign.ForEach((table) =>
                    {
                        if (table.Status == 3 && table.ID > 0)
                        {
                            sql = "UPDATE [info_OrganizationSign] set StateID=2 WHERE ID=@ID";
                            _databaseExt.ExecuteNonQuery(sql,
                            new string[] { "@ID" },
                            new object[] { table.ID }, System.Data.CommandType.Text, ts);
                        }
                        else
                        {
                            if (table.Status == 1)
                            {
                                table.StateID = 1;
                                sql = "INSERT INTO info_OrganizationSign (FIO,SignNumber,StateID,OrganizationID,CreatedUserID,PositionNameRus,PositionNameUzb) VALUES (@FIO,@SignNumber,@StateID,@OrganizationID,@CreatedUserID,@PositionNameRus,@PositionNameUzb)";
                                _databaseExt.ExecuteNonQuery(sql,
                                    new string[] { "@FIO", "@SignNumber", "@StateID", "@OrganizationID", "@CreatedUserID", "@PositionNameRus", "@PositionNameUzb" },
                                    new object[] { table.FIO, table.SignNumber, table.StateID, table.OrganizationID, UserID, table.PositionNameRus, table.PositionNameUzb }, System.Data.CommandType.Text, ts);
                            }
                            else if (table.Status == 2)
                            {
                                sql = "UPDATE info_OrganizationSign Set FIO=@FIO,SignNumber=@SignNumber,StateID=@StateID,DateOfModified=GETDATE(),PositionNameRus=@PositionNameRus,PositionNameUzb=@PositionNameUzb WHERE ID=@ID";
                                _databaseExt.ExecuteNonQuery(sql, new string[] { "@FIO", "@SignNumber", "@StateID", "@PositionNameRus", "@PositionNameUzb", "@ID" }, new object[] { table.FIO, table.SignNumber, 1, table.PositionNameRus, table.PositionNameUzb, table.ID }, System.Data.CommandType.Text, ts);
                            }
                        }
                    });


                    //organization.DocSettAccount.ForEach((table) =>
                    //{
                    //    //string sql = "SELECT OrganizationsSettlementAccountID FROM info_OrganizationsDocSettAccount WHERE TableID=@TableID AND ID<>@ID";
                    //    //string samesetaccount = _databaseExt.ExecuteScalar(sql,
                    //    //    new string[] { "@TableID","@ID" },
                    //    //    new object[] { table.TableID,table.ID }, System.Data.CommandType.Text, ts).ToString();
                    //    //if (samesetaccount != null)
                    //    //    throw new Exception("Этот Л/С уже задействован в документе (" + table.TableName + "), в организации (" + table.OwnerID + "). ");




                    //    if (table.Status == 3 && table.ID > 0)
                    //    {
                    //        sql = "DELETE FROM [info_OrganizationsDocSettAccount] WHERE ID=@ID";
                    //        _databaseExt.ExecuteNonQuery(sql,
                    //        new string[] { "@ID" },
                    //        new object[] { table.ID }, System.Data.CommandType.Text, ts);
                    //    }
                    //    else
                    //    {
                    //        if (table.Status == 1)
                    //        {
                    //            sql = "INSERT INTO info_OrganizationsDocSettAccount (OwnerID,TableID,OrganizationID,OrganizationsSettlementAccountID,CreatedUserID,DateOfCreated,OrganizationFunctionalItemID,OrganizationFunctionalItemCode,SettleCodeLevel,StateID) VALUES (@OwnerID,@TableID,@OrganizationID,@OrganizationsSettlementAccountID,@CreatedUserID,GETDATE(),@OrganizationFunctionalItemID,@OrganizationFunctionalItemCode,@SettleCodeLevel,@StateID)";
                    //            _databaseExt.ExecuteNonQuery(sql,
                    //                new string[] { "@OwnerID", "@TableID", "@OrganizationID", "@OrganizationsSettlementAccountID", "@CreatedUserID", "@OrganizationFunctionalItemID", "@OrganizationFunctionalItemCode", "@SettleCodeLevel", "@StateID" },
                    //                new object[] { table.OwnerID, table.TableID, table.OrganizationID, table.OrganizationsSettlementAccountID, UserID, table.OrganizationFunctionalItemID, table.OrganizationFunctionalItemCode, table.SettleCodeLevel, table.StateID }, System.Data.CommandType.Text, ts);
                    //        }
                    //        else if (table.Status == 2)
                    //        {
                    //            sql = "UPDATE info_OrganizationsDocSettAccount Set TableID=@TableID,OrganizationID=@OrganizationID,OrganizationsSettlementAccountID=@OrganizationsSettlementAccountID,ModifiedUserID=@ModifiedUserID,DateOfModified=GETDATE(),OrganizationFunctionalItemID=@OrganizationFunctionalItemID,OrganizationFunctionalItemCode=@OrganizationFunctionalItemCode,SettleCodeLevel=@SettleCodeLevel,StateID=@StateID WHERE ID=@ID";
                    //            _databaseExt.ExecuteNonQuery(sql,
                    //                new string[] { "@TableID", "@OrganizationID", "@OrganizationsSettlementAccountID", "@ModifiedUserID", "@OrganizationFunctionalItemID", "@OrganizationFunctionalItemCode", "@SettleCodeLevel", "@StateID", "@ID" },
                    //                new object[] { table.TableID, table.OrganizationID, table.OrganizationsSettlementAccountID, UserID, table.OrganizationFunctionalItemID, table.OrganizationFunctionalItemCode, table.SettleCodeLevel, table.StateID, table.ID }, System.Data.CommandType.Text, ts);
                    //        }
                    //    }
                    //});
                    ts.Commit();
                }
            }

            return organization;
        }

        public void FillNeighborhoodSettings(int organizationID, bool ReFill)
        {
            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {

                    string sql = "SELECT ID FROM info_OrganizationsFunctionalItem  WHERE OrganizationID=@OrganizationID";
                    var oldOrgFunctionalItem = _databaseExt.GetDataFromSql(sql,
                        new string[] { "@OrganizationID" },
                        new object[] { organizationID }, System.Data.CommandType.Text, ts).ToList();

                    if (!oldOrgFunctionalItem.Any() || ReFill)
                    {
                        var INN = GetOrganization(organizationID).INN;

                        sql = "SELECT org.SettlementAccountCode FROM treas_Organization org  WHERE org.INN = @INN AND (org.State = 'A' OR org.State = 'O')";
                        var Accs = _databaseExt.GetDataFromSql(sql,
                            new string[] { "@INN" },
                            new object[] { INN }, System.Data.CommandType.Text, ts).ToList();


                        Accs.ForEach(x =>
                        {
                            try
                            {
                                string FunctionalItem = x.SettlementAccountCode.Substring(14, 7);
                                string Chapter = x.SettlementAccountCode.Substring(21, 3);
                                string Funct = x.SettlementAccountCode.Substring(14, 10);

                                sql = "SELECT fnc.ID FROM info_FunctionalItemOfExpense fnc  WHERE fnc.Code=@FunctionalItem";
                                var FunctionalItemOfExpense = _databaseExt.GetDataFromSql(sql,
                                    new string[] { "@FunctionalItem" },
                                    new object[] { FunctionalItem }, System.Data.CommandType.Text, ts).First();

                                sql = "SELECT fnc.ID FROM info_Chapter fnc  WHERE fnc.Code=@Chapter";
                                var ChapterID = _databaseExt.GetDataFromSql(sql,
                                    new string[] { "@Chapter" },
                                    new object[] { Chapter }, System.Data.CommandType.Text, ts).First();


                                sql = "SELECT fnc.ID FROM info_OrganizationsFunctionalItem fnc  WHERE fnc.OrganizationID=@OrganizationID AND fnc.Code = @Funct";
                                var OrganizationsFunctionalItem = _databaseExt.GetDataFromSql(sql,
                                    new string[] { "@OrganizationID", "@Funct" },
                                    new object[] { organizationID, Funct }, System.Data.CommandType.Text, ts).ToList();

                                if (!OrganizationsFunctionalItem.Any())
                                {
                                    sql = "INSERT INTO [info_OrganizationsFunctionalItem] (Code,FunctionalItemID,ChapterID,OrganizationID,CreatedUserID) VALUES (@Code,@FunctionalItemID,@ChapterID,@OrganizationID,@CreatedUserID)";
                                    _databaseExt.ExecuteScalar(sql,
                                        new string[] { "@Code", "@FunctionalItemID", "@ChapterID", "@OrganizationID", "@CreatedUserID" },
                                        new object[] { x.SettlementAccountCode.Substring(14, 10), FunctionalItemOfExpense.ID, ChapterID.ID, organizationID, UserID }, System.Data.CommandType.Text, ts);
                                }
                            }
                            catch { }
                        });
                    }
                    sql = "SELECT ID FROM info_OrganizationsSettlementAccount  WHERE OrganizationID=@OrganizationID";
                    var OrganizationsSettlementAccount = _databaseExt.GetDataFromSql(sql,
                        new string[] { "@OrganizationID" },
                        new object[] { organizationID }, System.Data.CommandType.Text, ts).ToList();

                    if (!OrganizationsSettlementAccount.Any() || ReFill)
                    {
                        var INN = GetOrganization(organizationID).INN;

                        sql = "SELECT org.SettlementAccountCode Code,org.TreasuryBranchCode TreasuryBranch FROM treas_Organization org  WHERE org.INN = @INN AND (org.State = 'A' OR org.State = 'O')";
                        var Accs = _databaseExt.GetDataFromSql(sql,
                            new string[] { "@INN" },
                            new object[] { INN }, System.Data.CommandType.Text, ts).ToList();

                        Accs.ForEach(x =>
                        {
                            try
                            {
                                sql = "SELECT bd.ID FROM info_OrganizationsSettlementAccount bd  WHERE bd.OrganizationID = @OrganizationID AND bd.Code = @Code";
                                var OrganizationsSettlementAccount = _databaseExt.GetDataFromSql(sql,
                                    new string[] { "@OrganizationID", "@Code" },
                                    new object[] { organizationID, x.Code }, System.Data.CommandType.Text, ts).ToList();

                                if (!OrganizationsSettlementAccount.Any())
                                {
                                    string Name = "Бюджет";

                                    switch (x.Code.Substring(0, 4))
                                    {
                                        case "4005":
                                        case "4011":
                                        case "4010":
                                        case "4008":
                                        case "4014":
                                        case "4015":
                                        case "4999":
                                            Name = "Спец.Счет";
                                            break;
                                        case "4002":
                                            Name = "Пенсия";
                                            break;
                                        case "4001":
                                        case "4003":
                                            Name = "Фонд.Разв";
                                            break;
                                        case "4007":
                                            Name = "Пит.Сотр";
                                            break;
                                    }

                                    string FunctionalItem = x.Code.Substring(14, 10);

                                    sql = "SELECT ID,BankID FROM info_TreasuryBranch  WHERE Code=@TreasuryBranch";
                                    var TreasuryBranch = _databaseExt.GetDataFromSql(sql,
                                        new string[] { "@TreasuryBranch" },
                                        new object[] { x.TreasuryBranch }, System.Data.CommandType.Text, ts).First();

                                    sql = "SELECT ID FROM info_OrganizationsFunctionalItem  WHERE Code=@FunctionalItem AND OrganizationID=@OrganizationID";
                                    var OrganizationsFunctionalItem = _databaseExt.GetDataFromSql(sql,
                                        new string[] { "@FunctionalItem", "@OrganizationID" },
                                        new object[] { FunctionalItem, organizationID }, System.Data.CommandType.Text, ts).First();

                                    sql = "SELECT ID FROM [info_OrganizationsSettlementAccount] WHERE StateID=1 and Code=@Code";
                                    var checkaccount = _databaseExt.ExecuteScalar(sql,
                                        new string[] { "@Code" },
                                        new object[] { x.Code }, System.Data.CommandType.Text, ts);
                                    if (checkaccount != null && checkaccount != DBNull.Value)
                                        throw new Exception("Эти " + x.Code + " данные по КЛС имеются в базе ПК UzASBO.  Ушбу " + x.Code + " ШҒҲ UzASBO ДМ маълумотлар базасида мавжуд.");


                                    sql = "INSERT INTO [info_OrganizationsSettlementAccount] (Name,Code,BankID,OrganizationFunctionalItemID,TreasuryBranchID,StateID,OrganizationID,CreatedUserID) VALUES (@Name,@Code,@BankID,@OrganizationFunctionalItemID,@TreasuryBranchID,@StateID,@OrganizationID,@CreatedUserID)";
                                    _databaseExt.ExecuteScalar(sql,
                                        new string[] { "@Name", "@Code", "@BankID", "@OrganizationFunctionalItemID", "@TreasuryBranchID", "@StateID", "@OrganizationID", "@CreatedUserID" },
                                        new object[] { Name, x.Code, TreasuryBranch.BankID, OrganizationsFunctionalItem.ID, TreasuryBranch.ID, 1, organizationID, UserID }, System.Data.CommandType.Text, ts);


                                    sql = "SELECT ID FROM info_OrganizationsSettlementAccount  WHERE Code=@Code AND OrganizationID = @OrganizationID";
                                    var Settlement = _databaseExt.GetDataFromSql(sql,
                                        new string[] { "@Code", "@OrganizationID" },
                                        new object[] { x.Code, organizationID }, System.Data.CommandType.Text, ts).First();

                                    //int? HeaderOrganizationID = GetOrganization(organizationID).HeaderOrganizationID;

                                    //if (HeaderOrganizationID.HasValue)
                                    //{
                                    //    sql = "INSERT INTO [info_HeaderOrgSettlement] (HeaderOrganizationID,OrganizationsSettlementAccountID,StateID,CreatedUserID,ModifiedUserID,DateOfCreated) VALUES (@HeaderOrganizationID,@OrganizationsSettlementAccountID,@StateID,@CreatedUserID,@ModifiedUserID,@DateOfCreated)";
                                    //    _databaseExt.ExecuteScalar(sql,
                                    //        new string[] { "@HeaderOrganizationID", "@OrganizationsSettlementAccountID", "@StateID", "@CreatedUserID", "@ModifiedUserID", "@DateOfCreated" },
                                    //        new object[] { HeaderOrganizationID.Value, Settlement.ID, Status.Created, UserID, UserID, DateTime.Now }, System.Data.CommandType.Text, ts);
                                    //}
                                }

                            }
                            catch { }
                        });
                    }
                    ts.Commit();
                }
            }
        }

        public void RecalcAccAccountBookNeighborhood(int id, DateTime BeginDate, DateTime EndDate)
        {
            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    string sql = "WB_ReCalc_acc_AccountBook";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@StartDate", "@EndDate", "@OrganizationID" },
                        new object[] { BeginDate, EndDate, id, }, System.Data.CommandType.StoredProcedure, ts);

                    ts.Commit();
                }
            }
        }

        public void RestrictionNeighborhood(int id)
        {
            if (!(UserIsInRole("DepartmentOfBudget") || UserIsInRole("FinancialAuthority")))
                throw new Exception("Нет доступа.");

            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();

            using var ts = myConn.BeginTransaction();
            string sql = "SELECT Restriction FROM info_Organization where ID = @ID";
            var flag = Convert.ToInt32(_databaseExt.ExecuteScalar(sql,
                new string[] { "@ID" },
                new object[] { id }, System.Data.CommandType.Text, ts));
            if (UserID == 23071)
            {
                sql = "UPDATE [info_Organization] SET  Restriction = CASE WHEN @flag = 0 THEN 1 ELSE 0 END, RestrictionModifiedUserID=@RestrictionModifiedUserID WHERE ID=@ID ";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@ID", "@flag", "@RestrictionModifiedUserID" },
                    new object[] { id, flag, UserID }, System.Data.CommandType.Text, ts);
            }
            else
            {
                if (_databaseExt.GetDataFromSql(@"select bd.Restriction from info_Organization bd where bd.ID=@ID AND bd.Restriction=1 AND bd.RestrictionModifiedUserID is not null",
                    new string[] { "@ID" }, new object[] { id }).Any())
                {
                    sql = "UPDATE [info_Organization] SET  Restriction = 0, RestrictionModifiedUserID=@RestrictionModifiedUserID " +
                   "WHERE ID=@ID AND RestrictionModifiedUserID=@RestrictionModifiedUserID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@ID", "@RestrictionModifiedUserID" },
                        new object[] { id, UserID }, System.Data.CommandType.Text, ts);
                }
                else if (_databaseExt.GetDataFromSql(@"select bd.Restriction from info_Organization bd where bd.ID=@ID AND bd.Restriction=0 AND bd.RestrictionModifiedUserID is not null",
                    new string[] { "@ID" }, new object[] { id }).Any())
                {
                    sql = "UPDATE [info_Organization] SET  Restriction = 1, RestrictionModifiedUserID=@RestrictionModifiedUserID " +
                   "WHERE ID=@ID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@ID", "@RestrictionModifiedUserID" },
                        new object[] { id, UserID }, System.Data.CommandType.Text, ts);
                }
                else
                    sql = "UPDATE [info_Organization] SET  Restriction = CASE WHEN @flag = 0 THEN 1 ELSE 0 END, RestrictionModifiedUserID=@RestrictionModifiedUserID " +
                        "WHERE ID=@ID AND Restriction=0 AND RestrictionModifiedUserID is null";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@ID", "@flag", "@RestrictionModifiedUserID" },
                    new object[] { id, flag, UserID }, System.Data.CommandType.Text, ts);
            }
            ts.Commit();
        }

        public PagedDataEx GetAdminNeighborhoodList2(int ID, string INN, string Name, string Oblast, string Region, string HeaderOrganization)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = "SELECT";
            sqlselect += " org.ID,";
            sqlselect += " org.Name,";
            sqlselect += " org.INN,";
            sqlselect += " chap.Code ChapterCode,";
            sqlselect += " fin.DisplayName FinancingLevel,";
            sqlselect += " tre.Name TreasuryBranch,";
            sqlselect += " obl.Name OblastName,";
            sqlselect += " reg.Name RegionName,";
            sqlselect += " ok.Code OKED,";
            sqlselect += " horg.Name HeaderOrganizationName,";
            sqlselect += " centerorg.Name CentralOrganization,";
            sqlselect += " OrganizationType.Name OrganizationType,";
            sqlselect += " CASE WHEN org.StateID = '1' THEN 'Актив' ELSE 'Пассив' END [State],";
            sqlselect += " CASE WHEN org.IsFullBudget = '1' THEN 'Да' ELSE 'Нет' END [IsFullBudget],";
            sqlselect += " CASE WHEN org.Restriction = '1' THEN 'Да' ELSE 'Нет' END [Restriction],";
            sqlselect += " CASE WHEN recalcorg.IsRecalcNeed = 1 THEN 'Да' ELSE 'Нет' END IsRecalcNeed,";
            sqlselect += " recalcorg.EndDate RecalcDate,";
            sqlselect += " org.Director,";
            sqlselect += " org.Accounter,";
            sqlselect += " org.Cashier,";
            sqlselect += " org.ContactInfo,";
            sqlselect += " orgset.ID OrgSetID,";
            sqlselect += " orgset.Code OrgSetCode,";
            sqlselect += " (SELECT Code FROM info_OrganizationsFunctionalItem WHERE ID=orgset.OrganizationFunctionalItemID) [OrganizationFunctionalItemCode],";
            sqlselect += " orgset.StateID, ";
            sqlselect += " orgset.OutOfBalance ";
            string sqlfrom = @" from info_Organization org
                                    left join info_Chapter chap on chap.ID=org.ChapterID
                                    left join enum_FinancingLevel fin on fin.ID=org.FinancingLevelID
                                    left join info_TreasuryBranch tre on tre.ID=org.TreasuryBranchID
                                    join info_Oblast obl on obl.ID=org.OblastID
                                    join info_Region reg on reg.ID=org.RegionID
                                    left join info_OKONH ok on ok.ID=org.OKONHID
                                    LEFT JOIN info_OrganizationHeaderInfo headerorg ON headerorg.OrganizationID=org.ID AND headerorg.EndDate is null
                                    LEFT JOIN info_HeaderOrganization horg ON horg.ID=headerorg.HeaderOrganizationID
                                    LEFT JOIN enum_OrganizationType OrganizationType ON OrganizationType.ID=org.OrganizationTypeID
                                    LEFT JOIN info_Organization centerorg ON centerorg.ID=org.CentralOrganizationID
                                    LEFT JOIN WB_sys_Should_ReCalc_Org recalcorg ON recalcorg.OrganizationID=org.ID
                                    join info_OrganizationsSettlementAccount orgset on orgset.OrganizationID=org.ID ";
            string sqlwhere = " WHERE 1=1";
            string UserRegionID = "";
            switch (UserID)
            {

                case 28877: UserRegionID = "6"; break;//Кошжанов Жаксылык Алимбаевич
                case 27894: UserRegionID = "5"; break;//Паршинцев Н.В.
                case 56: UserRegionID = "10"; break;//Хазраткулов С.
                case 27895: UserRegionID = "8"; break;//Бобохолов Сайфиддин Холмуродович
                case 366: UserRegionID = "0000"; break;//hilola 
                case 29007: UserRegionID = "0000"; break;//Султанов Яшнар Бокижонович
                case 41544: UserRegionID = "0000"; break;//Агевнина Т. Н.
                case 92444: UserRegionID = "0000"; break;//Бегимов С. Ф.
                case 86501: UserRegionID = "4"; break;//Мухамедов Замонжон Замирович
                case 71856: UserRegionID = "11"; break;//Шопиев Зокир
                case 28819: UserRegionID = "14"; break;//Жуманиязов Музаффар
                case 28480: UserRegionID = "7"; break;//Рахимов Сардор Уралович
                case 28458: UserRegionID = "13"; break;//Ахроров Жасурбек Абдутоирович
                case 28151: UserRegionID = "3"; break;//Юсупов Акромжон Акылжонович
                case 28028: UserRegionID = "12"; break;//Курбонов Хамза Холбутаевич
                case 28025: UserRegionID = "0000"; break;//Сидиков Бекзод Бахрамович
                case 26910: UserRegionID = "10"; break;//Шукуров Шерзод Мамарасулович
                case 1583: UserRegionID = "9"; break;//Юлдашев Адхамжон Нурматжонович
                case 1888: UserRegionID = "0000"; break;//Обид Абдукодиров
                case 22412: UserRegionID = "0000"; break;//Фарход Мухамедкаримов
                case 29388: UserRegionID = "0000"; break;//Хасанов А. Э.
                case 29661: UserRegionID = "0000"; break;//Умиров Даврон
                case 41503: UserRegionID = "0000"; break;//Узаков Хасан
                case 91796: UserRegionID = "0000"; break;//Eshpolatov Kamol
                case 92357: UserRegionID = "0000"; break;//Boymanov E
                case 91775: UserRegionID = "0000"; break;//javohir
                case 23071: UserRegionID = "0000"; break;//Тошев Ф.
                default:
                    UserRegionID = "0";
                    break;
            }

            if (!(ID > 0 || !String.IsNullOrEmpty(Name) || !String.IsNullOrEmpty(INN) || !String.IsNullOrEmpty(Oblast) || !String.IsNullOrEmpty(Region) || !String.IsNullOrEmpty(HeaderOrganization)))
            {
                sqlwhere += " AND org.OblastID=@UserRegionID";
                sqlparamas.Add("@UserRegionID", UserRegionID);
            }

            if (ID > 0)
            {
                sqlwhere += " AND org.ID=@ID";
                sqlparamas.Add("@ID", ID);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND org.Name LIKE '%' + @Name + '%'";
                sqlparamas.Add("@Name", Name);
            }
            if (!String.IsNullOrEmpty(INN) && INN.Length == 9)
            {
                sqlwhere += " AND org.INN = @INN";
                sqlparamas.Add("@INN", INN);
            }
            if (!String.IsNullOrEmpty(Oblast))
            {
                sqlwhere += " AND obl.Name LIKE '%' + @Oblast + '%'";
                sqlparamas.Add("@Oblast", Oblast);
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sqlwhere += " AND reg.Name LIKE '%' + @Region + '%'";
                sqlparamas.Add("@Region", Region);
            }
            if (!String.IsNullOrEmpty(HeaderOrganization))
            {
                sqlfrom += ", info_HeaderOrganization hed";
                sqlwhere += " AND org.HeaderOrganizationID=hed.ID AND hed.Name LIKE '%' + @HeaderOrganization + '%'";
                sqlparamas.Add("@HeaderOrganization", HeaderOrganization);
            }


            sqlwhere += " ORDER BY org.ID DESC";


            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            return data;
        }

        private void CheckNeighborhood(string INN)
        {
            if (UserIsInRole("OrganizationDuplicate"))
                return;
            string sql = "SELECT INN FROM treas_Organization WHERE INN=@INN";
            var inn = _databaseExt.ExecuteScalar(sql,
                new string[] { "@INN" },
                new object[] { INN }, System.Data.CommandType.Text);
            if (inn == null)
                throw new Exception("Неправылные ИНН: " + INN);
        }

        private void CheckNeighborhoodINN(string INN, int ID)
        {
            if (UserIsInRole("OrganizationDuplicate"))
                return;
            string sql = "SELECT INN FROM info_Organization WHERE INN=@INN AND ID<>@ID";
            var inn = _databaseExt.ExecuteScalar(sql,
                new string[] { "@INN", "@ID" },
                new object[] { INN, ID }, System.Data.CommandType.Text);
            if (inn != null)
                throw new Exception("Организация с этим ИНН уже существует.");
        }
    }
}
