using Microsoft.Extensions.FileProviders;

namespace FolderSystem.Models;

public class Folder
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FullPath { get; set; } = string.Empty;

    public int Capacity { get; set; }
    // Relations
    public IEnumerable<FileContext> Files { get; set; }
    public IEnumerable<Folder> Folders { get; set; }
    
    public int? BaseFolderId { get; set; }
    public Folder? BaseFolder { get; set; }
}