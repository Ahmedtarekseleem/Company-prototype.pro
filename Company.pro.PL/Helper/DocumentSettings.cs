namespace Company.pro.PL.Helper
{
    public static class DocumentSettings
    {

        public static string UploadFile(IFormFile file ,string folderName)
        {
            //string folderPath = "E:\\project\\Company.pro\\Company.pro.PL\\wwwroot\\files\\" + folderName;
            //var folderPAth = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            var fileName = $"{Guid.NewGuid()}{file.FileName}"; 

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new  FileStream(filePath , FileMode.Create);    

            file.CopyTo(fileStream);

            return fileName ;

        }
        
        public static void DeleteFile(string fileName , string folderName )
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName , fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            
        }
    }
}
