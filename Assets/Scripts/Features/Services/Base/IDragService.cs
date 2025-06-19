using System;

public interface IDragService
{
    IObservable<DragData> OnDragStarted { get; }
    IObservable<DragData> OnDragUpdated { get; }
    IObservable<DragData> OnDragEnded { get; }
}