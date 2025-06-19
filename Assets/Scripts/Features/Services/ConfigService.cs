using UnityEngine;
using Zenject;

public class ConfigService : IConfigService
{
    private GameConfig _gameConfig;

    [Inject]
    public ConfigService(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    public GameConfig GetGameConfig()
    {
        return _gameConfig;
    }

    public void LoadConfig()
    {
        if (_gameConfig == null)
        {
            _gameConfig = Resources.Load<GameConfig>("Config/GameConfig");
        }
    }
}
