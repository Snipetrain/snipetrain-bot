using System;

[Serializable]
public class StreamerDoesntExistsException : Exception
{
    public StreamerDoesntExistsException() { }
    public StreamerDoesntExistsException(string message) : base(message) { }
}