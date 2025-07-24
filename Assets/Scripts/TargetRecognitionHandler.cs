using UnityEngine;
using TMPro;
using Vuforia;
using System.Linq;
using UnityEngine.UI;

public class TargetRecognitionHandler : DefaultObserverEventHandler
{
    [Header("UI References")]
    public GameObject infoPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI artistText;
    public TextMeshProUGUI yearText;
    public TextMeshProUGUI descriptionText;
    public Button takeQuizButton;
    public Button playAudioButton;
    public TextMeshProUGUI playAudioButtonText;

    private static ArtworkDatabase artworkDB;
    public QuizManager quizManager;

    [Header("Audio")]
    public AudioSource narrationSource;
    private string currentAudioFile = null;

    protected override void Start()
    {
        base.Start();

        if (artworkDB == null)
        {
            artworkDB = ArtworkDataLoader.LoadArtworkData();
            if (artworkDB == null)
            {
                Debug.LogError("Artworks database could not be loaded.");
                return;
            }
        }

        Debug.Log("🎨 Artwork database loaded with " + artworkDB.artworks.Length + " entries.");
        takeQuizButton?.gameObject.SetActive(false);
        playAudioButton?.gameObject.SetActive(false);
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        Debug.Log("🎯 Tracking found for: " + gameObject.name);
        ShowArtworkInfo();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        Debug.Log("❌ Tracking lost for: " + gameObject.name);

        if (infoPanel != null) infoPanel.SetActive(false);

        try
        {
            titleText?.SetText("Tracking lost");
            artistText?.SetText("");
            yearText?.SetText("");
            descriptionText?.SetText("");
            takeQuizButton?.gameObject.SetActive(false);
            playAudioButton?.gameObject.SetActive(false);

            StopNarration();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating UI on tracking lost: " + ex.Message);
        }
    }

    private void ShowArtworkInfo()
    {
        if (infoPanel == null) return;

        infoPanel.SetActive(true);

        string targetName = gameObject.name;
        if (string.IsNullOrEmpty(targetName))
        {
            Debug.LogWarning("Target name is empty.");
            return;
        }

        var data = artworkDB.artworks.FirstOrDefault(a => a.targetName == targetName);
        if (data != null)
        {
            titleText?.SetText(data.title);
            artistText?.SetText(data.artist);
            yearText?.SetText("Created in the year: " + data.year);
            descriptionText?.SetText(data.description);

            // Setup narration toggle button
            if (playAudioButton != null)
            {
                playAudioButton.gameObject.SetActive(true);
                playAudioButton.onClick.RemoveAllListeners();
                playAudioButtonText.text = "Play Audio";
                playAudioButton.onClick.AddListener(() => ToggleNarration(data.audioFileName));
            }

            // Autoplay
            ToggleNarration(data.audioFileName);

            Debug.Log($"✅ Found artwork: {data.title} by {data.artist} ({data.year})");

            // Setup quiz button
            if (takeQuizButton != null)
            {
                takeQuizButton.gameObject.SetActive(true);
                takeQuizButton.onClick.RemoveAllListeners();
                takeQuizButton.onClick.AddListener(() =>
                {
                    Debug.Log("📝 Quiz accepted for " + data.title);
                    StopNarration();
                    infoPanel.SetActive(false);
                    quizManager.LoadQuizFromArtwork(data.quiz, infoPanel);
                });
            }
        }
        else
        {
            titleText?.SetText("Artwork not found");
            Debug.LogWarning($"❌ No artwork found for target: {targetName}");
        }
    }

    private void ToggleNarration(string filename)
    {
        if (narrationSource == null || string.IsNullOrEmpty(filename))
        {
            Debug.LogWarning("🎧 Audio source or file name is invalid.");
            return;
        }

        if (narrationSource.isPlaying && currentAudioFile == filename)
        {
            narrationSource.Stop();
            playAudioButtonText.text = "Play Audio";
            Debug.Log("🔇 Stopped narration for: " + filename);
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + filename);
            if (clip != null)
            {
                narrationSource.clip = clip;
                currentAudioFile = filename;
                narrationSource.Play();
                playAudioButtonText.text = "Stop Audio";
                Debug.Log("🔊 Playing narration for: " + filename);
            }
            else
            {
                Debug.LogWarning("⚠️ Audio clip not found: " + filename);
            }
        }
    }

    private void StopNarration()
    {
        if (narrationSource != null && narrationSource.isPlaying)
        {
            narrationSource.Stop();
            playAudioButtonText.text = "Play Audio";
            Debug.Log("🔊 Stopped narration.");
        }

        currentAudioFile = null;
    }
}
