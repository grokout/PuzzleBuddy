using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIOptimizedList : MonoBehaviour
{
    private GameObject _markerPrefab;
    private List<OptiizedListData> _listData = new List<OptiizedListData>();
    private List<UIOptimizedListPrefab> _pool = new List<UIOptimizedListPrefab>();

    public Scrollbar scrollbar;

    

    private void Start()
    {
        scrollbar.onValueChanged.AddListener((value) =>
        {
            MoveTo(value);
        });

        StartCoroutine(RedisplayInAFraame());
    }

    IEnumerator RedisplayInAFraame()
    {
        yield return new WaitForSeconds(.1f);
        UpdateVisualList();
    }

    void Awake()
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

        CreatePool();
    }

    void CreatePool()
    {
        CreateMarkers(10);
    }

    public void CreateMarkers(int numMarkers = 1, bool autoResize = false, bool startActive = true)
    {
        if (_markerPrefab == null)
        {
            Init();
        }

        for (int x = 0; x < numMarkers; ++x)
        {
            UIOptimizedListPrefab obj = null;

            obj = Instantiate(_markerPrefab.gameObject).GetComponent<UIOptimizedListPrefab>();
            obj.name = _markerPrefab.name + " " + (transform.childCount).ToString();
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(startActive);
            obj.transform.localScale = new Vector3(1, 1, 1);

            _pool.Add(obj);
        }
    }

    public void ClearAll()
    {
        ClearMarkers();
        _listData.Clear();  
    }


    void ClearMarkers()
    {
        while (_pool.Count > 0)
        {
            _pool[0].gameObject.SetActive(true); ;
            _pool.RemoveAt(0);
        }

        for (int x = 1; x < transform.childCount; ++x)
        {
            _pool.Add(transform.GetChild(x).GetComponent<UIOptimizedListPrefab>());
            transform.GetChild(x).gameObject.SetActive(false);
        }
    }

    public void Add(OptiizedListData data)
    {
        _listData.Add(data);
        ResizeContainer();
        UpdateVisualList();
    }

    public void ResizeContainer()
    {
        float totalSizeY = 0;
        foreach (OPListPBPuzzleData data in _listData)
        {
            data.yPos = totalSizeY;
            totalSizeY += data.GetHeight();
        }


        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalSizeY);
    }


    void UpdateVisualList()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        // find out which ones should be visible
        float startY = 0;
        float endY = rectTransform.parent.GetComponent<RectTransform>().rect.height;


        float p = 1 - scrollbar.value;

        if (rectTransform.rect.height > endY)
        {
            float totalHeight = rectTransform.rect.height - endY;
            startY += totalHeight * p;
            endY += totalHeight * p;
        }

        foreach (OPListPBPuzzleData data in _listData)
        {
            if (data.yPos + data.GetHeight() >= startY && data.yPos < endY)
            {
                if (data.marker == null)
                {
                    data.marker = GetMarker();
                    data.marker.SetData(data);

                    Debug.Log("Create Marker at " + data.yPos);
                }
            }
            else if (data.marker != null)
            {
                _pool.Add(data.marker);
                data.marker.gameObject.SetActive(false);
                data.marker = null;
            }
        }
    }

    UIOptimizedListPrefab GetMarker()
    {
        if (_pool.Count == 0)
        {
            CreateMarkers(1);
        }

        UIOptimizedListPrefab marker = _pool[0];
        _pool.RemoveAt(0);
        marker.gameObject.SetActive(true);
        return marker;
    }

    void MoveTo(float perc)
    {
        UpdateVisualList();
    }
}


public class OptiizedListData
{
    public float yPos = 0;

    // recaclulate
    public virtual float GetHeight() 
    { 
        return 10; 
    }


}