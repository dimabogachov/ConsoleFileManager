using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    /// <summary>
    /// Список возможных команд
    /// </summary>
    [Flags]
    enum Commands
    {
        /// <summary>
        /// Вывод дерева файловой системы
        /// </summary>
        Ls,
        /// <summary>
        /// Копирование каталога или файла
        /// </summary>
        Cp,
        /// <summary>
        /// Удаление каталога или файла
        /// </summary>
        Rm,
        /// <summary>
        /// Вывод информации о каталоге или файле
        /// </summary>
        Inf,
        /// <summary>
        /// Справка
        /// </summary>
        Help,
        /// <summary>
        /// Выход из программы
        /// </summary>
        Exit
    }
}
