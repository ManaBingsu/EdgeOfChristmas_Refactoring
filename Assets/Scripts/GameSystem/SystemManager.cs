using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class SystemManager : MonoBehaviour
    {
        public static SystemManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

