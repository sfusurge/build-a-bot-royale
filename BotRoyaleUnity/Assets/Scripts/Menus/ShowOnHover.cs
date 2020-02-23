using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Graphic[] GraphicRenderers = default;

    void Start()
    {
        foreach (Graphic graphic in GraphicRenderers)
        {
            SetAlpha(graphic, 0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Graphic graphic in GraphicRenderers)
        {
            SetAlpha(graphic, 1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Graphic graphic in GraphicRenderers)
        {
            SetAlpha(graphic, 0f);
        }
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Graphic graphic in GraphicRenderers)
        {
            SetAlpha(graphic, 0f);
        }
    }
}
