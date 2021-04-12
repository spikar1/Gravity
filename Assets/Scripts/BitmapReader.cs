using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BitmapReader : MonoBehaviour {

    public Texture2D levelBitmap;
    SpriteRenderer spriteRenderer;

    public GameObject[] levelAssets;
    public TilePrefabSelector selector;

    private Color[] pixels;
    //GameObject[] levelBlocks = new GameObject[0];
    public List<GameObject> levelBlocks = new List<GameObject>();

    void Start()
    {
        
    }

    public void BuildLevel()
    {
        /*Transform[] objectsToDestroy = transform.GetComponentsInChildren<Transform>();
        foreach(var child in objectsToDestroy)
        {
            if (child.name == "Level Manager")
                print("hello");
            else
                DestroyImmediate(child.gameObject);
        }*/
        selector = GetComponent<TilePrefabSelector>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        print(levelBitmap.width);

        pixels = levelBitmap.GetPixels();

        foreach (var block in levelBlocks)
        {
            DestroyImmediate(block);
        }
        levelBlocks.Clear();
        //int blockid = 0;


        for (int y = 0; y < levelBitmap.height; y++)
        {
            for (int x = 0; x < levelBitmap.width; x++)
            {
                float coordinate = (y * levelBitmap.width) + x;

                if (Physics2D.OverlapCircle(new Vector2(x, y), .4f))
                {
                    DestroyImmediate(Physics2D.OverlapCircle(new Vector2(x, y), .4f).transform.gameObject);
                }

                GameObject createdBlock;
                for (int i = 0; i < selector.tiles.Length; i++)
                {
                    TilePrefabSelector.Tile tile = selector.tiles[i];
                    if (pixels[(y * levelBitmap.width) + x] == tile.color)
                    {

                        createdBlock = PrefabUtility.InstantiatePrefab(tile.gameObject) as GameObject;
                        createdBlock.transform.position = new Vector3(x, y, 0);
                        createdBlock.transform.rotation = Quaternion.identity;
                        //createdBlock = Instantiate(tile.gameObject, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                        levelBlocks.Add(createdBlock);
                        //createdBlock.name = "hallaballa_"+i;
                        //blockid ++;
                        if (!transform.Find(tile.groupName))
                        {

                            GameObject newGroup = new GameObject("rename me");
                            newGroup.name = tile.groupName;
                            newGroup.transform.SetParent(transform);

                        }
                        else
                        {
                            
                        }
                        createdBlock.transform.SetParent(transform.Find(tile.groupName));
                    }
                }

                //print(pixels[(y * levelBitmap.width) + x] * 255);
                /*if (pixels[(y * levelBitmap.width) + x] == Color.black)
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Wall"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    createdBlock.transform.SetParent(transform.FindChild("Walls"));
                }
                else if (pixels[(y * levelBitmap.width) + x] == Color.red)
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Trap"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    createdBlock.transform.SetParent(transform.FindChild("Traps"));
                }
                else if (pixels[(y * levelBitmap.width) + x] == Color.green)
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Misc"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    createdBlock.transform.SetParent(transform.FindChild("Misc"));
                }
                else if (pixels[(y * levelBitmap.width) + x] == Color.blue)
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Player"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                }
                else if (pixels[(y * levelBitmap.width) + x] == new Color(50f / 255, 100f / 255, 0))
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Green_Door"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                }
                else if (pixels[(y * levelBitmap.width) + x] == new Color(100f / 255, 150f / 255, 0))
                {
                    createdBlock = Instantiate(Resources.Load<GameObject>("TilemapPrefabs/Green_Key"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                }*/
            }
        }
    } 
    
}

[CustomEditor(typeof(BitmapReader))]
public class BitmapReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BitmapReader myScript = (BitmapReader)target;
        if (GUILayout.Button("BuildLevel"))
        {
            myScript.BuildLevel();
        }
    }
}
