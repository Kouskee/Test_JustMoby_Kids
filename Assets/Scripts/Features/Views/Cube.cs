using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class Cube : MonoBehaviour, IDraggable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _spriteRenderer;

    [Inject] private ITowerService _towerService;

    public bool IsInTower { get; private set; }
    public bool IsDragging { get; private set; }
    public Vector3 DragStartPosition => _dragStartPosition;

    public IObservable<Cube> OnDragStarted => _onDragStarted;
    public IObservable<Cube> OnDragEnded => _onDragEnded;

    public RectTransform RectTransform => _rectTransform;

    private Subject<Cube> _onDragStarted = new Subject<Cube>();
    private Subject<Cube> _onDragEnded = new Subject<Cube>();
    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Vector3 _dragStartPosition;
    private Transform _originalParent;
    private int _originalSiblingIndex;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    public void Initialize(Sprite sprite)
    {
        _originalScale = transform.localScale;
        _rectTransform = GetComponent<RectTransform>();

        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        if (sprite != null)
            _spriteRenderer.sprite = sprite;
    }

    public void SetInTower(bool inTower)
    {
        IsInTower = inTower;
    }

    public void SetDragMode(bool isDragging)
    {
        IsDragging = isDragging;
        transform.localScale = isDragging ? _originalScale * 1.1f : _originalScale;
        _canvasGroup.alpha = isDragging ? 0.8f : 1f;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPosition = _rectTransform.anchoredPosition;
        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        _originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling();

        _canvasGroup.blocksRaycasts = false;

        SetDragMode(true);
        _onDragStarted.OnNext(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out localPointerPosition))
        {
            _rectTransform.localPosition = localPointerPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        SetDragMode(false);
        _onDragEnded.OnNext(this);

        bool wasAccepted = _towerService.TryAcceptCube(this, eventData.position);

        if (!wasAccepted)
        {
            ReturnToOriginalPosition();
        }
    }

    public void ReturnToOriginalPosition()
    {
        transform.SetParent(_originalParent, false);
        transform.SetSiblingIndex(_originalSiblingIndex);
        _rectTransform.anchoredPosition = _originalPosition;
    }

    private void OnDestroy()
    {
        _onDragStarted?.OnCompleted();
        _onDragEnded?.OnCompleted();
        _onDragStarted?.Dispose();
        _onDragEnded?.Dispose();
    }
}