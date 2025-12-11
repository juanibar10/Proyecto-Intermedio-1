using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpStatusUI : MonoBehaviour
{
    public Transform powerUpContainer;
    public GameObject powerUpIconPrefab;

    [Header("Icons")]
    public Sprite magnetIcon;
    public Sprite shieldIcon;
    public Sprite multiplierIcon;

    private Dictionary<string, GameObject> activePowerUps = new Dictionary<string, GameObject>();
    private Dictionary<string, Coroutine> activeCoroutines = new Dictionary<string, Coroutine>();

    private void OnEnable()
    {
        PowerUpBase.OnPowerUpActivated += HandlePowerUpActivated;
    }

    private void OnDisable()
    {
        PowerUpBase.OnPowerUpActivated -= HandlePowerUpActivated;
    }

    private void HandlePowerUpActivated(string powerUpName, float duration)
    {
        StartPowerUpEffect(powerUpName, duration);
    }

    public void StartPowerUpEffect(string powerUpName, float duration)
    {
        // Si ya existe, se reinicia el contador
        if (activePowerUps.ContainsKey(powerUpName))
        {
            CircularProgressBar existingProgress = activePowerUps[powerUpName]
                .GetComponentInChildren<CircularProgressBar>();
            if (existingProgress != null)
                existingProgress.ActivateCountdown(duration);

            // Se cancela la coroutine anterior y se lanza una nueva
            if (activeCoroutines.ContainsKey(powerUpName))
                StopCoroutine(activeCoroutines[powerUpName]);

            activeCoroutines[powerUpName] = StartCoroutine(RemovePowerUpIcon(activePowerUps[powerUpName], duration));
            return;
        }

        // Si no existe, se crea el nuevo icono del power-up
        GameObject newIcon = Instantiate(powerUpIconPrefab, powerUpContainer);
        activePowerUps[powerUpName] = newIcon;

        Image iconImage = newIcon.GetComponent<Image>();
        if (iconImage != null)
            iconImage.sprite = GetIcon(powerUpName);

        CircularProgressBar progressBar = newIcon.GetComponentInChildren<CircularProgressBar>();
        if (progressBar != null)
            progressBar.ActivateCountdown(duration);

        activeCoroutines[powerUpName] = StartCoroutine(RemovePowerUpIcon(newIcon, duration));
    }

    private IEnumerator RemovePowerUpIcon(GameObject icon, float duration)
    {
        yield return new WaitForSeconds(duration);

        string keyToRemove = null;
        foreach (var kvp in activePowerUps)
        {
            if (kvp.Value == icon)
            {
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            activePowerUps.Remove(keyToRemove);
            activeCoroutines.Remove(keyToRemove);
        }

        Destroy(icon);
    }

    private Sprite GetIcon(string name)
    {
        switch (name)
        {
            case "PowerUpMagnet":
                return magnetIcon;
            case "PowerUpShield":
                return shieldIcon;
            case "PowerUpMultiplier":
                return multiplierIcon;
        }

        return null;
    }
}
