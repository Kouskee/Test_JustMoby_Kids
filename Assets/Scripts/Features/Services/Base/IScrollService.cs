using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IScrollService
{
    IReadOnlyReactiveCollection<Cube> ScrollCubes { get; }
    void InitializeScroll(List<Sprite> cubesData);
    void SetScrollEnabled(bool enabled);
}
