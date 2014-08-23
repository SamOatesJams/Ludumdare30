using UnityEngine;
using System.Collections;

public class GameEntryButton : UIButtonSound 
{
    public GameObject JoinGameButton = null;
    public Color ActiveButtonColor;
    public Color DisabledButtonColor;

    public string RoomName = null;

    public bool IsActive { get; private set; }
    
    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);

            IsActive = !IsActive;

            this.GetComponent<UISlicedSprite>().color = IsActive ? Color.white : Color.black;
            this.JoinGameButton.GetComponent<UIButtonTween>().enabled = IsActive;
            this.JoinGameButton.GetComponent<UIButtonColor>().enabled = IsActive;
            this.JoinGameButton.GetComponent<UIButtonPlayAnimation>().enabled = IsActive;
            this.JoinGameButton.GetComponentInChildren<UISlicedSprite>().color = IsActive ? ActiveButtonColor : DisabledButtonColor;

            if (IsActive)
            {
                if (GameListManager.ActiveRoom != this)
                {
                    if (GameListManager.ActiveRoom != null)
                    {
                        GameListManager.ActiveRoom.GetComponent<UISlicedSprite>().color = Color.black;
                    }                    
                    GameListManager.ActiveRoom = this;
                }                
            }
            else if (GameListManager.ActiveRoom == this)
            {
                GameListManager.ActiveRoom = null;
            }
        } 
    }
}
