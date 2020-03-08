using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

/// <summary>
/// The UI element that tells players how to join the game
/// </summary>
public class JoinGameUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Text content")]
    [SerializeField] private TMPro.TMP_Text JoinGameTextElement = default;
    [SerializeField] private string LoadingText = "Loading...";

    [Header("Collapse/expand animation")]
    [SerializeField] private float ExpanededHeight = 200f;
    [SerializeField] private float HeightChangeAnimationLength = 2f;
    [SerializeField] private AnimationCurve HeightChangeAnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private string MessageTemplate;
    private string gameID = null;

    private bool isExpaneded = false;
    private float collapsedHeight;
    private RectTransform rectTransform;

    private void Start()
    {
        Assert.IsNotNull(JoinGameTextElement, "JoinGameUI requires a reference to a text element");
        MessageTemplate = JoinGameTextElement.text;
        JoinGameTextElement.text = LoadingText;

        rectTransform = GetComponent<RectTransform>();
        collapsedHeight = rectTransform.sizeDelta.y;

        // TODO: register listeners to game manager to expand and collapse at different states of the game
    }

    public void SetGameID(string gameID)
    {
        JoinGameTextElement.text = MessageTemplate
            .Replace("{shorturl}", StaticNetworkSettings.ShortURL)
            .Replace("{gameid}", gameID);

        this.gameID = gameID;
    }

    public void SetExpanded(bool expanded)
    {
        if (expanded != isExpaneded)
        {
            // change ui element size
            isExpaneded = expanded;
            StopAllCoroutines();
            if (isExpaneded)
            {
                StartCoroutine(AnimateHeight(ExpanededHeight));
            }
            else
            {
                StartCoroutine(AnimateHeight(collapsedHeight));
            }
        }
    }

    private IEnumerator AnimateHeight(float targetHeight)
    {
        float initialHeight = rectTransform.sizeDelta.y;
        float elapsedTime = 0f;
        while (elapsedTime < HeightChangeAnimationLength)
        {
            float newHeight = Mathf.LerpUnclamped(
                initialHeight,
                targetHeight,
                HeightChangeAnimationCurve.Evaluate(elapsedTime / HeightChangeAnimationLength)
            );
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, targetHeight);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(StaticNetworkSettings.PlayerURL(gameID));
    }
}
