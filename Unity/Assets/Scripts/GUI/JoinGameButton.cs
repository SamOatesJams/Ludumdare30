using UnityEngine;
using System.Collections;

public class JoinGameButton : UIButtonSound
{
    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
            var sprite = this.GetComponent<UISlicedSprite>();

            // Join the game here
            Application.LoadLevel("NetworkSandbox");
            NetworkManager.ActiveRoom = GameListManager.ActiveRoom.RoomName;
        }
    }
}
