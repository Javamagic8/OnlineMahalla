using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GetEmployeeList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = "";
            sqlselect += " SELECT";
            sqlselect += " emp.ID,";
            sqlselect += " emp.Name,";
            sqlselect += " emp.Name NameCode,";
            sqlselect += " emp.INN,";
            sqlselect += " emp.Occupation,";
            sqlselect += " st.DisplayName [State],";
            sqlselect += " (Select dep.Name from hl_Department dep where emp.DepartmentID=dep.ID) Department";
            string sqlfrom = " FROM hl_Employee emp, enum_State st";
            string sqlwhere = " WHERE emp.OrganizationID=@OrganizationID AND emp.StateID=st.ID ";
            sqlparamas.Add("@OrganizationID", OrganizationID);
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND emp.Name LIKE '%' + @Name + '%'";
                sqlparamas.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(emp.ID)" + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY emp.ID " + (Order == "asc" ? " " : " DESC");

            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
        public Employee GetEmployee(int id)
        {
            var data = _databaseExt.GetDataFromSql("SELECT * FROM [hl_Employee] WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }).First();
            Employee employee = new Employee()
            {
                ID = data.ID,
                Name = data.Name,
                Occupation = data.Occupation,
                StateID = data.StateID,
                OrganizationID = data.OrganizationID,
                DateOfModified = data.DateOfModified,
                INN = data.INN,
                DepartmentID = data.DepartmentID,
                Department = data.DepartmentID > 0 ? _databaseExt.ExecuteScalar("SELECT Name FROM hl_Department WHERE ID=@DepartmentID", new string[] { "@DepartmentID" }, new object[] { data.DepartmentID }).ToString() : "",
            };
            return employee;
        }
        public void UpdateEmployee(Employee employee)
        {
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
