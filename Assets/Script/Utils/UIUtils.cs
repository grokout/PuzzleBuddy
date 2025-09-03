using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

static public class UIUtils
{
    public static Image GetImage(Transform current, string name)
    {
        GameObject imageObj = Utils.SearchForChildObj(current, name);
        if (imageObj != null)
        {
            return imageObj.GetComponent<Image>();
        }
        return null;
    }

    public static List<RaycastResult> RaycastMouse()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        Debug.Log(results.Count);

        return results;
    }

    public static UIBaseMarker RaycastMouseOverMarker()
    {
        List<RaycastResult> hits = UIUtils.RaycastMouse();

        UIBaseMarker marker;
        for (int x = 0;x < hits.Count;++x)
        {
            marker = hits[x].gameObject.GetComponent<UIBaseMarker>();
            if (marker != null)
            {
                return marker;
            }
        }

        return null;
    }


}
