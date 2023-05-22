using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneSystem
{
    public class StartSceneHandler : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string _sceneNameToLoad;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
        }

        #endregion Unity Functions

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

            await SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.None);
        }

        #endregion Functions
    }
}