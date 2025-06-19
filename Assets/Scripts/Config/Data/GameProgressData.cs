using System.Collections.Generic;

[System.Serializable]
public class GameProgressData
{
    public List<CubeSaveData> TowerCubes = new List<CubeSaveData>();
    public long SaveTimestamp;
}
