using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static void DestroyChildren(Transform transform)
    {
        for (int i = 0; i < transform.childCount; ++i)
            Object.Destroy(transform.GetChild(i).gameObject);
    }

    public static IEnumerator LoadTexture(string url, RawImage image)
    {
        Texture2D texture = new Texture2D(0, 0);

        WWW www = new WWW(url);
        yield return www;

        www.LoadImageIntoTexture(texture);
        www.Dispose();
        www = null;
        image.texture = texture;
    }
}
