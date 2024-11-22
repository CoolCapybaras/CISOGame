using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandLayout : MonoBehaviour
{
    private List<RectTransform> _cards = new(); // Ссылки на RectTransform карт
    public float curveRadius = 200f; // Радиус кривой
    public float angleRange = 90f; // Угол, на котором располагаются карты
    public float horizontalSpacingFactor = 50f; // Горизонтальное смещение
    public float verticalSpacingFactor = 50f;
    
    public void UpdateLayout()
    {
        _cards.Clear();
        foreach (var obj in transform)
            _cards.Add(obj as RectTransform);
        
        int cardCount = _cards.Count;
        if (cardCount == 0) return;

        if (cardCount == 1)
        {
            // Если всего одна карта, ставим ее в центр и убираем поворот
            _cards[0].anchoredPosition = Vector2.zero; // Центр
            _cards[0].rotation = Quaternion.identity; // Без поворота
            return;
        }

        float angleStep = angleRange / (cardCount - 1); // Угол между картами

        for (int i = 0; i < cardCount; i++)
        {
            // Вычисляем угол для текущей карты
            float angle = -angleRange / 2 + angleStep * i;
            float radian = angle * Mathf.Deg2Rad;

            // Основное смещение по кругу
            float x = Mathf.Sin(radian) * curveRadius;
            float y = Mathf.Cos(radian) * curveRadius;

            // Дополнительное горизонтальное смещение в зависимости от позиции
            x += (i - (cardCount - 1) / 2.0f) * horizontalSpacingFactor;

            // Дополнительное вертикальное смещение вниз в зависимости от угла
            y -= Mathf.Abs(angle) / angleRange * verticalSpacingFactor;

            // Обновляем позицию и поворот карты
            _cards[i].anchoredPosition = new Vector2(x, -y); // Устанавливаем позицию
            _cards[i].rotation = Quaternion.Euler(0, 0, -angle); // Поворот карты
        }
    }

    // Для теста можно вызвать метод в Start или через UI
    private void Awake()
    {
        UpdateLayout();
    }

    public void AddCard(RectTransform newCard)
    {
        _cards.Add(newCard);

        UpdateLayout();
    }

    public void RemoveCard(RectTransform cardToRemove)
    {
        // Удаляем карту из массива
        _cards.Remove(cardToRemove);

        UpdateLayout();
    }
}
