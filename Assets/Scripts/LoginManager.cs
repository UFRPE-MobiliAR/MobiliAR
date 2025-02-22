using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System;

public class LoginManager : MonoBehaviour
{
    public InputField Email_Login; // Campo de entrada de email
    public InputField Senha_Login; // Campo de entrada de senha
    public Button botaoLogar; // Botão de logar
    public Button botaoAcessar; // Botão que aparecerá após o login bem-sucedido
    public Text Falha_Login; // Texto para mostrar erro de login

    private string connectionString; // String de conexão com o banco de dados

    void Start()
    {
        // Define a string de conexão com o banco de dados MySQL
        connectionString = "Server=192.168.18.18;Database=MobiliAR;User ID=MobiliAR;Password=MOBMOB;Port=3306;";

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

        if (Falha_Login == null)
        {
            Debug.LogError("Falha_Login não atribuído no script LoginManager.");
        }
        else
        {
            Falha_Login.gameObject.SetActive(false); // Garante que o texto de erro esteja inicialmente oculto
        }

        if (botaoAcessar != null)
        {
            botaoAcessar.gameObject.SetActive(false); // Botão "Acessar" inicialmente oculto
        }
    }

    public void OnLoginButtonClicked()
    {
        // Obtém os textos dos InputFields
        string email = Email_Login.text;
        string senha = Senha_Login.text;

        // Verifica se os campos estão preenchidos
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            ShowErrorMessage("Preencha todos os campos.");
            return;
        }

        // Tenta logar o usuário verificando o email e a senha no banco de dados
        if (TryLogin(email, senha))
        {
            Debug.Log("Login realizado com sucesso!");

            // Oculta a mensagem de erro ao logar com sucesso
            Falha_Login.gameObject.SetActive(false);

            // Exibe o botão "Acessar" após o login bem-sucedido
            if (botaoAcessar != null)
            {
                botaoAcessar.gameObject.SetActive(true);
            }
        }
        else
        {
            // Se o login falhar, exibe uma mensagem de erro
            ShowErrorMessage("Senha ou email incorretos.");
        }
    }

    private bool TryLogin(string email, string senha)
    {
        bool loginSucesso = false;

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email AND Senha = @Senha";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    loginSucesso = (count > 0);
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Erro ao conectar ao banco de dados: " + ex.Message);
            ShowErrorMessage("Erro ao conectar ao servidor. Tente novamente mais tarde.");
        }

        return loginSucesso;
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
