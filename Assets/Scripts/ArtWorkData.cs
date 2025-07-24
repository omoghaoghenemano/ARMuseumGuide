using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuizQuestion
{
    public string question;
    public string[] options;  // ✅ Correct key matching JSON
    public int correctAnswerIndex;
}

[Serializable]
public class ArtworkData
{
    public string targetName;
    public string title;
    public string artist;
    public string year;
    public string description;
    public string audioFileName;

     public QuizQuestion[] quiz;
}

[Serializable]
public class ArtworkDatabase
{
    public ArtworkData[] artworks;
}
