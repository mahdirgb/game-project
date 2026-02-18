using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    private float targetWidth = 1080f;       // match design width
    
    private float pixelsPerUnit = 100f;      // typical PPU value

    private float fullhd_aspect = 9f / 16f;
    void Start()
    {
        float screenAspect = (float)Screen.height / Screen.width;
        Debug.Log(string.Format("width:{0},height:{1}", Screen.width, Screen.height));
        float diff = (1/screenAspect) / fullhd_aspect;
        if (diff > 1)//wider:fit height
        {
            float orthoSize = (1920 / pixelsPerUnit) * screenAspect;
            Camera.main.orthographicSize = orthoSize;
            Debug.Log("wider");
        }
        else//taller
        {
            float orthoSize = (targetWidth / pixelsPerUnit) * screenAspect * 0.5f;
            Camera.main.orthographicSize = orthoSize;
            Debug.Log("taller");
        }
        
    }
}
