using DG.Tweening;
using UniRx;

public class GameService : IGameService
{
    private readonly ReactiveProperty<GameState> _gameState = new ReactiveProperty<GameState>(GameState.Loading);

    public IReadOnlyReactiveProperty<GameState> GameStatePropery => _gameState;

    public void StartGame()
    {
        _gameState.Value = GameState.Playing;
    }

    public void PauseGame()
    {
        if (_gameState.Value == GameState.Playing)
            _gameState.Value = GameState.Paused;
    }

    public void ResetGame()
    {
        _gameState.Value = GameState.Loading;
    }
}
