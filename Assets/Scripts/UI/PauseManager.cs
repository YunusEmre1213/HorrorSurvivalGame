using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject pausePanel;
    public GameObject crosshair; // YENÝ: Niþangah referansý
    public InputActionReference pauseAction;

    public static bool IsGamePaused = false;

    void Start()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);

        // Oyun baþladýðýnda niþangahýn görünür olduðundan emin olalým
        if (crosshair != null) crosshair.SetActive(true);
    }

    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (IsGamePaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        pausePanel.SetActive(true);

        // YENÝ: Oyun durduðunda niþangahý kapat
        if (crosshair != null) crosshair.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        pausePanel.SetActive(false);

        // YENÝ: Oyuna dönüldüðünde niþangahý tekrar aç
        if (crosshair != null) crosshair.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (pauseAction != null) pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        if (pauseAction != null) pauseAction.action.Disable();
    }
}