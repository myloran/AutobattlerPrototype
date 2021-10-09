using NUnit.Framework;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

public class TestSilence
{
    [SetUp]
    protected void SetUp() 
    {
        // Debug.Log("SetUp");
        // var api = ScriptableObject.CreateInstance<TestRunnerApi>();
        // api.Execute(new ExecutionSettings(new Filter()
        // {
        //     testNames = new[] {"MyTestClass.NameOfMyTest", "SpecificTestFixture.NameOfAnotherTest"}
        // }));

    }
    
    [Test]
    public void TestSilenceSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator TestSilenceWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
}
