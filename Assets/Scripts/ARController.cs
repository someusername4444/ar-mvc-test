using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public interface IARController
{
    public bool Initialized
    {
        get;
    }
    public void Initialize();
    public void Activate();
    public void Deactivate();
}

public class ARController : IARController, IUpdateable
{
    public ILogger logger
    {
        get;
        set;
    }

    public IConfigManager configManager
    {
        get;
        set;
    }

    public IUpdateManager updateManager
    {
        get;
        set;
    }

    public bool Initialized
    {
        get;
        private set;
    }

    public bool Active
    {
        get;
        private set;
    }

    private ARControllerConfig arControllerConfig;
    private GameObject objectToPlace;
    private ARSessionOrigin arSessionOrigin;
    private ARSession arSession;
    private ARRaycastManager rayCastManager;
    private Transform arCamera;
    private List<TMP_Text> distanceViews = new List<TMP_Text>();

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public void Initialize()
    {
        App.Instance.factory.ResolveDependencies(this);
        updateManager.RegisterUpdateable(this);
        arControllerConfig = configManager.GetConfig<ARControllerConfig>();
        arSessionOrigin = UnityEngine.Object.Instantiate(arControllerConfig.arSessionOrigin);
        arSession = UnityEngine.Object.Instantiate(arControllerConfig.arSession);
        objectToPlace = arControllerConfig.objectToPlace;
        rayCastManager = arSessionOrigin.GetComponent<ARRaycastManager>();
        arCamera = arSessionOrigin.transform.Find("AR Camera");
    }

    public void Activate()
    {
        logger.Info("Activated arController");
        Active = true;
        arSessionOrigin.gameObject.SetActive(true);
        arSession.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        logger.Info("Deactivated arController");
        Active = false;
        arSessionOrigin.gameObject.SetActive(false);
        arSession.gameObject.SetActive(false);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (
            Input.touchCount > 0 &&
            Input.GetTouch(0) is Touch touch &&
            touch.phase == TouchPhase.Began
        )
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void UpdateMe()
    {
        foreach (var distanceView in distanceViews)
        {
            var distance = Vector3.Distance(distanceView.transform.parent.position, arCamera.position);
            distanceView.text = $"Distance {distance}";
        }

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        
        if (RaycastARPlane(touchPosition))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            var newObject = UnityEngine.Object.Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
            newObject.AddComponent<ARAnchor>();
            var distanceView = newObject.transform.Find("DistanceText").GetComponent<TMP_Text>();
            distanceViews.Add(distanceView);
        }
    }

    private bool RaycastARPlane(Vector2 touchPosition)
    {
        if (ClickingUI(touchPosition))
        {
            return false;
        }

        return rayCastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon);
    }

    private bool ClickingUI(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = touchPosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }
}

