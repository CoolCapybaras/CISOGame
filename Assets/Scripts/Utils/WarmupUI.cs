using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmupUI : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i).gameObject;

            if (child.activeInHierarchy) continue;
            child.SetActive(true);
            child.SetActive(false);
        }
    }
}
