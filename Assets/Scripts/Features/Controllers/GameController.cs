using System;
using UniRx;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameService _gameService;
    [Inject] private ITowerService _towerService;
    [Inject] private IScrollService _scrollService;
    [Inject] private INotificationService _notificationService;
    [Inject] private IDragService _dragService;
    [Inject] private IHoleService _holeService;
    [Inject] private ICommandService _commandService;
    [Inject] private ISaveService _saveService;
    [Inject] private IConfigService _configService;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        InitializeGame();
        SubscribeToEvents();
    }

    private void InitializeGame()
    {
        var config = _configService.GetGameConfig();
        _scrollService.InitializeScroll(config.AvailableCubes);

        if (_saveService.HasSavedProgress())
        {
            var progress = _saveService.LoadProgress();
            _towerService.LoadTowerState(progress.TowerCubes);
        }

        _gameService.StartGame();
    }

    private void SubscribeToEvents()
    {
        _dragService.OnDragEnded
            .Subscribe(HandleDragEnd)
            .AddTo(_disposables);

        _towerService.TowerCubes
            .ObserveCountChanged()
            .Throttle(TimeSpan.FromSeconds(60))
            .Subscribe(_ => SaveProgress())
            .AddTo(_disposables);
    }

    private void HandleDragEnd(DragData dragData)
    {
        ICommand command = null;

        if (_holeService.IsInHoleArea(dragData.EndPosition))
        {
            if (dragData.Cube.IsInTower)
            {
                command = new ThrowCubeToHoleCommand(_towerService, _holeService, dragData.Cube, _notificationService);
            }
            else
            {
                command = new MissCubeCommand(dragData.Cube, _notificationService);
            }
        }
        else if (_towerService.CanAddCube(dragData.Cube, dragData.EndPosition))
        {
            command = new AddCubeToTowerCommand(_towerService, dragData.Cube, _notificationService);
        }
        else
        {
            command = new MissCubeCommand(dragData.Cube, _notificationService);
        }

        if (command != null)
        {
            _commandService.ExecuteCommand(command);
        }
    }

    private void SaveProgress()
    {
        var progressData = new GameProgressData
        {
            TowerCubes = _towerService.GetTowerSaveData()
        };
        _saveService.SaveProgress(progressData);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}