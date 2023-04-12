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
            string sqlselect = @"SELECT Citizen.ID ID,
                                        Citizen.FullName,
                                        Citizen.PINFL,
                                        Citizen.DateOfBirth,
                                        Nationality.DisplayName Nation,
                                        IIF(Citizen.IsForeignCitizen = 1,'Chetel','Uzbekiston') ForeignCitizen,
                                        CitizenEmployment.DisplayName CitizenEmployment,
                                        Street.Name Street,
                                        Citizen.PhoneNumber,
                                        Neighborhood.Name Neighborhood,
                                        [State].DisplayName [State] ";

            string sqlfrom = @" FROM hl_Citizen Citizen 
                                     JOIN info_Nation Nationality ON Nationality.ID = Citizen.NationID
                                     JOIN enum_Gender Gender ON Gender.ID = Citizen.GenderID
                                     JOIN enum_Education Education ON Education.ID = Citizen.EducationID
                                     LEFT JOIN enum_AcademicTitle AcademicTitle ON AcademicTitle.ID = Citizen.AcademicTitleID
                                     LEFT JOIN enum_AcademicDegree AcademicDegree ON AcademicDegree.ID = Citizen.AcademicDegreeID
                                     JOIN enum_Married Married ON Married.ID = Citizen.MarriedID
                                     JOIN enum_CitizenEmployment CitizenEmployment ON CitizenEmployment.ID = Citizen.CitizenEmploymentID
                                     JOIN info_Region Region ON Region.ID = Citizen.BirthRegionID
                                     JOIN info_District District ON District.ID = Citizen.BirthDistrictID
                                     JOIN info_Neighborhood Neighborhood ON Neighborhood.ID = Citizen.NeighborhoodID
                                     JOIN enum_State [State] ON [State].ID = Citizen.StateID
									 JOIN info_Street Street ON Street.ID = Citizen.StreetID ";

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
        public Citizen GetCitizen(int? id)
        {

            var data = _databaseExt.GetDataFromSql("SELECT * FROM hl_Citizen WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }).First();
            Citizen citizen = new Citizen()
            {
                ID = data.ID,
                FirstName = data.FirstName,
                LastName = data.LastName,
                FamilyName = data.FamilyName,
                PINFL = data.PINFL,
                DateOfBirthday = data.DateOfBirth,
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
                BirthPlace = data.BithPlace,
                StateID = data.StateID,
                PhoneNumber = data.PhoneNumber,
                BirthdayDistrictID = data.BirthDistrictID,
                BirthdayRegionID = data.BirthRegionID,
                StreetID = data.StreetID,
                FamilyID = data.FamilyID,
                IsDisabled = data.IsDisabled,
            };
            return citizen;
        }
        public void UpdateCitizen(Citizen citizen)
        {
            if (!UserIsInRole("EmployeeEdit"))
                throw new Exception("Нет доступа.");

            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    if (citizen.PINFL == "" || citizen.PINFL == null || citizen.PINFL.Length != 14)
                        throw new Exception("JSHSHIR uzunligi 14 bo'lsihi kerak");

                    if (!NumberToWord.IsNumber(citizen.PINFL))
                        throw new Exception("JSHSHIR raqam bo'lishi kerak");


                    string sql0 = "SELECT ID FROM [hl_Citizen] WHERE ID<>@ID AND PINFL=@PINFL  AND NeighborhoodID=@NeighborhoodID";

                    var q = _databaseExt.GetDataFromSql(sql0,
                        new string[] { "@ID", "@PINFL", "@NeighborhoodID" },
                        new object[] { citizen.ID, citizen.PINFL, NeighborhoodID }).Any();
                    if (q)
                        throw new Exception("Bu JSHSHIR (" + citizen.PINFL + ") bilan fuqaro avval qo'shilgan.");

                    if (citizen.ID == 0)
                    {
                        string sql = @"INSERT INTO [dbo].[hl_Citizen]
                                                   ([FirstName],[LastName],[FamilyName],[FullName],[FIOTranslate],[PINFL],[DateOfBirth]
                                                   ,[NationID],[GenderID],[EducationID],[AcademicDegreeID],[AcademicTitleID],[PhoneNumber]
                                                   ,[MarriedID],[IsForeignCitizen],[CountChild],[CitizenEmploymentID],[IsLowIncome]
                                                   ,[IsConvicted],[IsCheckCityzen],[BirthRegionID],[BirthDistrictID],[BithPlace]
                                                   ,[NeighborhoodID],[CreateUserID], [StreetID], [FamilyID], [IsDisabled])
                                             VALUES
                                                   (@FirstName,@LastName,@FamilyName,@FullName,@FIOTranslate,@PINFL,@DateOfBirth
                                                   ,@NationID,@GenderID,@EducationID,@AcademicDegreeID,@AcademicTitleID,@PhoneNumber
                                                   ,@MarriedID,@IsForeignCitizen,@CountChild,@CitizenEmploymentID,@IsLowIncome
                                                   ,@IsConvicted,@IsCheckCityzen,@BirthRegionID,@BirthDistrictID,@BithPlace
                                                   ,@NeighborhoodID,@CreateUserID, @StreetID, @FamilyID, @IsDisabled) select [ID] FROM [hl_Citizen] WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                        var NewID = _databaseExt.ExecuteScalar(sql,
                            new string[] {"@FirstName","@LastName","@FamilyName","@FullName","@FIOTranslate","@PINFL","@DateOfBirth"
                                                   ,"@NationID","@GenderID","@EducationID","@AcademicDegreeID","@AcademicTitleID","@PhoneNumber"
                                                   ,"@MarriedID","@IsForeignCitizen","@CountChild","@CitizenEmploymentID","@IsLowIncome"
                                                   ,"@IsConvicted","@IsCheckCityzen","@BirthRegionID","@BirthDistrictID","@BithPlace"
                                                   ,"@NeighborhoodID","@CreateUserID", "@StreetID","@FamilyID", "@IsDisabled"},
                            new object[] { citizen.FirstName,citizen.LastName,citizen.FamilyName,citizen.FirstName +" " + citizen.LastName + " " + citizen.FamilyName,citizen.FirstName +" " + citizen.LastName + " " + citizen.FamilyName,citizen.PINFL,citizen.DateOfBirthday
                                                   ,citizen.NationID,citizen.GenderID,citizen.EducationID,citizen.AcademicDegreeID,citizen.AcademicTitleID,citizen.PhoneNumber
                                                   ,citizen.MarriedID,citizen.IsForeignCitizen,citizen.CountChild,citizen.CitizenEmploymentID,citizen.IsLowIncome
                                                   ,citizen.IsConvicted,citizen.IsCheckCityzen,citizen.BirthdayRegionID,citizen.BirthdayDistrictID,citizen.BirthPlace
                                                   ,NeighborhoodID,UserID, citizen.StreetID, citizen.FamilyID, citizen.IsDisabled}, System.Data.CommandType.Text, ts);
                        citizen.ID = Convert.ToInt32(NewID);
                    }
                    else
                    {
                        string sql = @"UPDATE [dbo].[hl_Citizen]
                                                      SET [FirstName] = @FirstName,[LastName] = @LastName,[FamilyName] = @FamilyName
                                                         ,[FullName] = @FullName,[FIOTranslate] = @FIOTranslate,[PINFL] = @PINFL
                                                         ,[DateOfBirth] = @DateOfBirth,[NationID] = @NationID,[GenderID] = @GenderID
                                                         ,[EducationID] = @EducationID,[AcademicDegreeID] = @AcademicDegreeID,[AcademicTitleID] = @AcademicTitleID
                                                         ,[PhoneNumber] = @PhoneNumber,[MarriedID] = @MarriedID,[IsForeignCitizen] = @IsForeignCitizen
                                                         ,[CountChild] = @CountChild,[CitizenEmploymentID] = @CitizenEmploymentID,[IsLowIncome] = @IsLowIncome
                                                         ,[IsConvicted] = @IsConvicted,[IsCheckCityzen] = @IsCheckCityzen,[BirthRegionID] = @BirthRegionID
                                                         ,[BirthDistrictID] = @BirthDistrictID,[BithPlace] = @BithPlace,[StateID] = @StateID
                                                         ,[DateOfModified] = GETDATE(),[ModifiedUserID] = @ModifiedUserID, [StreetID] = @StreetID, 
                                                          [FamilyID] = @FamilyID, [IsDisabled] = @IsDisabled
                                                    WHERE ID = @ID";
                        _databaseExt.ExecuteNonQuery(sql,
                            new string[] {                "@FirstName","@LastName","@FamilyName"
                                                         ,"@FullName","@FIOTranslate","@PINFL"
                                                         ,"@DateOfBirth","@NationID","@GenderID"
                                                         ,"@EducationID","@AcademicDegreeID","@AcademicTitleID"
                                                         ,"@PhoneNumber","@MarriedID","@IsForeignCitizen"
                                                         ,"@CountChild","@CitizenEmploymentID","@IsLowIncome"
                                                         ,"@IsConvicted","@IsCheckCityzen","@BirthRegionID"
                                                         ,"@BirthDistrictID","@BithPlace","@StateID"
                                                         ,"@ModifiedUserID","@ID","@StreetID","@FamilyID", "@IsDisabled"},
                            new object[] { citizen.FirstName, citizen.LastName, citizen.FamilyName, citizen.FirstName + " " + citizen.LastName + " " + citizen.FamilyName, citizen.FirstName + " " + citizen.LastName + " " + citizen.FamilyName, citizen.PINFL, citizen.DateOfBirthday, citizen.NationID, citizen.GenderID, citizen.EducationID, citizen.AcademicDegreeID, citizen.AcademicTitleID, citizen.PhoneNumber, citizen.MarriedID, citizen.IsForeignCitizen, citizen.CountChild, citizen.CitizenEmploymentID, citizen.IsLowIncome, citizen.IsConvicted, citizen.IsCheckCityzen, citizen.BirthdayRegionID, citizen.BirthdayDistrictID, citizen.BirthPlace, citizen.StateID, UserID, citizen.ID, citizen.StreetID, citizen.FamilyID, citizen.IsDisabled }, System.Data.CommandType.Text, ts);
                    }
                    ts.Commit();
                }
            }
        }
    }
}
