using AsyncStreams.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncStreams.Tests
{
    public class EntityFrameworkCoreTests
    {
        private readonly DbContextOptions<SampleDbContext> dbOptions = new DbContextOptionsBuilder<SampleDbContext>()
            .UseSqlite("Filename=Sample.db")
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Options;

        [SetUp]
        public async Task Setup()
        {
            using var context = new SampleDbContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
        }

        [Test]
        public async Task UseAsyncStreamsCorrectly()
        {
            using var context = new SampleDbContext(dbOptions);

            var persons = context.Persons
                .AsQueryable()
                .Where(person => person.LastName == "Doe")
                .AsAsyncEnumerable();

            await foreach (var person in persons)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }

        [Test]
        public async Task UseAsyncStreamsIncorrectly()
        {
            using var context = new SampleDbContext(dbOptions);

            var persons = context.Persons
                .AsAsyncEnumerable()
                .Where(person => person.LastName == "Doe");

            await foreach (var person in persons)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }
}