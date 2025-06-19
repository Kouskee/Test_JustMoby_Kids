using System;

public interface IDraggable
{
    IObservable<Cube> OnDragStarted { get; }
    IObservable<Cube> OnDragEnded { get; }
    void SetDragMode(bool isDragging);
}