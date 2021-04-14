using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
[CustomEditor(typeof(SpriteBuilder))]
[ExecuteInEditMode]
public class SpriteBuilder : MonoBehaviour
{
    public Sprite m_sprTopperSprite;
    [SerializeField]
    public Sprite m_sprMainSprite;
    public Sprite m_sprTopLeft;
    public Sprite m_sprTopRight;
    public Sprite m_sprTopCore;
    public Sprite m_sprCoreLeft;
    public Sprite m_sprCoreRight;

    public GameObject tilePrefab;

    private float m_fPrevX;
    private float m_fPrevY;

    [SerializeField]
    private bool m_bReload;

    public void ChangeSize(Vector3 scale)
    {
        float fXSize = scale.x;
        float fYSize = scale.y;

        float fSpriteWidth = m_sprMainSprite.rect.x;
        float fSpriteHeight = m_sprMainSprite.rect.y;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Mathf.Ceil(fYSize); i++)
        {
            
            for(int j = 0; j < Mathf.Floor(fXSize); j++)
            {
                Sprite sprCurrent;

                if (j == 0)
                {
                    sprCurrent = i == Mathf.Ceil(fYSize) - 2 ? m_sprTopLeft : m_sprCoreLeft;
                }
                else if(j == Mathf.Floor(fXSize) - 1)
                {
                    sprCurrent = i == Mathf.Ceil(fYSize) - 2 ? m_sprTopRight : m_sprCoreRight;
                }
                else
                {
                    sprCurrent = i == Mathf.Ceil(fYSize) -2  ? m_sprTopCore : m_sprMainSprite;
                }
                if(i == Mathf.Ceil(fYSize) - 1)
                {
                    if (m_sprTopperSprite == null)
                    {
                        continue;
                    }
                    else
                    {
                        sprCurrent = m_sprTopperSprite;
                    }
                }

                if (sprCurrent == null) sprCurrent = m_sprMainSprite;

                GameObject newTile = Instantiate(tilePrefab, new Vector3(j, i), Quaternion.identity, transform);
                Vector3 newPos = new Vector3(j / transform.localScale.x, i / transform.localScale.y);
                newTile.transform.localPosition = newPos;
                newTile.GetComponent<SpriteRenderer>().sprite = sprCurrent;
                Vector3 newScale = new Vector3(newTile.transform.localScale.x / transform.localScale.x, newTile.transform.localScale.y / transform.localScale.y);
                newTile.transform.localScale = newScale;
            }
        }
    }

    private void Awake()
    {
        Vector3 v3CurrentScale = transform.localScale;
        m_fPrevX = v3CurrentScale.x;
        m_fPrevY = v3CurrentScale.y;
        m_bReload = false;
        ChangeSize(v3CurrentScale);
    }

    void Update()
    {
        Vector3 v3CurrentScale = transform.localScale;
        
        if(v3CurrentScale.x != m_fPrevX || v3CurrentScale.y != m_fPrevY || m_bReload)
        {
            ChangeSize(v3CurrentScale);
            m_fPrevX = v3CurrentScale.x;
            m_fPrevY = v3CurrentScale.y;
            m_bReload = false;

        }
    }
}

// Custom Editor the "old" way by modifying the script variables directly.
// No handling of multi-object editing, undo, and Prefab overrides!
[CustomEditor(typeof(SpriteBuilder))]
public class SpriteBuilderEditor : Editor
{
    
}
#endif