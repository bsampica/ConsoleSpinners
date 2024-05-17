using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SaveFileWatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(65001);
            Console.CursorVisible = false;
            Console.Clear();


            var font = FigletFont.Load("fonts/serifcap.flf");
            AnsiConsole.Write(
                new FigletText(font, "File Backup")
                .Centered()
                .Color(Color.Blue));

            AddLineSpace(1);

            var progressTask = new ProgressTask(1, "Gogogo", 100000, true);
            var spinnerList = typeof(Spinner.Known).GetProperties().OrderBy(s => s.Name).ToList();
            var startPosition = Console.GetCursorPosition().Top + 1;
            var spinnerCol = new SpinnerColumn();



            do
            {
                while (!Console.KeyAvailable)
                {
                    RenderSpinnerList(startPosition, progressTask, spinnerList, spinnerCol);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);


            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine("");
            AnsiConsole.MarkupLine("[bold blue] -- Exiting Program -- [/]");

        }

        private static void AddLineSpace(int numberOfSpaces)
        {
            for (int i = 0; i <= numberOfSpaces; i++)
            {
                AnsiConsole.MarkupLine("");
            }
        }

        private static void RenderSpinnerList(int startPosition, ProgressTask progressTask, List<PropertyInfo> spinnerList, SpinnerColumn spinnerCol)
        {

            var pos = startPosition;
            var col = 0;


            for (int spinIndex = 0; spinIndex < spinnerList.Count();)
            {
                if (col == 0)
                {
                    RenderSpinner(1, pos, spinnerList[spinIndex++], progressTask, spinnerCol);
                    col++;
                }

                if (col == 1)
                {
                    RenderSpinner(40, pos, spinnerList[spinIndex++], progressTask, spinnerCol);
                    col++;
                }

                if (col == 2)
                {
                    RenderSpinner(80, pos, spinnerList[spinIndex++], progressTask, spinnerCol);
                    pos++;
                    col = 0;
                }
            }
           
            Thread.Sleep(50);
        }


        private static void RenderSpinner(int cursorLeftPosition, int cursorTopPosition, PropertyInfo spinner, ProgressTask task, SpinnerColumn spinnerColumn)
        {
            Console.SetCursorPosition(cursorLeftPosition, cursorTopPosition);
            AnsiConsole.Write($"{spinner.Name} : ");
            spinnerColumn.Spinner = (Spinner)spinner.GetValue(spinner)!;
            AnsiConsole.Write(spinnerColumn.Render(RenderOptions.Create(AnsiConsole.Console), task, TimeSpan.FromMilliseconds(1)));


        }

    }
}