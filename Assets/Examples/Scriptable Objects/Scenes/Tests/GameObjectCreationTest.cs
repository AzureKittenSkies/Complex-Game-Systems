using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameObjectCreationTest
{

    [Test]
    public void GameObjectCreationTest1SimplePasses()
    {
        // Use the Assert class to test conditions.
        var go = new GameObject("New GameObject");
        Assert.AreEqual("New GameObject", go.name);
    }

    [Test]
    public IEnumerator RigidbodyTest()
    {
        var go = new GameObject();
        go.AddComponent<Rigidbody>();
        Transform t = go.transform;
        Vector3 originalPos = t.position;

        yield return new WaitForFixedUpdate();

        Assert.AreNotEqual(originalPos, t.position.y);
    }


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator GameObjectCreationTest1WithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
