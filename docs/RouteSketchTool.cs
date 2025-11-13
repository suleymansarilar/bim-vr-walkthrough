using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lightweight route sketcher – press the add key to drop waypoints, press clear to reset.
/// Emits log events so you can replay how a group converged on a route.
/// </summary>
public class RouteSketchTool : MonoBehaviour
{
    [Header("Input")]
    public KeyCode addPointKey = KeyCode.R;
    public KeyCode clearRouteKey = KeyCode.C;

    [Header("Visuals")]
    public Material lineMaterial;
    public float lineWidth = 0.05f;

    private readonly List<Vector3> points = new List<Vector3>();
    private LineRenderer lineRenderer;
    private Camera sourceCamera;
    private CollaborativeSessionManager sessionManager;
    private SessionLogger logger;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = lineMaterial != null
            ? lineMaterial
            : new Material(Shader.Find("Sprites/Default"));

        sourceCamera = GetComponentInChildren<Camera>();
        if (sourceCamera == null)
        {
            sourceCamera = Camera.main;
        }

        sessionManager = CollaborativeSessionManager.Instance;
        logger = SessionLogger.Instance;
        UpdateLineColour();
    }

    private void Update()
    {
        if (Input.GetKeyDown(addPointKey))
        {
            TryAddPoint();
        }

        if (Input.GetKeyDown(clearRouteKey))
        {
            ClearRoute();
        }
    }

    private void TryAddPoint()
    {
        if (sourceCamera == null) return;

        Ray ray = sourceCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out var hitInfo, 100f))
        {
            return;
        }

        points.Add(hitInfo.point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        UpdateLineColour();

        string roleName = sessionManager != null ? sessionManager.CurrentRole.roleName : "unassigned";
        logger.LogEvent("route_point_added", roleName, $"Route point #{points.Count} on {hitInfo.collider.name}", hitInfo.point);
    }

    private void ClearRoute()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        string roleName = sessionManager != null ? sessionManager.CurrentRole.roleName : "unassigned";
        logger.LogEvent("route_cleared", roleName, "Route clearance requested");
    }

    private void UpdateLineColour()
    {
        if (sessionManager == null) return;

        var colour = sessionManager.CurrentRole.markerColor;
        lineRenderer.startColor = colour;
        lineRenderer.endColor = colour;
    }
}

