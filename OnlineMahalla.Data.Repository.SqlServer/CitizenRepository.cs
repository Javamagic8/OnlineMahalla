using Microsoft.Extensions.Options;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
using OnlineMahalla.Data.Model;
using OnlineMahalla.Data.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GeCitizenList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT 
                                        Citizen.ID ID,
                                        Citizen.FullName,
                                        Citizen.PINFL,
                                        Citizen.PhoneNumber,
                                        AcademicDegree.Name AcademicDegree,
                                        AcademicTitle.Name AcademicTitle,
                                        Neighborhood.Name Neighborhood ";

            string sqlfrom = @" FROM hl_Citizen Citizen 
                                     JOIN info_Nation Nationality ON Nationality.ID = Citizen.NationID
                                     JOIN enum_Gender Gender ON Gender.ID = Citizen.GenderID
                                     JOIN enum_Education Education ON Education.ID = Citizen.EducationID
                                     JOIN enum_AcademicTitle AcademicTitle ON AcademicTitle.ID = Citizen.AcademicTitleID
                                     JOIN enum_AcademicDegree AcademicDegree ON AcademicDegree.ID = Citizen.AcademicDegreeID
                                     JOIN enum_Married Married ON Married.ID = Citizen.MarriedID
                                     JOIN enum_CitizenEmployment CitizenEmployment ON CitizenEmployment.ID = Citizen.CitizenEmploymentID
                                     JOIN info_Region Region ON Region.ID = Citizen.BirthRegionID
                                     JOIN info_District District ON District.ID = Citizen.BirthDistrictID
                                     JOIN info_Neighborhood Neighborhood ON Neighborhood.ID = Citizen.NeighborhoodID
                                     JOIN enum_State [State] ON [State].ID = Citizen.StateID";
            string sqlwhere = " WHERE Citizen.NeighborhoodID=@NeighborhoodID ";

            sqlparamas.Add("@NeighborhoodID", NeighborhoodID);
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Citizen.FullName LIKE '%' + @FullName + '%'";
                sqlparamas.Add("@FullName", Name);
            }
            string sqlcount = "SELECT Count(Citizen.ID)" + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Citizen.ID " + (Order == "asc" ? " " : " DESC");

            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
        public Citizen GetCitizen(int id)
        {
            
            var data = _databaseExt.GetDataFromSql("SELECT * FROM hl_Citizen WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }).First();
            Citizen citizen = new Citizen()
            {
             ID = data.ID,
             FirstName = data.FirstName,
             LastName = data.LastName,
             FamilyName = data.FamilyName,
             PINFL = data.PINFL,
             DateOfBirthday = data.DateOfBirthday,
             NationID = data.NationID,
             GenderID = data.GenderID,
             EducationID = data.EducationID,
             AcademicDegreeID = data.AcademicDegreeID,
             AcademicTitleID = data.AcademicTitleID,
             MarriedID = data.MarriedID,
             CountChild = data.CountChild,
             CitizenEmploymentID = data.CitizenEmploymentID,
             IsLowIncome = data.IsLowIncome,
             IsConvicted = data.IsConvicted,
             BirthdayDistrictID = data.BirthdayDistrictID,
             BirthdayRegionID = data.BirthdayRegionID,
             MemberTypeFamilyId = data.MemberTypeFamilyID
            };
            return citizen;
        }
        public void UpdateCitizen(Citizen citizen)
        {
            Employee employee = new Employee();
            if (!UserIsInRole("EmployeeEdit"))
                throw new Exception("Нет доступа.");

            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    //if (employee.INN != "000000000")
                    //{
                    if (employee.INN == "" || employee.INN == null || employee.INN.Length != 14)
                        throw new Exception("Длина PINFL не 14 символов");

                    if (!NumberToWord.IsNumber(employee.INN))
                        throw new Exception("В PINFL не цифровые символы");

                    //if (!CheckModelUtil.CheckINNAlgorithm(employee.INN) /*|| (employee.INN.Substring(0, 1) != "4" && employee.INN.Substring(0, 1) != "5" && employee.INN.Substring(0, 1) != "6")*/)
                    //    throw new Exception("Неправильные ИНН. Зайдите my.soliq.uz раздел: Узнайте свой ИНН (http://my.soliq.uz)");

                    string sql0 = "SELECT ID FROM [hl_Employee] WHERE ID<>@ID AND INN=@INN  AND OrganizationID=@OrganizationID";

                    var q = _databaseExt.GetDataFromSql(sql0,
                        new string[] { "@ID", "@INN", "@OrganizationID" },
                        new object[] { employee.ID, employee.INN, OrganizationID }).Any();
                    if (q)
                        throw new Exception("Сотрудник с этим PINFL(" + employee.INN + ") уже существует.");
                    //}

                    string sql2 = "SELECT ID FROM [hl_Employee] WHERE ID<>@ID AND Name=@Name  AND OrganizationID=@OrganizationID";

                    var v = _databaseExt.GetDataFromSql(sql2,
                        new string[] { "@ID", "@Name", "@OrganizationID" },
                        new object[] { employee.ID, employee.Name, OrganizationID }).Any();
                    if (v)
                        throw new Exception("Этот сотрудник уже добавлен.");

                    //int ID = 0;
                    if (employee.ID == 0)
                    {
                        string sql = "INSERT INTO [hl_Employee] ([Name],[Occupation],[StateID],[OrganizationID],[INN],[DepartmentID],[DateOfModified]) VALUES (@Name,@Occupation,@StateID,@OrganizationID,@INN,@DepartmentID,GETDATE()) select [ID] FROM [hl_Employee] WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                        var NewID = _databaseExt.ExecuteScalar(sql,
                            new string[] { "Name", "@Occupation", "@StateID", "@OrganizationID", "@INN", "@DepartmentID" },
                            new object[] { employee.Name, employee.Occupation, employee.StateID, OrganizationID, employee.INN, employee.DepartmentID }, System.Data.CommandType.Text, ts);
                        employee.ID = Convert.ToInt32(NewID);
                    }
                    else
                    {
                        string sql1 = "SELECT Name FROM hl_Department WHERE ID=@DepartmentID";
                        var Department1 = _databaseExt.ExecuteScalar(sql1,
                            new string[] { "@DepartmentID" },
                            new object[] { employee.DepartmentID }, System.Data.CommandType.Text, ts);

                        Employee currentemp = GetEmployee(employee.ID);
                        if (employee.Name != currentemp.Name)
                            LogDataHistory(HelperStruct.Employee, employee.ID, "Name", currentemp.Name + "--->" + employee.Name, OrganizationID, UserID);
                        if (employee.INN != currentemp.INN)
                            LogDataHistory(HelperStruct.Employee, employee.ID, "INN", currentemp.INN + "--->" + employee.INN, OrganizationID, UserID);
                        if (employee.DepartmentID != currentemp.DepartmentID)
                            LogDataHistory(HelperStruct.Employee, employee.ID, "DepartmentID", currentemp.Department + "--->" + Department1, OrganizationID, UserID);


                        string sql = "UPDATE [hl_Employee] SET [Name]=@Name, [Occupation]=@Occupation, [StateID]=@StateID, [INN]=@INN, [DepartmentID]=@DepartmentID, [DateOfModified]=GETDATE() WHERE ID=@ID ";
                        _databaseExt.ExecuteNonQuery(sql,
                            new string[] { "@Name", "@Occupation", "StateID", "@INN", "DepartmentID", "@ID" },
                            new object[] { employee.Name, employee.Occupation, employee.StateID, employee.INN, employee.DepartmentID, employee.ID }, System.Data.CommandType.Text, ts);
                        var resPersonInfo = _databaseExt.GetFirstDataFromSql("SELECT ID FROM hl_ResponsiblePerson WHERE OrganizationID=@OrganizationID AND EmployeeID=@EmployeeID", new string[] { "@OrganizationID", "@EmployeeID" }, new object[] { OrganizationID, employee.ID }, System.Data.CommandType.Text, ts);
                        if (resPersonInfo != null)
                        {
                            _databaseExt.ExecuteNonQuery("UpdateSubCountName",
       new string[] { "@Value", "@TableID", "@Name" },
       new object[] { resPersonInfo.ID, HelperStruct.ResponsiblePerson, employee.Name }, System.Data.CommandType.StoredProcedure, ts);
                        }

                    }
                    ts.Commit();
                }
            }
        }
    }
}
