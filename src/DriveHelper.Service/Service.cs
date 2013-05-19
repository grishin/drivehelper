using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveHelper.Service
{
    public class DriveHelperService
    {
        public DriveService DriveService { get; private set; }

        public DriveHelperService(string clientId, string clientSecret, string refreshToken)
        {
            Auth.ClientId = clientId;
            Auth.ClientSecret = clientSecret;
            Auth.RefreshToken = refreshToken;
            DriveService = Auth.CreateDriveService();
        }

        public void UploadFile(System.IO.MemoryStream stream, string title, string mimeType, string folder="")
        {
            File body = new File();
            body.Title = title;
            body.MimeType = mimeType;

            if (!String.IsNullOrEmpty(folder)) {
                var folderRequest = DriveService.Files.List();
                folderRequest.Q = String.Format("title = '{0}'", folder);
                var folders = folderRequest.Fetch();

                if (folders.Items.Count == 0) { throw new Exception("Folder not found"); }

                var folderId = folders.Items[0].Id;
                if (!String.IsNullOrEmpty(folderId)) {
                    body.Parents = new List<ParentReference>() { new ParentReference() { Id = folderId } };
                }
            }

            FilesResource.InsertMediaUpload request = DriveService.Files.Insert(body, stream, mimeType);
            request.Upload();
        }
    }
}
