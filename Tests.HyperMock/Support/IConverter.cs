namespace Tests.HyperMock.Support
{
    public interface IConverter
    {
        bool TryParse(string text, out int value);
    }
}
