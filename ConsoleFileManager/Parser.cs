using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    /// <summary>
    /// Введенная пользователем строка делится непосредственно на саму команду и ее параметры
    /// </summary>
    class Parser
    {

        public static Commands Comand { get; private set; }
        public static string SourcePath { get; private set; }
        public static string DestPath { get; private set; }
        private static readonly string s = @":\";

        public static void TryParseComandLine(string str)
        {
            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                throw new InvalidOperationException("Команда не может быть пустой (используйте 'help')!");
            }
            if (words.Length >= 1)
            {
                words[0] = words[0].ToLower();
                Comand = words[0]
                    switch
                {
                    "ls" = Commands.Ls,
                    "cp" = Commands.Cp,
                    "rm" = Commands.Rm,
                    "inf" => Commands.Inf,
                    "help" => Commands.Help,
                    "exit" => Commands.Exit,
                    _ => throw new InvalidOperationException(
                        $"Команда {words[0]} не поддерживается (используйте 'help')!"),
                };
            }
            // TODO Не реализована работа с файлами и директориями с пробелом в имени
            if (words.Length >= 2)
            {
                words[1] = words[1].ToLower();
                if (File.Exists(words[1]) || Directory.Exists(words[1]))
                {
                    SourcePath = words[1];
                }
                else
                {
                    throw new FileNotFoundException($"Такой папки или файла не существует!\n   {words[1]} - проверьте написание!");
                }
            }
            if (words.Length >= 3)
            {
                words[2] = words[2].ToLower();
                if (words[2].Contains(s))
                {
                    DestPath = words[2];
                }
                else
                {
                    throw new FileNotFoundException($"Неправильно указан путь назначения!\n   {words[1]} - проверьте написание!");
                }
            }

        }
    }
}
