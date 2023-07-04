using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    public Vector3 speed;//速度&方向
    public Vector3 force;
    [SerializeField]
    public Vector3 accelerated;//加速度&方向

    public void A_Changed(Vector3 newAcc)
    {
        accelerated = newAcc;
    }
    public void V_Changed(Vector3 newSpeed)
    {
        accelerated = newSpeed;
    }


    // Start is called before the first frame update
/*    void Start()
    {
        speed = new Vector3(4, 4, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = speed + Time.deltaTime * accelerated;

        this.gameObject.transform.position = this.gameObject.transform.position + Time.deltaTime * new Vector3(speed.x,speed.y,0);
    }*/
}
