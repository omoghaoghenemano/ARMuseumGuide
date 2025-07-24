using UnityEngine;

public class MuseumSpawner : MonoBehaviour
{
    public GameObject museumPrefab;

    void Start()
    {
        if (MuseumPlacementData.museumPlaced)
        {
            Instantiate(
                museumPrefab,
                MuseumPlacementData.museumPosition,
                MuseumPlacementData.museumRotation
            );
        }
        else
        {
            Debug.LogWarning("Museum was not placed in the previous scene.");
        }
    }
}

