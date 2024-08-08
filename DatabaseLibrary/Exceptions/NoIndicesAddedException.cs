namespace DatabaseLibrary.Exceptions
{
    internal class NoIndicesAddedException : Exception
    {
        internal NoIndicesAddedException() { }
        internal NoIndicesAddedException(string message) : base(message) { }
    }
}
