using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;
    public Button nextButton;
    public Button retryButton;
    public Button exitButton;


    public List<Question> questions = new List<Question>();
    private int currentQuestionIndex = 0;
    private int score = 0;

    private GameObject infoPanel; // Reference to the info panel

    void Start()
    {
        quizPanel.SetActive(false); // Hide at start
        feedbackText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

         nextButton.onClick.AddListener(OnNextQuestion);
         retryButton.onClick.AddListener(RetryQuiz);
         exitButton.onClick.AddListener(ExitQuiz);
    }

    /// <summary>
    /// Call this method and pass the list of QuizQuestion from ArtworkData to load the quiz.
    /// </summary>
    public void LoadQuizFromArtwork(QuizQuestion[] quizData, GameObject infoPanel)
    {
        if (quizData == null || quizData.Length == 0)
        {
            Debug.LogWarning("No quiz data found for this artwork.");
            return;
        }

        questions.Clear();

        foreach (var quiz in quizData)
        {
            Question q = new Question
            {
                questionText = quiz.question,
                answers = quiz.options,
                correctAnswerIndex = quiz.correctAnswerIndex
            };
            questions.Add(q);
        }

        StartQuiz();
        infoPanel = infoPanel;
}


    public void StartQuiz()
    {
        quizPanel.SetActive(true);
        currentQuestionIndex = 0;
        score = 0;
        foreach (var btn in answerButtons) btn.gameObject.SetActive(true);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        feedbackText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        var q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < q.answers.Length)
            {
                var index = i;
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnAnswerSelected(int index)
    {
        bool isCorrect = index == questions[currentQuestionIndex].correctAnswerIndex;
        feedbackText.text = isCorrect ? "Correct!" : "Oops, try again!";
        feedbackText.color = isCorrect ? Color.green : Color.red;
        feedbackText.gameObject.SetActive(true);

        if (isCorrect) score++;

        nextButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        foreach (var btn in answerButtons) btn.interactable = false;
    }

    public void OnNextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            foreach (var btn in answerButtons) btn.interactable = true;
            ShowQuestion();
        }
        else
        {
            ShowResults();
        }
    }

    void ShowResults()
    {
        questionText.text = $"You scored {score}/{questions.Count}";
        feedbackText.gameObject.SetActive(false);
        foreach (var btn in answerButtons) btn.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void RetryQuiz()
    {
        foreach (var btn in answerButtons) btn.interactable = true;
        retryButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        StartQuiz();
    }

    public void ExitQuiz()
    {
        quizPanel.SetActive(false);
        infoPanel.SetActive(true); 

    }
}
