using Assets.Scripts.Particle;
using Assets.Scripts.CameraSystem;
using Assets.Scripts.SubscriberSystem;
using Assets.Scripts.CanvasSystem.Score.Combo;
using Assets.Scripts.CanvasSystem.Score.General;
using Assets.Scripts.CanvasSystem.Score.BubbleScore;

namespace Assets.Scripts.BubbleSystem.Subscriber
{
    public class BubbleManagerSubscriber : BaseSubscriber
    {
        #region Variables

        private BubbleManager _bubbleManager;

        private BubbleThrower _bubbleThrower;
        private CameraService _cameraService;
        private ParticlePlayer _particlePlayer;
        private BubbleComboHandler _comboHandler;
        private BubbleScoreHandler _bubbleScoreHandler;
        private GeneralScoreHandler _generalScoreHandler;

        #endregion Variables

        #region Functions

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _bubbleManager = FindObjectOfType<BubbleManager>();

            _bubbleThrower = FindObjectOfType<BubbleThrower>();
            _cameraService = FindObjectOfType<CameraService>();
            _particlePlayer = FindObjectOfType<ParticlePlayer>();
            _comboHandler = FindObjectOfType<BubbleComboHandler>();
            _bubbleScoreHandler = FindObjectOfType<BubbleScoreHandler>();
            _generalScoreHandler = FindObjectOfType<GeneralScoreHandler>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _bubbleManager = null;

            _cameraService = null;
            _particlePlayer = null;
            _comboHandler = null;
            _bubbleScoreHandler = null;
            _generalScoreHandler = null;
        }

        protected override void Subscribe()
        {
            if (_bubbleManager == null) return;

            _bubbleManager.CameraShakeRequested += _cameraService.ShakeCamera;

            _bubbleManager.UpdateCombo += _comboHandler.ShowCombo;
            _bubbleManager.UpdateCombo += _generalScoreHandler.UpdateMultiplier;

            _bubbleManager.UpdateGeneralScore += _generalScoreHandler.UpdateScore;
            _bubbleManager.ShowBubbleScore += _bubbleScoreHandler.ShowScore;

            _bubbleManager.BubbleParticleRequested += _particlePlayer.PlayBubbleParticle;

            _bubbleManager.MergeOperationCompleted += _bubbleThrower.OnMergeOperationCompleted;
        }

        protected override void UnSubscribe()
        {
            if (_bubbleManager == null) return;

            _bubbleManager.CameraShakeRequested -= _cameraService.ShakeCamera;

            _bubbleManager.UpdateCombo -= _comboHandler.ShowCombo;
            _bubbleManager.UpdateCombo -= _generalScoreHandler.UpdateMultiplier;

            _bubbleManager.UpdateGeneralScore -= _generalScoreHandler.UpdateScore;
            _bubbleManager.ShowBubbleScore -= _bubbleScoreHandler.ShowScore;

            _bubbleManager.BubbleParticleRequested -= _particlePlayer.PlayBubbleParticle;

            _bubbleManager.MergeOperationCompleted += _bubbleThrower.OnMergeOperationCompleted;
        }

        #endregion Functions
    }
}