﻿using UnityEngine;
using System.Collections;

public class JoinGameButton : UIButtonSound
{
    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick && GameListManager.Instance.ActiveRoom != null)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
            var sprite = this.GetComponent<UISlicedSprite>();

            // Join the game here
            NetworkManager.JoinGame(GameListManager.Instance.ActiveRoom.RoomName);
        }
    }
}
