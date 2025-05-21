// Singleton pattern: http://wiki.unity3d.com/index.php/Singleton
// Singleton pattern with lazy initialization that avoids FindObjectsByType during construction
using UnityEngine;

public class RCC_Singleton<T> : RCC_Core where T : RCC_Core
{

    private static T m_Instance;
    private static bool m_Initialized = false;
    private static readonly object m_Lock = new object();

    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance != null)
                    return m_Instance;

                if (!Application.isPlaying)
                    return null;

                // Only search for existing instance during runtime
                if (!m_Initialized)
                {
                    // Search for existing instance.
                    m_Instance = FindFirstObjectByType<T>();
                    m_Initialized = true;

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                    }
                }

                return m_Instance;
            }
        }
    }

    // Alternative accessor that doesn't use FindObjectsByType and is safe during construction
    public static T GetInstanceWithoutSearch()
    {
        lock (m_Lock)
        {
            return m_Instance;
        }
    }

    // Call this from Awake() or Start() to initialize the singleton properly
    protected virtual void InitializeSingleton()
    {
        lock (m_Lock)
        {
            if (!m_Initialized)
            {
                if (m_Instance == null || m_Instance == this as T)
                {
                    m_Instance = this as T;
                    m_Initialized = true;
                }
                else
                {
                    // Another instance exists, destroy this one
                    Destroy(this);
                }
            }
            else if (m_Instance != this)
            {
                // This is a duplicate, destroy it
                Destroy(this);
            }
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }
}