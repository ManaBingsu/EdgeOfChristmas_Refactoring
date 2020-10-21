using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Battle
{
    public class GiftUI : MonoBehaviour
    {
        Dictionary<SessionId, List<Image>> giftImages;

        public List<Image> LeftGifts;
        public List<Image> RightGifts;

        private void Awake()
        {
            BattleManager.Instance.SetPlayerInfoAction += Initialize;
            Debug.Log("gift uji AWAKE");
        }

        private void Initialize()
        {
            Debug.Log("gift uji initiszlied");
            int count = 0;
            giftImages = new Dictionary<SessionId, List<Image>>();
            foreach (SessionId id in BattleManager.Instance.players.Keys)
            {
                if (count == 0)
                {
                    giftImages.Add(id, LeftGifts);
                }
                else if (count == 1)
                {
                    giftImages.Add(id, RightGifts);
                }
                count++;
            }

            BattleManager.Instance.AddScoreAction += SwitchGift;
        }

        private void SwitchGift(SessionId id, int score)
        {
            Debug.Log("Switch!");
            
            for (int i = 0; i < BattleManager.Instance.RoundWinScore; i++)
            {
                if (i <= BattleManager.Instance.PlayerScores[id])
                {
                    giftImages[id][i].color = Color.red;
                }
                else
                {
                    giftImages[id][i].color = Color.black;
                }
            }
        }
    }
}

