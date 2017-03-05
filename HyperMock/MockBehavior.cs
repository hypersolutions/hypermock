namespace HyperMock
{
    /// <summary>
    /// Defines the mock behaviors available.
    /// </summary>
    public enum MockBehavior
    {
        /// <summary>
        /// Any calls that are not setup will return their default behavior if applicable. This is the default.
        /// </summary>
        Loose,

        /// <summary>
        /// Making a call on a mock that has no setup defined raises a 
        /// <see cref="HyperMock.Exceptions.StrictMockViolationException"/>.
        /// </summary>
        Strict
    }
}
