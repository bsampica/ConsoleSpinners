using System.Diagnostics;
using Spectre.Console;
using Exception = System.Exception;

namespace SaveFileWatcher

{
    internal class Program
    {
        private static readonly string ProfileSaveName = "playerprofiles8.lsf";

        private static readonly string ProfileSavePath =
            "%USERPROFILE%\\AppData\\Local\\Larian Studios\\Baldur's Gate 3\\PlayerProfiles\\";

        private static readonly string HonorModeSaveName = "HonourMode.lsv";
        private static readonly string HonorModeSaveImage = "HonourMode.WebP";

        private static readonly string HonorModeSaveGamePath =
            "%USERPROFILE%\\AppData\\Local\\Larian Studios\\Baldur's Gate 3\\PlayerProfiles\\Public\\Savegames\\Story\\cde0e96c-7bee-7aad-55c4-142d7afb149d__HonourMode\\";

        private static bool _runSuccess = true;

        static void Main(string[] args)
        {
            // TODO: Just to remove the warning about not being used.
            _ = args;

            AnsiConsole.MarkupLine(" ... Beginning Program ...");

            // TODO: Currently dumping the return of GetFileReference...do something with it.
            // Looking for the profile info
            var profileFileReference = GetFileReference(ProfileSavePath, ProfileSaveName);
            _ = TryToCopyFile(profileFileReference);

            
            // TODO: Currently dumping the return of GetFileReference...do something with it
            // looking for the save game files
            var honorModeSaveGameFile = GetFileReference(HonorModeSaveGamePath, HonorModeSaveName);
            var honorModeImageFile = GetFileReference(HonorModeSaveGamePath, HonorModeSaveImage);

            if (honorModeSaveGameFile.Exists)
            {
                AnsiConsole.MarkupLine("File was found...doing something important.");
                _runSuccess = TryToCopyFile(honorModeSaveGameFile);
            }
            else
            {
                _runSuccess = false;
            }
            
            //TODO: 
            if (honorModeImageFile.Exists)
            {
                AnsiConsole.MarkupLine("File was found...doing something important.");
                _runSuccess = TryToCopyFile(honorModeImageFile);
            }
            else
            {
                _runSuccess = false;
            }

            if (!_runSuccess)
            {
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine(
                    "[bold red]File backup could not be completed.  At least one of the files could not be found to backup.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to exit the program...");
            Console.ReadLine();
        }

        private static bool TryToCopyFile(FileInfo fi)
        {
            try
            {
                var guidPart = Guid.NewGuid().ToString()[..4];
                var tempFileName = fi.Name.Split('.')[0];
                var newFileName = $"{tempFileName}-{guidPart}{fi.Extension}.bak";
                var newFileInfo = fi.CopyTo(fi.DirectoryName + "\\" + newFileName);
                AnsiConsole.MarkupLine($"[bold green]File Copied : [[ {newFileName} ]][/]");
                Debug.WriteLine($"New File: {newFileInfo.FullName}");
                return true;
            }

            catch (Exception e)
            {
                //TODO:  Handle the exception in some way other than just moving on
                AnsiConsole.MarkupLine($"File could [bold red] NOT [/] be copied...");
                Debug.WriteLine(e.Message);
            }

            return true;
        }

        private static FileInfo GetFileReference(string filePath, string fileName)
        {
            var pathExpanded = Environment.ExpandEnvironmentVariables(filePath);
            var fullFileName = Path.Combine(pathExpanded, fileName);
            //AnsiConsole.MarkupLine(fullFileName);

            var fileInfoReference = new FileInfo(fullFileName);
            AnsiConsole.MarkupLine(
                !fileInfoReference.Exists
                    ? $"[bold red]File was not found: [/][bold white] [[ {fileInfoReference.Name} ]][/]"
                    : $"[bold green]File Found        : [/][bold white] [[ {fileInfoReference.Name} ]][/]");

            return fileInfoReference;
        }
    }
}