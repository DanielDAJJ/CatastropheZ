using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Compass : MonoBehaviour
{
    public RawImage compassImage;
    public Transform player;
    public TMP_Text compassDirectionText;

    public PlayerEnableCar playerEnableCar;


    // Start is called before the first frame update
    void Start()
    {
        playerEnableCar= GameObject.Find("Female Player").GetComponent<PlayerEnableCar>();
    }   

    // Update is called once per frame
    void Update()
    {
           
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);
        Vector3 forward = player.transform.forward;
        forward.y = 0f;

        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

        int displayAngle;
        displayAngle = Mathf.RoundToInt(headingAngle);

        switch (displayAngle)
        {
            case 0:
                compassDirectionText.text = "N";
                break;
            case 360:
                compassDirectionText.text = "N";
                break;
            case 45:
                compassDirectionText.text = "NE";
                break;
            case 90:
                compassDirectionText.text = "E";
                break;
            case 135:
                compassDirectionText.text = "SE";
                break;
            case 180:
                compassDirectionText.text = "S";
                break;
            case 225:
                compassDirectionText.text = "SW";
                break;
            case 270:
                compassDirectionText.text = "W";
                break;

            default:
                compassDirectionText.text = headingAngle.ToString();
                break;

        }
    }
}
