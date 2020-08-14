using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Battle.Management;
using System;

namespace Tests
{
    public class BattleFlowTest
    {
        private BattleFlow battleFlow;
        private int counter;


        [Test]
        public void InitializeStateToReady()
        {
            Assert.AreEqual(BattleFlow.EFlowState.Ready, battleFlow.FlowState);
        }

        [Test]
        public void SetState()
        {
            for (int i = 0; i < (int)BattleFlow.EFlowState.COUNT; i++)
            {
                battleFlow.FlowState = (BattleFlow.EFlowState)i;
                Assert.AreEqual((BattleFlow.EFlowState)i, battleFlow.FlowState);
            }
        }

        [Test]
        public void TriggerStateEvent()
        {
            for (int i = 0; i < (int)BattleFlow.EFlowState.COUNT; i++)
            {
                battleFlow.StateEvents[i] += DummyCounterEvent;
                battleFlow.FlowState = (BattleFlow.EFlowState)i;
                Assert.AreEqual(i + 1, counter);
            }

        }

        void DummyCounterEvent()
        {
            counter++;
        }

        [SetUp]
        public void SetUp()
        {
            battleFlow = new BattleFlow();
            counter = 0;
        }
    }
}
