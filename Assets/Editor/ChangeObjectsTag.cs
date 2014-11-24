using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class ChangeObjectsTag : MonoBehaviour
{
    private const string cTargetTag = "Death";

    [MenuItem ("GameObject/SetTag")]
    private static void ChangeTag()
    {
        GameObject selection = Selection.gameObjects.FirstOrDefault();
        SetTag(selection);
    }

    private static void SetTag(GameObject gameObject)
    {
        gameObject.tag = cTargetTag;

        foreach (Transform t in gameObject.transform)
        {
            SetTag(t.gameObject);
        }
    }

}
