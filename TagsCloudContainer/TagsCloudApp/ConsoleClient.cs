using System.Drawing;
using ErrorHandling;
using TagsCloudContainer;

namespace TagsCloudApp;

public class ConsoleClient : IClient
{
    private const string ReadFontColorPrompt = "Введите английское название цвета шрифта (enter для дефолтного значения):";
    private const string ReadBGColorPrompt = "Введите английское название цвета фона (enter для дефолтного значения):";
    private const string ReadFontNamePrompt = "Введите название шрифта (enter для дефолтного значения):";
    private const string ReadFontSizePrompt = "Введите размер изображения через пробел двумя числами (enter для дефолтного значения):";
    private const string ReadOutputFileNamePrompt = "Введите желаемое имя для выходного файла (enter для дефолтного значения):";
    private const string ReadBanWordsPrompt = "Введите через запятую слова, которые хотите игнорировать (enter, если хотите видеть все слова)";
    private const string ReadOutputFileFormat = "Введите желаемый формат выходного файла (enter для дефолтного значения)";
    
    public ImageGeneratorInfo GetImageGeneratorInfo()
    {
        var textColor = ReadColor(ReadFontColorPrompt).GetValueOrThrow();
        var bgColor = ReadColor(ReadBGColorPrompt).GetValueOrThrow();
        var font = ReadFont(ReadFontNamePrompt).GetValueOrThrow();
        var imageSize = ReadSize(ReadFontSizePrompt).GetValueOrThrow();
        var outputFile = ReadString(ReadOutputFileNamePrompt);
        var banWords = ReadList(ReadBanWordsPrompt);
        var outputFileFormat = ReadFormat(ReadOutputFileFormat).GetValueOrThrow();

        return new ImageGeneratorInfo(
            textColor,
            bgColor,
            font,
            imageSize,
            outputFile,
            banWords,
            outputFileFormat
        );
    }

    private Result<Color?> ReadColor(string prompt)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            return Result.Ok<Color?>(null);
        var color = Color.FromName(input);
        if (color.IsKnownColor)
            return Result.Ok<Color?>(color);
        return Result.Fail<Color?>($"Цвета {color} нет, попробуй ещё раз");
    }

    private Result<Font?> ReadFont(string prompt, float defaultSize = 60)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            return Result.Ok<Font?>(null);
        if (FontFamily.Families.Any(f => f.Name.Equals(input, StringComparison.OrdinalIgnoreCase)))
            return Result.Ok<Font?>(new Font(input, defaultSize));
        return Result.Fail<Font?>($"Шрифта {input} нет, попробуй ещё раз");
    }

    private Result<Size?> ReadSize(string prompt)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            return Result.Ok<Size?>(null);
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out var width) &&
            int.TryParse(parts[1], out var height))
        {
            return Result.Ok<Size?>(new Size(width, height));
        }
        return Result.Fail<Size?>("Нужно ввести два числа через пробел");
    }

    private string? ReadString(string prompt)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? null : input;
    }

    private List<string>? ReadList(string prompt)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? new List<string>() : input.Split(',').Select(s => s.Trim()).ToList();
    }

    private Result<string?> ReadFormat(string prompt)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            return Result.Ok<string?>(null);
        if (input[0] == '.')
            input = input[1..];
        switch (input)
        {
            case "png":
                break;
            case "jpg":
                break;
            case "jpeg":
                break;
            case "bmp":
                break;
            default:
                return Result.Fail<string?>(
                    $"Формат {input} не поддерживается, повторите попытку! (Доступные форматы: png, jpg, jpeg, bmp");
        }
        return Result.Ok<string?>(input);
    }

    public Result<string> GetImagePath()
    {
        Console.WriteLine("Введи название входного файла:");
        var fileName = Console.ReadLine();
        if (string.IsNullOrEmpty(fileName))
            return Result.Fail<string>("Имя файла не может быть пустым!");
        if (!File.Exists(fileName))
        {
            var fullPath = Path.GetFullPath(fileName);
            return Result.Fail<string>($"Файл {fileName} не найден по пути {fullPath}, повтори ввод");
        }
        return Result.Ok<string>(fileName);
    }
}
