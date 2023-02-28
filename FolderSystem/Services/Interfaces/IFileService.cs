using FolderSystem.Data.ViewModels;
using FolderSystem.Models;

namespace FolderSystem.Services.Interfaces;

public interface IFileService
{
    Task<bool> AddFileToFolder(AddFileToFolderVM file);
    Task<FileContext?> GetFileByIdAsync(int id);
    Task<int?> DeleteAsync(int id);
}