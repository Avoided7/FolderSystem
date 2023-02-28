using FolderSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderSystem.Data;

public class FolderDbContext : DbContext
{
    public DbSet<Folder> Folders { get; set; }
    public DbSet<FileContext> Files { get; set; }
    public FolderDbContext(DbContextOptions<FolderDbContext> options) : base(options) { }
}