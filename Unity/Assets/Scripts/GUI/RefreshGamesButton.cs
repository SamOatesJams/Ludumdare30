using UnityEngine;
using System.Collections;

public class RefreshGamesButton : UIButtonSound
{
    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
            var sprite = this.GetComponent<UISlicedSprite>();

            // Refresh games list
            GameListManager.Instance.RefreshGameList();
        }
    }
}
