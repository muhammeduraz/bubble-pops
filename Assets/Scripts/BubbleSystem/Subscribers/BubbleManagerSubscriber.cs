using Assets.Scripts.Particle;
using Assets.Scripts.ThrowSystem;
using Assets.Scripts.CameraSystem;
using Assets.Scripts.SubscribeSystem;
using Assets.Scripts.CanvasSystem.Score.Combo;
using Assets.Scripts.CanvasSystem.Score.General;
using Assets.Scripts.CanvasSystem.Score.BubbleScore;
using Assets.Scripts.EnvironmentSystem;

namespace Assets.Scripts.BubbleSystem.Subscriber
{
    public class BubbleManagerSubscriber : BaseSubscriber
    {
        #region Variables

        private BubbleManager _bubbleManager;

        private FailTrigger _failTrigger;
        private BubbleThrower _bubbleThrower;
        private CameraService _cameraService;
        private ParticlePlayer _particlePlayer;
        private BubbleComboHandler _comboHandler;
        private BubbleScoreHandler _bubbleScoreHandler;
        private GeneralScoreHandler _generalScoreHandler;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _bubbleManager = FindObjectOfType<BubbleManager>();

            _failTrigger = FindObjectOfType<FailTrigger>();
            _bubbleThrower = FindObjectOfType<BubbleThrower>();
            _cameraService = FindObjectOfType<CameraService>();
            _particlePlayer = FindObjectOfType<ParticlePlayer>();
            _comboHandler = FindObjectOfType<BubbleComboHandler>();
            _bubbleScoreHandler = FindObjectOfType<BubbleScoreHandler>();
            _generalScoreHandler = FindObjectOfType<GeneralScoreHandler>();
        }

        protected override void Dispose()
        {
            _bubbleManager = null;

            _failTrigger = null;
            _bubbleThrower = null;
            _cameraService = null;
            _particlePlayer = null;
            _comboHandler = null;
            _bubbleScoreHandler = null;
            _generalScoreHandler = null;
        }

        protected override void SubscribeEvents()
        {
            if (_bubbleManager == null) return;

            _failTrigger.Failed += _bubbleManager.OnFailed;

            _bubbleManager.CameraShakeRequested += _cameraService.ShakeCamera;

            _bubbleManager.UpdateCombo += _comboHandler.ShowCombo;
            _bubbleManager.UpdateCombo += _generalScoreHandler.UpdateMultiplier;

            _bubbleManager.UpdateGeneralScore += _generalScoreHandler.UpdateScore;
            _bubbleManager.ShowBubbleScore += _bubbleScoreHandler.ShowScore;

            _bubbleManager.BubbleParticleRequested += _particlePlayer.PlayBubbleParticle;

            _bubbleManager.MergeOperationCompleted += _bubbleThrower.OnMergeOperationCompleted;
        }

        protected override void UnSubscribeEvents()
        {
            if (_bubbleManager == null) return;

            _failTrigger.Failed -= _bubbleManager.OnFailed;

            _bubbleManager.CameraShakeRequested -= _cameraService.ShakeCamera;

            _bubbleManager.UpdateCombo -= _comboHandler.ShowCombo;
            _bubbleManager.UpdateCombo -= _generalScoreHandler.UpdateMultiplier;

            _bubbleManager.UpdateGeneralScore -= _generalScoreHandler.UpdateScore;
            _bubbleManager.ShowBubbleScore -= _bubbleScoreHandler.ShowScore;

            _bubbleManager.BubbleParticleRequested -= _particlePlayer.PlayBubbleParticle;

            _bubbleManager.MergeOperationCompleted -= _bubbleThrower.OnMergeOperationCompleted;
        }

        #endregion Functions
    }
}