using System.Drawing;
using ErrorHandling;

namespace TagsCloudVisualization;

public interface ILayouter
{
    public IEnumerable<Rectangle> GetLayout();
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}