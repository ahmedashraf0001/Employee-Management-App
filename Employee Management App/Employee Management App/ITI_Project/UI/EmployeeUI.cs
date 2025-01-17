using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_Project.UI
{
    internal static class EmployeeUI
    {
        public static void MenuGenerator(string str, string[] menu, int xdistFactor, int indexer)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(str);

            int xdist = Console.WindowWidth / 2;
            int ydist = Console.WindowHeight / (menu.Length + 1);
            for (int i = 0; i < menu.Length; i++)
            {
                
                Console.BackgroundColor = i == indexer ? ConsoleColor.Blue : ConsoleColor.Black;
                if (menu[i] == " Clear Database (Caution) " && i == indexer)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                Console.SetCursorPosition(xdist - xdistFactor, (i + 1) * ydist);
                Console.Write(menu[i]);
            }
        }
        public static void MenuDisplay(string title, string[] innerMenu, Action[] actions)
        {
            int innerIndex = 0;
            bool innerLooping = true;

            do
            {
                MenuGenerator(title, innerMenu, 4, innerIndex);
                ConsoleKeyInfo info = Console.ReadKey();

                switch (info.Key)
                {
                    case ConsoleKey.UpArrow: innerIndex = innerIndex == 0 ? innerMenu.Length - 1 : innerIndex - 1; break;
                    case ConsoleKey.DownArrow: innerIndex = innerIndex == innerMenu.Length - 1 ? 0 : innerIndex + 1; break;
                    case ConsoleKey.Home: innerIndex = 0; break;
                    case ConsoleKey.End: innerIndex = innerMenu.Length - 1; break;
                    case ConsoleKey.Escape: innerLooping = false; break;
                    case ConsoleKey.Enter:
                        if (innerIndex < actions.Length - 1)
                        {
                            actions[innerIndex]?.Invoke();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            innerLooping = false;
                        }
                        break;
                }
            } while (innerLooping);
        }
        public static void MainMenu(Action[] _actions)
        {
            string[] mainMenu = new[] { " New ", " Edit ", " Delete ", " Search ", " Display ", " Exit " };
            Action[] actions = _actions;
            MenuDisplay(" Employee Management App : ", mainMenu, actions);
        }
    }
}
