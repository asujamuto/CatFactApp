using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Recruitment_Project.Models;

public class FileSettings
{
    public string fileName { get; set; }
    
    public string directoryPath { get; set; }
}
