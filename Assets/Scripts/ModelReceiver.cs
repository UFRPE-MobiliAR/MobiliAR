using UnityEngine;

public class ModelReceiver : MonoBehaviour
{
    void Start()
    {
        // Obtém o modelo 3D selecionado do ModelManager
        GameObject selectedModel = ModelManager.Instance.GetSelectedModel();

        if (selectedModel != null)
        {
            // Exemplo: instanciar o modelo na cena
            Instantiate(selectedModel, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError("Nenhum modelo selecionado foi passado para esta cena.");
        }
    }
}
