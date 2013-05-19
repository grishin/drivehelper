using DriveHelper.Example.Properties;
using DriveHelper.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveHelper.Example
{
    public class Program
    {
        public static void Main()
        {
            var service = new DriveHelperService(Settings.Default.GoogleDriveClientId,
                                                        Settings.Default.GoogleDriveClientSecret,
                                                        Settings.Default.GoogleDriveRefreshToken
                );


            Console.WriteLine("Uploading file");
            var stream = new System.IO.MemoryStream(File.ReadAllBytes("test.txt"));
            service.UploadFile(stream, "Drive.Example test.txt", "text/plain", "Test");
            Console.WriteLine("Done!");

            Console.ReadLine();
        }

            
    }
}
