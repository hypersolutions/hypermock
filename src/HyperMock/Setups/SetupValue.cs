namespace HyperMock.Setups
{
    internal sealed class SetupValue
    {
        internal SetupValue(object value, bool isException = false)
        {
            Value = value;
            IsException = isException;
        }
        
        internal object Value { get; }
        
        internal bool IsException { get; }
    }
}
