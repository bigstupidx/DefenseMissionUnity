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
        Gizmos.DrawSphere(transform.position, 16f);
        if (Next != null)
        {
            Gizmos.DrawLine(transform.position, Next.transform.position);
        }
    }

}
