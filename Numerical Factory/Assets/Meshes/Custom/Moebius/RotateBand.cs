using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RotateBand : MonoBehaviour
{
    public GameObject XROrigin;

    public float speed =5;
    public bool automatic;

    // Update is called once per frame
    void Update()
    {
        float incrementPosition = XROrigin.transform.position.z;

        if (automatic)
        {
            transform.Rotate(0, 0, speed/10f);
            transform.parent.transform.Rotate(0, 0, -speed/20f);
        }
        else
        {
            transform.Rotate(0, 0, speed * incrementPosition);
            transform.parent.transform.Rotate(0, 0, speed * (-incrementPosition / 2));
        }

        XROrigin.transform.position = new Vector3(XROrigin.transform.position.x, 0, 0);


    }
}
