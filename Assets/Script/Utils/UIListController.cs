using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIListController : MonoBehaviour
{
    /// This grabs the first object to use as the prefab
    private GameObject _markerPrefab;
    public float defaultHeight = 100;

    public bool variableSize = false;
    public List<Color> backgroundColors = new List<Color>();

    private List<GameObject> _pool = new List<GameObject>();

	// Use this for initialization
	void Awake ()
    {
        Init();
    }
    void Init()
    {
        if (_markerPrefab != null)
        {
            return;
        }
        Transform markerTransform = transform.GetChild(0);
        
        if (markerTransform == null)
        {
            Debug.LogError("Missing prefab for list " + name);
        }
        else
        {
            _markerPrefab = markerTransform.gameObject;
            _markerPrefab.SetActive(false);
            UIBaseMarker marker = _markerPrefab.GetComponent<UIBaseMarker>();
            if (marker != null)
            {
                marker.DiableEvents();
            }
        }
	}

    public T CreateMarker<T>()
    {
        CreateMarkers(1, false, true);
        // Find first free
        int index = 0;
        for (int x = 1; x < transform.childCount;++x)
        {            
            if (!transform.GetChild(x).gameObject.activeSelf)
            {                   
                break;
            }
            index = x - 1;
        }
                
            
        return GetMarker<T>(index);
    }

    public void CreateMarkers(int numMarkers = 1, bool autoResize = false, bool startActive = true)
    {
        if (_markerPrefab == null)
        {
            Init();
        }        
        
        for (int x = 0;x < numMarkers;++x)
        {
            GameObject obj = null;
            if (_pool.Count > 0)
            {
                obj = _pool[0];
                _pool.RemoveAt(0);
            }
            else
            {
                obj = Instantiate(_markerPrefab);
                obj.name = _markerPrefab.name + " " + (transform.childCount).ToString();
            }
            obj.transform.SetParent(transform);
            obj.SetActive(startActive);
            obj.transform.localScale = new Vector3(1, 1, 1);

            UIBaseMarker baseMarker = obj.GetComponent<UIBaseMarker>();
            if ( baseMarker != null)
            {
                baseMarker.EnableEvents();
                baseMarker.Init(x);
                baseMarker.name = _markerPrefab.name + " " + x.ToString();
            }
        }

        if ( autoResize)
        {
            ResizeContainer();
        }
    }
    
    public int NumVisible()
    {
        int numMarkers = 0;
        for (int x = 1; x < transform.childCount; ++x)
        {
            if (transform.GetChild(x).gameObject.activeSelf)
            {
                ++numMarkers;
            }
        }

        return numMarkers;
    }
    
    public void SetBackgroundColors()
    {
        if (backgroundColors.Count <= 0)
        {
            return;
        }

        int i = 0;
        for (int x = 1; x < transform.childCount; ++x)
        {
            if (transform.GetChild(x).gameObject.activeSelf)
            {
                Image image = transform.GetChild(x).gameObject.GetComponent<Image>();
                if (image != null)
                {
                    image.color = backgroundColors[i % backgroundColors.Count];
                    ++i;                    
                }
            }
        }
    }

    public void ResizeContainer()
    {
        // Get number of visible markers
        int numMarkers = NumVisible();
        float totalSize = defaultHeight * numMarkers;
        // Some lists may not have a standard size. In that case we need to go through each item
        if (variableSize)
        {
            totalSize = 0;
            for (int x = 1; x < transform.childCount; ++x)
            {
                if (transform.GetChild(x).gameObject.activeSelf)
                {
                    RectTransform childRect = transform.GetChild(x).GetComponent<RectTransform>();
                    totalSize += childRect.sizeDelta.y;
                }
            }

            VerticalLayoutGroup layoutgroup = GetComponent<VerticalLayoutGroup>();
            if (layoutgroup != null)
            {
                totalSize += layoutgroup.spacing * numMarkers;
            }
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        float sizeY = totalSize;
        if (sizeY < 0)
        {
            sizeY = 0;
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sizeY);
        //rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, 0);
    }

    public T GetMarker<T>(int index)
    {
        if ( transform.childCount - 1 > index)
        {
            GameObject obj = transform.GetChild(index + 1).gameObject;
            T comp = obj.GetComponent<T>();
            return comp;
        }

        return default(T);
    }

    public int Count
    {
        get { return transform.childCount - 1 - _pool.Count; }
    }

    public void ClearAll()
    {
        while (_pool.Count > 0)
        {
            _pool[0].SetActive(true); ;
            _pool.RemoveAt(0);            
        }

        for (int x = 1; x < transform.childCount;++x)
        {
            _pool.Add(transform.GetChild(x).gameObject);
            transform.GetChild(x).gameObject.SetActive(false);
        }
    }

    public UIBaseMarker FindMarkerUnderMouse()
    {
        for (int x = 1; x < transform.childCount; ++x)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            { 
            }
        }

        return null;
    }

    public void RemoveAt(int index)
    {
        int i = 0;
        for (int x = 1; x < transform.childCount; ++x)
        {
            if (transform.GetChild(x).gameObject.activeSelf)
            {
                if (i == index)
                {
                    _pool.Add(transform.GetChild(x).gameObject);
                    transform.GetChild(x).gameObject.SetActive(false);
                    break;
                }
                i++;
            }
        }
    }
}
