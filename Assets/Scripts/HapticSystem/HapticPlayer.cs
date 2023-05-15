using UnityEngine;
using Lofelt.NiceVibrations;

namespace Assets.Scripts.HapticSystem
{
    public class HapticPlayer
    {
        #region Variables

        private float _hapticTime;
        private float _changeableHapticTime;

        #endregion Variables

        #region Functions

        /// <summary>
        /// Plays haptic once according to haptic type you select
        /// </summary>
        /// <param name="presetType"></param>
        public void PlayHaptic(HapticPatterns.PresetType presetType)
        {
            HapticPatterns.PlayPreset(presetType);
        }
        /// <summary>
        /// Plays soft haptic once
        /// </summary>
        public void PlaySoftHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }
        /// <summary>
        /// Plays light haptic once
        /// </summary>
        public void PlayLightHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        /// <summary>
        /// Plays medium haptic once
        /// </summary>
        public void PlayMediumHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        /// <summary>
        /// Plays heavy haptic once
        /// </summary>
        public void PlayHeavyHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
        /// <summary>
        /// Plays rigid haptic once
        /// </summary>
        public void PlayRigidHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
        }
        /// <summary>
        /// Plays success haptic once
        /// </summary>
        public void PlaySuccessHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        }
        /// <summary>
        /// Plays warning haptic once
        /// </summary>
        public void PlayWarningHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
        }
        /// <summary>
        /// Plays failure haptic once
        /// </summary>
        public void PlayFailureHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
        }

        /// <summary>
        /// Plays haptic intervals according to the frequency you choose
        /// </summary>
        /// <param name="frequency">The interval</param>
        /// <param name="presetType">Haptic type (default as light)</param>
        public void ContinuousHaptic(float frequency, HapticPatterns.PresetType presetType = HapticPatterns.PresetType.LightImpact)
        {
            _hapticTime += Time.deltaTime;
            _hapticTime = Mathf.Clamp(_hapticTime, 0, frequency);
            if (!Mathf.Approximately(_hapticTime, frequency))
            {
                return;
            }

            PlayHaptic(HapticPatterns.PresetType.LightImpact);
            _hapticTime = 0f;
        }
        /// <summary>
        /// Plays haptic intervals according to the frequency and amplitude you choose
        /// </summary>
        public void ContinuousChangeableHaptic(float frequency, float amplitude)
        {
            _changeableHapticTime += Time.deltaTime;
            _changeableHapticTime = Mathf.Clamp(_changeableHapticTime, 0, frequency);
            if (!Mathf.Approximately(_changeableHapticTime, frequency))
            {
                return;
            }
            HapticPatterns.PlayEmphasis(amplitude, 1f);
            _changeableHapticTime = 0f;
        }

        #endregion Functions
    }
}