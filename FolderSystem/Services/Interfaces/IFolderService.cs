using FolderSystem.Data.ViewModels;
using FolderSystem.Models;

namespace FolderSystem.Services.Interfaces;

public interface IFolderService
{
    Task<Folder?> GetByIdAsync(int id);
    Task<IEnumerable<Folder>> GetAllAsync();
    Task<bool> AddAsync(AddFolderVM model);
    Task<int?> DeleteAsync(int id);
    Task ImportFromZipFile(IFormFile file);
    Task<ExportFolderVM?> ExportFolder(int id);
    Task<bool> ClearAsync(int id);
}