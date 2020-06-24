using HyperMock.Core;

namespace HyperMock
{
    /// <summary>
    /// Provides the verification occurrence assertions.
    /// </summary>
    public abstract class Occurred
    {
        protected Occurred(int count)
        {
            Count = count;
        }

        internal int Count { get; }

        /// <summary>
        /// Returns an exactly once occurrence to check.
        /// </summary>
        /// <returns>Occurred instance</returns>
        public static Occurred Once()
        {
            return new ExactOccurred(1);
        }

        /// <summary>
        /// Returns a never occurrence to check.
        /// </summary>
        /// <returns>Occurred instance</returns>
        public static Occurred Never()
        {
            return new ExactOccurred(0);
        }

        /// <summary>
        /// Returns an exactly N occurrences to check.
        /// </summary>
        /// <returns>Occurred instance</returns>
        public static Occurred Exactly(int count)
        {
            return new ExactOccurred(count);
        }

        /// <summary>
        /// Returns an at least n occurrence to check.
        /// </summary>
        /// <returns>Occurred instance</returns>
        public static Occurred AtLeast(int count)
        {
            return new CountOrMoreOccurred(count);
        }

        /// <summary>
        /// Asserts the occurrence.
        /// </summary>
        /// <param name="actualCount">Count to check</param>
        public abstract void Assert(int actualCount);
    }
}