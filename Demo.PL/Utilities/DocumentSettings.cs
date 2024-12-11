namespace Demo.PL.Utilities
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {

            // Images 
            //1. Create FolderPath 
            // D:\\newFolder\MvcDemo\Demo.Pl\wwwroot\files\folderName
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\"+folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);
            //2.Create Unique File Name 
            string fileName = $"{Guid.NewGuid()}-{file.FileName}";
            //3. Create FIle Path 
            // D:\\newFolder\MvcDemo\Demo.Pl\wwwroot\files\folderName\File Name
            string filePath = Path.Combine(folderPath, fileName);
            //4. Create File Stream to Save the file as data per time 
            using var stream = new FileStream(filePath, FileMode.Create);
            //5. Copy the file to file stream 
            await file.CopyToAsync(stream);
            //6. Return File Name 
            return fileName;

        }

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
