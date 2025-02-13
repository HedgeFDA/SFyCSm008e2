using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SFyCSm008e2;

internal class Program
{
    /// <summary>
    /// Функция реализующая запрос ввода у пользоватеся строкового значения
    /// </summary>
    /// <param name="message">Текст запроса ввода строки к пользователю</param>
    /// <returns></returns>
    static string InputStringValue(string message)
    {
        Console.Write(message);

        string? value = Console.ReadLine();

        return value == null ? "" : value;
    }

    /// <summary>
    /// Процедура выводящяя информацию об ошибке в консоли.
    /// </summary>
    /// <param name="message">Текст сообщения который будет добавлен к сообщению после заголовка "Ошибка: "</param>
    static void ErrorMessage(string message) => Console.WriteLine("\nОшибка: {0}", message);

    /// <summary>
    /// Рекурсивный метод расчета размера каталога по вложенным файлам и каталогам
    /// </summary>
    /// <param name="folderPath">Путь к каталогу размер которого необходмо рассчитать</param>
    static long  CalculateFolderSize(string folderPath)
    {
        long result = 0;

        foreach (string file in Directory.GetFiles(folderPath))
        {
            FileInfo fileInfo = new FileInfo(file);

            result += fileInfo.Length;
        }

        foreach (string subFolderPath in Directory.GetDirectories(folderPath))
        {
            // Продолжение рекурсии расчета
            result += CalculateFolderSize(subFolderPath);
        }

        return result;
    }

    /// <summary>
    /// Процедура реализующая основной алгоритм работы программы по расчету размера каталога
    /// Включающая получение вводных значений от пользователя (при необходимости)
    /// </summary>
    /// <param name="folderPath">Путь к каталогу размер которого необходмо рассчитать</param>
    static void PerformCalculation(string folderPath = "")
    {
        while (folderPath == "" || !Directory.Exists(folderPath))
        {
            if (folderPath != "") Console.WriteLine("Каталог \"{0}\" не найден", folderPath);

            folderPath = InputStringValue("Укажите путь к каталогу размер которого необходимо рассчитать (для отмены введите пустое значение): ");

            if (string.IsNullOrEmpty(folderPath))
                return;
        }

        Console.WriteLine("\nВыполняем расчет размера каталога \"{0}\"...", folderPath);

        long size = 0;

        try
        {
            // Запуск рекурсии расчета размера
            size = CalculateFolderSize(folderPath);
        }
        catch (Exception ex)
        {
            ErrorMessage(ex.Message);
        }

        Console.WriteLine("Размер каталога: {0} байт", size);
    }

    /// <summary>
    /// Главная точка входа приложения
    /// </summary>
    /// <param name="args">Аргументы командной строки при запуске приложения.</param>    
    static void Main(string[] args)
    {
        string folderPath = "";

        if (args.Length > 0)
            folderPath = args[0];

        PerformCalculation(folderPath);

        Console.WriteLine("\nВыполнение программы завершено.");
    }
}