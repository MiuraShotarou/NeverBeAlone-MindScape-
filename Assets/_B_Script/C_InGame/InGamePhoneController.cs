using UnityEngine;
using UnityEngine.InputSystem;

public class InGamePhoneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _beforePhone;
    [SerializeField] GameObject _afterPhone;
    [SerializeField] GameObject _phoneObject;

    [Header("スワイプ判定")]
    [SerializeField] float swipeSpeed = 50f;

    Vector2 startPos;
    bool isSlided = false;

    void Update()
    {
        // === スマホタッチ ===
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            var phase = touch.phase.ReadValue();

            if (phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                startPos = touch.position.ReadValue();
            }
            else if (phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Vector2 delta = touch.position.ReadValue() - startPos;

                if (delta.x < -swipeSpeed && !isSlided) ShowPhone();
                else if (delta.x > swipeSpeed && isSlided) HidePhone();
            }
        }

        // === PCデバッグ ===
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPos = Mouse.current.position.ReadValue();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 delta = Mouse.current.position.ReadValue() - startPos;

            if (delta.x < -swipeSpeed && !isSlided) ShowPhone();
            else if (delta.x > swipeSpeed && isSlided) HidePhone();
        }
#endif
    }

    private void ShowPhone()
    {
        _beforePhone.SetActive(false);
        _phoneObject.SetActive(true);
        _afterPhone.SetActive(true);
        isSlided = true;
    }

    private void HidePhone()
    {
        _phoneObject.SetActive(false);
        _afterPhone.SetActive(false);
        _beforePhone.SetActive(true);
        isSlided = false;
    }
}
