namespace TravelTask.Extensions
{
    public static class FileConstants
    {
        public const string DestinationImagesFolderPath = "destinationimages";
    }

    public static class FormFileExtensions
    {

        private const string RootPath = "wwwroot";


        public async static Task<string> SaveImageAndProvidePathAsync(this IFormFile file,string path,IWebHostEnvironment env)
        {
            var folderPath = Path.Combine(env.ContentRootPath,RootPath, path);

            var fileName = Guid.NewGuid()+file.FileName;

            if(Directory.Exists(folderPath))
            {
                var newFile = File.Create(Path.Combine(folderPath,fileName));
                await file.CopyToAsync(newFile);
            }

            return fileName;
        }

    }
}
