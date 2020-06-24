namespace HyperMock.Tests.Support
{
    public interface IConverter
    {
        bool TryParse(string text, out int value);
        bool TryParse(ref string text, out int value);
        void Format(ref string text);
    }
}
