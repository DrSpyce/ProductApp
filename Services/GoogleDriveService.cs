using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class GoogleDriveService
    {
        private DriveService _driveService;

        public GoogleDriveService()
        {
            // Load service account credentials
            var credential = GoogleCredential.FromFile("client_secret.json")
                .CreateScoped(DriveService.ScopeConstants.DriveFile);

            // Create Drive service with service account credentials
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YourAppName"
            });
        }

        public List<GoogleDriveItem>? ListAllFilesAndFolders()
        {
            try
            {
                // Retrieve all files and folders in Drive
                var request = _driveService.Files.List();
                //request.Fields = "files(id, name, mimeType), nextPageToken";
                var result = request.Execute();

                var items = new List<GoogleDriveItem>();

                // Process each file/folder in the result
                foreach (var file in result.Files)
                {
                    var item = new GoogleDriveItem
                    {
                        Id = file.Id,
                        Name = file.Name,
                        IsFolder = string.Equals(file.MimeType, "application/vnd.google-apps.folder", StringComparison.OrdinalIgnoreCase)
                    };

                    items.Add(item);
                }

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        
        public async Task<string> UploadImageAsync(IFormFile file, string? name)
        {
            var lastIndex = file.FileName.LastIndexOf('.');
            // Create file metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = string.Concat(name, file.FileName.Substring(lastIndex)),
                MimeType = file.ContentType,
                Parents = new List<string>() { "1K4y_XBaUZjUoqsi7QNW34Km3RA4jLer5" }
            };
            
            // Upload image to Google Drive
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                await request.UploadAsync();
                var uploadedFile = request.ResponseBody;
                var permission = new Permission { Type = "anyone", Role = "reader" };
                _driveService.Permissions.Create(permission, uploadedFile.Id).Execute();
                var fileLink = $"https://drive.google.com/uc?id={uploadedFile.Id}";
                // Return the file ID
                return fileLink;
            }
        }

        public class GoogleDriveItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsFolder { get; set; }
        }
    }
}
