namespace HyperMock.Core
{
    internal class Visit
    {
        internal Visit(string name, object[] args)
        {
            Name = name;
            Args = args;
            VisitCount = 1;
        }

        internal string Name { get; }
        internal object[] Args { get; }
        internal int VisitCount { get; set; }
    }
}