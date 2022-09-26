using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIII.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIIII.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}