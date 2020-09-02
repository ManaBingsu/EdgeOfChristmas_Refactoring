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

        [Header("Setting")]
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

        [Header("Reference")]
        [SerializeField]
        private List<ItemInfo> itemInfos;
        [SerializeField]
        public FallingItemPool pool;

        private List<Tuple<float, float>> appearRanges;
        private Coroutine spawnCoroutine;

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
            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.Progress] += BeginSpawning;
                BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.RoundEnd] += StopSpawning;
            }

            appearRanges = new List<Tuple<float, float>>();

            for (int i = 0; i < itemInfos.Count; i++)
            {
                float min = 0;
                float max = 0;
                float sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += itemInfos[j].appearPercentage;
                }
                min = sum;
                max = sum + itemInfos[i].appearPercentage;
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
            pool.GetObject().Initialize(itemInfos[itemIndex], itemXPos, itemSpeed, itemRotate);
        }
    }
}
