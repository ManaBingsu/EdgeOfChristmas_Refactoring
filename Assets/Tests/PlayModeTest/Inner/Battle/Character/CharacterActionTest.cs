using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Battle.Character;
using CharacterController = Battle.Character.CharacterController;

namespace Tests
{
    public class CharacterActionTest : MonoBehaviour
    {
        CharacterController controller;

        [UnityTest]
        public IEnumerator GoRight()
        {/*
            CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
            float testMoveSpeed = 10f;
            characterData.moveSpeed = testMoveSpeed;
            CharacterStat characterStat = new CharacterStat(characterData);

            controller = Instantiate() as CharacterController;

            CharacterAction characterAction = new CharacterAction(characterStat);
            characterAction.Move(CharacterAction.EDirection.Right);
            Assert.IsTrue(characterAction.body.velocity.x > )*/


            yield return null;
        }
        [UnityTest]
        public IEnumerator StopWhenInputStop()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
