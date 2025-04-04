
using System.Collections;
using UnityEngine;

public class RatsBehaviour : MonoBehaviour
{
    public GameObject goal;
    public float vel;
    private bool active=false;
    private bool coroutine=false;
    
    // Update is called once per frame


    void Update()
    {   
        if(active)
        {
            transform.position=Vector3.MoveTowards(transform.position,goal.transform.position, vel*Time.deltaTime);
            if (!coroutine)
            {
                StartCoroutine(DestroyAfterSeconds());
                coroutine=true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            active=true;
        }
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
