using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FuriganaText : MonoBehaviour
{
    public string text;
    public Rect position;
    public GUISkin skin;
    public string textStyle = "Label";
    public string furiStyle = "Furigana";
    public float yOffset = 10;
    static Regex furiRegex;

    void OnGUI()
    {
        GUI.skin = skin;
        if (furiRegex == null)
        {
            furiRegex = new Regex("\\[(.*?):(.*?)\\]");
        }
        string cleanText;    // base text, without any furigana or markup
        List<int> furiCharIndex = new List<int>();    // char index (in base text) where furigana appears
        List<int> furiCharLen = new List<int>();    // length of base text the furigana is over
        List<string> furiText = new List<string>();    // actual text of the furigana

        cleanText = text;
        while (true)
        {
            Match match = furiRegex.Match(cleanText);
            if (!match.Success) break;
            furiCharIndex.Add(match.Index);
            furiText.Add(match.Groups[2].ToString());
            furiCharLen.Add(match.Groups[1].Length);
            cleanText = cleanText.Substring(0, match.Index) + match.Groups[1]
                    + cleanText.Substring(match.Index + match.Length);
        }

        GUIStyle textGUIStyle = skin.GetStyle(textStyle);
        GUIContent cleanContent = new GUIContent(cleanText);
        GUI.Label(position, cleanContent, textGUIStyle);

        GUIStyle furiGUIStyle = skin.GetStyle(furiStyle);
        for (int i = 0; i < furiCharIndex.Count; i++)
        {
            Vector2 leftPos = textGUIStyle.GetCursorPixelPosition(position, cleanContent, furiCharIndex[i]);
            Vector2 rightPos = textGUIStyle.GetCursorPixelPosition(position, cleanContent,
                                                                   furiCharIndex[i] + furiCharLen[i]);
            if (rightPos.x <= leftPos.x)
            {
                rightPos = new Vector2(position.x + position.width, leftPos.y);
            }
            Rect r = new Rect(leftPos.x, leftPos.y - yOffset - 20, rightPos.x - leftPos.x, 20);
            GUI.Label(r, furiText[i], furiGUIStyle);
        }
    }
}