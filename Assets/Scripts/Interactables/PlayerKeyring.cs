using UnityEngine;
using System.Collections.Generic;

public class PlayerKeyring : MonoBehaviour
{
    // Oyuncunun sahip olduðu anahtarlarýn listesi
    private List<string> collectedKeys = new List<string>();

    // Yeni bir anahtar eklemek için (Anahtar toplandýðýnda çaðrýlýr)
    public void AddKey(string keyName)
    {
        if (!collectedKeys.Contains(keyName))
        {
            collectedKeys.Add(keyName);
            Debug.Log("Envantere Eklendi: " + keyName);
        }
    }

    // Belirli bir anahtara sahip miyiz kontrol etmek için (Kapý kontrol eder)
    public bool HasKey(string keyName)
    {
        return collectedKeys.Contains(keyName);
    }
}