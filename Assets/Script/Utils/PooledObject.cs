using UnityEngine;
using System;
using System.Collections;

public class PooledObject : MonoBehaviour
{

    private ObjectPooler m_Pool = null;
    private bool m_Allocated = false;

    public PooledObject(ObjectPooler inPool)
    {
        m_Pool = inPool;
    }

    public ObjectPooler pool
    {
        set { m_Pool = value; }
    }

    public void SetAllocated(bool inAllocated)
    {
        if (inAllocated == m_Allocated)
            throw new Exception("Double allocation");

        m_Allocated = inAllocated;
    }

    public void ReturnToPool()
    {
        if (m_Pool != null)
        {
            m_Pool.ReturnToPool(this);
        }
    }
}
