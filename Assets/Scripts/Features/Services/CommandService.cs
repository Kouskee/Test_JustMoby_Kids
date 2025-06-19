using System.Collections.Generic;
using UniRx;

public class CommandService : ICommandService
{
    private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();
    private readonly ReactiveProperty<bool> _canUndo = new ReactiveProperty<bool>(false);

    public bool CanUndo => _canUndo.Value;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
        _canUndo.Value = _commandHistory.Count > 0;
    }

    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            var command = _commandHistory.Pop();
            command.Undo();
            _canUndo.Value = _commandHistory.Count > 0;
        }
    }
}