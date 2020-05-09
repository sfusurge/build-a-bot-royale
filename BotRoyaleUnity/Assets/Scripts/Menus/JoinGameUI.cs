using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoinGameUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMPro.TMP_Text TextUIElement = default;

    private string textTemplate;
    private string gameID;


    private void Start()
    {
        textTemplate = TextUIElement.text;
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void ShowGameID(string gameID)
    {
        this.gameID = gameID;

        GetComponent<CanvasGroup>().alpha = 1f;

        TextUIElement.text = textTemplate
            .Replace("{url}", StaticNetworkSettings.ShortURL)
            .Replace("{code}", gameID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string url = StaticNetworkSettings.QuickJoinURL(gameID);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
#pragma warning disable 0618
            Application.ExternalEval("window.open(\"" + url + "\")");
#pragma warning restore 0618
        }
        else
        {
            Application.OpenURL(url);
        }
    }
}
