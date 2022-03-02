using SQLite;
using System;

namespace XFreshUsers.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        [NotNull, MaxLength(20)]
        public string Name { get; set; }

        [NotNull, Indexed, MaxLength(15)]
        public string PassWord { get; set; }

        public int IsAdmin { get; set; }

        public bool IsValid()
        {
            return (!String.IsNullOrWhiteSpace(Name));
        }
    }
}
