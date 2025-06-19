using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private TMP_Text _notificationText;
    [SerializeField] private CanvasGroup _notificationPanel;
    [SerializeField] private float _showDuration = 2f;

    [Inject] private INotificationService _notificationService;
    [Inject] private ILocalizationService _localizationService;
    [Inject] private IAnimationService _animationService;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        _notificationService.OnMessageShown
            .Subscribe(ShowNotification)
            .AddTo(_disposables);

        _notificationPanel.alpha = 0f;
    }

    private void ShowNotification(string messageKey)
    {
        _localizationService.GetLocalizedText(messageKey)
            .Subscribe(localizedText =>
            {
                _notificationText.text = localizedText;
                AnimateNotification();
            })
            .AddTo(_disposables);
    }

    private void AnimateNotification()
    {
        _animationService.AnimateNotification(_notificationPanel, _showDuration)
            .Subscribe()
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}