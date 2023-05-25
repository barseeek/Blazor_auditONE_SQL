using Blazor_auditONE_SQL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;



namespace Blazor_auditONE_SQL.Data
{
    public class PhotoModelService
    {
        private readonly IConfiguration Configuration;
        public PhotoModelService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<List<PhotoModel>> GetPhotosAsync()
        {
            string query_photos = "SELECT dbo.Photo_from_tg.id, dbo.Users_from_tg.firstname, dbo.Users_from_tg.lastname, dbo.Photo_from_tg.file_id," +
                " dbo.Photo_from_tg.file_data, dbo.Photo_from_tg.description, dbo.Photo_from_tg.place, dbo.Photo_from_tg.zone, dbo.Photo_from_tg.date, dbo.Photo_from_tg.status, dbo.Photo_from_tg.comment, dbo.Photo_from_tg.comment_data" +
                " FROM dbo.Photo_from_tg, dbo.Users_from_tg WHERE dbo.Photo_from_tg.user_id=dbo.Users_from_tg.id";
            List<PhotoModel> PhotosList = new List<PhotoModel>();
            PhotosList.Clear();

            List<DepartmentModel> DepartmentsList = new List<DepartmentModel>();
            DepartmentsList.Clear();
            string query_deps = "SELECT dbo.Departments.id, dbo.Departments.department FROM dbo.Departments";

            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd_deps = new SqlCommand(query_deps, con);
                    SqlDataReader rdr1 = cmd_deps.ExecuteReader();
                    while (rdr1.Read())
                    {
                        DepartmentModel dep = new DepartmentModel();
                        dep.id = Convert.ToInt32(rdr1["id"]);
                        dep.department = Convert.ToString(rdr1["department"]);
                        dep.isChecked = false;
                        DepartmentsList.Add(dep);
                    }
                    rdr1.Close();
                    await rdr1.DisposeAsync();
                    cmd_deps.Dispose();
                    SqlCommand cmd = new SqlCommand(query_photos, con);
                    SqlDataReader rdr2 = cmd.ExecuteReader();
                    while (rdr2.Read())
                    {
                        PhotoModel d = new PhotoModel();
                        d.id = Convert.ToInt32(rdr2["id"]);
                        d.user_firstname = Convert.ToString(rdr2["firstname"]);
                        d.user_lastname = Convert.ToString(rdr2["lastname"]);
                        d.file_id = Convert.ToString(rdr2["file_id"]);
                        d.description = Convert.ToString(rdr2["description"]);
                        d.place = Convert.ToString(rdr2["place"]);
                        d.zone = Convert.ToString(rdr2["zone"]);
                        d.file_data = (byte[])rdr2["file_data"];
                        d.date = Convert.ToDateTime(rdr2["date"]);
                        //d.departments = new List<DepartmentModel>();
                        d.departments = new List<DepartmentModel>();
                        d.departments = DepartmentsList.ConvertAll(x => (DepartmentModel)x.Clone());
                        //d.departments.AddRange(DepartmentsList);
                        d.status = Convert.ToInt32(rdr2["status"]);
                        d.comment = Convert.ToString(rdr2["comment"]);
                        d.comment_data = DBNull.Value.Equals(rdr2["comment_data"]) ? Array.Empty<byte>() : (byte[])rdr2["comment_data"];
                        PhotosList.Add(d);
                    }
                    cmd.Dispose();
                    rdr2.Close();
                    await rdr2.DisposeAsync();

                    string query_photo_checked_deps = $"SELECT dbo.Photos_Departments.photo_id, dbo.Photos_Departments.department_id FROM dbo.Photos_Departments";
                    SqlCommand cmd_photos_deps = new SqlCommand(query_photo_checked_deps, con);
                    SqlDataReader rdr3 = cmd_photos_deps.ExecuteReader();
                    if (rdr3.HasRows)
                        while (rdr3.Read())
                        {
                            int photo_id = Convert.ToInt32(rdr3.GetValue(0));
                            int dep_id = Convert.ToInt32(rdr3.GetValue(1));
                            PhotosList.FirstOrDefault(x => x.id == photo_id).departments.FirstOrDefault(y => y.id == dep_id).isChecked = true;
                        }

                    rdr3.Close();
                    await rdr3.DisposeAsync();


                    con.Close();
                    con.Dispose();

                    return await Task.FromResult(PhotosList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public PhotoModel GetPhotoAsync(int id)
        {
            string query_photo = $"SELECT dbo.Photo_from_tg.id, dbo.Users_from_tg.firstname, dbo.Users_from_tg.lastname, dbo.Photo_from_tg.file_id, dbo.Photo_from_tg.file_data, dbo.Photo_from_tg.description, dbo.Photo_from_tg.place, dbo.Photo_from_tg.zone, dbo.Photo_from_tg.date, dbo.Photo_from_tg.status FROM dbo.Photo_from_tg, dbo.Users_from_tg WHERE (dbo.Photo_from_tg.user_id=dbo.Users_from_tg.id) AND (dbo.Photo_from_tg.id={id})";

            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query_photo, con);
                    SqlDataReader rdr2 = cmd.ExecuteReader();
                    PhotoModel d = new PhotoModel();
                    while (rdr2.Read())
                    {
                        d.id = Convert.ToInt32(rdr2["id"]);
                        d.user_firstname = Convert.ToString(rdr2["firstname"]);
                        d.user_lastname = Convert.ToString(rdr2["lastname"]);
                        d.file_id = Convert.ToString(rdr2["file_id"]);
                        d.description = Convert.ToString(rdr2["description"]);
                        d.place = Convert.ToString(rdr2["place"]);
                        d.zone = Convert.ToString(rdr2["zone"]);
                        d.file_data = (byte[])rdr2["file_data"];
                        d.date = Convert.ToDateTime(rdr2["date"]);
                        d.status = Convert.ToInt32(rdr2["status"]);
                    }
                    rdr2.Close();
                    con.Close();
                    rdr2.DisposeAsync();
                    con.Dispose();
                    cmd.Dispose();
                    return d;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> SaveDataAsync(string query, params byte[] data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    //foreach (string query in SaveQueries)
                    //{
                    SqlCommand cmd = new SqlCommand(query, con);
                    if (data != null && data != Array.Empty<byte>())
                    {
                        cmd.Parameters.Add("@image_data", SqlDbType.Image);
                        cmd.Parameters["@image_data"].Value = data;
                    }

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                    }
                    rdr.Close();
                    await rdr.DisposeAsync();
                    // }
                    con.Close();
                    await con.DisposeAsync();
                    return await Task.FromResult("OK");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message.ToString());
            }
        }
        public async Task<List<PhotosDepsModel>> GetPhotosDepsAsync()
        {
            List<PhotosDepsModel> photosDeps = new List<PhotosDepsModel>();
            photosDeps.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:ConnectionAudit"]))
                {
                    con.Open();
                    string query_photo_checked_deps = $"SELECT dbo.Photos_Departments.photo_id, dbo.Photos_Departments.department_id FROM dbo.Photos_Departments";
                    SqlCommand cmd_photos_deps = new SqlCommand(query_photo_checked_deps, con);
                    SqlDataReader rdr3 = cmd_photos_deps.ExecuteReader();
                    if (rdr3.HasRows)
                        while (rdr3.Read())
                        {
                            PhotosDepsModel d = new PhotosDepsModel();
                            d.photo_id = Convert.ToInt32(rdr3["photo_id"]);
                            d.photo_checked_dep = Convert.ToInt32(rdr3["department_id"]);
                            photosDeps.Add(d);
                        }

                    rdr3.Close();
                    await rdr3.DisposeAsync();


                    con.Close();
                    con.Dispose();
                }
                return await Task.FromResult(photosDeps);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
