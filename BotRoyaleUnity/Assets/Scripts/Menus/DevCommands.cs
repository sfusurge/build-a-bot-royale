using UnityEngine;

public class DevCommands : MonoBehaviour
{
    // disable the unsued private member warning because these methods are called through reflection
#pragma warning disable IDE0051 // Remove unused private members

    private void Print()
    {
        Debug.Log("Print!");
    }

#pragma warning restore IDE0051 // Remove unused private members
}
