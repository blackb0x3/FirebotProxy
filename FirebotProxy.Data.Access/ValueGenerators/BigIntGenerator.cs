using FirebotProxy.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FirebotProxy.Data.Access.ValueGenerators;

public class BigIntGenerator<T> : ValueGenerator<long> where T : EntityBase
{
    private static readonly Random Rnd = new();

    public override long Next(EntityEntry entry)
    {
        long idToTry = 0;
        var entityExistsAlready = true;

        while (entityExistsAlready)
        {
            idToTry = Rnd.NextInt64(0, long.MaxValue);
            entityExistsAlready = entry.Context.Find<T>(idToTry) != null;
        }

        return idToTry;
    }

    public override bool GeneratesTemporaryValues => false;
}