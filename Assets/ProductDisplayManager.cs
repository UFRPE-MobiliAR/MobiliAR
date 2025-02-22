using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Collections;

public class ProductDisplayManager : MonoBehaviour
{
    public GameObject panelItemPrefab; // Prefab do item do produto
    public Transform contentPanel; // Painel onde os produtos serão exibidos

    public InputField searchInputField; // Campo de busca para o usuário
    public Button searchButton; // Botão de busca para confirmar pesquisa

    public Button botaoAleatorio; // Botão para exibir produtos aleatórios
    public Button botaoCadeira; // Botão para exibir apenas cadeiras
    public Button botaoMesa; // Botão para exibir apenas mesas
    public Button botaoSofa; // Botão para exibir apenas sofás
    public Button botaoCamas; // Botão para exibir apenas camas

    private string connectionString = "Server=192.168.18.18;Database=MobiliAR;User ID=MobiliAR;Password=MOBMOB;Port=3306;";

    void Start()
    {
        // Carrega todos os produtos no primeiro acesso
        LoadProductsFromDatabase("");

        // Listener para o botão de busca
        searchButton.onClick.AddListener(() =>
        {
            string searchQuery = searchInputField.text; // Pega o texto da busca
            LoadProductsFromDatabase(searchQuery);
        });

        // Listeners para os botões de filtro
        botaoAleatorio.onClick.AddListener(() => LoadProductsFromDatabase("")); // Carrega todos os produtos
        botaoCadeira.onClick.AddListener(() => LoadProductsFromDatabase("Cadeira"));
        botaoMesa.onClick.AddListener(() => LoadProductsFromDatabase("Mesa"));
        botaoSofa.onClick.AddListener(() => LoadProductsFromDatabase("Sofa"));
        botaoCamas.onClick.AddListener(() => LoadProductsFromDatabase("Cama"));
    }

    // Função para carregar produtos do banco de dados com filtro opcional
    void LoadProductsFromDatabase(string filter)
    {
        List<Product> products = new List<Product>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Monta a query, com ou sem filtro
                string query = "SELECT ID, Nome, Preço, Hyperlink, Imagem FROM Produtos";
                if (!string.IsNullOrEmpty(filter))
                {
                    query += " WHERE Nome LIKE @filter";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@filter", "%" + filter + "%"); // Adiciona o filtro com LIKE
                }

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ID = reader.GetInt32("ID"),
                        Name = reader.GetString("Nome"),
                        Price = reader.GetDecimal("Preço"),
                        Hyperlink = reader.GetString("Hyperlink"),
                        ImageUrl = reader.GetString("Imagem")
                    };
                    products.Add(product);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }

        // Limpa os produtos exibidos antes de carregar novos
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Exibe os produtos filtrados ou todos, dependendo do filtro
        DisplayProducts(products);
    }

    // Função que exibe os produtos na tela
    void DisplayProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            GameObject productObject = Instantiate(panelItemPrefab, contentPanel);
            Text nameText = productObject.transform.Find("NomeItem").GetComponent<Text>();
            Text priceText = productObject.transform.Find("PreçoItem").GetComponent<Text>();
            Image productImage = productObject.transform.Find("ImagemItem").GetComponent<Image>();
            Button accessButton = productObject.transform.Find("BotaoAcessaItem").GetComponent<Button>();

            nameText.text = product.Name;
            priceText.text = product.Price.ToString("C");

            // Carregar imagem
            StartCoroutine(LoadImage(product.ImageUrl, productImage));

            // Adicionar listener ao botão
            accessButton.onClick.AddListener(() => OnProductClick(product));
        }
    }

    void OnProductClick(Product product)
    {
        Debug.Log("Produto clicado: " + product.Name);
        // Implementar a navegação para a tela com Node ID 1:171
    }

    // Corrigir erro de objeto destruído verificando se a imagem ainda existe antes de setar o sprite
    IEnumerator LoadImage(string url, Image imageComponent)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (www.error == null)
            {
                Texture2D texture = www.texture;

                if (imageComponent != null) // Verifica se a imagem ainda existe
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    imageComponent.sprite = sprite;
                }
            }
            else
            {
                Debug.LogError("Erro ao carregar imagem: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class Product
{
    public int ID;
    public string Name;
    public decimal Price;
    public string Hyperlink;
    public string ImageUrl;
}
