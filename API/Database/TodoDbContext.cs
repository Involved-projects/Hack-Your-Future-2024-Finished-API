using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Database;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}