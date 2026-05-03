using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WakeUpSequence : MonoBehaviour
{
    [Header("Uyanis Ayarlari")]
    public Image fadeScreen;       
    public float waitBeforeWake = 1.5f; 
    public float wakeUpDuration = 4f;   

    void Start()
    {
       
        fadeScreen.color = new Color(0, 0, 0, 1);
        fadeScreen.gameObject.SetActive(true);

       
        StartCoroutine(WakeUpRoutine());
    }

    IEnumerator WakeUpRoutine()
    {
       
        yield return new WaitForSeconds(waitBeforeWake);

        float elapsedTime = 0f;
        Color startColor = fadeScreen.color;
        Color endColor = new Color(0, 0, 0, 0); 

        
        while (elapsedTime < wakeUpDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeScreen.color = Color.Lerp(startColor, endColor, elapsedTime / wakeUpDuration);
            yield return null; 
        }

        
        fadeScreen.color = endColor;
        fadeScreen.gameObject.SetActive(false);
    }
}