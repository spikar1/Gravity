using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdditiveLevelLoader : SerializedMonoBehaviour
{
    public static int screenWidth = 832, screenHeight = 640;
    public static int screenWidthMeters => screenWidth / 32;
    public static int screenHeightMeters => screenHeight / 32;
    Player player;

    [SerializeField, TableMatrix]
    List<Level> levels = new();

    [SerializeField]
    int currentLevelIndex = 0;

    Level CurrentLevel => levels[currentLevelIndex];

    [AssetsOnly, SerializeField]
    GameObject minimapRoomPrefab;
    [SerializeField]
    GameObject minimapCanvas;

    [SerializeField, TableMatrix]
    int[,] lookupTable;

    Level levelPlayerIsIn;

    [SerializeField]
    Camera_FollowPlayer playerCamera;

    [ShowInInspector]
    List<string> loadedLevels = new List<string>();

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        playerCamera = FindAnyObjectByType<Camera_FollowPlayer>();
        BuildLookupTable();

        levelPlayerIsIn = GetLevelFromPosition(player.transform.position);
        LoadSceneAdditive(levelPlayerIsIn);
        ChangeSceneTo(levelPlayerIsIn);

    }

    [Button]
    private void BuildLookupTable()
    {
        var lookupTableSizeX = 0;
        var lookupTableSizeY = 0;
        foreach (var level in levels)
        {
            if(level.horizontalOffset + level.levelWidth > lookupTableSizeX)
                lookupTableSizeX = level.horizontalOffset + level.levelWidth;
            if(level.verticalOffset + level.levelHeight > lookupTableSizeY)
                lookupTableSizeY = level.verticalOffset + level.levelHeight;
        }
        lookupTable = new int[lookupTableSizeX, lookupTableSizeY];

        for (int x = 0; x < lookupTableSizeX; x++)
            for (int y = 0; y < lookupTableSizeY; y++)
                lookupTable[x, y] = -1;
        

        for (int i = 0; i < levels.Count; i++)
        {
            Level level = levels[i];
            for (int x = 0; x < level.levelWidth; x++)
            {
                for (int y = 0; y < level.levelHeight; y++)
                {
                    lookupTable[x + level.horizontalOffset, y + level.verticalOffset] = i;
                }
            }
        }
    }

    private void BuildMinimap()
    {
        foreach (Transform child in minimapCanvas.transform)
            Destroy(child.gameObject);

        foreach (var level in levels)
        {
            var mapTile = Instantiate(minimapRoomPrefab, minimapCanvas.transform);
            var tileImage = mapTile.GetComponent<Image>();

            Vector2 levelSize = new(level.levelWidth * 32 + 1, level.levelHeight * 32 + 1);
            Vector2 levelCenter = new((level.horizontalOffset * 32) + levelSize.x / 2f - (32 / 2), (level.verticalOffset * 32) + levelSize.y / 2f - (32 / 2));
            levelCenter -= CurrentLevel.Offset * 32;

            tileImage.rectTransform.sizeDelta = levelSize;
            tileImage.rectTransform.anchoredPosition = levelCenter;

            tileImage.color = level == CurrentLevel ? Color.white : Color.grey;
        }
    }


    private void Update()
    {
        if(GetLevelFromPosition(player.transform.position) != levelPlayerIsIn)
        {
            levelPlayerIsIn = GetLevelFromPosition(player.transform.position);
            ChangeSceneTo(GetLevelFromPosition(player.transform.position));
        }
    }

    private void ChangeSceneTo(Level level)
    {
        currentLevelIndex = levels.IndexOf(level);

        for (int i = loadedLevels.Count - 1; i >= 0 ; i--)
        {
            string loadedLevel = loadedLevels[i];
            if (loadedLevel == level.name)
                continue;
            if (level.adjacentLevels.Contains(loadedLevel))
                continue;

            SceneManager.UnloadSceneAsync(loadedLevel);
            loadedLevels.Remove(loadedLevel);
        }

        BuildMinimap();

        int minX = level.horizontalOffset * screenWidthMeters;
        int maxX = minX + (level.levelWidth - 1) * screenWidthMeters;
        int minY = level.verticalOffset * screenHeightMeters;
        int maxY = minY + (level.levelHeight - 1) * screenHeightMeters;

        playerCamera.InitCamera(minX, maxX, minY, maxY);

        LoadAdjacent();
    }

    public Level GetLevelFromPosition(Vector2 position)
    {
        return levels[lookupTable[Mathf.RoundToInt(position.x / screenWidthMeters), Mathf.RoundToInt(position.y / screenHeightMeters)]];
    }

    private void LoadAdjacent()
    {
        foreach (var adjacent in CurrentLevel.adjacentLevels)
        {
            if (loadedLevels.Contains(adjacent))
                continue;
            SceneManager.LoadScene(adjacent, LoadSceneMode.Additive);
            loadedLevels.Add(adjacent);
        }
    }

    void LoadSceneAdditive(Level level)
    {
        if (loadedLevels.Contains(level.name))
            return;
        SceneManager.LoadScene(level.name, LoadSceneMode.Additive);
        loadedLevels.Add(level.name);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            Level level = levels[i];
            UnityEngine.Random.InitState(i);
            Gizmos.color = UnityEngine.Random.ColorHSV(0, 1, .5f, 1, .5f, 1);
            Vector2 levelSize = new(level.levelWidth * screenWidthMeters, level.levelHeight * screenHeightMeters);
            Vector2 levelCenter = new((level.horizontalOffset * screenWidthMeters) + levelSize.x / 2f -(screenWidthMeters / 2), (level.verticalOffset * screenHeightMeters) + levelSize.y / 2f - (screenHeightMeters / 2));
            Gizmos.DrawWireCube(levelCenter, levelSize);  
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        foreach(var level in levels)
        {
            GUI.color = Color.white;
            UnityEditor.Handles.Label(level.Center, level.name);

        }
#endif

    }

    [Serializable]
    public class Level
    {
        [Title("$name")]
        public string name;
        public string[] adjacentLevels;

        public int levelWidth = 1;
        public int levelHeight = 1;

        public int horizontalOffset = 0;
        public int verticalOffset = 0;

        public Vector2 Center => new(
            (horizontalOffset * screenWidthMeters) + (levelWidth * screenWidthMeters / 2) - screenWidthMeters / 2,
            (verticalOffset * screenHeightMeters) + (levelHeight * screenHeightMeters / 2) - screenHeightMeters / 2);

        public Vector2 Offset => new(horizontalOffset, verticalOffset);


        public Level(string name)
        {
            this.name = name;
        }

#if UNITY_EDITOR
        [Button, ButtonGroup, GUIColor(.5f, 1, .5f)]
        void OpenSceneAdditive()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene($"Assets/Scenes/AdditiveLoad Levels/{name}.unity", UnityEditor.SceneManagement.OpenSceneMode.Additive);
        }
        [Button, ButtonGroup, GUIColor(1,.5f,.5f)]
        void CloseScene()
        {
            Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetSceneByName(name);
            if (scene == null)
                return;

            UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
        }
#endif
    }
}
