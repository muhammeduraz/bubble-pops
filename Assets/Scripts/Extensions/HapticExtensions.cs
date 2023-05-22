using UnityEngine;
using Lofelt.NiceVibrations;

namespace Assets.Scripts.HapticSystem
{
    public class HapticExtensions
    {
        #region Functions

        /// <summary>
        /// Plays haptic once according to haptic type you select
        /// </summary>
        /// <param name="presetType"></param>
        public static void PlayHaptic(HapticPatterns.PresetType presetType)
        {
            HapticPatterns.PlayPreset(presetType);
        }
        /// <summary>
        /// Plays soft haptic once
        /// </summary>
        public static void PlaySoftHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }
        /// <summary>
        /// Plays light haptic once
        /// </summary>
        public static void PlayLightHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        /// <summary>
        /// Plays medium haptic once
        /// </summary>
        public static void PlayMediumHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        /// <summary>
        /// Plays heavy haptic once
        /// </summary>
        public static void PlayHeavyHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
        /// <summary>
        /// Plays rigid haptic once
        /// </summary>
        public static void PlayRigidHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
        }
        /// <summary>
        /// Plays success haptic once
        /// </summary>
        public static void PlaySuccessHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        }
        /// <summary>
        /// Plays warning haptic once
        /// </summary>
        public static void PlayWarningHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
        }
        /// <summary>
        /// Plays failure haptic once
        /// </summary>
        public static void PlayFailureHaptic()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
        }

        #endregion Functions
    }
}