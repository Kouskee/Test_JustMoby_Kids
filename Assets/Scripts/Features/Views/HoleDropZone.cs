using UnityEngine;
using Zenject;

public class HoleDropZone : MonoBehaviour, IDropZone
{
    [Inject] private ITowerService _towerService;
    [Inject] private IHoleService _holeService;
    [Inject] private INotificationService _notificationService;
    [Inject] private ICommandService _commandService;

    public bool CanAcceptCube(Cube cube)
    {
        return cube.IsInTower;
    }

    public bool AcceptCube(Cube cube)
    {
        if (!CanAcceptCube(cube)) return false;

        var command = new ThrowCubeToHoleCommand(
            _towerService,
            _holeService,
            cube,
            _notificationService
        );

        _commandService.ExecuteCommand(command);
        return true;
    }
}