using UnityEngine;
using System.Collections;

public class FriendlyBase : MonoBehaviour
{
    public float CurrentHealth = 100f;
    public static FriendlyBase Instance;
    private void Start()
    {
        Instance = this;
    }

}
