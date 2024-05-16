using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

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

        static async Task Main(string[] args)
        {
            var font = FigletFont.Load("fonts/serifcap.flf");
            AnsiConsole.Write(
                new FigletText(font, "File Backup")
                .Centered()
                .Color(Color.Blue
                ));

            AddLineSpace(2);

            PropertyInfo[] spinners = typeof(Spinner.Known).GetProperties();
            await AnsiConsole.Status()
                 .AutoRefresh(true)
                 .StartAsync("Gogogo", async ctx =>
                 {
                    await Task.Delay(200);
                 });

            AddLineSpace(1);

            AnsiConsole.MarkupLine(" -- [bold green]Completed file backup...[/]");
            AnsiConsole.MarkupLine(" -- [bold yellow]Completed file backup, but with warnings...[/]");
            AnsiConsole.MarkupLine(" -- [red]Completed backup attempt.  Files could[/][bold red] *NOT* [/][red]be backed up...[/] ");
            AnsiConsole.MarkupLine("[blue] -- Exiting Program  --[/]");

            Console.ReadLine();

        }

        private static void AddLineSpace(int numberOfSpaces)
        {
            for (int i = 0; i <= numberOfSpaces; i++)
            {
                AnsiConsole.MarkupLine("");
            }
        }

    }
}