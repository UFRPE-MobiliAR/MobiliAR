using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Variável estática para manter a referência do GameObject entre cenas
    public static GameObject modelToPass;

    public GameObject modelToSelect; // O modelo 3D que será passado para a próxima cena

    public void ChangeScene(string sceneName)
    {
        // Armazena o modelo selecionado na variável estática
        modelToPass = modelToSelect;

        // Carrega a nova cena
        SceneManager.LoadScene(sceneName);
    }
}