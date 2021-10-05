namespace HyperMock.IntTests.Support
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ConverterService
    {
        private readonly IConverter _converter;

        public ConverterService(IConverter converter)
        {
            _converter = converter;
        }

        public int GetValue(string text)
        {
            return _converter.TryParse(text, out var value) ? value : -1;
        }

        public string InsertSpace(string text)
        {
            // Bit of a hack to test ref and out params together!

            if (_converter.TryParse(ref text, out var value))
            {
                return text + " " + value;
            }

            return null;
        }

        public string FormatText(string text)
        {
            _converter.Format(ref text);
            return text;
        }
    }
}