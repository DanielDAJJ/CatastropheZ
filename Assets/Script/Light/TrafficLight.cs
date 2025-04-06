using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [Header("Tiempo entre parpadeos")]
    public float minTime = 0.05f;
    public float maxTime = 0.2f;

    [Header("Apagados m�s largos (simula cortos fuertes)")]
    public float longOffChance = 0.1f;
    public float longOffDuration = 0.5f;

    [Header("Intensidad")]
    public float flickerIntensity = 0f;

    [SerializeField]private Light light1;
    [SerializeField]private Light light2;
    private float _originalIntensity;

    void Start()
    {
       
        _originalIntensity = light1.intensity;
        _originalIntensity = light2.intensity;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            // Apaga
            light1.intensity = flickerIntensity;
            light2.intensity = flickerIntensity;

            float offTime = (Random.value < longOffChance) ? longOffDuration : Random.Range(0.02f, 0.1f);
            yield return new WaitForSeconds(offTime);

            // Enciende
            light1.intensity = _originalIntensity;
            light2.intensity = _originalIntensity;
        }
    }
}
