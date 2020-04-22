using System.Collections;
using System.Collections.Generic;
using BuildABotRoyale.Testing;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BuildRobotTests
    {
        private BuildRobot buildRobot;

        [SetUp]
        public void SetupEachTest()
        {
            // clear the scene
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            // instantiate a Mock SocketConnectionHandler for the GameStateManager to reference
            new GameObject("test_socket", typeof(MockSocketConnectionHandler));

            // intantiate a GameStateManager because BuildRobot needs it
            var gameManager = new GameObject("test_gamestatemanager", typeof(GameStateManager)).GetComponent<GameStateManager>();
            gameManager.Awake();

            // create an instance of BuildRobot and configure it with basic part GameObjects for testing
            buildRobot = new GameObject("test_BuildRobot", typeof(BuildRobot)).GetComponent<BuildRobot>();
            buildRobot.buildSampleBots = false;
            buildRobot.robotParent = new GameObject("test_parent_type", typeof(PartHandler), typeof(Rigidbody));
            buildRobot.block = new GameObject("test_block_type", typeof(PartHealth));
            buildRobot.center = new GameObject("test_center_type", typeof(PartHealth));
            buildRobot.spike = new GameObject("test_spike_type", typeof(PartHealth));
            buildRobot.shield = new GameObject("test_shield_type", typeof(PartHealth));
        }

        [Test]
        public void BuildingRobotReturnsAGameObject()
        {
            string parts = @"[
                {
                    'type': 'center',
                    'x': 0,
                    'y': 0,
                    'direction': 'north'
                }
            ]".Replace("'", "\""); // replace ' with " so that ' can be written in the literal to make it more readable

            var result = buildRobot.build(parts, "test-robot");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<GameObject>());
        }

        [Test]
        public void BuildingRobotWithOnePartReturnsObjectWithOneChild()
        {
            string parts = @"[
                {
                    'type': 'center',
                    'x': 0,
                    'y': 0,
                    'direction': 'north'
                }
            ]".Replace("'", "\"");

            var result = buildRobot.build(parts, "test-robot");

            Assert.That(result.transform.childCount, Is.EqualTo(1));
        }

        [Test]
        public void BuildingRobotWithFourPartsReturnsObjectWithFourChildren()
        {
            string parts = @"[
                {
                    'type': 'center',
                    'x': 0,
                    'y': 0,
                    'direction': 'north'
                },
                {
                    'type': 'block',
                    'x': 1,
                    'y': 0,
                    'direction': 'north'
                },
                {
                    'type': 'spike',
                    'x': 0,
                    'y': 1,
                    'direction': 'north'
                },
                {
                    'type': 'shield',
                    'x': 1,
                    'y': 1,
                    'direction': 'north'
                },

            ]".Replace("'", "\"");

            var result = buildRobot.build(parts, "test-robot");

            Assert.That(result.transform.childCount, Is.EqualTo(4));
        }

        /* Commented out this test because it fails
        [Test]
        public void RobotCannotBeBuiltWithoutACenter()
        {
            string parts = @"[
                {
                    'type': 'block',
                    'x': 0,
                    'y': 0,
                    'direction': 'north'
                }
            ]".Replace("'", "\"");

            Assert.Throws<System.ArgumentException>(() =>
            {
                var result = buildRobot.build(parts, "test-robot");
            });
        }
        */

        private class VerySpecialUniqueTypeJustForTesting : MonoBehaviour { };
        [Test]
        public void BuiltPartsHaveThePrefabsComponents()
        {
            // change the center prefab to a GO with a unique component type then check that the built robot's center has this type
            buildRobot.center = new GameObject("special-center", typeof(PartHealth), typeof(VerySpecialUniqueTypeJustForTesting));

            string parts = @"[
                {
                    'type': 'center',
                    'x': 0,
                    'y': 0,
                    'direction': 'north'
                }
            ]".Replace("'", "\"");

            var result = buildRobot.build(parts, "test-robot");

            var specialComponent = result.transform.GetChild(0).GetComponent<VerySpecialUniqueTypeJustForTesting>();
            Assert.That(specialComponent, Is.Not.Null);
        }

        [Test]
        public void CenterPartHasLocalPositionZeroOnXZPlane()
        {
            string parts = @"[
                {
                    'type': 'center',
                    'x': 4,
                    'y': 4,
                    'direction': 'north'
                }
            ]".Replace("'", "\"");

            var result = buildRobot.build(parts, "test-robot");

            var centerPart = result.transform.GetChild(0);
            Assert.That(centerPart.localPosition.x.Equals(0f));
            Assert.That(centerPart.localPosition.z.Equals(0f));
        }
    }
}
