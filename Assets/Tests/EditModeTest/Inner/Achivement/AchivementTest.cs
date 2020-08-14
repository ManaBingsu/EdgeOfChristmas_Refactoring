using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AchivementTest
    {
        [Test]
        public void GetAchivementWhenPassed()
        {
            IAchivement achivement = new CountAchivement();

        }
    }
}
