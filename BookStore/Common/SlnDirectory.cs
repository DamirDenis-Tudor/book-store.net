using System.Reflection;

namespace Common;

public static class SlnDirectory
{
    public static string GetPath()
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        while (!string.IsNullOrEmpty(directory))
        {
            var solutionFiles = Directory.GetFiles(directory, "*.sln");
            if (solutionFiles.Length > 0)
                return directory;
            
            directory = Directory.GetParent(directory)?.FullName;
        }
        
        throw new FileNotFoundException("Solution file not found.");
    }
}