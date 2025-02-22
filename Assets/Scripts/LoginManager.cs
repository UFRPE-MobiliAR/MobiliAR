using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public InputField Email_Login; // Campo de entrada de email
    public InputField Senha_Login; // Campo de entrada de senha
    public Button botaoLogar; // Botão de logar
    public Text Falha_Login; // Texto para mostrar erro de login

    private string filePath; // Caminho para o arquivo .txt de cadastro

    void Start()
    {
        // Define o caminho para o arquivo .txt no diretório persistente da Unity
        filePath = Application.persistentDataPath + "/cadastro.txt";

        // Verifica se todos os componentes estão atribuídos
        if (Email_Login == null)
        {
            Debug.LogError("Email_Login não atribuído no script LoginManager.");
        }

        if (Senha_Login == null)
        {
            Debug.LogError("Senha_Login não atribuído no script LoginManager.");
        }

        if (botaoLogar != null)
        {
            botaoLogar.onClick.AddListener(OnLoginButtonClicked);
        }
        else
        {
            Debug.LogError("Botão de login não atribuído no script LoginManager.");
        }

        // Verifica se o componente Falha_Login está atribuído
        if (Falha_Login == null)
        {
            Debug.LogError("Falha_Login não atribuído no script LoginManager.");
        }
        else
        {
            Falha_Login.gameObject.SetActive(false); // Garante que o texto de erro esteja inicialmente oculto
        }
    }

    public void OnLoginButtonClicked()
    {
        // Obtém os textos dos InputFields
        string email = Email_Login.text;
        string senha = Senha_Login.text;

        // Tenta logar o usuário verificando o email e a senha no arquivo .txt
        if (TryLogin(email, senha))
        {
            Debug.Log("Login realizado com sucesso!");
            // Aqui você pode adicionar a lógica de sucesso de login (ex: ir para uma nova cena)
        }
        else
        {
            // Se o login falhar, exibe uma mensagem de erro
            ShowErrorMessage("Senha ou email incorretos.");
        }
    }

    private bool TryLogin(string email, string senha)
    {
        // Verifica se o arquivo existe antes de tentar ler
        if (File.Exists(filePath))
        {
            // Lê todas as linhas do arquivo
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Cada linha é dividida por vírgulas, então separamos os campos
                string[] fields = line.Split(',');

                // Verifica se há pelo menos 3 campos na linha
                if (fields.Length >= 3 && fields[0] == email && fields[2] == senha)
                {
                    return true; // Login bem-sucedido
                }
            }
        }
        else
        {
            Debug.LogError("Arquivo de cadastro não encontrado: " + filePath);
        }

        return false; // Login falhou
    }

    private void ShowErrorMessage(string message)
    {
        if (Falha_Login != null)
        {
            Falha_Login.text = message;
            Falha_Login.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Detecta toque na tela para ocultar o texto de erro
        if (Input.GetMouseButtonDown(0)) // 0 para o botão esquerdo do mouse (ou toque na tela)
        {
            if (Falha_Login != null && Falha_Login.gameObject.activeSelf)
            {
                Falha_Login.gameObject.SetActive(false);
            }
        }
    }
}
