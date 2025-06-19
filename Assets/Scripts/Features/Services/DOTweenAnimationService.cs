using DG.Tweening;
using System;
using UniRx;
using UnityEngine;

public class DOTweenAnimationService : IAnimationService
{
    public IObservable<Unit> AnimateCubeJump(RectTransform cubeTransform, Vector2 targetPosition)
    {
        return Observable.Create<Unit>(observer =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(cubeTransform.DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.OutBounce));
            sequence.OnComplete(() =>
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            });

            return Disposable.Create(() => sequence.Kill());
        });
    }

    public IObservable<Unit> AnimateCubeMove(RectTransform cubeTransform, Vector2 targetPosition)
    {
        return Observable.Create<Unit>(observer =>
        {
            var tween = cubeTransform.DOAnchorPos(targetPosition, 0.3f);
            tween.OnComplete(() =>
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            });

            return Disposable.Create(() => tween.Kill());
        });
    }

    public IObservable<Unit> AnimateNotification(CanvasGroup canvasGroup, float duration)
    {
        return Observable.Create<Unit>(observer =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1f, 0.2f));
            sequence.AppendInterval(duration);
            sequence.Append(canvasGroup.DOFade(0f, 0.2f));
            sequence.OnComplete(() =>
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            });

            return Disposable.Create(() => sequence.Kill());
        });
    }

    public IObservable<Unit> AnimateCubeToHole(RectTransform cubeTransform, Vector2 holePosition)
    {
        return Observable.Create<Unit>(observer =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(cubeTransform.DOMove(holePosition, 0.5f));
            sequence.Join(cubeTransform.DOScale(Vector2.zero, 0.5f));
            sequence.OnComplete(() =>
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            });

            return Disposable.Create(() => sequence.Kill());
        });
    }
}