using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadUIScene()
    {
        Debug.Log("cena carregada");
        SceneManager.LoadScene("UI_Scene_Home");
        
    }
    public void loadARScene()
    {
        SceneManager.LoadScene("AR_Scene");
    }


}
