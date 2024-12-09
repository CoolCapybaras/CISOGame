using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayout : MonoBehaviour
{
    public GameObject playerPrefab; // Префаб игрока
    public Transform topPlayerContainer; // Контейнер для верхних игроков
    public int maxPlayers = 5; // Максимальное количество игроков
    public List<GameObject> players;
    
    public void SetupPlayers(int totalPlayers)
    {
        players.Clear();
        totalPlayers = Mathf.Clamp(totalPlayers, 2, maxPlayers);
        
        Utils.DestroyChildren(topPlayerContainer);
        
        float startAngle = -120;
        float endAngle = -60;  
        
        if (totalPlayers == 2)
        {
            GameObject player = Instantiate(playerPrefab, topPlayerContainer);
            RectTransform playerRect = player.GetComponent<RectTransform>();
            players.Add(player);
            
            // Устанавливаем позицию прямо сверху
            playerRect.anchorMin = new Vector2(0.5f, 1);
            playerRect.anchorMax = new Vector2(0.5f, 1);
            playerRect.pivot = new Vector2(0.5f, 0.5f);
            playerRect.anchoredPosition = new Vector2(0, -300); // Радиус сверху
            return;
        }
        
        for (int i = 1; i < totalPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, topPlayerContainer);
            RectTransform playerRect = player.GetComponent<RectTransform>();
            players.Add(player);

            float angle = Mathf.Lerp(startAngle, endAngle, (float)(i - 1) / (totalPlayers - 2));
            
            // Вычисляем позицию игрока по дуге
            float radius = 700f; // Радиус дуги
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            // Устанавливаем позицию игрока
            playerRect.anchorMin = new Vector2(0.5f, 1);
            playerRect.anchorMax = new Vector2(0.5f, 1);
            playerRect.pivot = new Vector2(0.5f, 0.5f);
            playerRect.anchoredPosition = new Vector2(x, -y - radius - 200);
        }
    }
}
