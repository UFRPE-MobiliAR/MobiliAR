using UnityEngine;
using UnityEngine.UI;

public class BotaoCadastro : MonoBehaviour
{
    public Button botaoCadastrar; // Referência para o botão de cadastro
    public CadastroManager cadastroManager; // Referência para o script CadastroManager

    void Start()
    {
        // Verifica se todas as referências foram configuradas
        if (botaoCadastrar == null)
        {
            Debug.LogError("Botão Cadastrar não atribuído no script BotaoCadastro.");
            return;
        }

        if (cadastroManager == null)
        {
            Debug.LogError("CadastroManager não atribuído no script BotaoCadastro.");
            return;
        }

        // Adiciona a função OnRegisterButtonClicked ao evento OnClick do botão
        botaoCadastrar.onClick.AddListener(cadastroManager.OnRegisterButtonClicked);
    }
}
