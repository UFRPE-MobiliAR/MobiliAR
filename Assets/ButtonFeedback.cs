using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour
{
    public Color pressedColor = Color.gray; // Cor quando o botão é pressionado
    public float feedbackDuration = 0.1f; // Duração do feedback

    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    public void OnButtonPressed()
    {
        if (buttonImage != null)
        {
            StartCoroutine(ChangeButtonColor());
        }
    }

    IEnumerator ChangeButtonColor()
    {
        buttonImage.color = pressedColor; // Muda para a cor de pressionado
        yield return new WaitForSeconds(feedbackDuration); // Espera o tempo do feedback
        buttonImage.color = originalColor; // Reverte para a cor original
    }
}
