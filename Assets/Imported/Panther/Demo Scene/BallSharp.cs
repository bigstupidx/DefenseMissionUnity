// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class BallSharp : MonoBehaviour
{


    public float speed = 200;
    public float range = 400;

    public GameObject ExploPtcl;

    public bool Explode;

    private float dist;

    void Update()
    {

        // Move Ball forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // Record Distance.
        dist += Time.deltaTime * speed;

        // If reach to my range, Destroy. 
        if (dist >= range)
        {
            DestroyBall();
        }
    }

    void DestroyBall()
    {
        if (Explode)
            Instantiate(ExploPtcl, transform.position, transform.rotation);
        ObjectPool.instance.PoolObject(gameObject);
        dist = 0;
    }

}