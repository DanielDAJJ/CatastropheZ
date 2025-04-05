using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Damage : MonoBehaviour
{
    [Header("Tiempo entre parpadeos")]
    public float minTime = 0.05f;
    public float maxTime = 0.2f;

    [Header("Apagados más largos (simula cortos fuertes)")]
    public float longOffChance = 0.1f;
    public float longOffDuration = 0.5f;

    [Header("Intensidad")]
    public float flickerIntensity = 0f;

    private Light _light;
    private float _originalIntensity;

    void Start()
    {
        _light = GetComponent<Light>();
        _originalIntensity = _light.intensity;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            // Apaga
            _light.intensity = flickerIntensity;

            float offTime = (Random.value < longOffChance) ? longOffDuration : Random.Range(0.02f, 0.1f);
            yield return new WaitForSeconds(offTime);

            // Enciende
            _light.intensity = _originalIntensity;
        }
    }
}
