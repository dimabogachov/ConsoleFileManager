using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.Json;


namespace ConsoleFileManager
{
    /// <summary>
    /// В классе реализованы все команды, выполняемые программой
    /// </summary>
    public static class Actions
    {
        public static string CurrentDirectory { get; private set; }

        /// <summary>
        /// Из файла json читается стартовый путь, создается список файлов и поддиректорий,
        /// формируется информация о текущей директории и выводится на консоль
        /// </summary>
        public static void StartApp()
        {
            if (File.Exists("options.json"))
            {
                string json = File.ReadAllText("options.json");
                CurrentDirectory = JsonSerializer.Deserialize<string>(json);
            }
            else
            {
                CurrentDirectory = "c:\\";
            }
            FileTree.CreateList(CurrentDirectory);
            ConsoleWindow.InfoText = GetDefaultInfo();
            ConsoleWindow.Draw();
        }
        /// <summary>
        /// Формирование списка файлов и директорий на консоль
        /// </summary>
        public static void CreateListsOfFilesAndDirectories()
        {
            // TODO не реализован пейджинг
            if (Path.HasExtension(Parser.SourcePath))
            {
                CurrentDirectory = Path.GetDirectoryName(Parser.SourcePath);
            }
            else if (CurrentDirectory == Parser.SourcePath &&
                (Parser.Comand == Comands.Rm || Parser.Comand == Comands.Cp))
            {
                CurrentDirectory = Directory.GetParent(CurrentDirectory).FullName;
            }
            else
            {
                CurrentDirectory = Parser.SourcePath;
            }

            FileTree.CreateList(CurrentDirectory);
        }
        /// <summary>
        /// Удаление файла или каталога
        /// </summary>
        public static void FileOrDirectory()
        {
            if (Path.HasExtension(Parser.SourcePath))
            {
                File.Delete(Parser.SourcePath);
            }
            else
            {
                Directory.Delete(Parser.SourcePath, true);
            }
        }
        /// <summary>
        /// Копирование файла или каталога
        /// </summary>
        public static void CopyFileOrDirectory()
        {
            if (Path.HasExtension(Parser.SourcePath))
            {
                File.Copy(Parser.SourcePath, Parser.DestPath);
            }
            else
            {
                Process proc = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = true,
                        FileName = @"C:\WINDOWS\system32\xcopy.exe",
                        Arguments = $"{Parser.SourcePath} {Parser.DestPath} /E /I"
                    }
                };
                proc.Start();
            }
        }
        /// <summary>
        /// Формируется информация о текущей директории
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultInfo()
        {
            return $@"Текущая директория {CurrentDirectory}
   содержит {Directory.GetDirectories(CurrentDirectory).Length} поддиректорий и {Directory.GetFiles(CurrentDirectory).Length} файлов
   (скрытые не показаны)";
        }
        /// <summary>
        /// Вывод информации о запрошенном файле или каталоге
        /// </summary>
        /// <returns></returns>
        public static string GetAskedInfo()
        {
            if (Path.HasExtension(Parser.SourcePath))
            {
                FileInfo info = new FileInfo(Parser.SourcePath);
                return $@"Файл {info.Name}.{info.Extension}
   Время создания: {info.CreationTime}
   Размер: {FileSizeConverter(info.Length)} ";
            }
            else
            {
                DirectoryInfo info = new DirectoryInfo(Parser.SourcePath);
                return $@"Директория: {info.Name}
   Время создания: {info.CreationTime}
   Содержит: {info.GetDirectories().Length} директорий и {info.GetFiles().Length} файлов";
            }
        }
        /// <summary>
        /// Вывод справочной информации
        /// </summary>
        /// <returns>HELP</returns>
        public static string GetHelp()
        {
            return 
                $@"Вывод дерева файловой системы:     ls C:\Source
   Копирование каталога:              cp C:\Source D:\Target\n
   Копирование файла:                 cp C:\source.txt D:\target.txt
   Удаление каталога рекурсивно:      rm C:\Source
   Удаление файла:                    rm C:\source.txt
   Вывод информации:                  inf C:\source.txt";
        }
        /// <summary>
        /// Запись текущей директории в файл и выход из программы
        /// </summary>
        public static void ExitApp()
        {
            string json = JsonSerializer.Serialize(CurrentDirectory);
            File.WriteAllText("options.json", json);

            Environment.Exit(0);
        }

        private static string FileSizeConverter(long size) //Конвертор размера файлов
        {
            if (size < 1024)
                return $"{size} B";
            else if (1024 < size && size < 1_048_576)
                return $"{(double)size / 1024:F} KB";
            else if (1_048_576 < size && size < 1_073_741_824)
                return $"{(double)size / 1_048_576:F} MB";
            else
                return $"{(double)size / 1_073_741_824:F} GB";
        }
    }
}
