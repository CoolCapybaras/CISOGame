using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandLayout : MonoBehaviour
{
    public List<RectTransform> cards; // Ссылки на RectTransform карт
    public float curveRadius = 200f; // Радиус кривой
    public float angleRange = 90f; // Угол, на котором располагаются карты
    public float horizontalSpacingFactor = 50f; // Горизонтальное смещение
    public float verticalSpacingFactor = 50f;
    
    private void UpdateLayout()
    {
        int cardCount = cards.Count;
        if (cardCount == 0) return;

        if (cardCount == 1)
        {
            // Если всего одна карта, ставим ее в центр и убираем поворот
            cards[0].anchoredPosition = Vector2.zero; // Центр
            cards[0].rotation = Quaternion.identity; // Без поворота
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
            cards[i].anchoredPosition = new Vector2(x, -y); // Устанавливаем позицию
            cards[i].rotation = Quaternion.Euler(0, 0, -angle); // Поворот карты
        }
    }

    // Для теста можно вызвать метод в Start или через UI
    private void Start()
    {
        UpdateLayout();
    }

    private void Update()
    {
        UpdateLayout();
    }

    public void AddCard(RectTransform newCard)
    {
        cards.Add(newCard);

        UpdateLayout();
    }

    public void RemoveCard(RectTransform cardToRemove)
    {
        // Удаляем карту из массива
        cards.Remove(cardToRemove);

        UpdateLayout();
    }
}
