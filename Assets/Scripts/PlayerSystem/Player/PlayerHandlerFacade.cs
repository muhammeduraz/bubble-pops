using UnityEngine;

namespace Assets.Scripts.PlayerSystem
{
    public class PlayerHandlerFacade : MonoBehaviour
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {

        }

        private void OnDisable()
        {
            Terminate();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {

        }

        private void Terminate()
        {

        }

        #endregion Functions
    }
}