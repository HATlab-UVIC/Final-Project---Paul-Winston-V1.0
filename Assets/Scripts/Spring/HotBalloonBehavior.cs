using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotBalloonBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("Hot Balloons Spawner")]
    private GameObject hotBalloonsSpawner;

    private GameObject[] lockedPlanes;
    private List<Mesh> lockedPlanesMeshes;

    public static UnityEvent<GameObject[]> lockedPlanesGenerated;



    private void Start()
    {
        lockedPlanes = GameObject.FindGameObjectsWithTag("LockedPlane");
        lockedPlanesGenerated.AddListener(_OnLockedPlaneGeneratePath);
    }

    public void _OnLockedPlaneGeneratePath(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            lockedPlanesMeshes.Add(obj[i].GetComponent<MeshFilter>().mesh);
        }
    }

}
