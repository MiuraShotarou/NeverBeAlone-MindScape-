using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TransitionPhoneController : MonoBehaviour
{
    [Header("(C)Phone下にあるオブジェクトをすべてアタッチ")]
    [SerializeField] GameObject _phoneBase;
    [SerializeField] GameObject _beforeIcon;
    [SerializeField] GameObject _afterIcon;
    [Header("スワイプ許容範囲")]
    [SerializeField] float swipeSpeed = 50f;
    bool isSlided = false;
    Vector2 startPos;
    void Update()
    {
        // === スマホ用 ===
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
        // === PC用 ===
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
    }

    private void ShowPhone()
    {
        _phoneBase.SetActive(true);
        _beforeIcon.SetActive(false);
        _afterIcon.SetActive(true);
        isSlided = true;
    }

    private void HidePhone()
    {
        _phoneBase.SetActive(false);
        _beforeIcon.SetActive(true);
        _afterIcon.SetActive(false);
        isSlided = false;
    }
}