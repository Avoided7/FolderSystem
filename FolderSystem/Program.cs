using FolderSystem.Data;
using FolderSystem.Services;
using FolderSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("FolderDbContext");
        builder.Services.AddDbContext<FolderDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        
        builder.Services.AddControllersWithViews();

        var app = builder.Build();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}