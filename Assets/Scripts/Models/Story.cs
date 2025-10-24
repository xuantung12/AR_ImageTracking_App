using System;

/// <summary>
/// Story data model for ar_kid story reading feature
/// Ported from Flutter ar_kid app
/// </summary>
[Serializable]
public class Story
{
    public string id;
    public string title;
    public string author;
    public string content;
    public string summary;
    public string category;
    public int ageRange;

    public Story(string id, string title, string author, string content, string summary, string category, int ageRange)
    {
        this.id = id;
        this.title = title;
        this.author = author;
        this.content = content;
        this.summary = summary;
        this.category = category;
        this.ageRange = ageRange;
    }
}
