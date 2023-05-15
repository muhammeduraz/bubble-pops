using System;
using UnityEngine;
<<<<<<< Updated upstream
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneSystem
{
    public class SceneService : IInitializable, IDisposable
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties
=======
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Assets.Scripts.CanvasSystem.Loading;

namespace Assets.Scripts.SceneSystem
{
    public class SceneService : MonoBehaviour, IInitializable, IDisposable
    {
        #region Variables

        [BoxGroup("Settings")][SerializeField] private float _extraLoadingDuration;

        [BoxGroup("Components")][SerializeField] private LoadingPanel _loadingPanel;

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
>>>>>>> Stashed changes

        #region Functions

        public void Initialize()
        {
<<<<<<< Updated upstream
            LoadSceneAsync().Forget();
=======
            StartCoroutine(LoadSceneAsync());
>>>>>>> Stashed changes
        }

        public void Dispose()
        {
            
        }

<<<<<<< Updated upstream
        public async UniTask LoadSceneAsync()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

            await UniTask.WaitUntil(() => asyncOperation.isDone);

            Scene scene = SceneManager.GetSceneByBuildIndex(2);
            SceneManager.SetActiveScene(scene);
=======
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
>>>>>>> Stashed changes
        }

        #endregion Functions
    }
}