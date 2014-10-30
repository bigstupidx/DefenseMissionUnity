using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
    public MissionObject Target;
    public float Speed = 250;
    public float TargetSpeed = 1000;
    public float Acceleration = 150;
    public float DestroyDistance = 200;
    public GameObject DestroyPS;

    public AudioClip StartSound;

    private float time = 0;

    void Start()
    {
        gameObject.audio.clip = StartSound;
        gameObject.audio.Play();
        audio.volume = OptionsController.Instance.SFXLevel;
        Destroy(gameObject,15);
    }

    void FixedUpdate()
    {
        if (Speed < TargetSpeed)
            Speed += Acceleration * Time.deltaTime;
        rigidbody.position += transform.forward * Speed * Time.deltaTime;
    }

    void Update()
    {
        if (Target)
        {
            if (time > 0.2f)
            {
                float m = 1f;
                if (Vector3.Distance(transform.position, Target.transform.position) < 200f)
                {
                    m = 100f;
                }

                transform.rotation = Quaternion.Lerp(transform.rotation,
                                  Quaternion.LookRotation(Target.transform.position - transform.position),
                                  (time > 1.0f ? 0.6f  : time > 0.7f ? 0.35f : 0.05f)*m);
            } else
                time += Time.deltaTime;

        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("MissionObject"))
        {
            collision.gameObject.GetComponent<MissionObject>().SetDestroyEffect(true);
        }

        GameObject ps = GameObject.Instantiate(DestroyPS) as GameObject;
        ps.transform.position = transform.position;
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        EventController.Instance.PostEvent("RocketDestroyed",gameObject);
    }
}
