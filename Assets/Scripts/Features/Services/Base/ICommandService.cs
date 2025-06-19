public interface ICommandService
{
    void ExecuteCommand(ICommand command);
    void UndoLastCommand();
    bool CanUndo { get; }
}