using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class PlayerNameInput : MonoBehaviour
{
    UIInput mInput;
    bool mIgnoreNextEnter = false;

    private static string m_defaultText = null;

    /// <summary>
    /// Add some dummy text to the text list.
    /// </summary>

    void Start()
    {
        mInput = GetComponent<UIInput>();
        m_defaultText = mInput.text;
    }

    /// <summary>
    /// Pressing 'enter' should immediately give focus to the input field.
    /// </summary>

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (!mIgnoreNextEnter && !mInput.selected)
            {
                mInput.selected = true;
            }
            mIgnoreNextEnter = false;
        }

        if (mInput.text != m_defaultText && !string.IsNullOrEmpty(mInput.text) && mInput.text.Trim().Length >= 5)
        {
            var name = mInput.text;
            if (name.Length > 10)
            {
                name = name.Substring(0, 10);
                mInput.text = name;
            }
            GameOptions.Instance.SetPlayerName(name);
        }
    }

    /// <summary>
    /// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
    /// </summary>

    void OnSubmit()
    {
        mIgnoreNextEnter = true;
    }

    public static string GetPlayerName()
    {
        if (GameOptions.Instance.PlayerName == m_defaultText || string.IsNullOrEmpty(GameOptions.Instance.PlayerName))
        {
            return "Player_" + Random.Range(0, 100000);
        }

        return GameOptions.Instance.PlayerName;
    }
}