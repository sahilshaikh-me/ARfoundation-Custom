using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToSpawn;
    public GameObject spawnedObject;

    private ARRaycastManager arRayCastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    public bool canPlaceObject = false;

    void Start()
    {
        arRayCastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if(canPlaceObject == false) {
            placementIndicator.SetActive(false);
            return;
        }

        // spawnedObject == null &&
        if( placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            SpawnARObject();
        }
        
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void UpdatePlacementIndicator() {
        // if(spawnedObject == null && placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        // } else {
        //     placementIndicator.SetActive(false);
        // }
    }

    void UpdatePlacementPose() {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        var hits = new List<ARRaycastHit>();

        arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if(placementPoseIsValid) {
            placementPose = hits[0].pose;
        }
    }

    void SpawnARObject() {
        spawnedObject.transform.position = placementPose.position;
        canPlaceObject = false;
        placementIndicator.SetActive(false);
        // spawnedObject = Instantiate(objectToSpawn, placementPose.position, placementPose.rotation);
    }
}
