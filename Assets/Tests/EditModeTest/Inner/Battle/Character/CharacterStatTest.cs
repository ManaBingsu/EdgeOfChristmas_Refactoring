using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Battle.Character;
using System.Security.Cryptography;

namespace Tests
{
    public class CharacterStatTest
    {
        [Test]
        public void SetDefaultMoveSpeedUsingStatData()
        {
            CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
            float testMoveSpeed = 10f;
            characterData.moveSpeed = testMoveSpeed;
            CharacterStat characterStat = new CharacterStat(characterData);
            Assert.AreEqual(testMoveSpeed, characterStat.MoveSpeed);
        }
    }
}
