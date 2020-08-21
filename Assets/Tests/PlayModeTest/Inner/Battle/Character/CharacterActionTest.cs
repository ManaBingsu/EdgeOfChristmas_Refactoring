using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Battle.Character;
using CharacterController = Battle.Character.CharacterController;
using Debug = Helper.Debug;
using UnityEngine.PlayerLoop;

namespace Tests
{
    public class CharacterActionTest : MonoBehaviour
    {
        CharacterController controller;

        [UnityTest]
        public IEnumerator GoRight()
        {
            GameObject obj = Instantiate(Resources.Load("Test/CharacterDefault")) as GameObject;
            controller = obj.GetComponent<CharacterController>();
            float time = 0f;
            while (time < 1f)
            {
                controller.Move(CharacterAction.EMoveState.Right);
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Assert.IsTrue(controller.transform.position.x > 0);
            DestroyImmediate(controller.gameObject);
            yield return null;
        }
        [UnityTest]
        public IEnumerator GoLeft()
        {
            GameObject obj = Instantiate(Resources.Load("Test/CharacterDefault")) as GameObject;
            controller = obj.GetComponent<CharacterController>();
            float time = 0f;
            while (time < 1f)
            {
                controller.Move(CharacterAction.EMoveState.Left);
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Assert.IsTrue(controller.transform.position.x < 0);
            DestroyImmediate(controller.gameObject);
            yield return null;
        }


        [UnityTest]
        public IEnumerator Stop()
        {
            GameObject obj = Instantiate(Resources.Load("Test/CharacterDefault")) as GameObject;
            controller = obj.GetComponent<CharacterController>();
            float time = 0f;
            while (time < 1f)
            {
                controller.Move(CharacterAction.EMoveState.Zero);
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Assert.AreEqual(0, controller.transform.position.x);
            DestroyImmediate(controller.gameObject);
            yield return null;
        }
    }
}
