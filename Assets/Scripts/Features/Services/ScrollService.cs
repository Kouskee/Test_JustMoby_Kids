using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScrollService : IScrollService, IDisposable
{
    private readonly ReactiveCollection<Cube> _scrollCubes = new ReactiveCollection<Cube>();
    private readonly GameConfig _config;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private ScrollRect _scrollRect;
    private Transform _scrollContent;
    private PlaceholderFactory<Cube> _cubeFactory;
    private ITowerService _towerService;
    private List<Sprite> _availableCubesData;

    public IReadOnlyReactiveCollection<Cube> ScrollCubes => _scrollCubes;

    [Inject]
    public ScrollService(GameConfig config, PlaceholderFactory<Cube> cubeFactory, ITowerService towerService)
    {
        _config = config;
        _cubeFactory = cubeFactory;
        _towerService = towerService;

        SubscribeToTowerEvents();
    }

    public void InitializeScroll(List<Sprite> cubesData)
    {
        _availableCubesData = cubesData;
        FindScrollComponents();
        CreateScrollCubes(cubesData);
    }

    public void SetScrollEnabled(bool enabled)
    {
        if (_scrollRect != null)
            _scrollRect.enabled = enabled;
    }

    private void SubscribeToTowerEvents()
    {
        _towerService.TowerCubes
            .ObserveAdd()
            .Subscribe(OnCubeAddedToTower)
            .AddTo(_disposables);
    }

    private void OnCubeAddedToTower(CollectionAddEvent<Cube> addEvent)
    {
        var cubeInTower = addEvent.Value;
        RemoveCubeFromScroll(cubeInTower);

        if (_scrollCubes.Count < _config.ScrollCubesCount)
        {
            CreateNewScrollCube();
        }
    }

    private void RemoveCubeFromScroll(Cube cube)
    {
        if (_scrollCubes.Contains(cube))
        {
            _scrollCubes.Remove(cube);
        }
    }

    private void CreateNewScrollCube()
    {
        if (_availableCubesData == null || _availableCubesData.Count == 0)
            return;

        var cubeData = _availableCubesData[UnityEngine.Random.Range(0, _availableCubesData.Count)];
        var cube = _cubeFactory.Create();
        cube.transform.SetParent(_scrollContent);
        cube.Initialize(cubeData);
        _scrollCubes.Add(cube);
    }

    private void FindScrollComponents()
    {
        var scrollArea = GameObject.FindGameObjectWithTag("ScrollArea");
        if (scrollArea != null)
        {
            _scrollRect = scrollArea.GetComponent<ScrollRect>();
            _scrollContent = _scrollRect.content;
        }
    }

    private void CreateScrollCubes(List<Sprite> cubesData)
    {
        _scrollCubes.Clear();

        for (int i = 0; i < _config.ScrollCubesCount; i++)
        {
            var cubeData = cubesData[UnityEngine.Random.Range(0, cubesData.Count)];
            var cube = _cubeFactory.Create();
            cube.transform.SetParent(_scrollContent);

            cube.Initialize(cubeData);
            _scrollCubes.Add(cube);
        }
    }

    public void Dispose()
    {
        _disposables?.Dispose();
    }
}