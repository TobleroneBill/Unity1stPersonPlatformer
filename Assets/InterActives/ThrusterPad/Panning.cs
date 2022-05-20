using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panning : MonoBehaviour
{
    public float scrollY = 0.5f;
    // Update is called once per frame
    void Update()
    {
        float offsetY = scrollY * Time.time;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.3f, offsetY);
    }
}
