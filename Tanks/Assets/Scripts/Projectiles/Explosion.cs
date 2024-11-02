using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float fadeTime = 0.5f; //time for explosion to fade away
    private Material material;
    private Color startColor;
    private Color finalColor;

    private float currentFade = 0f;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        startColor = material.color;
        finalColor = material.color;
        finalColor.a = 0;
    }

    void Update()
    {
        currentFade += Time.deltaTime;
        if (currentFade > fadeTime) {
            Destroy(gameObject);
        }
        
        material.color = Color.Lerp(startColor, finalColor, currentFade / fadeTime);
    }
}
