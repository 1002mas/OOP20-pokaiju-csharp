using Optional;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Castorina.Npc;

public interface INpcSimple
{
    /// <summary>
    /// This function returns npc name.
    /// </summary>
    /// <returns>npc name</returns>
    string GetName();


    /// <summary>
    /// This function returns npc statement if it is a talking one.
    /// </summary>
    /// <returns>a string Option containing a sentence if is a talking one, an
    /// Option.None otherwise</returns>        
    Option<string> InteractWith();

    /// <summary>
    ///This function returns a triggered event.
    /// </summary>
    ///<returns>the event if any is triggered, Option.None otherwise</returns>
    Option<IGameEvent> GetTriggeredEvent();


    /// <summary>
    /// This function changes the interactWith string value if the npc has more than
    /// one sentence and it is active
    /// </summary>
    /// <param name="textId"></param>
    void SetDialogueText(int textId);

    ///<summary>
    /// This function returns the type of npc.
    /// </summary>
    /// <returns>npc type</returns>        
    TypeOfNpc GetTypeOfNpc();


    ///<summary>
    ///This function returns npc position.
    /// </summary>
    /// <returns>NpcPosition</returns>      
    Tuple<int, int> GetPosition();


    ///<summary>
    /// This function changes the npc position in the map.
    ///</summary>
    ///<param name="newPosition"></param>
    void ChangeNpcPosition(Tuple<int, int> newPosition);


    /// <summary>
    /// This function returns if the npc is visible.
    /// </summary>
    /// <returns>true if the npc is visible, false otherwise</returns>       
    bool IsVisible();

    /// <summary>
    /// This function set whether the npc must be visible or not.
    /// </summary>
    /// <param name="visible"></param>
    void SetVisible(bool visible);

    /// <summary>
    /// This function returns if the npc is enabled to interact with player.
    /// </summary>
    /// <returns>true if player can interact with, false otherwise</returns>        
    bool IsEnabled();

    /// <summary>
    /// This function set whether player can interact with the npc or not.
    /// </summary>
    /// <param name="enabled"></param>       
    void SetEnabled(bool enabled);

    ///<summary>
    ///This function adds an event that may trigger interacting with the player.
    /// </summary>
    /// <param name="gameEvent"></param>
    void AddGameEvent(IGameEvent gameEvent);


    ///<summary>
    ///This function returns a list of game events triggered by interacting with the npc.
    ///</summary>
    ///<returns>npc game events</returns>

    IList<IGameEvent> GetGameEvents();


    /// <summary>
    /// This function returns index of the current sentence.
    /// </summary>
    /// <returns>index current sentence</returns>       
    int GetCurrentSetence();
}

