using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common360.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Common360.Data
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyConfigurationFromAsembly(builder);

            ConfigureIdentity(builder);
        }

        private void ApplyConfigurationFromAsembly(ModelBuilder builder)
        {
            var applyGenericMethod = typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
            // replace GetExecutingAssembly with assembly where your configurations are if necessary
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
            {
                // use type.Namespace to filter by namespace if necessary
                foreach (var iface in type.GetInterfaces())
                {
                    // if type implements interface IEntityTypeConfiguration<SomeEntity>
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        // make concrete ApplyConfiguration<SomeEntity> method
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        // and invoke that with fresh instance of your configuration type
                        applyConcreteMethod.Invoke(builder, new object[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
        }

        private static void ConfigureIdentity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Id).HasColumnName("UserId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(e => e.Id).HasColumnName("RoleId");
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
