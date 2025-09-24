using UnityEngine;
/// <summary>
/// どのデバイスで実行しても画面アスペクトが変わらないようにするためAIにお願いして作成されたスクリプト
/// </summary>
public class CameraAspectController : MonoBehaviour
{
    //
    [SerializeField] float targetAspectWidth = 16f;
    [SerializeField] float targetAspectHeight = 9f;

    Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        UpdateCameraViewport();
    }
    /// <summary>
    /// 
    /// </summary>
    void UpdateCameraViewport()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // Letterbox（上下に黒帯）
            Rect rect = _cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            _cam.rect = rect;
        }
        else
        {
            // Pillarbox（左右に黒帯）
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = _cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            _cam.rect = rect;
        }
    }
}