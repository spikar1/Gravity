using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererAppendedData : MonoBehaviour
{
    [SerializeField, AssetsOnly]
    Texture2D sprite;

    private void OnEnable()
    {
        if(sprite == null)
            return;
        if (Application.isPlaying)
        {
                GetComponent<SpriteRenderer>().material.SetTexture("_SecondaryMap", sprite);
        }
        else
                GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_SecondaryMap", sprite);
    }
}
