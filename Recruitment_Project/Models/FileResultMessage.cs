namespace Recruitment_Project.Models;

public class FileResultMessage
{
    public FileOperationType FileOperationType { get; set; }
    public string FileOperationName{ get; set; }
    public string Message { get; set; }

    public FileResultMessage(FileOperationType fileOperationType, string message)
    {
        FileOperationType = fileOperationType;
        FileOperationName = fileOperationType.ToString();
        Message = message;
    }
}