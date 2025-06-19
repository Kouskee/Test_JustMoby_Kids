using UnityEngine;

public class PlayerPrefsSaveService : ISaveService
{
    private const string SAVE_KEY = "GameProgress";

    public void SaveProgress(GameProgressData data)
    {
        data.SaveTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public GameProgressData LoadProgress()
    {
        if (HasSavedProgress())
        {
            var json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<GameProgressData>(json);
        }

        return new GameProgressData();
    }

    public bool HasSavedProgress()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}