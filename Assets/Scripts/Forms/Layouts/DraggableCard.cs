using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform tableArea; // Область стола
    private Transform _originalParent; // Исходный родитель карты (рука)
    private Vector2 _originalPosition; // Исходная позиция карты
    private Canvas _canvas; // Canvas для работы с UI
    private CanvasGroup _canvasGroup; // CanvasGroup для управления блокировкой
    private RectTransform _rectTransform; // RectTransform карты
    private Vector2 _dragOffset; // Смещение между курсором и центром карты
    private int _siblingIndex;

    public bool canDrag = false;
    public Card card;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>(); // Получаем RectTransform карты
        _originalPosition = _rectTransform.anchoredPosition; // Сохраняем изначальную позицию
        _canvas = GetComponentInParent<Canvas>(); // Находим Canvas
        _canvasGroup = GetComponent<CanvasGroup>(); // CanvasGroup для карты
        _originalParent = transform.parent;
    }

    // Вызывается при начале перетаскивания
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;
        _originalPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false; // Отключаем raycast, чтобы карта "пропускала" сквозь себя
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetSiblingIndex(_originalParent.childCount);
        // Вычисляем смещение между курсором и центром карты
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out var localCursor
        );
        _dragOffset = localCursor - _rectTransform.anchoredPosition; // Смещение между курсором и картой
    }

    // Вызывается во время перетаскивания
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;
        
        // Перемещаем карту за курсором с учетом смещения
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out var localPoint
        );

        _rectTransform.anchoredPosition = localPoint - _dragOffset; // Учитываем смещение
    }

    
    // Вызывается, когда карта отпущена
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;        
        
        _canvasGroup.blocksRaycasts = true; // Включаем raycast обратно
        transform.SetSiblingIndex(_siblingIndex);
        // Проверяем, находится ли карта в области стола
        if (RectTransformUtility.RectangleContainsScreenPoint(tableArea, eventData.position, _canvas.worldCamera))
        {
            // Если карта попала в область стола, вызываем метод
            OnCardDroppedOnTable();
        }
        else
        {
            // Если не попала, возвращаем карту на место
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }

    // Метод вызывается, когда карта брошена на стол
    private void OnCardDroppedOnTable()
    {
        Debug.Log("Карта брошена на стол: " + gameObject.name);
        GameForm.Instance.OnCardDropped(this);
    }
}
