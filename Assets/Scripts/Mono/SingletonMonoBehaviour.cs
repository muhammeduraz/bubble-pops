using System;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// Base class for singletons that are monobehaviors. 
/// Subclasses implementing OnDestroy need to call the base.OnDestroy().
/// </summary>
/// <typeparam name="T">The type of the deriving class</typeparam>
namespace Assets.Scripts.Mono
{
    #region Classes

    public abstract class SingletonScope
    {
        public abstract void OnAwake(MonoBehaviour target);
        public abstract bool IsGameScope { get; }
    }

    public class SingletonScopeScene : SingletonScope
    {
        public override void OnAwake(MonoBehaviour target)
        {
        }

        public override bool IsGameScope
        {
            get { return false; }
        }
    }

    public class SingletonScopeGame : SingletonScope
    {
        public override void OnAwake(MonoBehaviour target)
        {
            Debug.Log("Game scope singleton: " + target.name);
            UnityEngine.Object.DontDestroyOnLoad(target);
        }

        public override bool IsGameScope
        {
            get { return true; }
        }
    }

    #endregion Classes

    public class SingletonMonoBehaviour<T, Scope> : MonoBehaviour where T : MonoBehaviour where Scope : SingletonScope, new()
    {
        #region Variables

<<<<<<< Updated upstream
        private static bool sm_applicationIsQuitting = false;
        private static bool sm_isBeingManuallyDestroyed = false;
        private static T sm_instance;
=======
        private static T sm_instance;
        private static bool sm_applicationIsQuitting = false;
        private static bool sm_isBeingManuallyDestroyed = false;
>>>>>>> Stashed changes

        #endregion Variables

        #region Properties

        public static bool HasInstance { get { return sm_instance != null; } }

        #endregion Properties

        #region Functions

        public virtual void OnShutdown() { }

        /// <summary>
        /// Singleton instance. Creates the instance if it does not exist. Can return null, if called during application shutdown. 
        /// </summary>
        public static T Instance
        {
            get
            {
                if (sm_instance == null)
                {
                    if (!Application.isPlaying)
                        throw new Exception("Singleton instance mustn't be created when not running the game.");

                    if (sm_applicationIsQuitting)
                    {
                        return null;
                    }

                    UnityEngine.Object[] instances = FindObjectsOfType(typeof(T));

                    if (instances.Length > 1)
                        throw new System.InvalidOperationException("Detected multiple instances of singleton type " +
                            typeof(T).Name);
                    else if (instances.Length > 0)
                        sm_instance = (T)instances[0];

                    if (sm_instance == null)
                    {
                        GameObject prefab = Resources.Load<GameObject>(typeof(T).Name);
                        if (prefab != null)
                        {
                            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
                            gameObject.name = typeof(T).Name;
                            sm_instance = gameObject.GetComponent<T>();
                            if (sm_instance == null)
                            {
                                throw new MissingComponentException(
                                    "There doesn't seem to be a " + typeof(T).Name + " component on the " + typeof(T).Name + " prefab.");
                            }
                        }
                        else
                        {
                            sm_instance = GetSingletonContainerObject<T>().AddComponent<T>();
                        }

                        new Scope().OnAwake(sm_instance);
                    }
                }
                return sm_instance;
            }
        }

        public void DestroySingleton()
        {
            if (sm_instance != null)
            {
                sm_isBeingManuallyDestroyed = true;
                Destroy(sm_instance.gameObject);
                sm_instance = null;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        protected static void ResetStatics()
        {
            sm_applicationIsQuitting = false;
            sm_isBeingManuallyDestroyed = false;
            sm_instance = null;
        }

        protected override void OnDestroy()
        {
            if (!sm_isBeingManuallyDestroyed && new Scope().IsGameScope)
            {
                // When Unity quits, it destroys objects in a random order.
                // In principle, a Singleton is only destroyed when application quits.
                // If any script calls Instance after it have been destroyed, 
                //   it will create a buggy ghost object that will stay on the Editor scene
                //   even after stopping playing the Application. Really bad!
                // So, this was made to be sure we're not creating that buggy ghost object.
                sm_applicationIsQuitting = true;

                Debug.Log("Singleton destroyed: " + typeof(T).Name);
                OnShutdown();
            }

            sm_isBeingManuallyDestroyed = false;
        }

        private static GameObject GetSingletonContainerObject<U>()
        {
            GameObject singleton;
            /*GameObject singleton = GameObject.Find(typeof(U).Name);
			if (singleton == null)*/
            {
                singleton = new GameObject();
                singleton.name = typeof(U).Name;
            }
            return singleton;
        }

        public static void PreWarm()
        {
            if (Instance == null)
            {
                Debug.LogError("PreWarm failed: " + typeof(T).ToString());
            }
        }

        #endregion Functions
    }
}