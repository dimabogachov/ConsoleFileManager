using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 100;
            Console.WindowHeight = 45;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            Actions.StartApp();

            while (true)
            {
                try
                {
                    Parser.TryParseComandLine(Console.ReadLine());

                    if (Parser.Comand == Commands.Ls)
                    {
                        Actions.CreateListsOfFilesAndDirectories();
                        ConsoleWindow.InfoText = Actions.GetDefaultInfo();
                    }

                    else if (Parser.Comand == Commands.Cp)
                    {
                        Actions.CopyFileOrDirectory();
                        Actions.CreateListsOfFilesAndDirectories();
                        ConsoleWindow.InfoText = Actions.GetDefaultInfo();
                    }

                    else if (Parser.Comand == Commands.Rm)
                    {
                        ConsoleWindow.Draw();
                        Warning.Caution();
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Actions.FileOrDirectory();
                        }
                        Actions.CreateListsOfFilesAndDirectories();
                        ConsoleWindow.InfoText = Actions.GetDefaultInfo();
                    }

                    else if (Parser.Comand == Commands.Inf)
                    {
                        ConsoleWindow.InfoText = Actions.GetAskedInfo();
                    }

                    else if (Parser.Comand == Commands.Help)
                    {
                        ConsoleWindow.InfoText = Actions.GetHelp();
                    }

                    else if (Parser.Comand == Commands.Exit)
                    {
                        Actions.ExitApp();
                    }

                    else
                        throw new ArgumentOutOfRangeException();
                }
                // При возникновении любого исключения - текстовое сообщение выводится в область информации
                catch (Exception e)
                {
                    ConsoleWindow.InfoText = e.Message;
                }


            }
        }
    }
}
}
