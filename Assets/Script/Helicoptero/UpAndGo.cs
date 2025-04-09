using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpAndGo : MonoBehaviour
{   
    public float velUp;
    public float velForw;
    public bool up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {     

       
        if(transform.position.y<22)
        {
            transform.Translate(Time.deltaTime * velUp * Vector3.up);
            print("sube");
        }
        else if (transform.eulerAngles.x<19)
        {
            transform.Rotate(1f,0f,0f);
            print("rota");
        }
        else
        {
            transform.Translate(Time.deltaTime * velForw * -Vector3.right,Space.World);
            print("Avanza");
        }

      if(transform.position.x<-250f)
      {
        SceneManager.LoadScene("IMGInicio");
      } 

    }

}
