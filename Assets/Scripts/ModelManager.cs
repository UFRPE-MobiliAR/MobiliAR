using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static ModelManager Instance;

    public GameObject selectedModel; // O modelo 3D selecionado

    void Awake()
    {
        // Garantir que exista apenas uma instância deste GameObject
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Não destrua ao carregar uma nova cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para definir o modelo 3D
    public void SetSelectedModel(GameObject model)
    {
        selectedModel = model;
    }

    // Método para obter o modelo 3D
    public GameObject GetSelectedModel()
    {
        return selectedModel;
    }
}