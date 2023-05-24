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

        private const string MainScene = "MainScene";
        private const string GameScene = "GameScene";

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
            StartCoroutine(LoadSceneAsync(GameScene, LoadSceneMode.Additive));
        }

        public void Dispose()
        {
            _loadingPanel = null;
        }

        public void ReloadCurrentLevel()
        {
            StartCoroutine(LoadSceneAsync(GameScene, LoadSceneMode.Additive));
        }

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (!_loadingPanel.IsVisible)
            {
                _loadingPanel.Appear();
                yield return new WaitForSeconds(_loadingPanel.AppearDuration);
            }

            Scene activeScene = SceneManager.GetActiveScene();
            if (loadSceneMode == LoadSceneMode.Additive && activeScene.name.Equals(sceneName))
            {
                Coroutine unloadCoroutine = StartCoroutine(UnloadSceneAsync(sceneName));
                yield return unloadCoroutine;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            while (!loadOperation.isDone)
            {
                _loadingPanel.UpdateProgress(0.33f + loadOperation.progress / 3f);
                yield return null;
            }

            _loadingPanel.UpdateProgress(1f, _extraLoadingDuration);

            Scene scene = SceneManager.GetSceneByBuildIndex(1);
            SceneManager.SetActiveScene(scene);

            yield return new WaitForSeconds(_extraLoadingDuration);

            _loadingPanel.Disappear();
        }

        public IEnumerator UnloadSceneAsync(string sceneName)
        {
            if (!_loadingPanel.IsVisible)
            {
                _loadingPanel.Appear();
                yield return new WaitForSeconds(_loadingPanel.AppearDuration);
            }

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(GameScene);

            while (!unloadOperation.isDone)
            {
                _loadingPanel.UpdateProgress(unloadOperation.progress / 3f);
                yield return null;
            }
        }

        #endregion Functions
    }
}