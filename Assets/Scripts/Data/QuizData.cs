using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quiz data repository for ar_kid app
/// Contains all quiz questions
/// Ported from Flutter ar_kid app
/// </summary>
public static class QuizData
{
    public static List<QuizQuestion> GetAllQuestions()
    {
        return new List<QuizQuestion>
        {
            new QuizQuestion(
                question: "Động vật nào sau đây sống ở biển?",
                options: new string[] { "Chó", "Mèo", "Cá", "Gà" },
                correctAnswer: 2
            ),
            new QuizQuestion(
                question: "2 + 2 bằng bao nhiêu?",
                options: new string[] { "3", "4", "5", "6" },
                correctAnswer: 1
            ),
            new QuizQuestion(
                question: "Trái đất có bao nhiêu mặt trăng?",
                options: new string[] { "0", "1", "2", "3" },
                correctAnswer: 1
            ),
            new QuizQuestion(
                question: "Màu gì là màu của lá cây?",
                options: new string[] { "Đỏ", "Xanh", "Vàng", "Tím" },
                correctAnswer: 1
            ),
            new QuizQuestion(
                question: "Con vật nào có thể bay?",
                options: new string[] { "Chim", "Cá", "Rùa", "Chuột" },
                correctAnswer: 0
            ),
            new QuizQuestion(
                question: "Một tuần có bao nhiêu ngày?",
                options: new string[] { "5", "6", "7", "8" },
                correctAnswer: 2
            ),
            new QuizQuestion(
                question: "Hình tròn có bao nhiêu góc?",
                options: new string[] { "0", "1", "3", "4" },
                correctAnswer: 0
            ),
            new QuizQuestion(
                question: "Mặt trời mọc ở hướng nào?",
                options: new string[] { "Tây", "Đông", "Nam", "Bắc" },
                correctAnswer: 1
            ),
            new QuizQuestion(
                question: "Con gì kêu gâu gâu?",
                options: new string[] { "Mèo", "Chó", "Gà", "Vịt" },
                correctAnswer: 1
            ),
            new QuizQuestion(
                question: "5 x 2 bằng bao nhiêu?",
                options: new string[] { "8", "10", "12", "15" },
                correctAnswer: 1
            )
        };
    }
    
    public const int POINTS_PER_QUESTION = 10;
    public const int TOTAL_QUESTIONS = 10;
    public const int MAX_SCORE = 100;
}
