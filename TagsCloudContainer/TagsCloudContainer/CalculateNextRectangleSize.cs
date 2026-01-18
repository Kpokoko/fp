using System.Drawing;

namespace TagsCloudContainer;

public class RectangleSizeCalculator
{
    private Size _maxSize;
    private Size _minSize;
    private int _maxFrequency;
    private bool _isMaxFrequencyInitialized;

    internal RectangleSizeCalculator(Size imageSize)
    {
        _maxSize = new Size((int)(imageSize.Width), (int)(imageSize.Height * 0.1));
        _minSize = new Size((int)(imageSize.Width * 0.01), (int)(imageSize.Height * 0.005));
    }

    internal Size CalculateNextRectangleSize(KeyValuePair<string, int> wordInfo,
        IWordMeasurer wordMeasurer, out float scale)
    {
        var wordFrequency = wordInfo.Value;
        if (!_isMaxFrequencyInitialized)
        {
            _isMaxFrequencyInitialized = true;
            _maxFrequency = wordFrequency;
        }

        scale = (float)wordFrequency / _maxFrequency;
        if (scale < 0.05)
            return Size.Empty;
        var rawSize = wordMeasurer.Measure(wordInfo.Key, scale);
        return ClampSize(rawSize);
    }

    private Size ClampSize(Size size)
    {
        size.Width = Math.Clamp(size.Width, _minSize.Width, _maxSize.Width);
        size.Height = Math.Clamp(size.Height, _minSize.Height, _maxSize.Height);
        
        return Size.Round(size);
    }
}