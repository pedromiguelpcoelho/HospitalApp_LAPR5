using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.OperationTypes;
using System;

public class InMemoryDbContextFactory
{
    public static DDDSample1DbContext Create()
    {
        var options = new DbContextOptionsBuilder<DDDSample1DbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DDDSample1DbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    public static void Destroy(DDDSample1DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}