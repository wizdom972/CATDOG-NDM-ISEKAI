using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace ISEKAI_Model
{
    public static class Parser
    {
        private static string[] _commandPattern =
             {@"^(\d?)\-?\-?(\d?) ?# ""(.*)""$", //0
             @"^(\d?)\-?\-?(\d?) ?## ""(.*)"" ""(.*)"" ?\-?(.*)?$", //1
             @"^(\d?)\-?\-?(\d?) ?Load Character ""(.*)"" \-(.*)$", //2
             @"^(\d?)\-?\-?(\d?) ?Unload Character \-(.*)$", //3
             @"^(\d?)\-?\-?(\d?) ?Load Background ""(.*)""$", //4
             @"^(\d?)\-?\-?(\d?) ?Play Music ""(.*)"" ?\-?(.*)?$", //5
             @"^(\d?)\-?\-?(\d?) ?Stop Music$", //6
             @"^(\d?)\-?\-?(\d?) ?Load CG ""(.*)""$", //7
             @"^(\d?)\-?\-?(\d?) ?Unload CG$", //8
             @"^(\d?)\-?\-?(\d?) ?VFX Camerashake$", //9
             @"^(\d?)\-?\-?(\d?) ?VFX Load Sprite ""(.*)"" \-(.*) \-(.*) ?\-?(.*)?", //10
             @"^(\d?)\-?\-?(\d?) ?VFX Unload Sprite$", //11
             @"^(\d?)\-?\-?(\d?) ?VFX Sound ""(.*)""$", //12
             @"^(\d?)\-?\-?(\d?) ?Load Minigame ""(.*)""$", //13
             @"^(\d?)\-?\-?(\d?) ?Load Video ""(.*)""$", //14
             @"^(\d?)\-?\-?(\d?) ?Choice$", //15
             @"-- ""(.*)""( \-(\w+) \(([\+\-\*])(\d+)\))*", // 16
             @"^(\d?)\-?\-?(\d?) ?VFX Transition$", //17
             @"^(\d?)\-?\-?(\d?) ?VFX Pause \-(.*)$"}; //18
        private static SpriteLocation _ParseSpriteLocation(string location)
        {
            if(location.Equals("left"))
                return SpriteLocation.Left;
            else if(location.Equals("center"))
                return SpriteLocation.Center;
            else if(location.Equals("right"))
                return SpriteLocation.Right;
            else
                return SpriteLocation.None;
        }
        private static ChoiceEffectKind _ParseChoiceEffectKind(string kind)
        {
            if(kind.Equals("Food"))
                return ChoiceEffectKind.Food;
            else if (kind.Equals("FoodP"))
                return ChoiceEffectKind.FoodP;
            else if (kind.Equals("Morale"))
                return ChoiceEffectKind.Morale;
            else if (kind.Equals("Horse"))
                return ChoiceEffectKind.Horse;
            else if (kind.Equals("HorseP"))
                return ChoiceEffectKind.HorseP;
            else if (kind.Equals("Steel"))
                return ChoiceEffectKind.Steel;
            else if (kind.Equals("SteelP"))
                return ChoiceEffectKind.SteelP;
            else
                return ChoiceEffectKind.None;
        }

        private static ChoiceEffectType _ParseChoiceEffectType(string type)
        {
            if (type.Equals("+"))
                return ChoiceEffectType.Add;
            else if (type.Equals("/"))
                return ChoiceEffectType.Divide;
            else if (type.Equals("-"))
                return ChoiceEffectType.Subtract;
            else if (type.Equals("*"))
                return ChoiceEffectType.Multiply;
            else
                return ChoiceEffectType.None;
        }
        public static void _SetChoiceDependency(Command command, string choiceNumber, string choiceResult)
        {
            if(choiceResult == "") return;
            else
            {
                int cNumber;
                int cResult = int.Parse(choiceResult);
                if (choiceNumber == "")
                    cNumber = -1;
                else
                    cNumber = int.Parse(choiceNumber);
                command.choiceDependency = (cNumber,cResult);
            }
        }
        public static List<Command> ParseScript(string scriptPath)
        {
            string[] commandList = File.ReadAllText(scriptPath).Trim().Split('\n');
            for (int i = 0; i < commandList.Length; i++)
                commandList[i] = commandList[i].Trim();
            
            List<Command> refinedList = new List<Command>();
            Match match;
            for (int i = 0; i < commandList.Length; i++)
            {
                string command = commandList[i];
                int choiceNumber = 0;
                if ((match = Regex.Match(command, _commandPattern[0])).Success)
                {
                    string contents = match.Groups[3].Value;
                    var explanation = new Explanation(contents);
                    _SetChoiceDependency(explanation, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(explanation);
                }
                else if ((match = Regex.Match(command, _commandPattern[1])).Success)
                {
                    string characterName = match.Groups[3].Value;
                    string contents = match.Groups[4].Value;
                    string brightCharacter = match.Groups[5].Value;
                    var conversation = new Conversation(characterName, contents, _ParseSpriteLocation(brightCharacter));
                    _SetChoiceDependency(conversation, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(conversation);
                }
                else if ((match = Regex.Match(command, _commandPattern[2])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    string location = match.Groups[4].Value;
                    var loadCharacter = new LoadCharacter(filePath, _ParseSpriteLocation(location));
                    _SetChoiceDependency(loadCharacter, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(loadCharacter);
                }
                else if ((match = Regex.Match(command, _commandPattern[3])).Success)
                {
                    string location = match.Groups[3].Value;
                    var unloadCharacter = new UnloadCharacter(_ParseSpriteLocation(location));
                    _SetChoiceDependency(unloadCharacter, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(unloadCharacter);
                }
                else if ((match = Regex.Match(command, _commandPattern[4])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    var loadBackground = new LoadBackground(filePath);
                    _SetChoiceDependency(loadBackground, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(loadBackground);
                }
                else if ((match = Regex.Match(command, _commandPattern[5])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    var playMusic = new PlayMusic(match.Groups[2].Value.Equals("r"), filePath);
                    _SetChoiceDependency(playMusic, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(playMusic);
                }
                else if ((match = Regex.Match(command, _commandPattern[6])).Success)
                {
                    var stopMusic = new StopMusic();
                    _SetChoiceDependency(stopMusic, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(stopMusic);
                }
                else if ((match = Regex.Match(command, _commandPattern[7])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    var loadCG = new LoadCG(filePath);
                    _SetChoiceDependency(loadCG, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(loadCG);
                }
                else if ((match = Regex.Match(command, _commandPattern[8])).Success)
                {
                    var unloadCG = new UnloadCG();
                    _SetChoiceDependency(unloadCG, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(unloadCG);
                }
                else if ((match = Regex.Match(command, _commandPattern[9])).Success)
                {
                    var vfxCameraShake = new VFXCameraShake();
                    _SetChoiceDependency(vfxCameraShake, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxCameraShake);
                }
                else if ((match = Regex.Match(command, _commandPattern[10])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    string width = match.Groups[4].Value;
                    string height = match.Groups[5].Value;
                    bool isGIF;
                    if (match.Groups[6].Value == "")
                        isGIF = false;
                    else
                        isGIF = true;
                    var vfxLoadSprite = new VFXLoadSprite(filePath, int.Parse(width), int.Parse(height), isGIF);
                    _SetChoiceDependency(vfxLoadSprite, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxLoadSprite);
                }
                else if ((match = Regex.Match(command, _commandPattern[11])).Success)
                {
                    var vfxUnloadSprite = new VFXUnloadSprite();
                    _SetChoiceDependency(vfxUnloadSprite, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxUnloadSprite);
                }
                else if ((match = Regex.Match(command, _commandPattern[12])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    var vfxSound = new VFXSound(filePath);
                    _SetChoiceDependency(vfxSound, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxSound);
                }
                else if ((match = Regex.Match(command, _commandPattern[13])).Success)
                {
                    string minigameName = match.Groups[3].Value;
                    var loadMinigame = new LoadMinigame(minigameName);
                    _SetChoiceDependency(loadMinigame, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(loadMinigame);
                }
                else if ((match = Regex.Match(command, _commandPattern[14])).Success)
                {
                    string filePath = match.Groups[3].Value;
                    var loadVideo = new LoadVideo(filePath);
                    _SetChoiceDependency(loadVideo, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(loadVideo);
                }
                else if ((match = Regex.Match(command, _commandPattern[15])).Success)
                {
                    List<ChoiceEffect> choiceList = new List<ChoiceEffect>(); // to put in Choice.
                    int choiceBranchNumber = 0;
                    while((match = Regex.Match(commandList[++i], _commandPattern[16])).Success)
                    {
                        List<(ChoiceEffectKind, ChoiceEffectType, float)> effectList = new List<(ChoiceEffectKind, ChoiceEffectType, float)>();
                        string choiceName = match.Groups[1].Value;
                        for (int _counter = 0; _counter < match.Groups[3].Captures.Count; _counter++)
                        {
                            var choiceEffectKind = _ParseChoiceEffectKind(match.Groups[3].Captures[_counter].Value.Trim());
                            var choiceEffectType = _ParseChoiceEffectType(match.Groups[4].Captures[_counter].Value.Trim());
                            float choiceEffectAmount = float.Parse(match.Groups[5].Captures[_counter].Value.Trim());
                            effectList.Add((choiceEffectKind, choiceEffectType, choiceEffectAmount));
                        }
                        var choiceEffect = new ChoiceEffect(choiceBranchNumber, choiceName, effectList);
                        choiceList.Add(choiceEffect);
                        choiceBranchNumber++;
                    }
                    var choice = new Choice(choiceNumber, choiceList);
                    refinedList.Add(choice);
                    choiceNumber++;
                }
                else if ((match = Regex.Match(command, _commandPattern[17])).Success)
                {
                    var vfxTransition = new VFXTransition();
                    _SetChoiceDependency(vfxTransition, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxTransition);
                }
                else if ((match = Regex.Match(command, _commandPattern[18])).Success)
                {
                    string second = match.Groups[3].Value;
                    var vfxPause = new VFXPause(int.Parse(second));
                    _SetChoiceDependency(vfxPause, match.Groups[1].Value, match.Groups[2].Value);
                    refinedList.Add(vfxPause);
                }
            }
            return refinedList;
        }
    }
}