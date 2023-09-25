using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

#region GameManager Events
public class GetBodyColorEvent : SDD.Events.Event
{

}
public class GetTailEvent : SDD.Events.Event
{
}
public class InitLevelsEvent : SDD.Events.Event
{
}
public class StartLevelEvent : SDD.Events.Event
{
}
public class StartMenuEvent : SDD.Events.Event
{
}
public class PlayMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}
public class HasSelectedMapEvent : SDD.Events.Event
{
    public int scene;
    public HasSelectedMapEvent(int scene)
    {
        this.scene = scene;
    }
}
public class WinMenuEvent : SDD.Events.Event
{
}
public class ScoreLevelMenuEvent : SDD.Events.Event
{
}
public class LoseMenuEvent : SDD.Events.Event
{
}
#endregion

#region MenuManager Events
public class ChangeTailColorEvent : SDD.Events.Event
{
    public Color color;
    public ChangeTailColorEvent(Color color)
    {
        this.color = color;
    }
}

public class ChangeBodyColorEvent : SDD.Events.Event
{
    public Color color;
    public ChangeBodyColorEvent(Color color)
    {
        this.color = color;
    }
}
public class ChangeTailEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ShopButtonClickedEvent : SDD.Events.Event
{
}
public class StartMenuButtonClickedEvent : SDD.Events.Event
{
}
public class PlayMenuButtonClickedEvent : SDD.Events.Event
{
}
public class QuitButtonClickedEvent : SDD.Events.Event
{
}
public class ShowScoreFinishedEvent : SDD.Events.Event
{
}
public class ShowTextScoreEvent : SDD.Events.Event
{
}

public class ShowTextScoreRecapEvent : SDD.Events.Event
{
}
#endregion

# region PauseManager Event
public class PauseButtonClickedEvent : SDD.Events.Event
{
}
public class PauseQuitButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region MapManager Event
public class SelectMapEvent : SDD.Events.Event
{
}
public class ChangeMapNameEvent : SDD.Events.Event
{
    public string name;
    public ChangeMapNameEvent(string name)
    {
        this.name = name;
    }
}
public class GiveMapCountEvent : SDD.Events.Event
{
    public int count;
    public GiveMapCountEvent(int count)
    {
        this.count = count;
    }

}
public class ChangeMapImageEvent : SDD.Events.Event
{
    public Sprite image;
    public ChangeMapImageEvent(Sprite image)
    {
        this.image = image;
    }
}
public class InitSelectMapEvent : SDD.Events.Event
{
}
#endregion

#region LevelManager Event
public class ShowLevelEvent : SDD.Events.Event
{
    public string level;
    public ShowLevelEvent(string level)
    {
        this.level = level;
    }
}
public class GetScoreEvent : SDD.Events.Event
{
}
public class ScoreLevelEvent : SDD.Events.Event
{
}
public class FinishLevelEvent : SDD.Events.Event
{
}
public class WinLevelEvent : SDD.Events.Event
{
}
public class StartBeginTimerEvent : SDD.Events.Event
{
}
public class LoseLevelEvent : SDD.Events.Event
{
}
public class StartGameTimerEvent : SDD.Events.Event
{
    public float timer;
    public StartGameTimerEvent(float timer)
    {
        this.timer = timer;
    }
}
#endregion


#region BeginTimer Event
public class FinishBeginTimerEvent : SDD.Events.Event
{
}
#endregion

#region GameTimer Event
public class FinishGameTimerEvent : SDD.Events.Event
{
}
public class GiveScoreEvent : SDD.Events.Event
{
    public float score;
    public GiveScoreEvent(float score)
    {
        this.score = score;
    }
}
#endregion

#region PlayerManager Event
public class PlayerLevelStartEvent : SDD.Events.Event
{
}
public class PlayerLevelEndEvent : SDD.Events.Event
{
}
public class PlayerSpawnEvent : SDD.Events.Event
{
}
#endregion

#region DeadZone Event
public class DeadLevelEvent : SDD.Events.Event
{
}
public class RespawnObjectEvent : SDD.Events.Event
{
    public GameObject respawnObject;
    public RespawnObjectEvent(GameObject respawnObject)
    {
        this.respawnObject = respawnObject;
    }
}
#endregion

public class NewCheckPointEvent : SDD.Events.Event
{
    public List<Vector3> spawns;
    public int zone;
    public NewCheckPointEvent(List<Vector3>spawns,int zone)
    {
        this.spawns = spawns;
        this.zone = zone;
    }
}

#region SkinManager Event
public class GiveTailEvent : SDD.Events.Event
{
    public bool tailVisible;
    public Color color;
    public GiveTailEvent(bool tailVisible, Color color)
    {
        this.tailVisible = tailVisible;
        this.color = color;
    }
}

public class GiveBodyColorEvent : SDD.Events.Event
{
    public Color color;
    public GiveBodyColorEvent(Color color)
    {
        this.color = color;
    }
}
#endregion

#region Drawing Event
public class ChangeDrawingEvent : SDD.Events.Event
{
}
public class InitDrawingEvent : SDD.Events.Event
{
}

public class ShowDrawingEvent : SDD.Events.Event
{
    public int drawing;
    public int drawingMax;
    public ShowDrawingEvent(int drawing, int drawingMax)
    {
        this.drawing = drawing;
        this.drawingMax = drawingMax;
    }
}
#endregion

#region Ring Event
public class SpeedPlayerEvent : SDD.Events.Event
{ 
    public float time;
    public float boostSpeed;
    public SpeedPlayerEvent(float time, float boostSpeed)
    {
        this.time = time;
        this.boostSpeed = boostSpeed;
    }
}
#endregion

#region Falling Ground Event
public class DestroyGroundEvent : SDD.Events.Event
{
    public GameObject ground;
    public DestroyGroundEvent(GameObject ground)
    {
        this.ground = ground;
    }
}
#endregion

#region Select Mode Event
public class SelectModeEvent : SDD.Events.Event
{
}
public class AddSelectMapEvent : SDD.Events.Event
{
    public int mapInd;
    public AddSelectMapEvent(int mapInd)
    {
        this.mapInd = mapInd;
    }
}
public class RemoveSelectMapEvent : SDD.Events.Event
{
    public int mapInd;
    public RemoveSelectMapEvent(int mapInd)
    {
        this.mapInd = mapInd;
    }
}
#endregion

#region Best Score Event

public class ShowBestScoreEvent : SDD.Events.Event
{
}

#endregion