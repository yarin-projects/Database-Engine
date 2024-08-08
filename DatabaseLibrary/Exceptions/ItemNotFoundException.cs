namespace DatabaseLibrary.Exceptions
{
    internal class ItemNotFoundException : Exception
    {
        internal ItemNotFoundException() { }
        internal ItemNotFoundException(string message) : base(message) { }
    }
}
