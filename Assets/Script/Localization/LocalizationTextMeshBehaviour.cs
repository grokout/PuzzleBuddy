using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationTextMeshBehaviour : LocalizationBehaviour
{
	[Tooltip("If PhraseName couldn't be found, this text will be used.")]
	public string fallbackText;

	// This gets called every time the translation needs updating
	public override void UpdateTranslation(string translation)
	{
		// Get the Text component attached to this GameObject
		TMP_Text text = GetComponent<TMP_Text>();

		// Use translation?
		if (!string.IsNullOrEmpty(translation))
		{
			text.text = translation;// LeanTranslation.FormatText(translation, text.text, this, gameObject);
		}
		// Use fallback?
		else
		{
			text.text = fallbackText;// LeanTranslation.FormatText(fallbackText, text.text, this, gameObject);
		}
	}

	protected virtual void Awake()
	{
		// Should we set FallbackText?
		if (string.IsNullOrEmpty(fallbackText) == true)
		{
			// Get the Text component attached to this GameObject
			var text = GetComponent<TMP_Text>();

			if (text == null)
            {
				Debug.LogError("Missing tmp text on " + name);
				return;
            }
			// Copy current text to fallback
			fallbackText = text.text;
		}
	}
}
