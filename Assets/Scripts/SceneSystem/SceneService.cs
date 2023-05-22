using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.CanvasSystem.Loading;

namespace Assets.Scripts.SceneSystem
{
    public class SceneService : MonoBehaviour, IInitializable, IDisposable
    {
        #region Variables

        [SerializeField] private float _extraLoadingDuration;

        [SerializeField] private LoadingPanel _loadingPanel;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        public void Initialize()
        {
            StartCoroutine(LoadSceneAsync());
        }

        public void Dispose()
        {
            _loadingPanel = null;
        }

        public IEnumerator LoadSceneAsync()
        {
            _loadingPanel.Appear();
            yield return new WaitForSeconds(_loadingPanel.AppearDuration);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            while (!asyncOperation.isDone)
            {
                _loadingPanel.UpdateProgress(asyncOperation.progress / 2f);
                yield return null;
            }

            _loadingPanel.UpdateProgress(1f, _extraLoadingDuration);

            Scene scene = SceneManager.GetSceneByBuildIndex(1);
            SceneManager.SetActiveScene(scene);

            yield return new WaitForSeconds(_extraLoadingDuration);

            _loadingPanel.Disappear();
        }

        #endregion Functions
    }
}