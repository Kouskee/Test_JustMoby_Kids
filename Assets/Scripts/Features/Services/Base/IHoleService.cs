using UnityEngine;

public interface IHoleService
{
    Vector2 HolePosition { get; }
    bool IsInHoleArea(Vector2 screenPosition);
    void ThrowCubeToHole(Cube cube);
}