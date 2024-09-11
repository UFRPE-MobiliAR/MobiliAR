using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string url; // O URL que você quer abrir

    // Método para ser chamado ao clicar no botão
    public void OpenHyperlink()
    {
        Application.OpenURL(url); // Abre o link no navegador padrão
    }
}
