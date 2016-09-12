namespace Tests.HyperMock.Support
{
    public class ConverterService
    {
        private readonly IConverter _converter;

        public ConverterService(IConverter converter)
        {
            _converter = converter;
        }

        public int GetValue(string text)
        {
            int value;

            return _converter.TryParse(text, out value) ? value : -1;
        }
    }
}