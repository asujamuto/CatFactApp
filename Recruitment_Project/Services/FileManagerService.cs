using System.Text.Json;
using Microsoft.Extensions.Options;
using Recruitment_Project.Models;

namespace Recruitment_Project.Services;

public class FileManagerService : IFileManagerService
{

   private string _fileSettings;
   private readonly HttpClient _httpClient;


   public FileManagerService(HttpClient httpClient, IConfiguration configuration)
   {
      _httpClient = httpClient;
      _fileSettings = configuration["FileSettings"];
   }

   public async Task<CatFact> FetchCatFact()
   {
      try
      {
         var res = await _httpClient.GetAsync("https://catfact.ninja/fact");

         if (!res.IsSuccessStatusCode)
            return null;

         var json = await res.Content.ReadAsStringAsync();

         CatFact catFact = JsonSerializer.Deserialize<CatFact>(json, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });

         return catFact;
      }
      catch (Exception ex)
      {
         // Optional: log the error
         Console.WriteLine($"Error fetching cat fact: {ex.Message}");
         return null;
      }
   }

   public FileResultMessage SaveToFile(string fact)
   {
      FileSettings fileSettings = this.GetFileSettings();
      try {
         
         if (!Directory.Exists(fileSettings.directoryPath))
            Directory.CreateDirectory(fileSettings.directoryPath);
         
         //Gdy plik nie istnieje, StreamWriter sam utworzy dany plik
         using (StreamWriter outputFile =
                new StreamWriter(Path.Combine(fileSettings.directoryPath, fileSettings.fileName), append: true)) 
         {
            outputFile.WriteLine(fact);
         }

         return new FileResultMessage(
            FileOperationType.Updated, 
            $"Message:\n{fact}\nwas successfully added to {fileSettings.directoryPath}/{fileSettings.fileName}"
            );
      } 
      catch (DirectoryNotFoundException ex)
      {
         /* Jeśli nie został katalog utworzony, 
            stwórz i zapisz. 
            Dodałem poniższe wyjątki dla pokazania alternatywy dla "ifów".
          */
         Console.WriteLine(ex.Message);
         return new FileResultMessage(FileOperationType.NotFound,  "Catalog was not found");
      }
      catch (FileNotFoundException ex)
      {
         Console.WriteLine(ex.Message);
         return new FileResultMessage(FileOperationType.NotFound, "File was not found");
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
         return new FileResultMessage(FileOperationType.Error, ex.Message);
      }
   }

   public FileResultMessage RemoveFile()
   {
      FileSettings fileSettings = this.GetFileSettings();
      try
      {
         string fullPath = Path.Combine(fileSettings.directoryPath, fileSettings.fileName);
         if (!File.Exists(fullPath))
            return new FileResultMessage(FileOperationType.NotFound, "File doesn't exist; Nothing To Remove");
         
         File.Delete(fullPath);
         return new FileResultMessage(FileOperationType.Deleted, "File has been removed");
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
         return new FileResultMessage(FileOperationType.Error, ex.Message);
      }
   }

   public FileResultMessage GetFileContent()
   {
      FileSettings fileSettings = this.GetFileSettings();
      try
      { 
         string fullPath = Path.Combine(fileSettings.directoryPath, fileSettings.fileName);
         if (!File.Exists(fullPath)) {
            return new FileResultMessage(FileOperationType.NotFound, "File was not found");
         }
         
         string content = File.ReadAllText(fullPath);
         return new FileResultMessage(FileOperationType.Fetched, content);
         
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
         return new FileResultMessage(FileOperationType.NotFound, ex.Message);
      }
   }

   public FileResultMessage SetFileName(string filename)
   {
      FileSettings fileSettings = this.GetFileSettings();
      this.SaveFileSettings(filename);
      Console.WriteLine("_fileName teraz to: " + filename);
      return new FileResultMessage(FileOperationType.FileNameReplaced, "File name has been replaced");
   }

   public FileSettings GetFileSettings()
   {
      try
      {
         using StreamReader r = new StreamReader(_fileSettings);
         string json = r.ReadToEnd();

         var options = new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         };

         return JsonSerializer.Deserialize<FileSettings>(json, options);
      }
      catch (Exception ex)
      {
         Console.WriteLine("Error reading settings: " + ex.Message);
         return null; 
      }
   }

   public void SaveFileSettings(string fileName)
   {
      try
      {
         var settings = new FileSettings
         {
            fileName = fileName,
            directoryPath = "./Files"
         };

         var options = new JsonSerializerOptions
         {
            WriteIndented = true
         };

         string json = JsonSerializer.Serialize(settings, options);

         File.WriteAllText(_fileSettings, json);
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
      }
   }
}