namespace HyperMock.Core
{
    internal class Visit
    {
        internal Visit(string name, Parameter[] parameters)
        {
            Name = name;
            Parameters = parameters;
            VisitCount = 1;
        }

        internal string Name { get; }
        internal Parameter[] Parameters { get; }
        internal int VisitCount { get; set; }
    }
}