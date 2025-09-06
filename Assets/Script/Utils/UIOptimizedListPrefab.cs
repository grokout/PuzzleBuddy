using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptimizedListPrefab : MonoBehaviour
{

    private OPListPBPuzzleData _data;

    

    public virtual void SetData(OPListPBPuzzleData data)
    {
        _data = data;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -_data.yPos);
        float w = rectTransform.parent.GetComponent<RectTransform>().rect.width;
        rectTransform.sizeDelta = new Vector2(w, data.GetHeight());
    }
}
