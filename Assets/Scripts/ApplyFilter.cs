using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFilter : MonoBehaviour
{
    public Material effectMaterial;

    // Apply a Material to src before render to dest
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Copy the source Render Texture to the destination,
        // applying the material along the way.
        Graphics.Blit(src, dest, effectMaterial);
    }
}
