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
            // If we want ANSI Support in the console, this is one way to do it
            // this may not work in all windows consoles or systems.  
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(65001);
            Console.CursorVisible = false;
            Console.Clear();

            // FIGLET~  YAy!
            var font = FigletFont.Load("fonts/serifcap.flf");
            AnsiConsole.Write(
                new FigletText(font, "File Backup")
                .Centered()
                .Color(Color.Blue));

            AddLineSpace(1);

            // HACK: We setup a few things so that we can render the spinner directly.
            // We need a place to send an IRenderable, but we also have to send it the DeltaTime to play
            // the animation.
            // This is taken from hacking apart the AnsiConsole.Progress setup, and using a progress 
            // column to render the spinner.
            var progressTask = new ProgressTask(1, "Gogogo", 100000, true);
            var spinnerList = typeof(Spinner.Known).GetProperties().OrderBy(s => s.Name).ToList();
            var startPosition = Console.GetCursorPosition().Top + 1;
            var spinnerCol = new SpinnerColumn();

            // Lets wait for key input, pressing the ESC key will break out of the loop and end the program.
            do
            {
                while (!Console.KeyAvailable)
                {
                    RenderSpinnerList(startPosition, progressTask, spinnerList, spinnerCol);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);


            // Add some some extra space below the rendered spinners when the program exits.
            AddLineSpace(1);
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
            //TODO: There's an exception in here if the console window isn't big enough to 
            //      render all the columns, or rows, or it resizes at the wrong time
            //      It probably needs to calculate, rather than have a hardcoded 3 columns
            //      or use columns / number of spinners to determine the rows.


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

                AddLineSpace(4);

            }
            AnsiConsole.MarkupLine("-- [bold yellow]Press the[/] [bold white][[ ESC ]][/] [bold yellow]key to exit --[/]");
            Thread.Sleep(50);
        }


        private static void RenderSpinner(int cursorLeftPosition, int cursorTopPosition, PropertyInfo spinner, ProgressTask task, SpinnerColumn spinnerColumn)
        {
            // The exception occures here during SetCursorPosition() - when it's outside the bounds of the window,
            // Maybe it could write into the buffer instead?
            Console.SetCursorPosition(cursorLeftPosition, cursorTopPosition);
            AnsiConsole.Write($"{spinner.Name} : ");
            spinnerColumn.Spinner = (Spinner)spinner.GetValue(spinner)!;
            AnsiConsole.Write(spinnerColumn.Render(RenderOptions.Create(AnsiConsole.Console), task, TimeSpan.FromMilliseconds(1)));


        }

    }
}