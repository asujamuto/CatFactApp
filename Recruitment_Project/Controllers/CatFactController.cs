using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Recruitment_Project.Models;
using Recruitment_Project.Services;

namespace Recruitment_Project.Controllers;

[ApiController]
[Route("[controller]")]
public class CatFactController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IFileManagerService _fileManagerService;

    public CatFactController( IFileManagerService fileManagerService, IConfiguration configuration)
    {
        _fileManagerService = fileManagerService;
        _configuration = configuration;
    }

    [Route("/")]
    public IActionResult Home()
    {
      return View();  
    } 
     
    [HttpGet]
    [Route("/appendCatFact")]
    public async Task<IActionResult> Index()
    {
        CatFact catFact = await _fileManagerService.FetchCatFact();
        if (catFact?.fact == null) return StatusCode(404, "CatFact Not Found");
        
        FileResultMessage result = _fileManagerService.SaveToFile(catFact.fact); 
        if (result.FileOperationType == FileOperationType.Error) return StatusCode(404, result.Message);
        
        return Ok(result);
    }

    [Route("/fetchFile")]
    public IActionResult FetchFile()
    {
        FileResultMessage res = _fileManagerService.GetFileContent();
        if (res.FileOperationType == FileOperationType.NotFound) 
            return NotFound(res);
        if (res.FileOperationType == FileOperationType.Error)
            return StatusCode(500, res);
        return Ok(res);
    }
    
    [Route("/removeCatFact")]
    public IActionResult Remove()
    {
       FileResultMessage res = _fileManagerService.RemoveFile(); 
       if (res.FileOperationType == FileOperationType.Error) return NotFound(res);
       
       return Ok(res);
    }

    [Route("/updateFileName")]
    public IActionResult UpdateFileName([FromBody] FileNameRequest fileName)
    {
        FileResultMessage res = _fileManagerService.SetFileName(fileName.fileName);
        return Ok(res);
    }
    
}