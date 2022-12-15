using System;
using System.Linq;
using System.Collections.Generic;

namespace TabloidCLI.Models
{
    public class ThemeEditor
    {
        private static readonly List<ConsoleColor> _colors = new((ConsoleColor[] )Enum.GetValues(typeof(ConsoleColor))); // Creates an array of console colors, then converts it to a list -- Shorthand for new List<ConsoleColor>
        public static ConsoleColor BackgroundColor => Console.BackgroundColor; // Current background color, computed when called
        public static ConsoleColor ForegroundColor => Console.ForegroundColor; // Current foreground color, computed when called
        public static void ExecuteMenu()
        {
            Console.Clear();
            Console.WriteLine(ASCII.ThemeEditor);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Theme Menu");
            Console.WriteLine(" 1) Edit Background Color");
            Console.WriteLine(" 2) Edit Foreground (Text) Color");
            Console.WriteLine(" 3) Reset Theme");
            Console.WriteLine(" 0) Go Back"); // Happens on any input besides 1 || 2

            Console.WriteLine("> ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                ChangeBackground();
            }
            else if (choice == "2")
            {
                ChangeForeground();
            }
            else if (choice == "3")
            {
                Console.ResetColor(); //! Restores original color theme
                ExecuteMenu();
            }
        }
        private static void ChangeBackground()
        {
            Console.Clear();
            Console.WriteLine(ASCII.Background);
            Console.WriteLine();

            List<ConsoleColor> availableBackgroundColors = _colors.Where(x => x != BackgroundColor && x != ForegroundColor).ToList();

            foreach(ConsoleColor color in availableBackgroundColors)
            {
                Console.WriteLine($" {availableBackgroundColors.IndexOf(color) + 1}) {color}");
            }
            Console.WriteLine(" 0) Go Back");

            Console.WriteLine();
            Console.WriteLine("Select an option");
            Console.Write("> ");
            
            int? choice = null; // Nullable integer declaration

            while (choice == null)
            {
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    continue;
                }
            }

            if (choice > 0 && choice - 1 < availableBackgroundColors.Count)
            {
                Console.BackgroundColor = availableBackgroundColors[Convert.ToInt32(choice) - 1]; // Converted because can't perform operations with nullable int and int together
            }
            ExecuteMenu(); // "Go Back"
        }
        private static void ChangeForeground()
        {
            Console.Clear();
            Console.WriteLine(ASCII.Foreground);
            Console.WriteLine();

            List<ConsoleColor> availableForegroundColors = _colors.Where(x => x != BackgroundColor && x != ForegroundColor).ToList();

            foreach (ConsoleColor color in availableForegroundColors)
            {
                Console.WriteLine($" {availableForegroundColors.IndexOf(color) + 1}) {color}");
            }
            Console.WriteLine(" 0) Go Back");

            Console.WriteLine();
            Console.WriteLine("Select an option");
            Console.Write("> ");

            int? choice = null; // Nullable integer declaration

            while (choice == null)
            {
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    continue;
                }
            }

            if (choice > 0 && choice - 1 < availableForegroundColors.Count)
            {
                Console.ForegroundColor = availableForegroundColors[Convert.ToInt32(choice) - 1]; // Converted because can't perform operations with nullable int and int together
            }
            ExecuteMenu(); // "Go Back"
        }
    }
}
