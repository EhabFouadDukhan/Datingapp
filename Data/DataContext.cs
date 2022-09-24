using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIII.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIII.Data
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