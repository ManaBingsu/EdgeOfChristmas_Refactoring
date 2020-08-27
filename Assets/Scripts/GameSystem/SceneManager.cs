using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [Header("Reference")]
        [SerializeField]
        LoadCurtain loadCurtain;

        [Header("Setting")]
        public string lobbySceneName;

        string loadSceneName;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
        }

        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += LoadSceneEnd;
            loadSceneName = sceneName;
            StartCoroutine(Load(sceneName));
        }

        private IEnumerator Load(string sceneName)
        {
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            loadCurtain.gameObject.SetActive(true);
            loadCurtain.StartCoroutine(loadCurtain.FadeIn());

            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                if (op.progress >= 0.9f && timer >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == loadSceneName)
            {
                StartCoroutine(loadCurtain.FadeOut());
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= LoadSceneEnd;
            }
        }


    }
}

