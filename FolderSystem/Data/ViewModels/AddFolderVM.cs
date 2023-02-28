using System.ComponentModel.DataAnnotations;

namespace FolderSystem.Data.ViewModels;

public class AddFolderVM
{
    [Required]
    public int BaseFolderId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}