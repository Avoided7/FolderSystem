using System.Text;
using System.Text.RegularExpressions;
using FolderSystem.Data;
using FolderSystem.Data.ViewModels;
using FolderSystem.Models;
using FolderSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FolderSystem.Services;

public class FolderService : IFolderService
{
    private readonly FolderDbContext _dbContext;

    public FolderService(FolderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Folder?> GetByIdAsync(int id)
    {
        return await _dbContext.Folders
            .Include(f => f.Files)
            .Include(f => f.Folders)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Folder>> GetAllAsync()
    {
        return await _dbContext.Folders.AsNoTracking().ToListAsync();
    }

    public async Task<bool> AddAsync(AddFolderVM model)
    {
        var baseFolder = await _dbContext.Folders.SingleOrDefaultAsync(folder => folder.Id == model.BaseFolderId);

        if (baseFolder == null)
        {
            return false;
        }

        model.Name = model.Name.Trim();

        while (model.Name.LastOrDefault() == '\0')
        {
            model.Name = String.Join("", model.Name.SkipLast(1));
        }
        
        if (model.Name.IsNullOrEmpty())
        {
            return false;
        }
        
        var capacity = baseFolder.Capacity + 1;

        var fullPath = (baseFolder.Id == 1 ? baseFolder.FullPath + model.Name : baseFolder.FullPath + "/" + model.Name);

        var suffix = "";
        var countFolders = 0;
        
        while (_dbContext.Folders.Any(folder => folder.FullPath == fullPath + suffix))
        {
            countFolders++;
            suffix = $" ({countFolders})";
        }
        
        var folder = new Folder
        {
            Name = model.Name + suffix,
            BaseFolderId = model.BaseFolderId,
            FullPath = fullPath + suffix,
            Capacity = capacity
        };
        await _dbContext.Folders.AddAsync(folder);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        if (id == 1)
        {
            return null;
        }
        
        var currentFolder = await _dbContext.Folders
                                             .Include(folder => folder.Folders)
                                             .Include(folder => folder.Files)
                                             .SingleOrDefaultAsync(folder => folder.Id == id);

        if (currentFolder == null)
        {
            return null;
        }
        
        foreach (var folder in currentFolder.Folders)
        {
            await DeleteAsync(folder.Id);
        }

        foreach (var file in currentFolder.Files)
        {
            _dbContext.Files.Remove(file);
        }
        
        _dbContext.Remove(currentFolder);
        
        await _dbContext.SaveChangesAsync();

        return currentFolder.BaseFolderId;
    }

    public async Task<bool> ClearAsync(int id)
    {
        var currentFolder = await _dbContext.Folders
            .Include(folder => folder.Folders)
            .Include(folder => folder.Files)
            .SingleOrDefaultAsync(folder => folder.Id == id);

        if (currentFolder == null)
        {
            return false;
        }
        
        foreach (var folder in currentFolder.Folders)
        {
            await DeleteAsync(folder.Id);
        }

        foreach (var file in currentFolder.Files)
        {
            _dbContext.Files.Remove(file);
        }
        
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<int> ImportFromFile(IFormFile file)
    {
        int addedCount = 0;
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            var regex = new Regex(@"^\/(.+)(\/([^\/]*)|)$");
            
            var result = reader
                                    .ReadToEnd()
                                    .Replace('\r', '\0')
                                    .Split('\n')
                                    .Select(folder => folder
                                                                 .Split('/')
                                                                 .Select(part => part.Trim())
                                                                 .Where(part => !part.IsNullOrEmpty())
                                                                 .ToList());
            foreach (var path in result)
            {
                if (!regex.IsMatch("/" + string.Join("/", path)) || path.Any(part => part.Contains('/')))
                {
                    continue;
                }
                int previousFolderId = 1;
                for (int index = 0; index < path.Count(); index++)
                {
                    var fullPath = "/" + string.Join("/", path.Take(index + 1));
                    
                    var folder = _dbContext.Folders.FirstOrDefault(folder => folder.Capacity == index + 1 &&
                                                       folder.FullPath == fullPath);
                    
                    if (folder == null)
                    {
                        addedCount++;
                        await AddAsync(new AddFolderVM
                        {
                            Name = path.ElementAt(index),
                            BaseFolderId = previousFolderId
                        });
                        folder = _dbContext.Folders.First(iterFolder => iterFolder.Capacity == index + 1 &&
                                                                        iterFolder.Name == path.ElementAt(index));
                    }

                    previousFolderId = folder.Id;
                }
            }
        }
        return addedCount;

    }

    public async Task<ExportFolderVM?> ExportFolder(int id)
    {
        var currentFolder = await _dbContext.Folders.SingleOrDefaultAsync(folder => folder.Id == id);

        if (currentFolder == null)
        {
            return null;
        }
        
        List<string> allPathes = new List<string>();

        async Task GetAllFolders(int folderId)
        {
            var folder = await _dbContext.Folders
                .Include(folder => folder.Folders)
                .AsNoTracking()
                .SingleAsync(folder => folder.Id == folderId);

            allPathes.Add(folder.FullPath);
            foreach (var iterFolder in folder.Folders)
            {
                await GetAllFolders(iterFolder.Id);
            }
        }

        await GetAllFolders(id);

        var exportVM = new ExportFolderVM
        {
            Name = currentFolder.Name,
            Content = Encoding.UTF8.GetBytes(string.Join("\n", allPathes))
        };
        
        return exportVM;
    }
}