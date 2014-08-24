using UnityEngine;
using System.Collections;

public class CreateGameButton : UIButtonSound
{
    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick && !string.IsNullOrEmpty(ServerNameInput.GameName))
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
            var sprite = this.GetComponent<UISlicedSprite>();

            // Create game
            Application.LoadLevel("NetworkSandbox");
            NetworkManager.ActiveRoom = ServerNameInput.GameName;
        }
    }
}
