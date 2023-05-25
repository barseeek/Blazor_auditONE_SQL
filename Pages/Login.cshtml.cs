using Blazor_auditONE_SQL.Data;
using Blazor_auditONE_SQL.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor_auditONE_SQL.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration Configuration;

        UserData _user;
        //string returnUrl = Url.Content("~/");
        public LoginModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //public string ReturnUrl { get; set; }
        public async Task<IActionResult>
            OnGetAsync(string paramUsername, string paramPassword)
        {

            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }
            // *** !!! This is where you would validate the user !!! ***
            // In this example we just log the user in
            // (Always log the user in for this demo)

            try
            {
                //if (paramPassword != "" && paramUsername != "" && paramPassword != null && paramUsername != null)
                //{
                //    bool valid = false;
                //    string pwd = Encryption.Decrypt(paramPassword, "Mmkarton123!#_");
                //    DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://pof.mm-pof.ru", paramUsername, pwd);

                //    using (PrincipalContext context = new PrincipalContext(ContextType.Domain,  "pof.mm-pof.ru", "OU=POF,DC=pof,DC=mm-pof,DC=ru"))
                //    {

                //        valid = context.ValidateCredentials(paramUsername, pwd);
                //        if (valid)
                //        {
                //            UserPrincipal user = UserPrincipal.FindByIdentity(context, paramUsername);

                //            List<string> userRoles = new List<string>();
                //            List<string> userDepartment = new List<string>();
                //            List<string> userDepartmentIDSigur = new List<string>();

                //            if (user != null)
                //            {
                //                _user = new UserData();
                //                _user.samaccountname = user.SamAccountName;
                //                _user.FIO = user.DisplayName;
                //                 get a user's group memberships 
                //                foreach (Principal principal in user.GetGroups())
                //                {
                //                    GroupPrincipal gp = (principal as GroupPrincipal);

                //                    if (gp != null && gp.Name.StartsWith("pof_audit"))
                //                    {
                //                        _user.role = gp.Name.Substring(10, gp.Name.Length - 10);
                //                        DirectoryEntry properties = (DirectoryEntry)gp.GetUnderlyingObject();

                //                        foreach (object property in properties.Properties["proxyaddresses"])
                //                        {
                //                            _user.DepartmentSigurIDs.Add(Convert.ToInt32(property));
                //                            userRoles.Add(gp.Name.Substring(10, gp.Name.Length - 10) + ":" + property.ToString());
                //                        }

                //                    }
                //                }

                //                if (_user.role == null) { return await Task.FromResult(ReturnError("User have no access to the app")).Result; }


                //                AuthorizedUsers.LogInUser(_user);


                //                var claims = new List<Claim>
                //                {
                //                    new Claim(ClaimTypes.Name, _user.samaccountname)
                //                };


                //                var claimsIdentity = new ClaimsIdentity(
                //                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //                var authProperties = new AuthenticationProperties
                //                {
                //                    IsPersistent = true,
                //                    RedirectUri = this.Request.Host.Value
                //                };
                //                try
                //                {
                //                    await HttpContext.SignInAsync(
                //                    CookieAuthenticationDefaults.AuthenticationScheme,
                //                    new ClaimsPrincipal(claimsIdentity),
                //                    authProperties);
                //                }
                //                catch (Exception ex1)
                //                {
                //                    string error = ex1.Message;
                //                }
                //                return LocalRedirect(Url.Content("~/datagrid"));
                //            }
                //            else
                //            {
                //                return await Task.FromResult(ReturnError("User not found")).Result;
                //            }
                //        }
                //        else//wrong username or password
                //        {
                //            return await Task.FromResult(ReturnError("Wrong username or password")).Result;
                //        }
                //    }
                //}
                if (paramPassword != "" && paramUsername != "" && paramPassword != null && paramUsername != null)
                {
                    bool valid = false;
                    string pwd = Encryption.Decrypt(paramPassword, "Mmkarton123!#_");

                    using (DirectoryEntry entry = new DirectoryEntry("LDAP://pof.mm-pof.ru", paramUsername, pwd))
                    {
                        // Attempt to bind to the domain with the user's credentials
                        using (DirectorySearcher searcher = new DirectorySearcher(entry))
                        {
                            searcher.Filter = $"(&(objectClass=user)(sAMAccountName={paramUsername}))";
                            SearchResult result = searcher.FindOne();

                            if (result != null)
                            {
                                // User found, perform further actions
                                _user = new UserData();
                                _user.samaccountname = result.Properties["sAMAccountName"][0].ToString();
                                _user.FIO = result.Properties["displayName"][0].ToString();

                                // Get user's group memberships
                                using (DirectoryEntry userEntry = result.GetDirectoryEntry())
                                {
                                    foreach (object property in userEntry.Properties["memberOf"])
                                    {
                                        //string groupName = new DirectoryEntry($"LDAP://{property.ToString()}").Name;

                                        if (property.ToString().Contains("pof_audit"))
                                        {
                                            _user.role = property.ToString().Substring(13,10);
                                            //_user.DepartmentSigurIDs.Add(Convert.ToInt32(property));
                                        }
                                    }
                                }

                                if (_user.role == null)
                                {
                                    return await Task.FromResult(ReturnError("User has no access to the app")).Result;
                                }

                                AuthorizedUsers.LogInUser(_user);

                                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, _user.samaccountname)
                        };

                                var claimsIdentity = new ClaimsIdentity(
                                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                var authProperties = new AuthenticationProperties
                                {
                                    IsPersistent = true,
                                    RedirectUri = this.Request.Host.Value
                                };

                                try
                                {
                                    await HttpContext.SignInAsync(
                                        CookieAuthenticationDefaults.AuthenticationScheme,
                                        new ClaimsPrincipal(claimsIdentity),
                                        authProperties);
                                }
                                catch (Exception ex1)
                                {
                                    string error = ex1.Message;
                                }

                                return LocalRedirect(Url.Content("~/datagrid"));
                            }
                            else
                            {
                                return await Task.FromResult(ReturnError("User not found")).Result;
                            }
                        }
                    }
                }
                else
                {
                    return await Task.FromResult(ReturnError("Username or password cant be null")).Result;
                }


            }
            catch (Exception ex)
            {
                return await Task.FromResult(ReturnError(ex.Message)).Result;
            }

        }

        public async Task<IActionResult> ReturnError(string errMessage)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.Role, errMessage)
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };
            try
            {
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            }
            catch (Exception ex1)
            {
                string error = ex1.Message;
            }
            return LocalRedirect(Url.Content("~/"));
        }


    }
}

