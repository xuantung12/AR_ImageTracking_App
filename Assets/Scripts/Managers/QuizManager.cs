using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages quiz state, scoring, and logic for ar_kid quiz feature
/// Handles question progression, answer validation, and result calculation
/// </summary>
public class QuizManager : MonoBehaviour
{
    // Quiz state
    private List<QuizQuestion> questions;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private int? selectedAnswerIndex = null;
    private bool hasAnswered = false;
    private bool isQuizCompleted = false;
    
    // Events for UI updates
    public delegate void QuizStateChanged();
    public event QuizStateChanged OnQuizStateChanged;
    
    public delegate void AnswerSelected(int answerIndex, bool isCorrect);
    public event AnswerSelected OnAnswerSelected;
    
    public delegate void QuizCompleted(int finalScore, int totalQuestions);
    public event QuizCompleted OnQuizCompleted;
    
    void Start()
    {
        Debug.Log("🎯 QuizManager Started - Initializing quiz...");
        InitializeQuiz();
    }

    void InitializeQuiz()
    {
        questions = QuizData.GetAllQuestions();
        currentQuestionIndex = 0;
        score = 0;
        selectedAnswerIndex = null;
        hasAnswered = false;
        isQuizCompleted = false;
        
        Debug.Log($"✅ Quiz initialized with {questions.Count} questions");
        OnQuizStateChanged?.Invoke();
    }

    // ========== PUBLIC GETTERS ==========
    
    public QuizQuestion GetCurrentQuestion()
    {
        if (questions != null && currentQuestionIndex < questions.Count)
        {
            return questions[currentQuestionIndex];
        }
        return null;
    }
    
    public int GetCurrentQuestionIndex()
    {
        return currentQuestionIndex;
    }
    
    public int GetTotalQuestions()
    {
        return questions != null ? questions.Count : 0;
    }
    
    public int GetScore()
    {
        return score;
    }
    
    public int? GetSelectedAnswerIndex()
    {
        return selectedAnswerIndex;
    }
    
    public bool HasAnswered()
    {
        return hasAnswered;
    }
    
    public bool IsQuizCompleted()
    {
        return isQuizCompleted;
    }
    
    public int GetProgressPercentage()
    {
        if (questions == null || questions.Count == 0) return 0;
        return Mathf.RoundToInt(((float)(currentQuestionIndex + 1) / questions.Count) * 100);
    }

    public int GetScorePercentage()
    {
        if (questions == null || questions.Count == 0) return 0;
        int maxScore = questions.Count * QuizData.POINTS_PER_QUESTION;
        return Mathf.RoundToInt(((float)score / maxScore) * 100);
    }

    // ========== QUIZ ACTIONS ==========
    
    /// <summary>
    /// Select an answer for the current question
    /// </summary>
    public void SelectAnswer(int answerIndex)
    {
        if (hasAnswered || isQuizCompleted)
        {
            Debug.LogWarning("⚠️ Cannot select answer - already answered or quiz completed");
            return;
        }
        
        QuizQuestion currentQuestion = GetCurrentQuestion();
        if (currentQuestion == null)
        {
            Debug.LogError("❌ No current question available");
            return;
        }
        
        selectedAnswerIndex = answerIndex;
        hasAnswered = true;
        
        // Check if answer is correct
        bool isCorrect = answerIndex == currentQuestion.correctAnswer;
        
        if (isCorrect)
        {
            score += QuizData.POINTS_PER_QUESTION;
            Debug.Log($"✅ Correct answer! Score: {score}");
        }
        else
        {
            Debug.Log($"❌ Wrong answer. Correct was: {currentQuestion.correctAnswer}");
        }
        
        // Notify UI
        OnAnswerSelected?.Invoke(answerIndex, isCorrect);
        OnQuizStateChanged?.Invoke();
        
        // Auto-advance after 1.5 seconds
        Invoke(nameof(NextQuestion), 1.5f);
    }
    
    /// <summary>
    /// Move to the next question or complete quiz
    /// </summary>
    public void NextQuestion()
    {
        if (currentQuestionIndex < questions.Count - 1)
        {
            // Move to next question
            currentQuestionIndex++;
            selectedAnswerIndex = null;
            hasAnswered = false;
            
            Debug.Log($"📝 Moving to question {currentQuestionIndex + 1}/{questions.Count}");
            OnQuizStateChanged?.Invoke();
        }
        else
        {
            // Quiz completed
            CompleteQuiz();
        }
    }
    
    /// <summary>
    /// Complete the quiz and show results
    /// </summary>
    void CompleteQuiz()
    {
        isQuizCompleted = true;
        
        int percentage = GetScorePercentage();
        Debug.Log($"🎉 Quiz completed! Score: {score}/{questions.Count * QuizData.POINTS_PER_QUESTION} ({percentage}%)");
        
        OnQuizCompleted?.Invoke(score, questions.Count);
        OnQuizStateChanged?.Invoke();
    }
    
    /// <summary>
    /// Restart the quiz from the beginning
    /// </summary>
    public void RestartQuiz()
    {
        Debug.Log("🔄 Restarting quiz...");
        InitializeQuiz();
    }
    
    /// <summary>
    /// Return to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        Debug.Log("🏠 Returning to main menu...");
        SceneTransitionManager.LoadMainMenu();
    }
    
    // ========== RESULT HELPERS ==========
    
    /// <summary>
    /// Get result message based on score percentage
    /// </summary>
    public string GetResultMessage()
    {
        int percentage = GetScorePercentage();
        
        if (percentage >= 80)
        {
            return "Xuất sắc!";
        }
        else if (percentage >= 60)
        {
            return "Tốt lắm!";
        }
        else if (percentage >= 40)
        {
            return "Khá tốt!";
        }
        else
        {
            return "Cố gắng lên nhé!";
        }
    }
    
    /// <summary>
    /// Get result icon based on score percentage
    /// </summary>
    public string GetResultIcon()
    {
        int percentage = GetScorePercentage();
        
        if (percentage >= 80)
        {
            return "🏆"; // Trophy
        }
        else if (percentage >= 60)
        {
            return "😊"; // Happy face
        }
        else if (percentage >= 40)
        {
            return "🙂"; // Slight smile
        }
        else
        {
            return "😐"; // Neutral face
        }
    }
    
    /// <summary>
    /// Get result color based on score percentage
    /// </summary>
    public Color GetResultColor()
    {
        int percentage = GetScorePercentage();
        
        if (percentage >= 80)
        {
            return new Color(1f, 0.76f, 0.03f); // Gold/Amber
        }
        else if (percentage >= 60)
        {
            return new Color(0.3f, 0.8f, 0.3f); // Green
        }
        else if (percentage >= 40)
        {
            return new Color(0.2f, 0.6f, 1f); // Blue
        }
        else
        {
            return new Color(1f, 0.6f, 0.2f); // Orange
        }
    }
}
