using UniRx;

public interface IGameService
{
    IReadOnlyReactiveProperty<GameState> GameStatePropery { get; }
    void StartGame();
    void PauseGame();
    void ResetGame();
}