﻿using Microsoft.EntityFrameworkCore;
using Reminders.Infrastructure.Data.EntityFramework.Configurations;

namespace Reminders.Infrastructure.Data.EntityFramework.Contexts
{
    public class RemindersContext
        : DbContext
    {
        public RemindersContext(DbContextOptions<RemindersContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RemindersConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}