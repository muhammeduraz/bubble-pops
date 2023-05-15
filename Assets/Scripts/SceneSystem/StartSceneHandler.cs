using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneSystem
{
    public class StartSceneHandler : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Scene Data")][SerializeField] private string _sceneNameToLoad;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Awake - OnDisable

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
        }

        #endregion Awake - OnDisable

        #region Functions

        private void Initialize()
        {
            LoadMainSceneAsync().Forget();
        }

        private void Terminate()
        {

        }

        public async UniTask LoadMainSceneAsync()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneNameToLoad, LoadSceneMode.Additive);

            await UniTask.WaitUntil(() => asyncOperation.isDone);

            Scene scene = SceneManager.GetSceneByBuildIndex(1);
            SceneManager.SetActiveScene(scene);

            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.None);
        }

        #endregion Functions
    }
}