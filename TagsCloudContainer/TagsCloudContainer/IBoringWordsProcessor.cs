namespace TagsCloudContainer;

public interface IBoringWordsProcessor
{
    public Dictionary<string, int> WordsToLowerAndRemoveBoringWords(List<string> words);
}