using System;
[Serializable]
public class DocDoesntExist : Exception
{
    public DocDoesntExist() { }
    public DocDoesntExist(string message) : base(message) { }
}