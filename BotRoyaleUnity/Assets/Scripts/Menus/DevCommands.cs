using System.Reflection;
using UnityEngine;

public class DevCommands : MonoBehaviour
{
    // disable the unsued private member warning because these methods are called through reflection
#pragma warning disable IDE0051 // Remove unused private members

    private void Print()
    {
        Debug.Log("Print!");
    }

    private void BuildSampleRobots()
    {
        var instance = FindObjectOfType<BuildRobot>();
        RunMethodOnObject(instance, "BuildSampleRobots");
    }

    private void RunMethodOnObject<T>(T instance, string methodName)
    {
        typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, null);
    }

#pragma warning restore IDE0051 // Remove unused private members
}
