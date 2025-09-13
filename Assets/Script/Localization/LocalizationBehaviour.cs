using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocalizationBehaviour : MonoBehaviour, ILocalizationBehaviourHandler
{
	[SerializeField]
	[LocalizationKey]
    private string _key;

	public string key
	{
		set
		{
			if (_key != value)
			{
				_key = value;

				UpdateLocalization();
			}
		}

		get
		{
			return _key;
		}
	}

	public abstract void UpdateTranslation(string translation);

	[ContextMenu("Update Localization")]
	public void UpdateLocalization()
	{
		UpdateTranslation(LocalizationManager.GetTranslation(_key));
	}

	protected virtual void OnEnable()
	{
		EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.LanguageChanged, OnUpdateLocalization);
		UpdateLocalization();
	}

	protected virtual void OnDisable()
	{
        EventMsgManager.instance.RemoveListener(EventMsgManager.GameEventIDs.LanguageChanged, OnUpdateLocalization);
	}

	void OnUpdateLocalization(EventMsgManager.GameEventArgs args)
    {
		UpdateLocalization();
	}

#if UNITY_EDITOR
	protected virtual void OnValidate()
	{
		if (isActiveAndEnabled == true)
		{
			UpdateLocalization();
		}
	}
#endif
}
