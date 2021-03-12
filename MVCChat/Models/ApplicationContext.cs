using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MVCChat.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // добавляем тестовые роли
         
            // добавляем тестовых пользователей
            User adminUser1 = new User { Id = 1, Name = "admin@mail.com", Password = "123456", Role = "admin" };
            User adminUser2 = new User { Id = 2, Name = "tom@mail.com", Password = "123456", Role = "admin" };
            User simpleUser1 = new User { Id = 3, Name = "bob@mail.com", Password = "123456", Role = "user" };
            User simpleUser2 = new User { Id = 4, Name = "sam@mail.com", Password = "123456", Role = "user" };

            modelBuilder.Entity<User>().HasData(new User[] { adminUser1, adminUser2, simpleUser1, simpleUser2 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
