using UnityEngine;
using System.Collections;

public class MoveTank : MonoBehaviour
{
    public GameObject Target;

    [SerializeField]
    private float acceleration = 5;

    [SerializeField]
    private float currentVelocity = 0;

    [SerializeField]
    private float maxSpeed = 25;

    [SerializeField]
    private float rotationSpeed = 30;

    [SerializeField]
    private Transform spawnPoint;
    
    [SerializeField]
    private GameObject bulletObject;

    [SerializeField]
    private GameObject fireEffect;

    private void Start()
    {
    }


    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        //if (Input.GetKey(KeyCode.UpArrow))
        {
            if (currentVelocity <= maxSpeed)
                currentVelocity += acceleration*Time.deltaTime;

        }
//        else if (Input.GetKey(KeyCode.DownArrow))
//        {
//            // minus speed
//            if (currentVelocity >= -maxSpeed)
//                currentVelocity -= acceleration*Time.deltaTime;
//
//        }
//        else
//        {
//            // No key input. 
//            if (currentVelocity > 0)
//                currentVelocity -= acceleration*Time.deltaTime;
//            else if (currentVelocity < 0)
//                currentVelocity += acceleration*Time.deltaTime;
//
//        }


        // Turn off engine if currentVelocity is too small. 
        if (Mathf.Abs(currentVelocity) <= 0.05f)
            currentVelocity = 0;

        // Move Tank by currentVelocity
        transform.Translate(new Vector3(0, 0, currentVelocity*Time.deltaTime));



        var to = Quaternion.LookRotation(Target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, to, rotationSpeed*Time.deltaTime);
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y, 0);

//        // Fire!
//        if (Input.GetButtonDown("Fire1"))
//        {
//            // make fire effect.
//            Instantiate(fireEffect, spawnPoint.position, spawnPoint.rotation);
//
//            // make ball
//            Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation);
//        }

    }
}