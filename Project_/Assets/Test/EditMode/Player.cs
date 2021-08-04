using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Player
{
    // A Test behaves as an ordinary method
    [Test]
    public void Catch_Error()
    {
        GameObject gameObject = new GameObject("Player");

        Assert.Throws<MissingComponentException>(
            () => gameObject.GetComponent<Rigidbody>().velocity = Vector3.one
        );
    }
    [Test]
    public void LogAssertExample()
    {
        LogAssert.Expect(LogType.Log, "Log message");
        Debug.Log("Log message");
        Debug.LogError("Error message");
        LogAssert.Expect(LogType.Error, "Error message");
    }
}
