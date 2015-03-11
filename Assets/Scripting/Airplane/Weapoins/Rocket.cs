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
            Speed += Acceleration * Time.fixedDeltaTime;
        rigidbody.position += transform.forward * Speed * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Target)
        {
            if (time > 0.5f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(Target.transform.position - transform.position),
                    (time > 1.5f ? 10f : 5f)*Time.deltaTime*0.25f);

            }
            else
            {
                time += Time.deltaTime;
            }
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
