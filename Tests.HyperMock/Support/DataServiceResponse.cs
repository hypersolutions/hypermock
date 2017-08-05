namespace Tests.HyperMock.Support
{
    public class DataServiceResponse<T>
    {
        public T Result { get; set; }
        public ErrorModel Error { get; set; }
        public ResponseTypes ResponseType { get; set; }
    }
}