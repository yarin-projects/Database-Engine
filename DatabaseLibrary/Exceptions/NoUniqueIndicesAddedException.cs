namespace DatabaseLibrary.Exceptions
{
    internal class NoUniqueIndicesAddedException : Exception
    {
        internal NoUniqueIndicesAddedException() { }
        internal NoUniqueIndicesAddedException(string message) : base(message) { }
    }
}
