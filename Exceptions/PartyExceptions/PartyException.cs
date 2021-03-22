using System;
[Serializable]
public class PartyException : Exception
{
    public PartyException() { }
    public PartyException(string message) : base(message) { }
}