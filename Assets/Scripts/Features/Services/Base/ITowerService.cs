using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ITowerService
{
    IReadOnlyReactiveCollection<Cube> TowerCubes { get; }

    bool CanAddCube(Cube cube, Vector2 screenPosition);
    bool TryAcceptCube(Cube cube, Vector2 screenPosition);
    void AddCube(Cube cube);
    void RemoveCube(Cube cube);
    void InsertCubeAtIndex(Cube cube, int index);
    int GetCubeIndex(Cube cube);
    List<CubeSaveData> GetTowerSaveData();
    void LoadTowerState(List<CubeSaveData> cubesData);
}