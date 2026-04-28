using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PsychologyManager : MonoBehaviour
{
    [Header("Psikoloji Ayarlarý")]
    public float currentPsycho = 100f;
    public float maxPsycho = 100f;

    [Header("Karanlýk ve Iþýk Ayarlarý")]
    public bool isFlashlightOn = false;
    public float darknessDrainRate = 1.5f;
    public float darknessPsychoLimit = 30f;
    public float lightRecoveryRate = 2f;

    [Header("Referanslar")]
    public Volume globalVolume;
    public AudioSource heartbeatAudio;

    // Yeni Efekt Referanslarý eklendi
    private DepthOfField dof;
    private ChromaticAberration chromatic;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    void Start()
    {
        // URP Volume içindeki efektleri çekiyoruz
        if (globalVolume != null)
        {
            if (globalVolume.profile.TryGet(out DepthOfField d)) dof = d;
            if (globalVolume.profile.TryGet(out ChromaticAberration c)) chromatic = c;
            if (globalVolume.profile.TryGet(out Vignette v)) vignette = v;
            if (globalVolume.profile.TryGet(out LensDistortion ld)) lensDistortion = ld;
        }

        WakeUpSequence();
    }

    void Update()
    {
        if (!isFlashlightOn && currentPsycho > darknessPsychoLimit)
        {
            DecreasePsycho(darknessDrainRate * Time.deltaTime);
        }
        else if (isFlashlightOn && currentPsycho < maxPsycho)
        {
            IncreasePsycho(lightRecoveryRate * Time.deltaTime);
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

    public void DecreasePsycho(float amount)
    {
        currentPsycho -= amount;
        currentPsycho = Mathf.Clamp(currentPsycho, 0, maxPsycho);
    }

    public void IncreasePsycho(float amount)
    {
        currentPsycho += amount;
        currentPsycho = Mathf.Clamp(currentPsycho, 0, maxPsycho);
    }

    public void SetFlashlightState(bool state)
    {
        isFlashlightOn = state;
    }

    private void HandleEffects()
    {
        float psychoPercent = currentPsycho / maxPsycho;

        if (psychoPercent <= 0.5f)
        {
            // Þiddet hesaplamasý (0.5'ten aþaðý indikçe 0'dan 1'e doðru artar)
            float intensity = 1f - (psychoPercent * 2f);

            // 1. Renk Ayrýþmasý
            if (chromatic != null) chromatic.intensity.value = intensity;

            // 2. Tünel Vizyonu (Kenarlar kararýr, klostrofobi yaratýr)
            if (vignette != null)
            {
                vignette.active = true;
                vignette.intensity.value = intensity * 0.45f; // Çok abartmamak için 0.45 ile çarptýk
            }

            // 3. SARHOÞLUK/DALGALANMA EFEKTÝ (Zamanla ileri geri sallanýr)
            if (lensDistortion != null)
            {
                lensDistortion.active = true;
                // Sinüs dalgasý kullanarak ekraný nefes alýyormuþ gibi çarpýtýrýz
                lensDistortion.intensity.value = Mathf.Sin(Time.time * 3f) * (intensity * 0.3f);
            }

            // 4. KALP ATIÞI HIZLANMASI
            if (heartbeatAudio != null)
            {
                if (!heartbeatAudio.isPlaying) heartbeatAudio.Play();
                // Psikoloji düþtükçe kalp atýþ hýzý (pitch) artar!
                heartbeatAudio.pitch = 1f + (intensity * 0.6f);
            }
        }
        else
        {
            // Her þey normalse tüm efektleri sýfýrla
            if (chromatic != null) chromatic.intensity.value = 0f;
            if (vignette != null) vignette.active = false;
            if (lensDistortion != null) lensDistortion.active = false;

            if (heartbeatAudio != null && heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Stop();
                heartbeatAudio.pitch = 1f; // Sesi normale döndür
            }
        }
    }
}