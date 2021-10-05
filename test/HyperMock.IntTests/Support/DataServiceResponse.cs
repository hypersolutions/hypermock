// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace HyperMock.IntTests.Support
{
    public class DataServiceResponse<T>
    {
        public T Result { get; set; }
        public ErrorModel Error { get; set; }
        public ResponseTypes ResponseType { get; set; }
    }
}