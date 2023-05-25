using Blazor_auditONE_SQL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;



namespace Blazor_auditONE_SQL.Data
{
    public class UserModelService
    {
        private readonly IConfiguration Configuration;
        public UserModelService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<List<UserModel>> GetUsersAsync(string query)
        {
            List<UserModel> UsersList = new List<UserModel>();
            UsersList.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UserModel d = new UserModel();
                        d.id = Convert.ToInt32(rdr["id"]);
                        d.login = Convert.ToString(rdr["login"]);
                        d.firstname = Convert.ToString(rdr["firstname"]);
                        d.lastname = Convert.ToString(rdr["lastname"]);
                        d.head_firstname = Convert.ToString(rdr["hueta"]);
                        d.head_lastname = Convert.ToString(rdr["huetahueta"]);
                        d.head_fullname = d.head_firstname + " " + d.head_lastname;
                        //d.head_id = Convert.ToString(rdr["head_id"]);
                        d.permissions = Convert.ToString(rdr["permissions"]);
                        d.login_mm = Convert.ToString(rdr["login_mm"]);
                        UsersList.Add(d);
                    }
                    rdr.Close();
                    con.Close();
                    await rdr.DisposeAsync();
                    con.Dispose();
                    cmd.Dispose();
                    return await Task.FromResult(UsersList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<UserModel>> GetUsersDropDownAsync()
        {
            List<UserModel> UsersList = new List<UserModel>();
            UsersList.Clear();
            string query_users = "SELECT dbo.Users_from_tg.id, dbo.Users_from_tg.login, dbo.Users_from_tg.firstname, dbo.Users_from_tg.lastname, dbo.Users_from_tg.head_id, dbo.Users_from_tg.permissions, dbo.Users_from_tg.login_mm, dbo.Users_from_tg.mail FROM dbo.Users_from_tg";

            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query_users, con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UserModel d = new UserModel();
                        d.id = Convert.ToInt32(rdr["id"]);
                        d.login = Convert.ToString(rdr["login"]);
                        d.firstname = Convert.ToString(rdr["firstname"]);
                        d.lastname = Convert.ToString(rdr["lastname"]);
                        d.head_id = Convert.ToString(rdr["head_id"]);
                        d.permissions = Convert.ToString(rdr["permissions"]);
                        d.login_mm = Convert.ToString(rdr["login_mm"]);
                        d.mail = Convert.ToString(rdr["mail"]);
                        UsersList.Add(d);
                    }
                    rdr.Close();
                    con.Close();
                    await rdr.DisposeAsync();
                    con.Dispose();
                    cmd.Dispose();
                    return await Task.FromResult(UsersList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            List<DepartmentModel> DepartmentsList = new List<DepartmentModel>();
            DepartmentsList.Clear();
            string query_deps = "SELECT dbo.Departments.id, dbo.Departments.department FROM dbo.Departments";
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query_deps, con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DepartmentModel d = new DepartmentModel();
                        d.id = Convert.ToInt32(rdr["id"]);
                        d.department = Convert.ToString(rdr["department"]);
                        DepartmentsList.Add(d);
                    }
                    rdr.Close();
                    con.Close();
                    await rdr.DisposeAsync();
                    con.Dispose();
                    cmd.Dispose();
                    return await Task.FromResult(DepartmentsList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<List<UserDepartmentModel>> GetUsersDepartmentsAsync()
        {
            List<UserDepartmentModel> UsersDepartmentsList = new List<UserDepartmentModel>();
            UsersDepartmentsList.Clear();
            string query_users_deps = "SELECT dbo.Users_Departments.user_id, dbo.Users_Departments.department_id FROM dbo.Users_Departments";
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query_users_deps, con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UserDepartmentModel d = new UserDepartmentModel();
                        d.user_id = Convert.ToInt32(rdr["user_id"]);
                        d.department_id = Convert.ToInt32(rdr["department_id"]);
                        UsersDepartmentsList.Add(d);
                    }
                    rdr.Close();
                    con.Close();
                    await rdr.DisposeAsync();
                    con.Dispose();
                    cmd.Dispose();
                    return await Task.FromResult(UsersDepartmentsList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
