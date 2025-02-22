using UnityEngine;
using System.Collections;

public class ClickActivateEffects : MonoBehaviour
{
    public ParticleSystem particleEffect; // Efeito de partícula
    public GameObject siren1; // Sirene 1
    public GameObject siren2; // Sirene 2
    public Material emissiveMaterial; // Material emissivo
    public Material nonEmissiveMaterial; // Material não emissivo
    public GameObject[] planes; // Array de planes
    public float delayBetweenEffects = 1f; // Delay entre os efeitos
    public float delayBetweenPlanes = 0.5f; // Delay entre os planes
    public float sirenBlinkInterval = 0.5f; // Intervalo de troca entre materiais

    private bool isActivated = false;
    private bool isEffectsActive = false;
    private Renderer siren1Renderer;
    private Renderer siren2Renderer;

    private void Start()
    {
        if (siren1 != null)
            siren1Renderer = siren1.GetComponent<Renderer>();
        if (siren2 != null)
            siren2Renderer = siren2.GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        if (isEffectsActive)
        {
            DeactivateEffects();
        }
        else
        {
            StartCoroutine(ActivateEffects());
        }
    }

    private IEnumerator ActivateEffects()
    {
        isActivated = true;
        isEffectsActive = true;

        // Ligar partícula
        if (particleEffect != null)
        {
            particleEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBetweenEffects);
        }

        // Ligar sirene 1
        if (siren1 != null && siren1Renderer != null)
        {
            StartCoroutine(BlinkSiren(siren1Renderer));
            yield return new WaitForSeconds(delayBetweenEffects);
        }

        // Ativar planes com delay entre cada um
        foreach (var plane in planes)
        {
            if (plane != null)
            {
                plane.SetActive(true);
                yield return new WaitForSeconds(delayBetweenPlanes);
            }
        }

        // Ligar sirene 2
        if (siren2 != null && siren2Renderer != null)
        {
            StartCoroutine(BlinkSiren(siren2Renderer));
            yield return new WaitForSeconds(delayBetweenEffects);
        }

        //isActivated = false;
    }

    private void DeactivateEffects()
    {
        isActivated = false;
        isEffectsActive = false;

        // Desligar partículas
        if (particleEffect != null)
        {
            particleEffect.gameObject.SetActive(false);
        }

        // Garantir que as sirenes terminam no material não emissivo
        if (siren1Renderer != null)
        {
            siren1Renderer.material = nonEmissiveMaterial;
        }
        if (siren2Renderer != null)
        {
            siren2Renderer.material = nonEmissiveMaterial;
        }

        // Desativar planes
        foreach (var plane in planes)
        {
            if (plane != null)
            {
                plane.SetActive(false);
            }
        }
    }

    private IEnumerator BlinkSiren(Renderer sirenRenderer)
    {
        while (isActivated)
        {
            sirenRenderer.material = emissiveMaterial;
            yield return new WaitForSeconds(sirenBlinkInterval);

            sirenRenderer.material = nonEmissiveMaterial;
            yield return new WaitForSeconds(sirenBlinkInterval);
        }

    }
}
