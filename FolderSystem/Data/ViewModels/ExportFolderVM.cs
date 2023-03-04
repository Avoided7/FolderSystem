namespace FolderSystem.Data.ViewModels;

public class ExportFolderVM
{
    public string Name { get; set; } = string.Empty;
    public byte[] Content { get; set; } = null!;
}