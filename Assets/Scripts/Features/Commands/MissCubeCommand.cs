using UnityEngine;

public class MissCubeCommand : ICommand
{
    private readonly Cube _cube;
    private readonly INotificationService _notificationService;
    private readonly Vector3 _originalPosition;

    private bool _wasExecuted = false;

    public MissCubeCommand(Cube cube, INotificationService notificationService)
    {
        _cube = cube;
        _notificationService = notificationService;
        _originalPosition = cube.transform.position;
    }

    public void Execute()
    {
        if (_wasExecuted) return;

        _cube.ReturnToOriginalPosition();
        _notificationService.ShowMessage("cube_missed");
        _wasExecuted = true;
    }

    public void Undo()
    {
        if (!_wasExecuted) return;

        _cube.transform.position = _originalPosition;
        _cube.SetVisible(true);
        _wasExecuted = false;
    }
}