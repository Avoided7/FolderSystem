using System.ComponentModel.DataAnnotations;

namespace FolderSystem.Data.ViewModels;

public class AddFileToFolderVM
{
    [Required]
    public int FolderId { get; set; }
    [Required]
    public IFormFile File { get; set; }
}