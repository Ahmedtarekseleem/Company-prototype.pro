using Company.pro.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.pro.DAL.Data.Contexts
{
    public class CompanyDbContext : IdentityDbContext<AppUser>
    {
        // CLR
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = .; Database = CompanyPro; Trusted_Connection = True ; TrustServerCertificate = True"); 
        //}
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
