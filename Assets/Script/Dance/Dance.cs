using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    public Transform target; // Tu personaje

    private float initialTargetX;
    private float initialSelfX;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No se asignó un target al script FollowDeltaX.");
            enabled = false;
            return;
        }

        initialTargetX = target.position.x;
        initialSelfX = transform.position.x;
    }

    void Update()
    {
        float deltaX = target.position.x - initialTargetX;
        transform.position = new Vector3(initialSelfX + deltaX, transform.position.y, transform.position.z);
    }
}
