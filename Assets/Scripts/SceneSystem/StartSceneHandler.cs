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

<<<<<<< Updated upstream
        #region Awake - OnDisable
=======
        #region Unity Functions
>>>>>>> Stashed changes

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
        }

<<<<<<< Updated upstream
        #endregion Awake - OnDisable
=======
        #endregion Unity Functions
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.None);
=======
            await SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.None);
>>>>>>> Stashed changes
        }

        #endregion Functions
    }
}