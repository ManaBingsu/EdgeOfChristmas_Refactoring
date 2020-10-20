using BackEnd.Tcp;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;

        public enum Item
        {
            Gift,
            GoldenGift,
            Snowball,
            Candy
        }

        public enum ActiveItem
        {
            Snowball,
            Candy
        }

        [Header("Setting")]

        public int maxItemNum;

        [Space(10)]
        [SerializeField]
        private float minSpawnTime;
        [SerializeField]
        private float maxSpawnTime;

        [Space(10)]
        [SerializeField]
        private float minFallingSpeed;
        [SerializeField]
        private float maxFallingSpeed;

        [Space(10)]
        [SerializeField]
        public float yPos;
        [SerializeField]
        private float minXPos;
        [SerializeField]
        private float maxXPos;

        [Header("Item data")]
        public List<ItemData> itemDatas;
        // GameObjectPool

        [Header("Pool")]
        public GameObjectPool FallingItemPool;
        public GameObjectPool SnowballPool;
        public GameObjectPool CandyPool;

        #region Public value

        // ItemInfo
        public FallingItemInfo FallingItemInfo { get; set; }

        public FlyingItemInfo FlyingItemInfo { get; set; }
        #endregion

        #region Inner value

        private List<Tuple<float, float>> appearRanges;
        private Coroutine spawnCoroutine;

        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            FallingItemPool = GetComponentInChildren<GameObjectPool>();
            FallingItemInfo = new FallingItemInfo();
            FlyingItemInfo = new FlyingItemInfo();

            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.Progress] += BeginSpawning;
                BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.RoundEnd] += StopSpawning;
            }

            appearRanges = new List<Tuple<float, float>>();

            // Set item percentage range
            for (int i = 0; i < itemDatas.Count; i++)
            {
                float min = 0;
                float max = 0;
                float sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += itemDatas[j].appearPercentage;
                }
                min = sum;
                max = sum + itemDatas[i].appearPercentage;
                Tuple<float, float> tuple = new Tuple<float, float>(min, max);
                appearRanges.Add(tuple);
            }
        }

        private void BeginSpawning()
        {
            spawnCoroutine = StartCoroutine(Spawning());
        }

        private void StopSpawning()
        {
            StopCoroutine(spawnCoroutine);
        }

        private IEnumerator Spawning()
        {
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;
                if (time > spawnTime)
                {
                    time = 0f;
                    CommandSpawnItem();
                }
                yield return null;
            }
        }

        private int GetRandomItemIndex()
        {
            float rand = Random.Range(0f, 1f);
            for (int i = 0; i < appearRanges.Count; i++)
            {
                if (rand >= appearRanges[i].Item1 && rand < appearRanges[i].Item2)
                {
                    return i;
                }
            }
            return 0;
        }

        public void CommandSpawnItem()
        {
            if (!BackEndMatchManager.GetInstance().IsHost())
            {
                return;
            }

            int randomItemIndex = GetRandomItemIndex();
            float randomXPos = Random.Range(minXPos, maxXPos);
            float randomItemSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);
            float randomRotate = Random.Range(0f, 360f);

            SpawnItemMessage msg = new SpawnItemMessage(randomItemIndex, randomXPos, randomItemSpeed, randomRotate);
            BattleManager.Instance.ProcessSpawnEvent(msg);
        }

        public void SpawnItem(int itemIndex, float itemXPos, float itemSpeed, float itemRotate)
        {
            FallingItemInfo.SetItemInfo(itemIndex, itemXPos, itemSpeed, itemRotate);
            FallingItemPool.GetObject();
        }

        public void UseItem(SessionId sessionId, int itemIndex, Vector3 pos)
        {
            FlyingItemInfo.SetItemInfo(sessionId, itemDatas[itemIndex], pos);
            switch((Item)itemIndex)
            {
                case Item.Snowball:
                    SnowballPool.GetObject();
                    break;
                case Item.Candy:
                    CandyPool.GetObject();
                    break;
            }
        }
    }

    public class FallingItemInfo
    {
        public int ItemIndex { get; set; }
        public float ItemXPos { get; set; }
        public float ItemSpeed { get; set; }
        public float ItemRotate { get; set; }

        public FallingItemInfo()
        {

        }

        public FallingItemInfo(int itemIndex, float itemXPos, float itemSpeed, float itemRotate)
        {
            ItemIndex = itemIndex;
            ItemXPos = itemXPos;
            ItemSpeed = itemSpeed;
            ItemRotate = itemRotate;
        }

        public void SetItemInfo(int itemIndex, float itemXPos, float itemSpeed, float itemRotate)
        {
            ItemIndex = itemIndex;
            ItemXPos = itemXPos;
            ItemSpeed = itemSpeed;
            ItemRotate = itemRotate;
        }
    }

    public class FlyingItemInfo
    {
        public SessionId SessionId { get; set; }
        public ItemData ItemData { get; set; }
        public Vector3 Pos { get; set; } 

        public FlyingItemInfo()
        {

        }

        public void SetItemInfo(SessionId sessionId, ItemData itemData, Vector3 pos)
        {
            SessionId = sessionId;
            ItemData = itemData;
            Pos = pos;
        }
    }
}
