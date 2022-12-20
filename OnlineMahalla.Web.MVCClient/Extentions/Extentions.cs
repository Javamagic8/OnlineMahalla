using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineMahalla.Data.Model;

namespace OnlineMahalla.Web.MVCClient.Extentions
{

    public static class AppConst
    {
        /// <summary>
        /// Avtorizatsiya server
        /// </summary>
        public static string AuthUrl { get; set; }
        /// <summary>
        /// E-imzo va ZP infolar malumotlari
        /// </summary>
        public static string AuthServiceUrl { get; set; }

    }
    public static class UserAddInfo
    {
        public static int GetUserID(this System.Security.Claims.ClaimsPrincipal user)
        {
            var useridclaim = user.Claims.FirstOrDefault(x => x.Type == "userid");
            if (useridclaim == null)
                return 0;
            return Convert.ToInt32(useridclaim.Value);

        }
        public static string GetUserName(this System.Security.Claims.ClaimsPrincipal user)
        {
            var useridclaim = user.Claims.FirstOrDefault(x => x.Type == "sub" || x.Type.Contains("claims/name"));
            if (useridclaim == null)
                return "";
            return useridclaim.Value;

        }
        public static int GetOrganizationID(this System.Security.Claims.ClaimsPrincipal user)
        {
            var orgidclaim = user.Claims.FirstOrDefault(x => x.Type == "orgid");
            //var temporgidclaim = user.Claims.FirstOrDefault(x => x.Type == "temporgid");

            if (orgidclaim == null) // && temporgidclaim == null
                return 0;

            return Convert.ToInt32( orgidclaim.Value);//(temporgidclaim != null && temporgidclaim.Value != "0") ? temporgidclaim.Value :
        }
        public static string GetOrganizationName(this System.Security.Claims.ClaimsPrincipal user)
        {
            var orgnameclaim = user.Claims.FirstOrDefault(x => x.Type == "orgname");
            if (orgnameclaim == null)
                return "";
            return orgnameclaim.Value;
        }
        public static bool GetIsChildLogOut(this System.Security.Claims.ClaimsPrincipal user)
        {
            var ischildlogoutclaim = user.Claims.FirstOrDefault(x => x.Type == "ischildlogout");
            if (ischildlogoutclaim == null)
                return true;
            return Convert.ToBoolean(ischildlogoutclaim.Value);
        }
        public static string GetUserIP(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            string userip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (userip == "127.0.0.1")
                userip = "";
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                userip += (!string.IsNullOrEmpty(userip) ? ";" : "") + httpContext.Request.Headers["X-Forwarded-For"];
            return userip;
        }
    }

    public static class AuthService
    {
        public static async Task<VerifySignResult> VerifySign (VerifySignInfo verifySignInfo)
        {
            VerifySignResult result = new VerifySignResult();

            using (System.Net.Http.HttpClient clienthttp = new System.Net.Http.HttpClient())
            {
                try
                {
                    clienthttp.Timeout = TimeSpan.FromSeconds(60);
                    string serverurl = AppConst.AuthServiceUrl + "api/Publickey/VerifySign";
                    System.Net.Http.StringContent postcontent = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(verifySignInfo), Encoding.UTF8, "application/json");
                    System.Net.Http.HttpResponseMessage response = await clienthttp.PostAsync(serverurl, postcontent);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifySignResult>(responseString);
                    }
                    else
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        throw new Exception("Ошибка:" + response.StatusCode.ToString() + " Error:" + responseString);
                    }

                    return result;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message + " in.error:" + ex.InnerException?.Message);
                }
            }
        }
        public static async Task<string> GetJsonFromApi(string serverurl)
        {
            using System.Net.Http.HttpClient clienthttp = new System.Net.Http.HttpClient();
            clienthttp.Timeout = TimeSpan.FromSeconds(60);

            System.Net.Http.HttpResponseMessage response = await clienthttp.GetAsync(serverurl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
                throw new Exception("Ошибка:" + response.StatusCode.ToString());
        }
    }

    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
        }
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(400);
            }
        }
    }


}
