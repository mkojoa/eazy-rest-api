using Microsoft.EntityFrameworkCore;
using System;

namespace eazy.rest.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            if (!Database.CanConnect()) Database.EnsureCreated();
        }
    }
}
