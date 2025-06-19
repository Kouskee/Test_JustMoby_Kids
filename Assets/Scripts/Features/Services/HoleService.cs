using UniRx;
using UnityEngine;
using Zenject;

public class HoleService : IHoleService
{
    private readonly IAnimationService _animationService;
    private readonly ITowerService _towerService;
    private Transform _holeTransform;
    private Camera _camera;

    public Vector2 HolePosition => _holeTransform != null ? _holeTransform.position : Vector3.zero;

    [Inject]
    public HoleService(IAnimationService animationService, ITowerService towerService)
    {
        _animationService = animationService;
        _towerService = towerService;
        _camera = Camera.main;
        FindHoleTransform();
    }

    public bool IsInHoleArea(Vector2 worldPosition)
    {
        if (_holeTransform == null) return false;

        var holeArea = GameObject.FindGameObjectWithTag("HoleArea");
        if (holeArea == null) return false;

        var rectTransform = holeArea.GetComponent<RectTransform>();
        var screenPoint = _camera.WorldToScreenPoint(worldPosition);

        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, _camera);
    }

    public void ThrowCubeToHole(Cube cube)
    {
        if (_holeTransform == null) return;

        if (cube.IsInTower)
        {
            _towerService.RemoveCube(cube);
        }

        _animationService.AnimateCubeToHole(cube.RectTransform, _holeTransform.position)
            .Subscribe(_ =>
            {
                cube.SetVisible(false);
                Object.Destroy(cube.gameObject);
            });
    }

    private void FindHoleTransform()
    {
        var holeObject = GameObject.FindGameObjectWithTag("HoleArea");
        if (holeObject != null)
            _holeTransform = holeObject.transform;
    }
}