using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class TowerService : ITowerService
{
    private readonly ReactiveCollection<Cube> _towerCubes = new ReactiveCollection<Cube>();
    private readonly GameConfig _config;
    private Canvas _canvas;
    private RectTransform _towerArea;

    public IReadOnlyReactiveCollection<Cube> TowerCubes => _towerCubes;

    [Inject]
    public TowerService(GameConfig config)
    {
        _config = config;
        FindTowerComponents();
    }

    public bool CanAddCube(Cube cube, Vector2 screenPosition)
    {
        if (cube.IsInTower) return false;
        if (WillCubeExceedBounds(cube.RectTransform)) return false;

        return IsInTowerBuildAreaWithCubeSize(cube, screenPosition);
    }

    public bool TryAcceptCube(Cube cube, Vector2 screenPosition)
    {
        if (!CanAddCube(cube, screenPosition)) return false;

        AddCube(cube);
        cube.SetInTower(true);
        return true;
    }

    public void AddCube(Cube cube)
    {
        if (!cube.IsInTower)
        {
            _towerCubes.Add(cube);
        }
    }

    public void RemoveCube(Cube cube)
    {
        if (_towerCubes.Contains(cube))
        {
            _towerCubes.Remove(cube);
        }
    }

    public void InsertCubeAtIndex(Cube cube, int index)
    {
        if (index >= 0 && index <= _towerCubes.Count)
        {
            _towerCubes.Insert(index, cube);
        }
    }

    private bool WillCubeExceedBounds(RectTransform cubeRect)
    {
        if (_towerCubes.Count == 0) return false;

        var cubeBelow = _towerCubes[GetCubeIndex(_towerCubes.Last())];
        float heightCube = cubeBelow.RectTransform.rect.height;
        Vector2 positionCube = cubeBelow.RectTransform.anchoredPosition;

        float yOffset = positionCube.y + heightCube;
        var targetPosition = new Vector2(positionCube.x, yOffset);

        cubeRect.SetParent(_towerArea, true);
        var containerBounds = _towerArea.rect;

        var cubeSize = cubeRect.rect;

        float cubeTopY = targetPosition.y + cubeSize.height * 0.5f;
        if (cubeTopY > 0)
            return true;

        float cubeBottomY = targetPosition.y - cubeSize.height * 0.5f;
        if (cubeBottomY < -containerBounds.height)
            return true;

        return false;
    }


    private bool IsInTowerBuildAreaWithCubeSize(Cube cube, Vector2 screenPosition)
    {
        if (_towerArea == null)
            return false;

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _towerArea, screenPosition, _canvas.worldCamera, out localPoint))
        {
            return false;
        }
        var cubeRect = cube.RectTransform;

        float cubeWidth = cubeRect.rect.width;
        float cubeHeight = cubeRect.rect.height;

        cubeWidth *= cubeRect.lossyScale.x;
        cubeHeight *= cubeRect.lossyScale.y;

        var towerRect = _towerArea.rect;
        float leftBound = towerRect.xMin + cubeWidth * 0.5f;
        float rightBound = towerRect.xMax - cubeWidth * 0.5f;
        float bottomBound = towerRect.yMin + cubeHeight * 0.5f;
        float topBound = towerRect.yMax - cubeHeight * 0.5f;

        bool isInBounds = localPoint.x >= leftBound &&
                         localPoint.x <= rightBound &&
                         localPoint.y >= bottomBound &&
                         localPoint.y <= topBound;

        return isInBounds;
    }

    public int GetCubeIndex(Cube cube)
    {
        return _towerCubes.IndexOf(cube);
    }

    public List<CubeSaveData> GetTowerSaveData()
    {
        return _towerCubes.Select((cube, index) => new CubeSaveData
        {
            IndexInTower = index,
            PosX = cube.RectTransform.anchoredPosition.x,
            PosY = cube.RectTransform.anchoredPosition.y
        }).ToList();
    }

    public void LoadTowerState(List<CubeSaveData> cubesData)
    {
        _towerCubes.Clear();
    }

    private void FindTowerComponents()
    {
        var towerAreaGO = GameObject.FindGameObjectWithTag("TowerArea");
        if (towerAreaGO != null)
        {
            _towerArea = towerAreaGO.GetComponent<RectTransform>();
            _canvas = towerAreaGO.GetComponentInParent<Canvas>();
        }
    }
}