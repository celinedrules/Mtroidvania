using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool isPersistent;

    public bool IsPersistent
    {
        set
        {
            isPersistent = value;
            SetPersistence();
        }
    }

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindFirstObjectByType<T>();

            if (_instance != null)
                return _instance;

            GameObject newGo = new GameObject
            {
                name = typeof(T).ToString()
            };

            _instance = newGo.AddComponent<T>();

            return _instance;
        }
        protected set => _instance = value;
    }

    protected virtual void Awake()
    {
        if (isPersistent)
            SetPersistence();

        if (_instance == null)
            _instance = this as T;
        else
            Destroy(transform.parent == null ? gameObject : transform.root.gameObject);
    }

    private void SetPersistence () =>
        DontDestroyOnLoad(transform.parent == null ? gameObject : transform.root.gameObject);
}
