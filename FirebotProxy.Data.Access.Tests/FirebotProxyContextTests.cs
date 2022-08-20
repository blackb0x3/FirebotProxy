using FirebotProxy.Data.Entities;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FirebotProxy.Data.Access.Tests;

[TestFixture]
public class A_FirebotProxy_Database_Context
{
    private FirebotProxyContext _context;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _context = FakeContextGenerator.GenerateFakeContext();
    }

    [Test]
    public async Task Updates_Created_And_LastUpdated_Timestamps_When_Entity_Created()
    {
        var entityToStore = new ChatMessage { Content = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow, SenderUsername = "blah" };

        await _context.ChatMessages.AddAsync(entityToStore);
        await _context.SaveChangesAsync();

        entityToStore.Created.Should().Be(entityToStore.LastUpdated);
    }

    [Test]
    public async Task Updates_Only_The_LastUpdated_Timestamp_When_Entity_Modified()
    {
        // add the entity
        var entityToStore = new ChatMessage { Content = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow, SenderUsername = "blah" };

        await _context.ChatMessages.AddAsync(entityToStore);
        await _context.SaveChangesAsync();

        // allow some time to elapse to ensure the dates will be different
        await Task.Delay(100);

        // now edit something
        entityToStore.Content = Guid.NewGuid().ToString();

        await _context.SaveChangesAsync();

        entityToStore.Created.Should().BeBefore(entityToStore.LastUpdated);
    }
}