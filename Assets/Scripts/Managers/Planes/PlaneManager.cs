using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.MagicLeap;

public class PlaneManager : MonoBehaviour
{
    public GameObject planeSpawner;
    public Material planeMaterial;
    public Text text;

    private ARPlaneManager planeManager;

    [SerializeField, Tooltip("Maximum number of planes to return each query")]
    private uint maxResults = 100;

    [SerializeField, Tooltip("Minimum plane area to treat as a valid plane")]
    private float minPlaneArea = 0.25f;

    private readonly MLPermissions.Callbacks permissionCallbacks = new MLPermissions.Callbacks();

    private void Awake()
    {
        // subscribe to permission events
        permissionCallbacks.OnPermissionGranted += OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied += OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain += OnPermissionDenied;
    }

    private void OnDestroy()
    {
        // unsubscribe to permission events
        permissionCallbacks.OnPermissionGranted -= OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied -= OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain -= OnPermissionDenied;
    }

    private void Start()
    {
        // make sure the plane manager is disabled at the start of the scene before permissions are granted
        planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager == null)
        {
            Debug.LogError("Failed to find ARPlaneManager in scene. Disabling Script");
            text.text += "Failed to find ARPlaneManager in scene. Disabling Script";
            enabled = false;
        }
        else
        {
            // disable planeManager until we have successfully requested required permissions
            planeManager.enabled = false;
        }

        // request spatial mapping permission for plane detection
        MLPermissions.RequestPermission(MLPermission.SpatialMapping, permissionCallbacks);
    }

    private void Update()
    {
        UpdateQuery();
    }

    private void UpdateQuery()
    {
        if (planeManager.enabled)
        {
            PlanesSubsystem.Extensions.Query = new PlanesSubsystem.Extensions.PlanesQuery
            {
                // Detects Ceiling and Floor. For more information of flags: https://developer-docs.magicleap.cloud/docs/api-ref/api/Modules/group___planes#enums-mlplanesqueryflags
                Flags = planeManager.requestedDetectionMode.ToMLQueryFlags() | PlanesSubsystem.Extensions.MLPlanesQueryFlags.Polygons | PlanesSubsystem.Extensions.MLPlanesQueryFlags.Semantic_Ceiling | PlanesSubsystem.Extensions.MLPlanesQueryFlags.Semantic_Floor,
                /// The center of the bounding box which defines where planes extraction
                // should occur.
                BoundsCenter = Camera.main.transform.position,
                /// The rotation of the bounding box where planes extraction will occur.
                BoundsRotation = Camera.main.transform.rotation,
                /// The size of the bounding box where planes extraction will occur.
                BoundsExtents = Vector3.one * 20f,
                /// The maximum number of results that should be returned.
                MaxResults = maxResults,
                // The minimum area (in squared meters) of planes to be returned. This
                // value cannot be lower than 0.04 (lower values will be capped to this
                // minimum).
                MinPlaneArea = minPlaneArea
            };
        }
    }


    public void Lock()
    {
        foreach (var plane in planeManager.trackables)
        {
            var temp = Instantiate(plane);
            var planeClassification = plane.GetComponent<ARPlane>();

            Debug.Log(planeClassification.classification);

            Color color = Color.gray;
            switch (planeClassification.classification)
            {
                case PlaneClassification.Floor:
                    color = Color.yellow;
                    break;
                case PlaneClassification.Ceiling:
                    color = Color.white;
                    break;
                case PlaneClassification.Wall:
                    color = Color.red;
                    break;
            }

            temp.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            temp.GetComponent<MeshRenderer>().enabled = true;
            temp.GetComponent<PlanePrefab>().enabled = false;
            temp.GetComponent<ARPlane>().enabled = false;
            temp.GetComponent<MeshRenderer>().material = planeMaterial;
            temp.transform.parent = planeSpawner.transform;
            plane.gameObject.SetActive(false);
        }
        planeManager.enabled = false;
    }

    public void Unlock()
    {
        planeManager.enabled = true;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(true);
        }
        foreach (Transform child in planeSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OpenWindow()
    {

    }
    public void CloseWindow()
    {

    }

    private void OnPermissionGranted(string permission)
    {
        if (permission == MLPermission.SpatialMapping)
        {
            planeManager.enabled = true;
            Debug.Log("Plane manager is active");
            text.text += "Plane manager is active";
        }
        UpdateQuery();
    }

    private void OnPermissionDenied(string permission)
    {
        Debug.LogError($"Failed to create Planes Subsystem due to missing or denied {MLPermission.SpatialMapping} permission. Please add to manifest. Disabling script.");
        text.text += $"Failed to create Planes Subsystem due to missing or denied {MLPermission.SpatialMapping} permission. Please add to manifest. Disabling script.";
        enabled = false;
    }
}
