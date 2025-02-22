using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;
using UnityEngine.InputSystem;
using System.Linq;

public class NavMeshHowTo : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private LightshipNavMeshManager _navmeshManager;

    public int dot = 0;
    public GameObject _agentPrefab; // Ser� definido dinamicamente

    private GameObject _creature;
    private LightshipNavMeshAgent _agent;

    private Transform mainCameraTransform;

    private float lastClickTime = 0f;
    private float doubleClickTimeThreshold = 0.3f;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;

    //     // Pegar o modelo armazenado na vari�vel est�tica do SceneChanger
    //     _agentPrefab = SceneChanger.modelToPass;

    //     // Verificar se o modelo foi realmente passado
    //     if (_agentPrefab == null)
    //     {
    //         Debug.LogError("Nenhum modelo foi passado para esta cena.");
    //     }
    //     Debug.LogError("modelo foi passado para esta cena.");
     }

    void Update()
    {
        HandleTouch();
    }

    public void ToggleVisualisation()
    {
        _navmeshManager.GetComponent<LightshipNavMeshRenderer>().enabled =
            !_navmeshManager.GetComponent<LightshipNavMeshRenderer>().enabled;

        _agent.GetComponent<LightshipNavMeshAgentPathRenderer>().enabled =
            !_agent.GetComponent<LightshipNavMeshAgentPathRenderer>().enabled;
    }

    private void HandleTouch()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
#else
        if (Input.touchCount <= 0)
            return;

        var touch = Input.GetTouch(0);

        if (Input.touchCount <= 0)
            return;
        if (touch.phase == UnityEngine.TouchPhase.Began)
#endif
        {
#if UNITY_EDITOR
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
#else
            Ray ray = _camera.ScreenPointToRay(touch.position);
#endif
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                float timeSinceLastClick = Time.time - lastClickTime;
                if (timeSinceLastClick <= doubleClickTimeThreshold)
                {
                    // Double click
                    if (dot == 0)
                    {
                        dot = 1;

                        // Verifica se o modelo foi carregado corretamente antes de instanciar
                        if (_agentPrefab != null)
                        {
                            _creature = Instantiate(_agentPrefab);
                            _creature.transform.position = hit.point;
                            _agent = _creature.GetComponent<LightshipNavMeshAgent>();
                        }
                        else
                        {
                            Debug.LogError("O _agentPrefab n�o est� definido.");
                        }
                    }
                    else
                    {
                        _agent.transform.position = hit.point;
                    }
                }
                else
                {
                    // Single click
                    lastClickTime = Time.time;
                }
            }
        }
    }

    // Fun��o auxiliar para buscar GameObjects por nome, caso necess�rio
    public static GameObject FindGameObjectsAll(string name) => Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == name);
}
