using System;
using UniRx;
using UnityEngine;

public interface IAnimationService
{
    IObservable<Unit> AnimateCubeJump(RectTransform cubeTransform, Vector2 targetPosition);
    IObservable<Unit> AnimateCubeMove(RectTransform cubeTransform, Vector2 targetPosition);
    IObservable<Unit> AnimateNotification(CanvasGroup canvasGroup, float duration);
    IObservable<Unit> AnimateCubeToHole(RectTransform cubeTransform, Vector2 holePosition);
}