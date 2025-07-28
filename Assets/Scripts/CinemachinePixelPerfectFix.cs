using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachinePixelPerfectFix : MonoBehaviour
{
    private int ppu = 32;
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x * ppu) / ppu, Mathf.Round(transform.position.y * ppu) / ppu, transform.position.z);
    }
    // Replace ppu with the value of "Assets Pixel per Unit" in your Pixel Perfect Component
}
