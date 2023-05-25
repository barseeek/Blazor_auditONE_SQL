namespace Blazor_auditONE_SQL.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string login { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string head_firstname { get; set; }
        public string head_lastname { get; set; }
        public string head_fullname { get; set; }
        public string head_id { get; set; }
        public string permissions { get; set; }
        public string login_mm { get; set; }
        public string mail { get; set; }
        public string getUserRole()
        {
            return permissions;
        }
    }
}
