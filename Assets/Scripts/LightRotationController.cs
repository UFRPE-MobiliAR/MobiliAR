using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LightRotationController : MonoBehaviour
{
    // Sensibilidade da rotação
    public float rotationSpeed = 0.5f;

    // Referência ao Toggle
    [SerializeField]
    private Toggle rotationToggle;

    // Referência à Directional Light
    [SerializeField]
    private Light directionalLight;

    // Eixo de rotação
    [SerializeField]
    private Vector3 rotationAxis = Vector3.forward;

    // Variáveis para armazenar o estado do toque
    private bool isTouching = false;
    private Vector2 lastTouchPosition;

    private void Start()
    {
        if (rotationToggle == null)
        {
            Debug.LogError("Rotation Toggle não atribuído!");
        }

        if (directionalLight == null)
        {
            Debug.LogError("Directional Light não atribuída!");
        }

        // Adiciona o listener ao toggle
        rotationToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void Update()
    {
        // Verifica se há um toque na tela e se o toggle está ativo
        if (Input.touchCount > 0 && rotationToggle.isOn)
        {
            Touch touch = Input.GetTouch(0);

            // Se o toque começou, armazena a posição inicial do toque
            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                lastTouchPosition = touch.position;
            }
            // Se o toque está em andamento e o dedo ainda está pressionando
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                // Calcula o movimento do toque
                Vector2 currentTouchPosition = touch.position;
                float deltaX = currentTouchPosition.x - lastTouchPosition.x;

                // Calcula a rotação com base no movimento horizontal
                float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;

                // Rotaciona a Directional Light
                if (directionalLight != null)
                {
                    directionalLight.transform.Rotate(rotationAxis, rotationAmount, Space.World);
                }

                // Atualiza a posição do último toque para a próxima iteração
                lastTouchPosition = currentTouchPosition;
            }
            // Se o toque terminou, marca que não estamos mais tocando
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }

    // Método para verificar a mudança no estado do Toggle
    private void OnToggleChanged(bool isOn)
    {
        // Atualiza o estado de rotação conforme o toggle
        isTouching = isOn;
    }

    private void OnDestroy()
    {
        // Remove o listener ao destruir o objeto
        if (rotationToggle != null)
        {
            rotationToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}