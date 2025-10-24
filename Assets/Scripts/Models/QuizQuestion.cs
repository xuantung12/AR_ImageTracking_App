using System;

/// <summary>
/// Quiz question data model for ar_kid quiz feature
/// Ported from Flutter ar_kid app
/// </summary>
[Serializable]
public class QuizQuestion
{
    public string question;
    public string[] options;
    public int correctAnswer;

    public QuizQuestion(string question, string[] options, int correctAnswer)
    {
        this.question = question;
        this.options = options;
        this.correctAnswer = correctAnswer;
    }
}
