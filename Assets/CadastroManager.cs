using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CadastroManager : MonoBehaviour
{
    // Referências para os InputFields
    public InputField Email_cadastro;
    public InputField Nome_Cadastro;
    public InputField Senha_Cadastro;

    // Caminho do arquivo .txt onde os dados serão salvos
    private string filePath;

    // Referência para o Text de erro
    public Text Email_Erro;

    void Start()
    {
        // Define o caminho para o arquivo no diretório persistente da Unity
        filePath = Application.persistentDataPath + "/cadastro.txt";

        // Garante que o texto de erro esteja inicialmente oculto
        if (Email_Erro != null)
        {
            Email_Erro.gameObject.SetActive(false);
        }

        // Verifica se o arquivo já existe e imprime mensagem no console
        if (File.Exists(filePath))
        {
            Debug.Log("Arquivo de cadastro já existe: " + filePath);
        }
        else
        {
            // Tenta criar o arquivo e verifica o sucesso
            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    Debug.Log("Arquivo cadastro.txt criado com sucesso em: " + filePath);
                }
            }
            catch (IOException ioEx)
            {
                Debug.LogError("Erro ao criar o arquivo cadastro.txt: " + ioEx.Message);
            }
        }
    }

    public void OnRegisterButtonClicked()
    {
        // Obtém os textos dos InputFields
        string email = Email_cadastro.text;
        string nome = Nome_Cadastro.text;
        string senha = Senha_Cadastro.text;

        // Verifica se o email já existe no arquivo
        if (EmailExists(email))
        {
            Debug.Log("Email já cadastrado.");
            ShowErrorMessage("Email já cadastrado.");
        }
        else
        {
            // Se o email não existe, adiciona o novo registro ao arquivo
            AddEntryToFile(email, nome, senha);
            Debug.Log("Cadastro realizado com sucesso!"); // Mensagem de sucesso no console
        }
    }

    private bool EmailExists(string email)
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
                if (fields[0] == email) // O email é o primeiro campo
                {
                    return true; // Email encontrado
                }
            }
        }
        return false; // Email não encontrado
    }

    private void AddEntryToFile(string email, string nome, string senha)
    {
        // Formata a nova entrada para o arquivo
        string newEntry = email + "," + nome + "," + senha;

        // Adiciona a nova entrada ao arquivo
        File.AppendAllText(filePath, newEntry + "\n");
    }

    private void ShowErrorMessage(string message)
    {
        if (Email_Erro != null)
        {
            Email_Erro.text = message;
            Email_Erro.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Detecta toque na tela para ocultar o texto de erro
        if (Input.GetMouseButtonDown(0)) // 0 para o botão esquerdo do mouse (ou toque na tela)
        {
            if (Email_Erro != null && Email_Erro.gameObject.activeSelf)
            {
                Email_Erro.gameObject.SetActive(false);
            }
        }
    }
}
