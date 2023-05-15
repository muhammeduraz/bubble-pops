using System;
using UnityEngine;
using System.Reflection;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.Mono
{
    [AddComponentMenu("")]
    public class MonoBehaviour : UnityEngine.MonoBehaviour, IDisposable
    {
        #region Variables

        bool m_updateRegistered = false;
        bool m_lateUpdateRegistered = false;

#if UNITY_EDITOR
        bool m_warningsLogged = false;
#endif

        #endregion Variables

        #region OnEnable - Awake - Start - OnDestroy

        protected virtual void OnEnable()
        {
            LogWarnings();
        }

        private void Awake()
        {
            LogWarnings();
            OnAwake();
        }

        private void Start()
        {
            LogWarnings();
            OnStart();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
            UnregisterUpdate();
            UnregisterLateUpdate();
        }

        #endregion OnEnable - Awake - Start - OnDestroy

        #region Functions

        protected virtual void OnAwake()
        {
            this.AssertAllRequiredValuesSerialized();
        }

        protected virtual void OnStart() { }

        // You need to specifically register to receive calls to the update functions!
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void Dispose() { }

        protected void RegisterUpdate()
        {
            if (m_updateRegistered) return;

            m_updateRegistered = true;
            UpdateManager.Register(UpdateManager.UpdatePhase.Update, this);
        }

        protected void RegisterHighPriorityUpdate()
        {
            if (m_updateRegistered) return;

            m_updateRegistered = true;
            UpdateManager.RegisterHighPriority(UpdateManager.UpdatePhase.Update, this);
        }

        protected void UnregisterUpdate()
        {
            if (!m_updateRegistered) return;

            m_updateRegistered = false;
            UpdateManager.Unregister(UpdateManager.UpdatePhase.Update, this);
        }

        protected void RegisterLateUpdate()
        {
            if (m_lateUpdateRegistered) return;

            m_lateUpdateRegistered = true;
            UpdateManager.Register(UpdateManager.UpdatePhase.LateUpdate, this);
        }

        protected void RegisterHighPriorityLateUpdate()
        {
            if (m_lateUpdateRegistered) return;

            m_lateUpdateRegistered = true;
            UpdateManager.RegisterHighPriority(UpdateManager.UpdatePhase.LateUpdate, this);
        }

        protected void UnregisterLateUpdate()
        {
            if (!m_lateUpdateRegistered) return;

            m_lateUpdateRegistered = false;
            UpdateManager.Unregister(UpdateManager.UpdatePhase.LateUpdate, this);
        }

        #region Editor Functions

#if UNITY_EDITOR
        void LogMethodWarning(string name, string suggestion, Warnings suppression)
        {
            MethodInfo method = GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (method != null)
            {
                if (!SuppressWarningAttribute.Suppresses(method, suppression))
                {
                    Debug.LogWarning(string.Format("{0} defines {1} method! Use {2} instead!", method.DeclaringType.FullName, name, suggestion));
                }
            }
        }
#endif

        void LogWarnings()
        {

#if UNITY_EDITOR
            if (!m_warningsLogged)
            {
                m_warningsLogged = true;
                LogMethodWarning("Awake", "OnAwake", Warnings.None);
                LogMethodWarning("Start", "OnStart", Warnings.None);
                LogMethodWarning("Update", "OnUpdate", Warnings.UnityUpdate);
                LogMethodWarning("LateUpdate", "OnLateUpdate", Warnings.UnityUpdate);
            }
#endif

            #endregion Editor Functions

        }

        #endregion Functions
    }
}