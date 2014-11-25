using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WayPoint:MonoBehaviour
{
    public WayPoint Next;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1f,1f, 0.7f);
        Gizmos.DrawSphere(transform.position, 43f);
        if (Next != null)
        {
            Gizmos.DrawLine(transform.position, Next.transform.position);
            Gizmos.color = new Color(1, 0f, 0f, 1.0f);

            Gizmos.DrawLine(transform.position, Vector3.Lerp(transform.position, Next.transform.position, 0.3f));
        }
    }

}
