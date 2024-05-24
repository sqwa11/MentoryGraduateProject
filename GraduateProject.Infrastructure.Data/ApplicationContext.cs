using GraduateProject.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateProject.Infrastructure.Data
{
    public  class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options) {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Login = "qwe", Password = "qwe" },
                new User { Id = Guid.NewGuid(), Login = "asd", Password = "asd" }
                );
        }
    }
}
