using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    public bool wasMichiFound;
    public CatFollow_2 catFollow;

    [SerializeField]  float distanceSqr;
    [SerializeField]  Vector3 catPosition;

    void Start()
    {
     catPosition = catFollow.gameObject.transform.position;
    }

    void Update()
    {
      distanceSqr = (catPosition - transform.position).sqrMagnitude;
      if(distanceSqr<10)
        {
            catFollow.enabled=true;
            wasMichiFound=true;
        }
    }

}
