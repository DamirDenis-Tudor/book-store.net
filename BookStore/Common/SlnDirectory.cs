/**************************************************************************
 *                                                                        *
 *  Description: Utility class that return the sln relative path          *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System.Reflection;

namespace Common;

/// <summary>
/// Provides a method to get the path of the solution directory.
/// </summary>
public static class SlnDirectory
{
    /// <summary>
    /// Gets the path of the solution directory.
    /// </summary>
    /// <returns>The path of the solution directory.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the solution file is not found.</exception>
    public static string GetPath()
    {
        // Get the directory of the currently executing assembly
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Traverse up the directory hierarchy until a solution file is found
        while (!string.IsNullOrEmpty(directory))
        {
            // Check if there are any solution files in the current directory
            var solutionFiles = Directory.GetFiles(directory, "*.sln");
            if (solutionFiles.Length > 0)
            {
                // Return the current directory if a solution file is found
                return directory;
            }

            // Move up one directory level
            directory = Directory.GetParent(directory)?.FullName;
        }

        // Throw an exception if no solution file is found
        throw new FileNotFoundException("Solution file not found.");
    }
}