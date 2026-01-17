using ErrorHandling;
using TagsCloudContainer;

namespace TagsCloudApp;

public interface IClient
{
    public ImageGeneratorInfo GetImageGeneratorInfo();

    public Result<string> GetImagePath();
}