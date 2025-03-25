using UnityEngine;
using UnityEngine.AI;

public class ZombieIA : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    GameObject player;
    [SerializeField]private bool followPlayer;
    [SerializeField]private float distanceSquared;
    [SerializeField]float thresholdSquared;
    void Start()
    {
        navMeshAgent=gameObject.GetComponent<NavMeshAgent>();
        player=GameObject.Find("Female Player");
        thresholdSquared = 25f;
    }

    // Update is called once per frame
    void Update()
    {   
        distanceSquared = (player.transform.position - transform.position).sqrMagnitude;
        if( distanceSquared < thresholdSquared)
            {
                navMeshAgent.SetDestination(player.transform.position);
                followPlayer=true;
            } 
        else
            {
                navMeshAgent.SetDestination(transform.position);
                followPlayer=false;
            }   
    }
}
