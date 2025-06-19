public class ThrowCubeToHoleCommand : ICommand
{
    private readonly ITowerService _towerService;
    private readonly IHoleService _holeService;
    private readonly Cube _cube;
    private readonly INotificationService _notificationService;

    private int _originalIndex;
    private bool _wasExecuted = false;

    public ThrowCubeToHoleCommand(ITowerService towerService, IHoleService holeService, Cube cube, INotificationService notificationService)
    {
        _towerService = towerService;
        _holeService = holeService;
        _cube = cube;
        _notificationService = notificationService;
        _originalIndex = _towerService.GetCubeIndex(cube);
    }

    public void Execute()
    {
        if (_wasExecuted) return;

        _holeService.ThrowCubeToHole(_cube);
        _notificationService.ShowMessage("cube_thrown_to_hole");
        _wasExecuted = true;
    }

    public void Undo()
    {
        if (!_wasExecuted) return;

        _towerService.InsertCubeAtIndex(_cube, _originalIndex);
        _wasExecuted = false;
    }
}