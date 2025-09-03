using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnumArray<T>  : IEnumerable
{
    public readonly string[] enumNames;

#if DEBUG
    private Type _enumType;
#endif

    [SerializeField]
    private T[] _data;

    public EnumArray(Type t)
    {
#if !DEBUG
        string[] enumNames;
#endif
        enumNames = Enum.GetNames(t);
        _data = new T[enumNames.Length];

#if DEBUG
        if (! t.IsEnum)
        {
            Debug.LogError("Type " + t + " is not an enum");
        }
        _enumType = t;
#endif

    }

    public EnumArray(Type t, T[] data)
    {
#if DEBUG
        if (data.Length != enumNames.Length)
        {
            Debug.LogError("Mismatch length");
        }
#endif

        for (int i = 0;i < data.Length;++i)
        {
            _data[i] = data[i];
        }
    }

    public T this[int index]
    {
        get { return _data[index]; }
        set { _data[index] = value; }
    }

    public T this[object obj]
    {
        get
        {
#if DEBUG
            if ( obj.GetType() != _enumType)
            {
                Debug.LogError(obj + " is not of type " + _enumType);
            }
#endif
            return _data[Convert.ToInt32(obj)];
        }
        set
        {
#if DEBUG
            if (obj.GetType() != _enumType)
            {
                Debug.LogError(obj + " is not of type " + _enumType);
            }
#endif
            _data[Convert.ToInt32(obj)] = value;
        }
    }

    public int Length {  get { return _data.Length; } }

    public IEnumerator GetEnumerator()
    {
        return _data.GetEnumerator();
    }
}

[System.Serializable]
public class EnumeratedGameObject : EnumArray<GameObject>
{
    public EnumeratedGameObject(Type t) : base(t) { }
}


[System.Serializable]
public class EnumeratedTexture2D : EnumArray<Texture2D>
{
    public EnumeratedTexture2D(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedInt : EnumArray<int>
{
    public EnumeratedInt(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedFloat : EnumArray<float>
{
    public EnumeratedFloat(Type t) : base(t) { }
}


[System.Serializable]
public class EnumeratedBool : EnumArray<bool>
{
    public EnumeratedBool(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedSprite : EnumArray<Sprite>
{
    public EnumeratedSprite(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedSpriteRenderer : EnumArray<SpriteRenderer>
{
    public EnumeratedSpriteRenderer(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedUIListController : EnumArray<UIListController>
{
	public EnumeratedUIListController(Type t) : base(t) { }
}


[System.Serializable]
public class EnumeratedButton : EnumArray<Button>
{
    public EnumeratedButton(Type t) : base(t) { }
}


[System.Serializable]
public class EnumeratedArrayVector2 : EnumArray<Vector2>
{
    public EnumeratedArrayVector2(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedColor : EnumArray<Color>
{
    public EnumeratedColor(Type t) : base(t) { }
}

[System.Serializable]
public class EnumeratedImage : EnumArray<Image>
{
	public EnumeratedImage(Type t) : base(t) { }
}

