using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanePrefab : MonoBehaviour
{
    private const int GRAY_PLANE_QUEUE = 3001;
    private const int DEFAULT_PLANE_QUEUE = 3000;

    void Start()
    {
        ColorClassify();
    }

    private void ColorClassify()
    {
        var plane = GetComponent<ARPlane>();
        Color color = Color.gray;
        switch (plane.classification)
        {
            case PlaneClassification.Floor:
                color = new Color(0, 0.5f, 0, 1);
                break;
            case PlaneClassification.Ceiling:
                color = new Color(0, 0, 0.7f, 1);
                break;
            case PlaneClassification.Wall:
                color = Color.red;
                break;
        }

        var mat = GetComponent<MeshRenderer>().material;
        mat.color = color;
        mat.renderQueue = color == Color.gray ? GRAY_PLANE_QUEUE : DEFAULT_PLANE_QUEUE;
    }
}
