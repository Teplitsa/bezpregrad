using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Database.Entities
{
    public class DbUser
    {
        public DbUser()
        {
            Points = new List<DbPoint>();
        }

        [Required]
        [Key]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public string Login
        {
            get;
            set;
        }

        [Required]
        public string Password
        {
            get;
            set;
        }

        [Required]
        public virtual ICollection<DbPoint> Points
        {
            get;
            set;
        }
    }
}