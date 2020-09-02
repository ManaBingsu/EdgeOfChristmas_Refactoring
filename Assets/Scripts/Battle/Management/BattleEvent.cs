using BackEnd.Tcp;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : MonoBehaviour
    {
        const float startTime = 3f;

        private Stack<SessionId> gameRecord;

        private void RegistEvent()
        {
            StateEvents[(int)EFlowState.Start] += OnGameStart;
            //StateEvents[(int)BattleManager.EFlowState.Progress] += OnProgressUIEvent;
        }
        #region Start event
        public void OnGameStart()
        {
            StartCoroutine(ProgressTimer());
        }

        private IEnumerator ProgressTimer()
        {
            float time = 0f;
            while (time <= startTime)
            {
                time += Time.deltaTime;
                yield return null;
            }
            FlowState = EFlowState.Progress;
        }
        #endregion
        
        public void OnGameOver()
        {
            Debug.Log("Game End");
            if (BackEndMatchManager.GetInstance() == null)
            {
                Debug.LogError("매치매니저가 null 입니다.");
                return;
            }
            BackEndMatchManager.GetInstance().MatchGameOver(gameRecord);
        }

        private void MatchEndEvent()
        {
            if (!BackEndMatchManager.GetInstance().IsHost())
            {
                return;
            }
            SendGameEndOrder();
        }

        public void SendGameEndOrder()
        {
            // 게임 종료 전환 메시지는 호스트에서만 보냄
            Debug.Log("Make GameResult & Send Game End Order");
            // Find loser
            foreach (SessionId session in BackEndMatchManager.GetInstance().sessionIdList)
            {
                if (!players[session].IsWinner && !gameRecord.Contains(session))
                {
                    gameRecord.Push(session);
                }
            }
            // Find winnner
            foreach (SessionId session in BackEndMatchManager.GetInstance().sessionIdList)
            {
                if (players[session].IsWinner && !gameRecord.Contains(session))
                {
                    gameRecord.Push(session);
                }
            }
            GameEndMessage message = new GameEndMessage(gameRecord);
            BackEndMatchManager.GetInstance().SendDataToInGame<GameEndMessage>(message);
        }
    }

}
