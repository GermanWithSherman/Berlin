using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameException : System.Exception
{
    public new string Message;

    public GameException(string message)
    {
        Message = message;
    }

    public override string ToString()
    {
        return $"{Message}\n{base.ToString()}";
    }
}
