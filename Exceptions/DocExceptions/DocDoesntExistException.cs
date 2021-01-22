using System;
[Serializable]
public class DocDoesntExistException : Exception
{
    public DocDoesntExistException() { }
    public DocDoesntExistException(string message) : base(message) { }
}