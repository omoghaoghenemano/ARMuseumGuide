using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MuseumPlacementManager : MonoBehaviour
{
    public SceneFadeController fadeController;
    public GameObject museumPrefab;
    public GameObject placementIndicator;
    public CameraSwitcher cameraSwitcher;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool museumPlaced = false;
    public GameObject tapToPlaceUI;

    void Start()
    {
        Debug.Log("MuseumPlacementManager started.");



        if (museumPrefab == null)
        {
            Debug.LogError("Museum prefab is not assigned. Please assign it in the inspector.");
            return;
        }
        else
        {
            Debug.Log($"Museum prefab assigned: {museumPrefab.name}");
        }

        raycastManager = FindFirstObjectByType<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found in the scene. Please add it to an AR Session Origin.");
        }

        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher is not assigned. Please assign it in the inspector.");
            return;
        }

        cameraSwitcher.SwitchToARFoundation();

        if (tapToPlaceUI != null)
        {
            Debug.Log("Tap to place UI is active.");
            tapToPlaceUI.SetActive(true);
        }
        
    }

    void Update()
    {
   
    if (museumPlaced) return;

    UpdatePlacementPose();
    UpdatePlacementIndicator();

    Debug.Log($"Placement pose valid: {placementPoseIsValid}, Museum placed: {museumPlaced}");
    Debug.Log("Touch count: " + Input.touchCount);

    if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
        Debug.Log($"Placing museum at position: {placementPose.position}, rotation: {placementPose.rotation}");
        PlaceMuseum();
    }
    }

    void UpdatePlacementPose()
    {
        if (Input.touchCount == 0) return;

        Vector2 touchPosition = Input.GetTouch(0).position;
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(placementPoseIsValid && !museumPlaced);
            if (placementPoseIsValid)
            {
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
        }
    }
    async void PlaceMuseum()
    {
        MuseumPlacementData.museumPosition = placementPose.position;
        MuseumPlacementData.museumRotation = placementPose.rotation;
        MuseumPlacementData.museumPlaced = true;

        museumPlaced = true;

        if (placementIndicator != null)
            placementIndicator.SetActive(false);

        try
        {
        if (tapToPlaceUI != null)
        {
            Debug.Log("Tap to place UI is active.");
            tapToPlaceUI.SetActive(false);
        }
            await LoadSceneSmoothly("VuforiaScene");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load Vuforia scene: {ex.Message}");
        }
    }


    IEnumerator LoadVuforiaSceneSmoothly()
    {
        Debug.Log("Starting async load of VuforiaScene...");

        // Optional: Trigger fade-out here
        // Example: yield return FadeOutScreen();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("VuforiaScene");
        asyncLoad.allowSceneActivation = false;

        // Wait until scene is loaded (but not activated)
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Optional: wait extra time or show loading indicator
                Debug.Log("Scene loaded, activating...");
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    
     public async Task LoadSceneSmoothly(string sceneName)
    {
        if (fadeController == null)
        {
            Debug.LogError("FadeController not assigned!");
            return;
        }

        await fadeController.FadeOutAsync();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            await Task.Yield();
        }

        asyncLoad.allowSceneActivation = true;

        // Wait one frame for new scene to activate
        await Task.Yield();

        // Optionally, find fade controller again in new scene
        SceneFadeController newFader = FindAnyObjectByType<SceneFadeController>();
        if (newFader != null)
        {
            await newFader.FadeInAsync();
        }
    }
}
