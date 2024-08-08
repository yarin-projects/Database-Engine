namespace DatabaseLibrary.Exceptions
{
    internal class PropertyNotFoundException : Exception
    {
        internal PropertyNotFoundException() { }
        internal PropertyNotFoundException(string message) : base(message) { }
    }
}