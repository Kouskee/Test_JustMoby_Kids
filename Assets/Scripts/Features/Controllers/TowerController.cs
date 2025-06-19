using UniRx;
using UnityEngine;
using Zenject;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Transform _towerContainer;
    [SerializeField] private RectTransform _containerRect;

    [Inject] private ITowerService _towerService;
    [Inject] private IAnimationService _animationService;
    [Inject] private GameConfig _config;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        if (_containerRect == null)
            _containerRect = _towerContainer.GetComponent<RectTransform>();

        _towerService.TowerCubes
            .ObserveAdd()
            .Subscribe(OnCubeAdded)
            .AddTo(_disposables);

        _towerService.TowerCubes
            .ObserveRemove()
            .Subscribe(OnCubeRemoved)
            .AddTo(_disposables);
    }

    private void OnCubeAdded(CollectionAddEvent<Cube> addEvent)
    {
        var cube = addEvent.Value;
        var cubeRect = cube.RectTransform;

        cubeRect.SetParent(_towerContainer, true);

        if (addEvent.Index > 0)
        {
            var targetPosition = CalculateCubePosition(addEvent.Index);

            _animationService.AnimateCubeJump(cubeRect, targetPosition)
                .Subscribe()
                .AddTo(_disposables);
        }
    }

    private void OnCubeRemoved(CollectionRemoveEvent<Cube> removeEvent)
    {
        var cube = removeEvent.Value;
        cube.SetInTower(false);
    }

    private Vector2 CalculateCubePosition(int index)
    {
        float xOffset = 0f;

        var cubeBelow = _towerService.TowerCubes[index - 1];
        float heightCube = cubeBelow.RectTransform.rect.height;
        Vector2 positionCube = cubeBelow.RectTransform.anchoredPosition;
        float yOffset = positionCube.y + heightCube;

        if (_towerService.TowerCubes.Count > 0)
        {
            var firstCube = _towerService.TowerCubes[0];
            var firstCubeRect = firstCube.RectTransform;
            xOffset = firstCubeRect.anchoredPosition.x;

            var maxOffset = _config?.MaxHorizontalOffset ?? 20f;
            var randomOffset = Random.Range(-maxOffset, maxOffset);
            xOffset += randomOffset;
        }

        return new Vector2(xOffset, yOffset);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}