using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineMahalla.Common.Model;
using OnlineMahalla.Data.Model;

namespace OnlineMahalla.Web.MVCClient.Extentions.BasicAuth
{
    public class BasicAuthFilter : IAuthorizationFilter
    {
        private readonly string _realm;
        public BasicAuthFilter(string realm)
        {
            _realm = realm;
            if (string.IsNullOrWhiteSpace(_realm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                                            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                            .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            if (IsAuthorized(context, credentials[0], credentials[1]))
                            {
                                return;
                            }
                        }
                    }
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        public bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();

            var userService = context.HttpContext.RequestServices.GetRequiredService<IntegrationMultiOptionsInfo>();

            string useragent = "";
            string userip = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            if (userip != "127.0.0.1" && context.HttpContext.Request.Headers.ContainsKey("X -Forwarded-For"))
                userip = context.HttpContext.Request.Headers["X-Forwarded-For"];
            if (context.HttpContext.Request.Headers.ContainsKey("User-Agent"))
                useragent = context.HttpContext.Request.Headers["User-Agent"];
             if (_realm == "minvuz")
            {
                if (!string.IsNullOrWhiteSpace(userService.apitominvuz.iplist))
                {
                    if (!userService.apitominvuz.iplist.Contains(userip))
                    {
                        logger.LogWarning("Not Allowed IP:" + userip + " Agent:" + useragent);
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(userService.apitominvuz.login) || userService.apitominvuz.login.ToLower() != username.ToLower())
                    return false;
                if (string.IsNullOrWhiteSpace(userService.apitominvuz.pswd) || userService.apitominvuz.pswd.ToLower() != password.ToLower())
                    return false;
                return true;
            }
             else if (_realm == "apifornewprojects")
            {
                if (!string.IsNullOrWhiteSpace(userService.apifornewprojects.iplist))
                {
                    if (!userService.apifornewprojects.iplist.Contains(userip))
                    {
                        logger.LogWarning("Not Allowed IP:" + userip + " Agent:" + useragent);
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(userService.apifornewprojects.login) || userService.apifornewprojects.login.ToLower() != username.ToLower())
                    return false;
                if (string.IsNullOrWhiteSpace(userService.apifornewprojects.pswd) || userService.apifornewprojects.pswd.ToLower() != password.ToLower())
                    return false;
                return true;
            }
            return false;
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }
    }
}