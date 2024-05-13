using Spectre;
using Spectre.Console;
using System.ComponentModel;

namespace SaveFileWatcher

{
    internal class Program
    {
        private static readonly string ProfileSaveName = "playerprofiles8.lsf";
        private static readonly string ProfileSavePath = "%USERPROFILE%\\AppData\\Local\\Larian Studios\\Baldur's Gate 3\\PlayerProfiles\\";
        private static readonly string HonorModeSaveName = "HonourMode.lsv";
        private static readonly string HonorModeSaveImage = "HonourMode.WebP";
        private static readonly string HonorModeSaveGamePath = "%USERPROFILE%\\AppData\\Local\\Larian Studios\\Baldur's Gate 3\\PlayerProfiles\\Public\\Savegames\\Story\\db54c19e-3939-8a4a-0930-96573098bf82__HonourMode\\";


        static void Main(string[] args)
        {
            Console.WriteLine("");
            AnsiConsole.MarkupLine("[bold blue]Starting file search...[/]");
            AnsiConsole.Status()
                   .AutoRefresh(true)
                   .Spinner(Spinner.Known.Bounce)
                   .SpinnerStyle(Style.Parse("green bold"))
                   .Start("Cool text here...", ctx =>
                   {
                       AnsiConsole.MarkupLine("Looking for the profile File...");
                       FindProfileFile(ctx);
                       Thread.Sleep(5000);

                       AnsiConsole.MarkupLine("Looking for the save game files");
                       FindSaveGameFiles(ctx);
                       Thread.Sleep(5000);

                       AnsiConsole.MarkupLine("[blue] - DONE - [/]");
                   });

            AnsiConsole.MarkupLine(" - [bold white] [[ Press any key to exit ]] [/] -");
            Console.ReadLine();
        }

        static void FindProfileFile(StatusContext context)
        {
            context.Status("[bold green]Looking for profile game files ...[/]");
            FileInfo profile = new(ProfileSavePath + ProfileSaveName);
            Thread.Sleep(5000);
            if (profile.Exists)
            {
                AnsiConsole.MarkupLine($"[bold green]Found - {ProfileSavePath}{ProfileSaveName}[/]");
                Thread.Sleep(5000);
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold red]Could not locate profile...[/]");
                Thread.Sleep(5000);
            }

        }

        static void FindSaveGameFiles(StatusContext context)
        {
            context.Status("[bold green]Looking for save game files ...[/]");
            FileInfo fiSave = new(HonorModeSaveGamePath + HonorModeSaveName);
            FileInfo fiImage = new(HonorModeSaveGamePath + HonorModeSaveImage);

            if (fiSave.Exists)
            {
                AnsiConsole.MarkupLine($"[bold green]Found - {HonorModeSaveGamePath}{HonorModeSaveName}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold red]Could not locate honor mode save path [/]");
                AnsiConsole.MarkupLine($"[bold red]This is a hard failure, the program cannot coninue[/]");
                return;
            }

            if (fiImage.Exists)
            {

                AnsiConsole.MarkupLine($"[bold green]Found - {HonorModeSaveGamePath}{HonorModeSaveImage}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold yellow]Could not locate the honor mode save image ...[/]");
                AnsiConsole.MarkupLine($"[bold yellow]This isn't a hard failure, the game can continue without this file...[/]");

            }
        }
    }
}
