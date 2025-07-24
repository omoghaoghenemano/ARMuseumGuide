using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class ArtworkDataLoader : MonoBehaviour
{
    public static ArtworkDatabase LoadArtworkData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "artworks.json");

        if (!File.Exists(path))
        {
            Debug.LogError("Artwork JSON not found at: " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<ArtworkDatabase>("{\"artworks\":" + json + "}");
    }
}
