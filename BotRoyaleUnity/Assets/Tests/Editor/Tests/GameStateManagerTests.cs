using System.Collections;
using System.Collections.Generic;
using BuildABotRoyale.Testing;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameStateManagerTests
    {
        private GameStateManager gameStateManager;
        private MockSocketConnectionHandler mockSocketIO;
        [SetUp]
        public void SetupEachTest()
        {
            // clear the scene
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            // instantiate a Mock SocketConnectionHandler for the GameStateManager to reference
            mockSocketIO = new GameObject("test_socket", typeof(MockSocketConnectionHandler)).GetComponent<MockSocketConnectionHandler>();

            // intantiate a GameStateManager to test
            gameStateManager = new GameObject("test_gamestatemanager", typeof(GameStateManager)).GetComponent<GameStateManager>();
            gameStateManager.Awake();
        }

        [Test]
        public void DoesNotStartAtBattleState()
        {
            // should not initially be BATTLE because these tests rely on switching to the BATTLE state
            Assert.That(gameStateManager.GameState, Is.Not.EqualTo(GameStateManager.GameStates.BATTLE));
        }

        [Test]
        public void ChangeState_ChangesTheGameState()
        {
            var initialState = gameStateManager.GameState;
            gameStateManager.ChangeState(GameStateManager.GameStates.BATTLE);

            Assert.That(gameStateManager.GameState, Is.Not.EqualTo(initialState));
        }

        [Test]
        public void ChangeState_CallsSocketIO()
        {
            gameStateManager.ChangeState(GameStateManager.GameStates.BATTLE);

            Assert.That(mockSocketIO.ChangeGameState_Called, Is.EqualTo(1));
        }

        /*
        [Test]
        public void ChangeState_CallsRegisteredAction()
        {
            bool actionWasCalled = false;
            gameStateManager.RegisterActionToState(GameStateManager.GameStates.BATTLE, () =>
            {
                actionWasCalled = true;
            });

            gameStateManager.ChangeState(GameStateManager.GameStates.BATTLE);
            Assert.IsTrue(actionWasCalled);
        }

        [Test]
        public void ChangeState_CallsManyRegisteredActions()
        {
            int numberOfActions = 1000;

            bool[] actionWasCalled = new bool[numberOfActions];
            for (int i = 0; i < numberOfActions; i++)
            {
                actionWasCalled[i] = false;
            }

            for (int i = 0; i < numberOfActions; i++)
            {
                int cachedIndex = i;

                gameStateManager.RegisterActionToState(GameStateManager.GameStates.BATTLE, () =>
                {
                    actionWasCalled[cachedIndex] = true;
                });
            }

            gameStateManager.ChangeState(GameStateManager.GameStates.BATTLE);

            for (int i = 0; i < numberOfActions; i++)
            {
                Assert.IsTrue(actionWasCalled[i]);
            }
        }
        */
    }
}
