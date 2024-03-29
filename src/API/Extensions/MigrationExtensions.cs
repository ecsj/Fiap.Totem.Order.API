using System.Diagnostics.CodeAnalysis;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

[ExcludeFromCodeCoverage]
public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();
    }
}

