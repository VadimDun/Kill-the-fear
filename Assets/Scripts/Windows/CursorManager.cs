using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager instance;

    public static CursorManager Instance => instance;

    [SerializeField] private Texture2D menuCursorTexture;

    public Texture2D GetMenuCursorTexture => menuCursorTexture;

    [SerializeField] private Texture2D scopeCursorTexture;

    public Texture2D GetScopeCursorTexture => scopeCursorTexture;

    private Vector2 ScopeHotspot = new Vector2(13, 13);

    public Vector2 GetScopeHotspot => ScopeHotspot;

    private Vector2 MenuHotspot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    

    public void Awake()
    {
        Cursor.SetCursor(menuCursorTexture, MenuHotspot, cursorMode);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);


    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(menuCursorTexture, MenuHotspot, cursorMode);
    }

    public void SetScopeCursor()
    {
        Cursor.SetCursor(scopeCursorTexture, ScopeHotspot, cursorMode);
    }


}