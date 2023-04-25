using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeasurementController : MonoBehaviour
{   

    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private LineRenderer measureLine;

    [SerializeField]
    private TMP_Text distanceText;

    private GameObject startPoint, endPoint;
    private Vector2 touchPosition = default;

    private bool isMeasuringModeEnabled = false;

    void Awake() {
        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);

        startPoint.SetActive(false);
        endPoint.SetActive(false);
    }

    public void StartMeasuring()
    {
        isMeasuringModeEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && isMeasuringModeEnabled) {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began) {
                touchPosition = touch.position;


                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Models") {
                    // Enable start point
                    startPoint.SetActive(true);

                    startPoint.transform.position = hit.point;
                    // startPoint.transform.SetPositionAndRotation(hit.transform.position, hit.transform.rotation);
                }
                Debug.DrawRay(ray.origin , ray.direction * 1000, Color.red, 100, true);
            }

            if(touch.phase == TouchPhase.Moved) {
                touchPosition = touch.position;

                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Models") {
                    // Enable start point
                    endPoint.SetActive(true);

                    measureLine.gameObject.SetActive(true);

                    endPoint.transform.position = hit.point;
                    // startPoint.transform.SetPositionAndRotation(hit.transform.position, hit.transform.rotation);
                }
                Debug.DrawRay(ray.origin , ray.direction * 1000, Color.red, 100, true);
            }

            if(startPoint.activeSelf && endPoint.activeSelf) {
                // distanceText.transform.position = endPoint.transform.position;
                // distanceText.transform.rotation = endPoint.transform.rotation;

                distanceText.text = $"Distance: {Math.Round(Vector3.Distance(startPoint.transform.position, endPoint.transform.position) * 100, 2)} cm";

                measureLine.SetPosition(0, startPoint.transform.position);
                measureLine.SetPosition(1, endPoint.transform.position);
            }
        }
    }
}
