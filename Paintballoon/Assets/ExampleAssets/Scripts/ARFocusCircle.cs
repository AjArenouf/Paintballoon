using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARFocusCircle : MonoBehaviour
{
    //public GameObject scanText;
    //public GameObject placeText;

    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;
    public GameObject object5;

    public GameObject button;

    public GameObject placementIndicator;
    //private GameObject carParent;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    private bool placementIndicatorEnabled = true;
    
    bool isUIHidden = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        //scanText.SetActive(true);
        //placeText.SetActive(false);
    }

    void Update()
    {

        if (placementIndicatorEnabled == true)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }

        //if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    PlaceObject();
        //}
    }

    public void HideUI()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        if (isUIHidden == false)
        {
                foreach (var button in buttons)
                {
                    button.gameObject.SetActive(false);
                }

                placementIndicatorEnabled = false;
                placementIndicator.SetActive(false);

                isUIHidden = true;
            }
            else if (isUIHidden == true)
            {
                foreach (var button in buttons)
                {
                    button.gameObject.SetActive(true);
                }

                placementIndicatorEnabled = true;
                placementIndicator.SetActive(true);

                isUIHidden = false;
            }
        }

        public void PlaceObject()
        {
            GameObject[] virtualObjects = new GameObject[] { object1, object2, object3, object4, object5 };

            for (int i = 0; i < virtualObjects.Length; i++)
            {
                GameObject objectToPlace = Instantiate(virtualObjects[i]);
                objectToPlace.SetActive(true);
                objectToPlace.transform.position = placementPose.position;
                objectToPlace.transform.rotation = placementPose.rotation;
            }
        }

    public void SpawnAllObjects()
    {
        PlaceObject();
    }

        private void UpdatePlacementIndicator()
        {
            if (placementPoseIsValid)
            {
                placementIndicator.SetActive(true);

                Button[] buttons = FindObjectsOfType<Button>();

                foreach (var button in buttons)
                {
                    button.gameObject.SetActive(true);
                }

                //scanText.SetActive(false);
                //placeText.SetActive(true);

                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
                placementIndicator.SetActive(false);

                Button[] buttons = FindObjectsOfType<Button>();

                foreach (var button in buttons)
                {
                    button.gameObject.SetActive(false);
                }

                //placeText.SetActive(false);
            }
        }

        private void UpdatePlacementPose()
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();

            arOrigin.GetComponent<ARRaycastManager>().Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneEstimated);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;

                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }