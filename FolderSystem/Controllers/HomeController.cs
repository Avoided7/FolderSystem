using FolderSystem.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FolderSystem.Services.Interfaces;

namespace FolderSystem.Controllers;

public class HomeController : Controller
{
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;

    public HomeController(IFileService fileService,
                          IFolderService folderService)
    {
        _fileService = fileService;
        _folderService = folderService;
    }

    #region Main Page
    
    public async Task<IActionResult> Index(int id = 1)
    {
        var folder = await _folderService.GetByIdAsync(id);

        if (folder == null)
        {
            return BadRequest();
        }
        
        return View(folder);
    }
    
    #endregion

    #region All Folders

    [HttpGet]
    public async Task<IActionResult> AllFolders()
    {
        var folders = await _folderService.GetAllAsync();

        return View(folders);
    }

    #endregion
    
    #region Import From File

    [HttpGet]
    public IActionResult Import()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file.ContentType != "text/plain")
        {
            ModelState.AddModelError("", "Incorrect file format.");
            return View();
        }
        
        var addedCount = await _folderService.ImportFromFile(file);

        Console.WriteLine(addedCount);
        
        return RedirectToAction(nameof(Index));
    }
    
    #endregion

    #region Export to File

    public async Task<IActionResult> ExportToFile(int id)
    {
        var folderInfo = await _folderService.ExportFolder(id);

        if (folderInfo == null)
        {
            return BadRequest();
        }
        
        return File(folderInfo.Content, "text/plain", folderInfo.Name + ".txt");
    }

    #endregion
    
    #region Add File to Folder

    [HttpGet]
    public IActionResult AddFileToFolder(int id = 1)
    {
        ViewData["FolderId"] = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddFileToFolder(AddFileToFolderVM addVM)
    {
        var result = await _fileService.AddFileToFolder(addVM);

        if (!result)
        {
            return BadRequest();
        }
        
        return RedirectToAction(nameof(Index), new { id = addVM.FolderId });
    }

    #endregion

    #region Add Folder
    
    [HttpGet]
    public IActionResult AddFolder(int id = 1)
    {
        ViewData["ID"] = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddFolder(AddFolderVM folder)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ID"] = folder.BaseFolderId;
            return View(folder);
        }
        
        var result = await _folderService.AddAsync(folder);

        if (!result)
        {
            return BadRequest();
        }
        
        return RedirectToAction(nameof(Index), new { id = folder.BaseFolderId });
    }

    #endregion

    #region Download File
    
    [HttpGet]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var file = await _fileService.GetFileByIdAsync(id);

        if (file == null)
        {
            return BadRequest();
        }

        return File(file.Content, file.ContentType, file.Name);
    }

    #endregion

    #region Delete File
    
    [HttpGet]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var result = await _fileService.DeleteAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index), new { id = result });
    }

    #endregion

    #region Delete Folder

    [HttpGet]
    public async Task<IActionResult> DeleteFolder(int id)
    {
        var result = await _folderService.DeleteAsync(id);

        if (result == null)
        {
            return NotFound();
        }
        
        return RedirectToAction(nameof(Index), new { id = result });
    }
    
    #endregion

    #region Clear Folder
    
    public async Task<IActionResult> ClearFolder(int id)
    {
        var result = await _folderService.ClearAsync(id);

        if (!result)
        {
            return BadRequest();
        }

        return RedirectToAction(nameof(Index), new { id });
    }
    
    #endregion
}