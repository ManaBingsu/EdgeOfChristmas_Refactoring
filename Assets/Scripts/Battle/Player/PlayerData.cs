using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Battle/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public SessionId sessionId;

        [SerializeField]
        private int maxRage;

        [SerializeField]
        private int rageChargingPoint;

        [SerializeField]
        private float moveSpeed;
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        [SerializeField]
        private float jumpPower;
        public float JumpPower { get => jumpPower; set => jumpPower = value; }
    }
}

