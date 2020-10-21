using BackEnd.Tcp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : MonoBehaviour
    {
        public Dictionary<SessionId, int> PlayerScores { get; set; }

        public Action<SessionId, int> AddScoreAction { get; set; }

        public void InitializeScore()
        {
            PlayerScores = new Dictionary<SessionId, int>();
            List<SessionId> sessionIds = BackEndMatchManager.GetInstance().sessionIdList;
            for (int i = 0; i < sessionIds.Count; i++)
            {
                PlayerScores.Add(sessionIds[i], 0);
            }
        }

        public void ResetScore()
        {
            foreach(SessionId id in PlayerScores.Keys)
            {
                PlayerScores[id] = 0;
            }
        }

        public void AddScore(SessionId id, int score)
        {
            if (PlayerScores[id] + score >= RoundWinScore)
            {
                // Round 종료
            }
            else
            {
                PlayerScores[id] += score;
                Debug.Log($"Add score! {AddScoreAction}");
                AddScoreAction?.Invoke(id, score);
            }

        }
    }

}

