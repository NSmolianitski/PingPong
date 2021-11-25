using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color clickColor;

    [Header("Other")]
    [SerializeField] private float hoverSizeModifier = 1.2f;
    [SerializeField] private float clickSizeModifier = 0.8f;
    
    private Image _image;
    private Vector3 _defaultSize;
    private Vector3 _hoverSizeVector;
    private Vector3 _clickSizeVector;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _defaultSize = transform.localScale;
        _hoverSizeVector = new Vector3(hoverSizeModifier, hoverSizeModifier, hoverSizeModifier);
        _clickSizeVector = new Vector3(clickSizeModifier, clickSizeModifier, clickSizeModifier);
    }

    public void OnPointerEnter()
    {
        _image.color = hoverColor;
        
        transform.localScale = _hoverSizeVector;
    }

    public void OnPointerLeave()
    {
        _image.color = defaultColor;
        
        transform.localScale = _defaultSize;
    }

    public void OnPointerDown()
    {
        _image.color = clickColor;
        
        transform.localScale = _clickSizeVector;
    }

    public void OnPointerUp()
    {
        _image.color = hoverColor;

        transform.localScale = _defaultSize;
    }
}
