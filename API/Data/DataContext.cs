using System;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }
    protected DataContext()
    {
    }
    public DbSet<AppUser> Users { get; set; }
}
