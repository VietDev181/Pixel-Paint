using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILevelButton : MonoBehaviour
{
    public GameObject starContainer;     // chứa các ngôi sao
    public List<Image> stars;            // list các sao (0 = tắt, 1 = bật)
    public Sprite starOn;
    public Sprite starOff;

    public void SetStars(int count)
    {
        if (starContainer == null || stars == null) return;

        // Luôn hiện container nếu level đã mở
        starContainer.SetActive(true);

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].sprite = (i < count) ? starOn : starOff;
        }
    }
}
