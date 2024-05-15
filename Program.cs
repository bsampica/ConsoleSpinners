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
        private static bool _dryRun = false;

        static void Main(string[] args)
        {
            var font = FigletFont.Load("fonts/serifcap.flf");
            AnsiConsole.Write(
                new FigletText(font, "Begin Program")
                .Centered()
                .Color(Color.BlueViolet));
            
            AddLineSpace(2);

            AnsiConsole.Status()
                .Start("Searching for files...", c =>
                {
                    AnsiConsole.MarkupLine("Found Something...");
                    Thread.Sleep(5000);

                    AnsiConsole.MarkupLine("Found Something Else...");
                    Thread.Sleep(5000);
                });
        }

        static void OldMain(string[] args)
        {
            Console.Clear();

            AddLineSpace(1);
            AnsiConsole.Write(
                new FigletText("Honor-Mode Backup")
                    .Centered()
                    .Color(Color.Blue));

            AnsiConsole.MarkupLine("[blue bold] Beginning Program ...[/]");

            if (args == null || args.Length == 0)
            {
                var argList = new List<string>() { "true" };
                args = [.. argList];
                PrintHelpText();
                AnsiConsole.MarkupLine(" -- [blue]No Arguments provided, assuming[/][default] : Dry Run =[/] [bold white] [[ true ]] [/]");
            }

            var argParse = bool.TryParse(args[0], out _dryRun);

            if (!argParse)
            {
                AnsiConsole.MarkupLine($"[blue]Input arguments were not understood, ignoring...[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($" -- [blue]Running with arguments[/] [default]         : Dry Run =[/] [bold white] [[ {args[0]} ]][/]");
            }

            AddLineSpace(1);

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
                AnsiConsole.MarkupLine(" -- File was found...doing something important.");
                _runSuccess = TryToCopyFile(honorModeSaveGameFile);
            }
            else
            {
                _runSuccess = false;
            }

            //TODO: 
            if (honorModeImageFile.Exists)
            {
                AnsiConsole.MarkupLine(" -- File was found...doing something important.");
                _runSuccess = TryToCopyFile(honorModeImageFile);
            }
            else
            {
                _runSuccess = false;
            }

            if (!_runSuccess)
            {
                AddLineSpace(1);
                AnsiConsole.MarkupLine(
                    " -- [bold red]File backup could not be completed.  At least one of the files could not be found to backup.[/]");
            }

            AddLineSpace(1);
            AnsiConsole.MarkupLine("[blue] --- Press any key to exit the program --- [/]");
            Console.ReadLine();
        }

        private static void AddLineSpace(int numberOfSpaces)
        {
            for (int i = 0; i <= numberOfSpaces; i++)
            {
                AnsiConsole.MarkupLine("");
            }
        }

        private static bool TryToCopyFile(FileInfo fi)
        {
            try
            {
                if (_dryRun) return true;
                var guidPart = Guid.NewGuid().ToString()[..4];
                var tempFileName = fi.Name.Split('.')[0];
                var newFileName = $"{tempFileName}-{guidPart}{fi.Extension}.bak";
                var newFileInfo = fi.CopyTo(fi.DirectoryName + "\\" + newFileName);
                AnsiConsole.MarkupLine($" -- [bold green]File Copied : [[ {newFileName} ]][/]");
                Debug.WriteLine($" ---- New File: {newFileInfo.FullName}");
                return true;
            }

            catch (Exception e)
            {
                //TODO:  Handle the exception in some way other than just moving on
                AnsiConsole.MarkupLine($" -- File could [bold red] NOT [/] be copied...");
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
                    ? $" -- [bold red]File was not found : [/][bold white] [[ {fileInfoReference.Name} ]][/]"
                    : $" -- [bold green]File Found         : [/][bold white] [[ {fileInfoReference.Name} ]][/]");

            return fileInfoReference;
        }

        private static void PrintHelpText()
        {
            AddLineSpace(1);
            AnsiConsole.MarkupLine("[bold white] Parameters that can be used are --dry-run [[boolean:true:false]][/]");
            AddLineSpace(1);
        }
    }

}