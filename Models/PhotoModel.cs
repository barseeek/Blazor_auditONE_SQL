using System;
using System.Collections.Generic;
using System.Threading;

namespace Blazor_auditONE_SQL.Models
{
    public class PhotoModel 
    {
        public int id { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string file_id { get; set; }
        public byte[] file_data { get; set; }
        public string description { get; set; }
        public string place { get; set; }
        public string zone { get; set; }
        public DateTime date { get; set; }
        public List<DepartmentModel> departments { get; set; }
        public int status { get; set; }
        // 1 - создан
        // 2 - в работе (назначены исполнители)
        // 3 - ожидает подтверждения
        // 4 - завершен
        public string comment { get; set; }
        public byte[] comment_data { get; set; }


    }

}
