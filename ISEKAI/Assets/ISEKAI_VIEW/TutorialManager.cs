using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject clickDetector;
    public GameObject entireTutorial;
    public GameObject[] tutorials;
    public bool isInTutorial = true;
    public int currentTutorialIdx = 0;
    public List<string> allTexts = new List<string>();
    public List<int> tutorialLengths = new List<int>();
    private List<string> plainText = refineText("안녕하지비 동무!\n본격적으로 이 본인을 제대로 소개하고 싶수다!\n내 이름은 \"리선녀\"라우!\n본인의 아바이는 이 부락의 작업반장이시자 본명은 \"리모콘\" 동지일수다!\n쟝 동무에게 우리 부락의 구조를 설명해드리겠올소이다!\n");
    private List<string> resourceBarText = refineText("맨 먼저 위를 보면 동무는 현재 날짜와 계절, 부락에 남아있는 음식량, 그리고 부락 주민들의 쾌적도를 알수 있소이다!\n");
    private List<string> nextTurnText = refineText("현재의 날짜와 계절은 매 턴마다 바뀔수다.\n한 턴은 6개월의 길이로써, 여름-가을 턴과 겨울-봄 턴으로 종류가 나뉠수다.\n계절이 바꿨다고 새로운 턴이 된건 아니니 주의하기 바랄습세!\n");
    private List<string> foodAndPleasantText = refineText("현재의 날짜와 계절은 매 턴마다 바뀔수다.\n한 턴은 6개월의 길이로써, 여름-가을 턴과 겨울-봄 턴으로 종류가 나뉠수다.\n계절이 바꿨다고 새로운 턴이 된건 아니니 주의하기 바랄습세!\n");
    private List<string> outskirtsText = refineText("다음으로 동무는 지금 『마을 부락』을 보고 계시올습메다.\n여기는 마을에 사는 여러 동지들의 집들과 같은 장소들이 있습메다.\n동무가 더 마을에 오랬동안 있음에 따라 더 많은 장소들이 등장할 수도 있소다.\n이런 장소들에서 무슨 일이 있을 때마다 사람이 기다리고 있을테니 그들과 만나서 대화를 해보는게 좋을 것이수다.\n");
    private List<string> apText = refineText("단, 매 턴마다 대화를 할수 있는 횟수가 제한되어있수다.\n여기 오른쪽 아래에는 쟝 동무가 이번 턴에 할수 있는 행동을 제한하는 행동력, 즉 Action Point(AP)가 표시되어 있수다!\n 대화를 하기 전에 그 대화가 행동력을 얼마나 소모하는지 잘 살펴보고 신중히 결정해야 할것이수다!\n이 말고도 어떤 대화들은 특정 시기에만 할수 있으니 주의하기 바라우!\n");
    private List<string> turnAndAPText = refineText("또한, 남아있는 AP가 2 이하로 떨어질 경우 한 턴 내의 계절이 바뀌니 그것 또한 주의해야 할것이오!\n");
    private List<string> eventText = refineText("만약 어디서 누구랑 대화를 할수 있게 된다면, 이렇게 생긴 알림이 뜰것일수다!\n이것을 보면 해당 대화를 하는데 소모하는 AP는 물론, 그 대화가 사라지기 까지 남은 턴이나 나타나는 특정 계절 같은것 또한 알수 있수다!\n");
    private List<string> townButtonText = refineText("그럼 이제 마을 안쪽을 한번 살펴 봅소다!\n");
    private List<string> townText = refineText("무사히 부락안으로 돌아왔기래 동무!\n나는 또 길을 잃을 까봐 걱정했슴메.\n보다시피 마을 외곽과 똑같이 마을 안쪽에도 여러 장소들이 있소이다.\n마을 외곽 처럼 여기서도 많은 사람들과 만나 얘기 해볼 수 있으니, 꼭 사람들이 나타날때마다 말을 걸어보는걸 추천할수다!\n흠, 이쯤 됬으면 통계원 동지와 아바이 동지가 서류 작업을 다 끝냈을 것소이다.\n마을 외곽의 논밭 에서 기다릴테니 마을과 마을의 음식 상황을 더 살펴보고 싶다면 마음 껏 보라우 쟝 동무!\n");

    public void ProceedTutorial()
    {
        if (currentTutorialIdx == allTexts.Count)
        {
            GameManager.instance.isTutorialPlayed = true;
            entireTutorial.SetActive(false);
            return;
        }
        SetActiveTutorial(GetTutNum(currentTutorialIdx));
        tutorials[GetTutNum(currentTutorialIdx)].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = allTexts[currentTutorialIdx];
        Debug.Log(allTexts[currentTutorialIdx] + " " + currentTutorialIdx + " " + GetTutNum(currentTutorialIdx));
        currentTutorialIdx++;
    }

    public void InitTexts()
    {
        allTexts.AddRange(plainText);
        allTexts.AddRange(resourceBarText);
        allTexts.AddRange(nextTurnText);
        allTexts.AddRange(foodAndPleasantText);
        allTexts.AddRange(outskirtsText);
        allTexts.AddRange(apText);
        allTexts.AddRange(turnAndAPText);
        allTexts.AddRange(eventText);
        allTexts.AddRange(townButtonText);
        allTexts.AddRange(townText);

        tutorialLengths.Add(plainText.Count);
        tutorialLengths.Add(resourceBarText.Count);
        tutorialLengths.Add(nextTurnText.Count);
        tutorialLengths.Add(foodAndPleasantText.Count);
        tutorialLengths.Add(outskirtsText.Count);
        tutorialLengths.Add(apText.Count);
        tutorialLengths.Add(turnAndAPText.Count);
        tutorialLengths.Add(eventText.Count);
        tutorialLengths.Add(townButtonText.Count);
        tutorialLengths.Add(townText.Count);
    }

    public int GetTutNum(int idx)
    {
        if (idx < 5)
            return 0;
        else if (idx < 6)
            return 1;
        else if (idx < 9)
            return 2;
        else if (idx < 12)
            return 3;
        else if (idx < 16)
            return 4;
        else if (idx < 19)
            return 5;
        else if (idx < 20)
            return 6;
        else if (idx < 23)
            return 7;
        else if (idx < 24) // asdf
        {
            clickDetector.SetActive(false);
            entireTutorial.transform.SetSiblingIndex(5);
            return 8;
        }
        else
        {
            clickDetector.SetActive(true);
            entireTutorial.transform.SetSiblingIndex(6);
            return 9;
        }
    }

    public static List<string> refineText(string str)
    {
        return str.Trim().Split('\n').Select(s => s.Trim()).ToList();
    }

    public void SetActiveTutorial(int idx)
    {
        foreach (GameObject go in tutorials)
            go.SetActive(false);
        tutorials[idx].SetActive(true);
    }
}
