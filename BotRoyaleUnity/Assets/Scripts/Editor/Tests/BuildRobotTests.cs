using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BuildRobotTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BuildRobotTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BuildRobotTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [Test]
        public void BuildingRobotReturnsAGameObject()
        {
            var buildRobot = new GameObject("test_BuildRobot", typeof(BuildRobot));
        }
    }
}
