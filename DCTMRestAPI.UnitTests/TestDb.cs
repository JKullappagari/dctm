using System;
using DCTMRestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DCTMRestAPI.UnitTests
{
    /// <summary>
    /// Creates an isolated in-memory <see cref="DCTrackContext"/> per call. EF Core's InMemory
    /// provider is used instead of mocking DbSet/IQueryable, which is the recommended approach for
    /// exercising code that queries a DbContext (mocking async LINQ providers is brittle).
    /// </summary>
    internal static class TestDb
    {
        public static DCTrackContext NewContext()
        {
            var options = new DbContextOptionsBuilder<DCTrackContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DCTrackContext(options);
        }
    }
}
