using UnityEngine;

public class TwoFingerRotate : MonoBehaviour
{
    // Sensibilidade da rotação
    public float rotationSpeed = 0.5f;

    // Referência à Directional Light
    [SerializeField]
    private Light directionalLight;

    // Variáveis para armazenar o estado do toque
    private bool isRotating = false;
    private Vector2 initialTouch1Position;
    private Vector2 initialTouch2Position;
    private float initialAngle;

    private void Update()
    {
        // Verifica se há dois toques na tela
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                // Armazena as posições iniciais dos toques
                initialTouch1Position = touch1.position;
                initialTouch2Position = touch2.position;

                // Calcula o ângulo inicial entre os dois toques
                initialAngle = Vector2.SignedAngle(initialTouch2Position - initialTouch1Position, Vector2.right);
                isRotating = true;
            }
            else if ((touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) && isRotating)
            {
                // Calcula as posições atuais dos toques
                Vector2 currentTouch1Position = touch1.position;
                Vector2 currentTouch2Position = touch2.position;

                // Calcula o ângulo atual entre os dois toques
                float currentAngle = Vector2.SignedAngle(currentTouch2Position - currentTouch1Position, Vector2.right);

                // Calcula a diferença de ângulo e aplica a rotação
                float angleDelta = currentAngle - initialAngle;
                if (directionalLight != null)
                {
                    directionalLight.transform.Rotate(Vector3.up, angleDelta * rotationSpeed, Space.World);
                }

                // Atualiza as posições e ângulo iniciais
                initialTouch1Position = currentTouch1Position;
                initialTouch2Position = currentTouch2Position;
                initialAngle = currentAngle;
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                isRotating = false;
            }
        }
    }
}
