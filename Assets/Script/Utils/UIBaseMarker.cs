using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;

public class UIBaseMarker : MonoBehaviour
{

    public enum MarkerType
    {
        None,
        Inventory,
        Skill,
        PassiveSkill,
        Currency
    }

    protected Image _iconImage;
    public int _id { get; protected set; }

    private bool _listenersActive = false;
    protected bool _canDrag = true;

    public virtual MarkerType GetMarkerType()
    {
        return MarkerType.None;
    }

    public void Init(int id)
    {
        _iconImage = UIUtils.GetImage(transform, "Image_Icon");
        if (_iconImage == null)
        {
         
        }
        else
        {
            _iconImage.gameObject.SetActive(false);
        }

        SetID(id);
        PostInit();
    }


    public virtual void SetID(int id)
    {
        _id = id;
    }

    public virtual void Refresh()
    {

    }

    protected virtual void PostInit()
    {

    }

    void OnEnable()
    {
        EnableEvents();
        UpdateWithCurrentValues();
    }

    protected virtual void UpdateWithCurrentValues()
    {

    }

    public void SetIcon(string iconName, string bundleName)
    {
        if (_iconImage == null)
        {
            Debug.LogError("Missing Icon Image " + name);
            return;
        }

        if (_iconImage != null && !string.IsNullOrEmpty(iconName))
        {
            _iconImage.gameObject.SetActive(true);
            //_iconimage.sprite = resources.load<sprite>(iconname);
        }
        else
        {            
            _iconImage.gameObject.SetActive(false);
        }        
    }

  

    public virtual bool CanDrag()
    {
        return _canDrag;
    }

    public virtual bool CanDropOnTo()
    {
        return true;
    }

    public void EnableEvents()
    {
        if (!_listenersActive)
        {
            AddListeners();
        }
    }

    public void DiableEvents()
    {
        if (_listenersActive)
        {
            RemoveListeners();
        }
    }

    protected virtual void AddListeners()
    {
        _listenersActive = true;
    }

    protected virtual void RemoveListeners()
    {
        _listenersActive = false;
    }

    public virtual bool ShouldShowToolTip()
    {
        return false;
    }


}
