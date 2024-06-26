﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<LoggedUser> LoggedUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ToDo.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne(x => x.Notification)
                .WithOne(x => x.Project)
                .HasForeignKey<Project>(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Task>()
                .HasOne(x => x.Notification)
                .WithOne(x => x.Task)
                .HasForeignKey<Task>(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
