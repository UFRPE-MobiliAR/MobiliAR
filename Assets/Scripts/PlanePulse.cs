using UnityEngine;

public class PlanePulse : MonoBehaviour
{
    public Material emissiveMaterial; // Material emissivo
    public Material nonEmissiveMaterial; // Material não emissivo
    public float pulseSpeed = 2f; // Velocidade da pulsação

    private Renderer planeRenderer;
    private bool isEmissive = false; // Indica se o emissivo está ativo
    private float timer;

    private void Start()
    {
        planeRenderer = GetComponent<Renderer>();
        if (planeRenderer == null)
        {
            Debug.LogError("PlanePulse: Nenhum Renderer encontrado no objeto!");
        }
        planeRenderer.material = nonEmissiveMaterial; // Começa com o material não emissivo
    }

    private void Update()
    {
        PulseEmissive();
    }

    private void PulseEmissive()
    {
        timer += Time.deltaTime * pulseSpeed;
        if (timer >= 1f)
        {
            timer = 0f;
            isEmissive = !isEmissive; // Alterna o estado emissivo
            planeRenderer.material = isEmissive ? emissiveMaterial : nonEmissiveMaterial;
        }
    }
}
