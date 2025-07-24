using Recruitment_Project.Models;

namespace Recruitment_Project.Services;

public interface IFileManagerService
{
        Task<CatFact> FetchCatFact();
        FileResultMessage SaveToFile(string fact);
        FileResultMessage RemoveFile();
        
        FileResultMessage GetFileContent();
        
        FileResultMessage SetFileName(string filename);
        
        FileSettings GetFileSettings();
        void SaveFileSettings(string fileName);
}