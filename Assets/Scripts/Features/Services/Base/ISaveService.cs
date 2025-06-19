public interface ISaveService
{
    void SaveProgress(GameProgressData data);
    GameProgressData LoadProgress();
    bool HasSavedProgress();
}
