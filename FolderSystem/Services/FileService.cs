using FolderSystem.Data;
using FolderSystem.Data.ViewModels;
using FolderSystem.Models;
using FolderSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FolderSystem.Services;

public class FileService : IFileService
{
    private readonly FolderDbContext _dbContext;

    public FileService(FolderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddFileToFolder(AddFileToFolderVM file)
    {
        var newFile = new FileContext();
        newFile.FolderId = file.FolderId;
        newFile.Name = file.File.FileName;
        newFile.ContentType = file.File.ContentType;
        using (var reader = new StreamContent(file.File.OpenReadStream()))
        {
            var result = await reader.ReadAsByteArrayAsync();

            newFile.Content = new byte[file.File.Length];
            
            result.CopyTo(newFile.Content, 0);
        }

        _dbContext.Files.Add(newFile);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<FileContext?> GetFileByIdAsync(int id)
    {
        var file = await _dbContext.Files.SingleOrDefaultAsync(file => file.Id == id);

        if (file == null)
        {
            return null;
        }

        return file;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var file = await _dbContext.Files.SingleOrDefaultAsync(file => file.Id == id);

        if (file == null)
        {
            return null;
        }
        
        _dbContext.Files.Remove(file);
        await _dbContext.SaveChangesAsync();

        return file.FolderId;
    }
}