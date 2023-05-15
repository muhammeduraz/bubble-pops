using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Mono;
using UnityEngine.Profiling;
using Assets.Scripts.Attributes;
using System.Collections.Generic;

namespace Assets.Scripts.Mono
{
    public class UpdateManager : SingletonMonoBehaviour<UpdateManager, SingletonScopeGame>
    {
        #region Classes

        public enum UpdatePhase
        {
            Update,
            LateUpdate
        }

        // To avoid heap allocations when using an enum as a dictionary key
        private class UpdatePhaseComparer : IEqualityComparer<UpdatePhase>
        {
            public bool Equals(UpdatePhase a, UpdatePhase b)
            {
                return a == b;
            }

            public int GetHashCode(UpdatePhase phase)
            {
                return ((int)phase).GetHashCode();
            }
        }

        #endregion Classes

        #region Variables

        private bool m_isPaused = false;
        private const int INITIAL_UPDATE_LIST_SIZE = 128;
        private Dictionary<UpdatePhase, List<MonoBehaviour>> m_registeredBehaviours;

        #endregion Variables

        #region Unity Functions

        protected override void OnAwake()
        {
            base.OnAwake();

            m_registeredBehaviours = new Dictionary<UpdatePhase, List<MonoBehaviour>>(new UpdatePhaseComparer());

            Array updatePhases = Enum.GetValues(typeof(UpdatePhase));
            for (int i = 0; i < updatePhases.Length; ++i)
            {
                UpdatePhase phase = (UpdatePhase)updatePhases.GetValue(i);
                m_registeredBehaviours.Add(phase, new List<MonoBehaviour>(INITIAL_UPDATE_LIST_SIZE));
            }
        }

        [SuppressWarning(Warnings.UnityUpdate)]
        private void Update()
        {
            if (m_isPaused) return;
            Profiler.BeginSample("Rovio.UpdateManager.Update");
            UpdateBehaviours(UpdatePhase.Update);
            Profiler.EndSample();
        }

        [SuppressWarning(Warnings.UnityUpdate)]
        private void LateUpdate()
        {
            if (m_isPaused) return;
            Profiler.BeginSample("Rovio.UpdateManager.LateUpdate");
            UpdateBehaviours(UpdatePhase.LateUpdate);
            Profiler.EndSample();
        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("[UpdateManager] OnApplicationPause(" + pause + ")");
            m_isPaused = pause;
        }

        #endregion Unity Functions
        
        #region Functions

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetSingleton()
        {
            ResetStatics();
        }

        public static void Register(UpdatePhase phase, MonoBehaviour behaviour)
        {
            if (Instance == null) return;

            Instance.m_registeredBehaviours[phase].Add(behaviour);
        }

        public static bool IsRegistered(UpdatePhase phase, MonoBehaviour b)
        {
            if (Instance == null) return false;

            return Instance.m_registeredBehaviours[phase].Contains(b);
        }

        public static void RegisterHighPriority(UpdatePhase phase, MonoBehaviour behaviour)
        {
            if (Instance == null) return;

            Instance.m_registeredBehaviours[phase].Insert(0, behaviour);
        }

        public static void Unregister(UpdatePhase phase, MonoBehaviour behaviour)
        {
            if (Instance == null) return;

            Instance.m_registeredBehaviours[phase].Remove(behaviour);
        }

        private void UpdateBehaviours(UpdatePhase phase)
        {
            List<MonoBehaviour> behaviours = m_registeredBehaviours[phase];
            List<int> nullBehaviourIndices = null;

            for (int i = 0; i < behaviours.Count; ++i)
            {
                MonoBehaviour behaviour = behaviours[i];
                if (behaviour == null)
                {
                    nullBehaviourIndices = nullBehaviourIndices ?? new List<int>();
                    nullBehaviourIndices.Add(i);
                }
                else
                {
#if PROFILE_INDIVIDUAL_OBJECTS
					Profiler.BeginSample(behaviour.name);
#endif

                    UpdateBehaviour(phase, behaviour);

#if PROFILE_INDIVIDUAL_OBJECTS
					Profiler.EndSample();
#endif
                }
            }

            if (nullBehaviourIndices != null)
            {
                for (int i = nullBehaviourIndices.Count - 1; i >= 0; --i)
                {
                    behaviours.RemoveAt(nullBehaviourIndices[i]);
                }
                nullBehaviourIndices.Clear();
            }
        }

        private void UpdateBehaviour(UpdatePhase phase, MonoBehaviour behaviour)
        {
            switch (phase)
            {
                case UpdatePhase.Update:
                    behaviour.OnUpdate();
                    break;

                case UpdatePhase.LateUpdate:
                    behaviour.OnLateUpdate();
                    break;
            }
        }

        #endregion Functions
    }
}