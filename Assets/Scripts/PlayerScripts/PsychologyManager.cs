using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem; // YENÝ: Yeni Input Sistemi kütüphanesi eklendi

public class PsychologyManager : MonoBehaviour
{
    [Header("Psikoloji Ayarlarý")]
    public float currentPsycho = 100f;
    public float maxPsycho = 100f;

    [Header("Referanslar")]
    public Volume globalVolume;
    public AudioSource heartbeatAudio; // Kalp atýþý sesi

    // Efekt Referanslarý
    private DepthOfField dof;
    private ChromaticAberration chromatic;

    void Start()
    {
        if (globalVolume.profile.TryGet(out DepthOfField d)) dof = d;
        if (globalVolume.profile.TryGet(out ChromaticAberration c)) chromatic = c;

        WakeUpSequence();
    }

    void Update()
    {
        // YENÝ: Yeni Input Sistemine göre klavyeden K tuþunu okuma (Test Ýçin)
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            DecreasePsychology(10f);
        }

        HandleEffects();
    }

    public void WakeUpSequence()
    {
        if (dof != null)
        {
            dof.active = true;
            dof.gaussianMaxRadius.value = 1.5f;
        }
    }

    public void DecreasePsychology(float amount)
    {
        currentPsycho -= amount;
        currentPsycho = Mathf.Clamp(currentPsycho, 0, maxPsycho);
    }

    private void HandleEffects()
    {
        if (chromatic != null)
        {
            float psychoPercent = currentPsycho / maxPsycho;

            if (psychoPercent <= 0.5f)
            {
                chromatic.intensity.value = 1f - (psychoPercent * 2f);

                if (!heartbeatAudio.isPlaying) heartbeatAudio.Play();
            }
            else
            {
                chromatic.intensity.value = 0f;
                if (heartbeatAudio.isPlaying) heartbeatAudio.Stop();
            }
        }
    }
}