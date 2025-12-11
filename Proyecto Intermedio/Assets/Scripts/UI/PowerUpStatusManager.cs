using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpStatusManager : MonoBehaviour
{
    public Image powerUpEffect;
    public bool isActivated;

    public Transform powerUpContainer;
    public GameObject powerUpIconPrefab;

    private List<GameObject> activePowerUps = new List<GameObject>();

    private void OnEnable()
    {
        PowerUpBase.OnPowerUpActivated += HandlePowerUpActivated;
    }

    private void OnDisable()
    {
        PowerUpBase.OnPowerUpActivated -= HandlePowerUpActivated;
    }

    private void HandlePowerUpActivated(Sprite icon, float duration)
    {
        StartPowerUpEffect(icon, duration);
    }

    public void StartPowerUpEffect(Sprite icon, float duration)
    {
        //isActivated = true;
        //powerUpEffect.sprite = icon;
        //powerUpEffect.gameObject.SetActive(true);
        //powerUpEffect.transform.Find("RadialProgressBar")
        //    .GetComponent<CircularProgressBar>()
        //    .ActivateCountdown(duration);

        //StartCoroutine(EndPowerUpEffect(duration));

        // Instanciamos el prefab como hijo del contenedor
        GameObject newIcon = Instantiate(powerUpIconPrefab, powerUpContainer);
        activePowerUps.Add(newIcon);

        // Asignamos el sprite
        Image iconImage = newIcon.GetComponent<Image>();
        if (iconImage != null)
            iconImage.sprite = icon;

        // Activamos la barra
        CircularProgressBar progressBar = newIcon.GetComponentInChildren<CircularProgressBar>();
        if (progressBar != null)
            progressBar.ActivateCountdown(duration);

        // Destruir el icono después de que termine la duración
        Debug.Log(activePowerUps.Count);
        StartCoroutine(RemovePowerUpIcon(newIcon, duration));
    }

    private IEnumerator RemovePowerUpIcon(GameObject icon, float duration)
    {
        yield return new WaitForSeconds(duration);

        activePowerUps.Remove(icon);
        Destroy(icon);
    }

    //IEnumerator EndPowerUpEffect(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    isActivated = false;
    //    powerUpEffect.gameObject.SetActive(false);
    //}
}
