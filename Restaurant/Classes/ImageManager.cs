using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public class ImageManager
    {
        private static ImageManager _instance;
        private static readonly object _lock = new object();

        public static ImageManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ImageManager();
                    }
                    return _instance;
                }
            }
        }

        private ImageManager() { }

        public string CalculateImageHash(byte[] imageData)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(imageData);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public string FindImageByHash(string targetHash)
        {
            if (string.IsNullOrEmpty(targetHash)) return null;

            string[] possibleDirs = {
                Path.Combine(Application.StartupPath, "Resources", "image", "Menu"),
                Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu")
            };

            foreach (string dir in possibleDirs)
            {
                if (!Directory.Exists(dir)) continue;

                try
                {
                    foreach (string filePath in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => f.ToLower().EndsWith(".jpg") || f.ToLower().EndsWith(".jpeg") || f.ToLower().EndsWith(".png")))
                    {
                        try
                        {
                            byte[] fileData = File.ReadAllBytes(filePath);
                            string fileHash = CalculateImageHash(fileData);

                            if (fileHash == targetHash)
                            {
                                return filePath;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return null;
        }

        public string FindExistingImageByHash(string targetHash)
        {
            string imagePath = FindImageByHash(targetHash);
            return imagePath != null ? Path.GetFileName(imagePath) : null;
        }

        public string GetPlugImagePath()
        {
            string[] possiblePaths = {
                Path.Combine(Application.StartupPath, "Resources", "image", "plug.png"),
                Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "plug.png")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        public Image LoadImageFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        return Image.FromStream(fileStream);
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public Image LoadImageByHash(string photoHash)
        {
            if (string.IsNullOrEmpty(photoHash)) return null;

            string imagePath = FindImageByHash(photoHash);
            return imagePath != null ? LoadImageFromFile(imagePath) : null;
        }

        public async Task<Image> LoadImageByHashAsync(string photoHash)
        {
            return await Task.Run(() => LoadImageByHash(photoHash));
        }

        public string GenerateUniqueFileName(string baseName, string extension)
        {
            string[] possibleDirs = {
                Path.Combine(Application.StartupPath, "Resources", "image", "Menu"),
                Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu")
            };

            string fileName = baseName + extension;
            int counter = 1;

            foreach (string dir in possibleDirs)
            {
                if (!Directory.Exists(dir)) continue;

                while (File.Exists(Path.Combine(dir, fileName)))
                {
                    fileName = $"{baseName}({counter}){extension}";
                    counter++;
                }
            }

            return fileName;
        }

        public bool SaveImageToMenuDirectory(byte[] imageData, string fileName)
        {
            try
            {
                string sourceDir = Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu");
                string debugDir = Path.Combine(Application.StartupPath, "Resources", "image", "Menu");

                Directory.CreateDirectory(sourceDir);
                Directory.CreateDirectory(debugDir);

                string sourcePath = Path.Combine(sourceDir, fileName);
                string debugPath = Path.Combine(debugDir, fileName);

                File.WriteAllBytes(sourcePath, imageData);
                File.WriteAllBytes(debugPath, imageData);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateImageFile(string filePath)
        {
            try
            {
                string fileExtension = Path.GetExtension(filePath).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    return false;
                }

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > 3 * 1024 * 1024) 
                {
                    return false;
                }

                using (var image = Image.FromFile(filePath))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}