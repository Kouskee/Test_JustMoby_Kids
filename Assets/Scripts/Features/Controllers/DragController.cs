using UniRx;
using UnityEngine;
using Zenject;

public class DragController : MonoBehaviour
{
    [Inject] private IDragService _dragService;
    [Inject] private IScrollService _scrollService;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        _dragService.OnDragStarted
            .Subscribe(OnDragStarted)
            .AddTo(_disposables);

        _dragService.OnDragEnded
            .Subscribe(OnDragEnded)
            .AddTo(_disposables);
    }

    private void OnDragStarted(DragData dragData)
    {
        _scrollService.SetScrollEnabled(false);

        dragData.Cube.SetDragMode(true);
    }

    private void OnDragEnded(DragData dragData)
    {
        _scrollService.SetScrollEnabled(true);

        dragData.Cube.SetDragMode(false);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}