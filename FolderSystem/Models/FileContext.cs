namespace FolderSystem.Models;

public class FileContext
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; }
    
    // Relations
    public int FolderId { get; set; }
    public Folder Folder { get; set; }
}