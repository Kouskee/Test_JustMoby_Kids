using UnityEngine;

public class AddCubeToTowerCommand : ICommand
{
    private readonly ITowerService _towerService;
    private readonly Cube _cube;
    private readonly INotificationService _notificationService;

    private Transform _originalParent;
    private Vector2 _originalAnchoredPosition;
    private bool _wasExecuted = false;

    public AddCubeToTowerCommand(ITowerService towerService, Cube cube, INotificationService notificationService)
    {
        _towerService = towerService;
        _cube = cube;
        _notificationService = notificationService;

        var cubeRect = cube.RectTransform;
        _originalParent = cubeRect.parent;
        _originalAnchoredPosition = cubeRect.anchoredPosition;
    }

    public void Execute()
    {
        if (_wasExecuted) return;

        _notificationService.ShowMessage("cube_added_to_tower");
        _wasExecuted = true;
    }

    public void Undo()
    {
        if (!_wasExecuted) return;

        _towerService.RemoveCube(_cube);

        var cubeRect = _cube.RectTransform;
        cubeRect.SetParent(_originalParent, true);
        cubeRect.anchoredPosition = _originalAnchoredPosition;
        _cube.SetInTower(false);

        _wasExecuted = false;
    }
}