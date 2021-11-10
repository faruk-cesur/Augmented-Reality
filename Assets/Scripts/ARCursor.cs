using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    public GameObject cursorChildObject; // The Cursor Image
    public GameObject objectToPlace; // The 3D Model To Place In Scene
    public ARRaycastManager raycastManager; // Detects Planes We Touch

    public bool useCursor = true;
    public bool instantiated;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    private void Update()
    {
        if (useCursor)
        {
            UpdateCursor();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (useCursor)
            {
                if (!instantiated)
                {
                    instantiated = true;
                    var transform1 = transform;
                    GameObject.Instantiate(objectToPlace, transform1.position, transform1.rotation);
                    cursorChildObject.SetActive(false);
                }
            }
            else
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                if (hits.Count > 0)
                {
                    GameObject.Instantiate(objectToPlace, hits[0].pose.position, hits[0].pose.rotation);
                }
            }
        }
    }

    private void UpdateCursor()
    {
        Vector2 screenPosition = _camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            var transform1 = transform;
            transform1.position = hits[0].pose.position;
            transform1.rotation = hits[0].pose.rotation;
        }
    }
}