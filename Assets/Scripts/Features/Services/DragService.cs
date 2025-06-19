using System;
using UniRx;
using Zenject;

public class DragService : IDragService, IDisposable
{
    private readonly Subject<DragData> _onDragStarted = new Subject<DragData>();
    private readonly Subject<DragData> _onDragUpdated = new Subject<DragData>();
    private readonly Subject<DragData> _onDragEnded = new Subject<DragData>();

    [Inject] private IScrollService _scrollService;
    [Inject] private ITowerService _towerService;

    private CompositeDisposable _disposables = new CompositeDisposable();

    public IObservable<DragData> OnDragStarted => _onDragStarted;
    public IObservable<DragData> OnDragUpdated => _onDragUpdated;
    public IObservable<DragData> OnDragEnded => _onDragEnded;

    [Inject]
    public void Initialize()
    {
        _scrollService.ScrollCubes
            .ObserveAdd()
            .Subscribe(addEvent => SubscribeToCube(addEvent.Value))
            .AddTo(_disposables);

        foreach (var cube in _scrollService.ScrollCubes)
        {
            SubscribeToCube(cube);
        }

        _towerService.TowerCubes
            .ObserveAdd()
            .Subscribe(addEvent => SubscribeToCube(addEvent.Value))
            .AddTo(_disposables);

        foreach (var cube in _towerService.TowerCubes)
        {
            SubscribeToCube(cube);
        }
    }

    private void SubscribeToCube(Cube cube)
    {
        cube.OnDragStarted
            .Subscribe(draggedCube => HandleDragStarted(draggedCube))
            .AddTo(_disposables);

        cube.OnDragEnded
            .Subscribe(draggedCube => HandleDragEnded(draggedCube))
            .AddTo(_disposables);
    }

    private void HandleDragStarted(Cube cube)
    {
        var dragData = new DragData
        {
            Cube = cube,
            Position = cube.transform.position,
            IsValid = false
        };

        _onDragStarted.OnNext(dragData);
    }

    private void HandleDragEnded(Cube cube)
    {
        var dragData = new DragData
        {
            Cube = cube,
            EndPosition = cube.transform.position,
            Position = cube.transform.position,
            IsValid = true
        };

        _onDragEnded.OnNext(dragData);
    }

    public void Dispose()
    {
        _onDragStarted?.OnCompleted();
        _onDragUpdated?.OnCompleted();
        _onDragEnded?.OnCompleted();
        _onDragStarted?.Dispose();
        _onDragUpdated?.Dispose();
        _onDragEnded?.Dispose();
    }
}