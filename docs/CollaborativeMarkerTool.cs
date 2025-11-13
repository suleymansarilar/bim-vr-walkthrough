using UnityEngine;


/// </summary>
public class CollaborativeMarkerTool : MonoBehaviour
{
    [Header("Marker Settings")]
    public KeyCode placeMarkerKey = KeyCode.E;
    public float markerScale = 0.15f;
    public float markerLifetimeSeconds = 0f; 
    public GameObject markerPrefab;
    public LayerMask placementMask = Physics.DefaultRaycastLayers;

    [Header("Optional Toggle")]
    public KeyCode toggleKey = KeyCode.M;
    public bool markerModeEnabled = true;

    private Camera sourceCamera;
    private CollaborativeSessionManager sessionManager;
    private SessionLogger logger;

    private void Start()
    {
        sourceCamera = GetComponentInChildren<Camera>();
        if (sourceCamera == null)
        {
            sourceCamera = Camera.main;
        }

        sessionManager = CollaborativeSessionManager.Instance;
        logger = SessionLogger.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            markerModeEnabled = !markerModeEnabled;
        }

        if (!markerModeEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(placeMarkerKey))
        {
            TryPlaceMarker();
        }
    }

    private void TryPlaceMarker()
    {
        if (sourceCamera == null) return;

        Ray ray = sourceCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out var hitInfo, 100f, placementMask))
        {
            return;
        }

        var marker = CreateMarker(hitInfo.point, hitInfo.normal);
        var role = sessionManager != null ? sessionManager.CurrentRole : null;
        var roleName = role != null ? role.roleName : "unassigned";

        if (logger != null)
        {
            logger.LogEvent("marker_placed", roleName, $"Marker placed on {hitInfo.collider.name}", hitInfo.point);
        }

        if (markerLifetimeSeconds > 0f)
        {
            Destroy(marker, markerLifetimeSeconds);
        }
    }

    private GameObject CreateMarker(Vector3 position, Vector3 normal)
    {
        GameObject markerInstance;
        if (markerPrefab != null)
        {
            markerInstance = Instantiate(markerPrefab, position, Quaternion.LookRotation(normal));
        }
        else
        {
            markerInstance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            markerInstance.transform.position = position;
            markerInstance.transform.rotation = Quaternion.LookRotation(normal);
            var collider = markerInstance.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }

        markerInstance.transform.localScale = Vector3.one * markerScale;

        var colour = Color.cyan;
        if (sessionManager != null)
        {
            colour = sessionManager.CurrentRole.markerColor;
        }

        ApplyColour(markerInstance, colour);
        markerInstance.name = $"Marker_{colour}";
        markerInstance.transform.SetParent(null);

        return markerInstance;
    }

    private static void ApplyColour(GameObject marker, Color colour)
    {
        var renderer = marker.GetComponent<Renderer>();
        if (renderer != null)
        {
            var material = renderer.material;
            material.color = colour;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", colour * 0.5f);
        }

        foreach (Transform child in marker.transform)
        {
            ApplyColour(child.gameObject, colour);
        }
    }
}

