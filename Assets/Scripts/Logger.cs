using UnityEngine;

public interface ILogger
{
    void Info(string message);
    void Warning(string message);
    void Error(string message);
}

public class Logger : ILogger
{
    public void Error(string message) => Debug.LogError(message);
    public void Info(string message) => Debug.Log(message);
    public void Warning(string message) => Debug.LogWarning(message);
}