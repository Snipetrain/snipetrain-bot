using System;

[Serializable]
public class StreamerAlreadyExistsException : Exception
{
    public StreamerAlreadyExistsException() { }
    public StreamerAlreadyExistsException(string message) : base(message) { }
}