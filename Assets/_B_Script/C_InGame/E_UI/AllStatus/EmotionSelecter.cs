using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EmotionSelecter : MonoBehaviour
{
    private Image _image = default;
    [SerializeField] private Sprite _imageSelected;
    [SerializeField] private Sprite _imageNotSelected;

    private List<EmotionSelecter> _siblingEmotionSelecters = new List<EmotionSelecter>();

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        Transform parent = transform.parent;

        foreach (Transform child in parent)
        {

            if (child == this.transform) continue;

            var es = child.gameObject.GetComponent<EmotionSelecter>();

            if (es != null) _siblingEmotionSelecters.Add(es);
        }
    }

    public void Select(bool selected)
    {
        if (selected)
        {
            foreach (var es in _siblingEmotionSelecters)
            {
                Debug.Log(es);
                es.Select(false);
            }

            _image.sprite = _imageSelected;
            _image.SetNativeSize();
   
        }
        else
        {
            _image.sprite = _imageNotSelected;
            _image.SetNativeSize();
        }
    }
}
