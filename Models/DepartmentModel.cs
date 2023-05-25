using System;

namespace Blazor_auditONE_SQL.Models
{
    public class DepartmentModel : ICloneable
    {
        public int id { get; set; }
        public bool isChecked { get; set; }
        public string department { get; set; }

        public object Clone()
        {
            return new DepartmentModel
            {
                id = this.id,
                department = this.department,
                isChecked = this.isChecked
            };
        }
    }
}
