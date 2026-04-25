using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        // Zaman farkýný hesaplayýp yumuþatýyoruz ki sayýlar çok hýzlý titremesin
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Ekranýn geniþliðini ve yüksekliðini al
        int w = Screen.width, h = Screen.height;

        // Yazý stilini ayarla
        GUIStyle style = new GUIStyle();

        // Yazýnýn ekrandaki yerini ve boyutunu ayarla (Sol üst köþe)
        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 25; // Yazý büyüklüðü
        style.normal.textColor = new Color(0.0f, 1.0f, 0.0f, 1.0f); // Yeþil renk

        // FPS'i hesapla
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        // Ekrana yazýlacak metni oluþtur
        string text = string.Format("{0:0.0} ms ({1:0.} FPS)", msec, fps);

        // Ekrana yazdýr
        GUI.Label(rect, text, style);
    }
}