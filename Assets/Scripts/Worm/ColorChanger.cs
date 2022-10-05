using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    private Renderer _renderer = null;
    void Start()
    {
        _renderer = this.GetComponent<Renderer>();
        GetComponentInParent<PlayerController>().HandleColor += HandleColor;
    }

    private void HandleColor(Color color)
    {
        _renderer.sharedMaterial.color = color;
    }
}


