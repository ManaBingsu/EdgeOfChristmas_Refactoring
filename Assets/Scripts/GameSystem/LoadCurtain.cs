using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    [RequireComponent(typeof(Animator))]
    public class LoadCurtain : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public IEnumerator FadeIn()
        {
            animator.SetTrigger("TrgBeginLoad");
            yield return null;
        }

        public IEnumerator FadeOut()
        {
            yield return null;
            yield return null;
            animator.SetTrigger("TrgEndLoad");
        }

        public void LoadSceneEnd()
        {
            gameObject.SetActive(false);
        }
    }

}
