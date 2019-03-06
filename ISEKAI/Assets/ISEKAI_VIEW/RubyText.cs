using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text), typeof(RectTransform))]
public class RubyText : MonoBehaviour
{

    Text text;
    RectTransform rt;
    public GameObject TextPrefab;  // テキストはセンタリングしておくこと

    public Rect position;
    public float yOffset = 10;

    /*
     * 
     * 유동크기 다시 돌릴떄
     * 1. EventScene 2번째 캔버스 constant pixel size 로 바꾸기
     * 2. 밑에거 주석 풀기
     * 3. 밑에거 없애기
     */
    public float canvasHeight = 1080;
    public float canvasWidth = 1920;
    public float canvasMatch = 1f;

    public bool isCalled = false;

    private Regex rubyRex = new Regex("\\{(.*?):(.*?)\\}");

    string cleanText;    // base text, without any furigana or markup

    void LateUpdate()
    {
        if(isCalled == false)
        {
            isCalled = true;

            List<int> furiCharIndex = new List<int>();    // char index (in base text) where furigana appears
            List<int> furiCharLen = new List<int>();    // length of base text the furigana is over
            List<string> furiText = new List<string>();    // actual text of the furigana

            text = GetComponent<Text>();
            rt = GetComponent<RectTransform>();

            cleanText = text.text;

            Debug.Log("???");

            for (; ; )
            {
                Match match = rubyRex.Match(cleanText);
                if (!match.Success) break;
                furiCharIndex.Add(match.Index);
                furiText.Add(match.Groups[2].ToString());
                furiCharLen.Add(match.Groups[1].Length);
                cleanText = cleanText.Substring(0, match.Index) + match.Groups[1]
                        + cleanText.Substring(match.Index + match.Length);

                Debug.Log("몇번나오나");
            }

            text.text = cleanText;

            var generator = new TextGenerator();
            generator.Populate(text.text, text.GetGenerationSettings(rt.sizeDelta));

            var cleanCharArray = generator.GetCharactersArray();
            Debug.Log(cleanCharArray == null);

            for (int i = 0; i < (furiCharIndex.Count); i++)
            {
                Debug.Log("furiCharIndex.Count: " + furiCharIndex.Count);
                Debug.Log("cleanCharArray Length: " + cleanCharArray.Length);

                Vector2 leftPos = cleanCharArray[furiCharIndex[i]].cursorPos;
                Vector2 rightPos = cleanCharArray[furiCharIndex[i] + furiCharLen[i]].cursorPos;

                if (rightPos.x <= leftPos.x)
                {
                    //rightPos = new Vector2(position.x + position.width, leftPos.y);
                }
                var o = GameObject.Instantiate(TextPrefab, GetComponent<Transform>());

                var prt = o.GetComponent<RectTransform>();


                float screenScale = (canvasWidth / Screen.width) * (1.0f - canvasMatch) + (canvasHeight / Screen.height) * canvasMatch;
                //2. 밑에거 주석 풀기
                //prt.localPosition = new Vector3((leftPos.x + rightPos.x) / 2 , leftPos.y + yOffset, 0);
                //3.밑에거 없애기
                prt.localPosition = new Vector3(((leftPos.x + rightPos.x) / 2) * screenScale, (leftPos.y ) * screenScale + yOffset, 0);

                o.GetComponent<Text>().text = furiText[i];
            }

            generator.Invalidate();
        }
       
    }
}
