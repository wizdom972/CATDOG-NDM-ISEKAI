using ISEKAI_Model;
using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{

    public EventManager(EventCore eventCore) // when playing new event, this instance should be made.
    {
        currentEvent = eventCore;
        scriptEnumerator = eventCore.script.GetEnumerator();
    }

    public EventCore currentEvent { get; private set; } // when event SD is clicked and scene changed, it should be set to that event.

    public List<Command>.Enumerator scriptEnumerator;

    public void ExecuteOneScript()
    {
        // TODO:
        // when MoveNext() returns false, it must go back to TownScene.
        scriptEnumerator.MoveNext();
        _ExecuteCommand(scriptEnumerator.Current);
    }

    private void _ExecuteCommand(Command c)
    {
        switch (c.commandNumber)
        {
            case 0:
                _Explanation(c as Explanation);
                break;
            case 1:
                _Conversation(c as Conversation);
                break;
            case 2:
                _LoadCharacter(c as LoadCharacter);
                break;
            case 3:
                _UnloadCharacter(c as UnloadCharacter);
                break;
            case 4:
                _LoadBackground(c as LoadBackground);
                break;
            case 5:
                _PlayMusic(c as PlayMusic);
                break;
            case 6:
                _StopMusic(c as StopMusic);
                break;
            case 7:
                _LoadCG(c as LoadCG);
                break;
            case 8:
                _UnloadCG(c as UnloadCG);
                break;
            case 9:
                _VFXCamerashake(c as VFXCameraShake);
                break;
            case 10:
                _VFXLoadSprite(c as VFXLoadSprite);
                break;
            case 11:
                _VFXUnloadSprite(c as VFXUnloadSprite);
                break;
            case 12:
                _VFXSound(c as VFXSound);
                break;
            case 13:
                _LoadMinigame(c as LoadMinigame);
                break;
            case 14:
                _LoadVideo(c as LoadVideo);
                break;
            case 15:
                _Choice(c as Choice);
                break;
            case 16:
                break;
            case 17:
                _VFXTransition(c as VFXTransition);
                break;
            case 18:
                _VFXPause(c as VFXPause);
                break;
            default:
                throw new NotImplementedException("The command which holds that number is not implemented.");
        }
    }

    private void _Explanation(Explanation explanation)
    {

    }

    private void _Conversation(Conversation conversation)
    {

    }

    private void _LoadCharacter(LoadCharacter loadCharacter)
    {

    }

    private void _UnloadCharacter(UnloadCharacter unloadCharacter)
    {

    }

    private void _LoadBackground(LoadBackground loadBackground)
    {

    }

    private void _PlayMusic(PlayMusic playMusic)
    {

    }

    private void _StopMusic(StopMusic stopMusic)
    {

    }

    private void _LoadCG(LoadCG loadCG)
    {

    }

    private void _UnloadCG(UnloadCG unloadCG)
    {

    }

    private void _VFXCamerashake(VFXCameraShake vfxCameraShake)
    {

    }

    private void _VFXLoadSprite(VFXLoadSprite vfxLoadSprite)
    {

    }

    private void _VFXUnloadSprite(VFXUnloadSprite vfxUnloadSprite)
    {

    }

    private void _VFXSound(VFXSound vfxSound)
    {

    }

    private void _LoadMinigame(LoadMinigame loadMinigame)
    {

    }

    private void _LoadVideo(LoadVideo loadVideo)
    {

    }

    private void _Choice(Choice choice)
    {

    }

    private void _VFXTransition(VFXTransition vfxTransition)
    {

    }

    private void _VFXPause(VFXPause vfxPause)
    {

    }
}
