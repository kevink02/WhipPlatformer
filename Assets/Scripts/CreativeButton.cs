using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreativeButton : MonoBehaviour
{
    private void Awake()
    {
        // Allows clicking to not "hit" transparent space of image
        Image image = GetComponent<Image>();
        if (image)
            image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
