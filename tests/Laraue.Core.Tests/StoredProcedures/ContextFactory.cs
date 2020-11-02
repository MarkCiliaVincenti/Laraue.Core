﻿using Laraue.Core.DataAccess.StoredProcedures.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Laraue.Core.Tests.StoredProcedures
{
    public class ContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            return CreatePgDbContext();
        }

        public TestDbContext CreatePgDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=tests;")
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new TestDbContext(options);
        }
    }
}
