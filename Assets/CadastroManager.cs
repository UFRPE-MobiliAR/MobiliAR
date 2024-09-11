using UnityEngine;
using UnityEngine.UI;
using System;
using MySql.Data.MySqlClient; // Namespace para MySQL
public class CadastroManager : MonoBehaviour
{
    // Referências para os InputFields
    public InputField Email_cadastro;
    public InputField Nome_Cadastro;
    public InputField Senha_Cadastro;

    // Referência para o Text de erro
    public Text Email_Erro;

    // String de conexão com o banco de dados
    private string connectionString = "Server=192.168.18.18;Database=MobiliAR;User ID=MobiliAR;Password=MOBMOB;Port=3306;";

    void Start()
    {
        // Garante que o texto de erro esteja inicialmente oculto
        if (Email_Erro != null)
        {
            Email_Erro.gameObject.SetActive(false);
        }
    }

    public void OnRegisterButtonClicked()
    {
        // Obtém os textos dos InputFields
        string email = Email_cadastro.text;
        string nome = Nome_Cadastro.text;
        string senha = Senha_Cadastro.text;

        // Verifica se o email já existe no banco de dados
        if (EmailExists(email))
        {
            Debug.Log("Email já cadastrado.");
            ShowErrorMessage("Email já cadastrado.");
        }
        else
        {
            // Se o email não existe, realiza o cadastro no banco de dados
            RegisterUser(email, nome, senha);
            Debug.Log("Cadastro realizado com sucesso!"); // Mensagem de sucesso no console
        }
    }

    private bool EmailExists(string email)
    {
        bool emailExists = false;

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                emailExists = (count > 0);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Erro ao verificar email: " + ex.Message);
        }

        return emailExists;
    }

    private void RegisterUser(string email, string nome, string senha)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Usuarios (Email, Nome, Senha) VALUES (@Email, @Nome, @Senha)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Senha", senha);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Erro ao cadastrar usuário: " + ex.Message);
        }
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
