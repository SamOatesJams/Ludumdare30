using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ServerNameInput : MonoBehaviour
{
	UIInput mInput;
	bool mIgnoreNextEnter = false;

    public GameObject CreateButton = null;
    public Color ActiveButtonColor;
    public Color DisabledButtonColor;

    public static string GameName { get; private set; }

    private string m_defaultText = null;

	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>

	void Start ()
	{
		mInput = GetComponent<UIInput>();
        m_defaultText = mInput.text;
	}

	/// <summary>
	/// Pressing 'enter' should immediately give focus to the input field.
	/// </summary>

	void Update ()
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
            ServerNameInput.GameName = mInput.text;
            EnableCreateButton(true);
        }
        else
        {
            EnableCreateButton(false);
        }
	}

	/// <summary>
	/// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>

	void OnSubmit ()
	{
		mIgnoreNextEnter = true;
	}

    private void EnableCreateButton(bool enable)
    {
        this.CreateButton.GetComponent<UIButtonTween>().enabled = enable;
        this.CreateButton.GetComponent<UIButtonColor>().enabled = enable;
        this.CreateButton.GetComponent<UIButtonPlayAnimation>().enabled = enable;
        this.CreateButton.GetComponentInChildren<UISlicedSprite>().color = enable ? ActiveButtonColor : DisabledButtonColor;
    }
}