using UnityEngine;
using UnityEngine.UI;

public class BotaoLogin : MonoBehaviour
{
    public Button botaoLogar; // Referência para o botão de login
    public LoginManager loginManager; // Referência para o script LoginManager

    void Start()
    {
        // Verifica se todas as referências foram configuradas
        if (botaoLogar == null)
        {
            Debug.LogError("Botão Logar não atribuído no script BotaoLogin.");
            return;
        }

        if (loginManager == null)
        {
            Debug.LogError("LoginManager não atribuído no script BotaoLogin.");
            return;
        }

        // Adiciona a função OnLoginButtonClicked ao evento OnClick do botão
        botaoLogar.onClick.AddListener(loginManager.OnLoginButtonClicked);
    }
}
