using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MoreMountains.Feedbacks;

public class EditTilemapTest : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    public Vector3Int startTile;

    int[] x = { -1, 0, 1, 0 };
    int[] y = { 0, 1, 0, -1 };

    public bool unlockLevel = false;

    public MMFeedbacks feedbacks;

    private void Awake()
    {
        if (GameManager.Instance.hubAreasUnlocked.Contains(1))
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private IEnumerator Start()
    {

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.P) || unlockLevel);
        feedbacks.PlayFeedbacks();
        tilemap.SetTile(startTile, null);
        StartCoroutine(RemoveNeighbours(startTile));
    }

    IEnumerator RemoveNeighbours(Vector3Int pos)
    {
        yield return new WaitForSeconds(.1f);
        feedbacks.PlayFeedbacks();
        for (int i = 0; i < 4; i++)
        {
            var newpos = pos + new Vector3Int(x[i], y[i], 0);
            if(tilemap.GetTile(newpos) != null)
            {
                tilemap.SetTile(newpos, null);
                StartCoroutine(RemoveNeighbours(newpos));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector3)startTile + new Vector3(1, 1, 0) * .5f + Vector3.back * 5, Vector3.one * .95f);
        Gizmos.DrawWireCube((Vector3)startTile + new Vector3(1, 1, 0) * .5f + Vector3.back * 5, Vector3.one * .9f);
    }

    public void Unhide()
    {
        unlockLevel = true;
        Invoke("AddHubAreaToManager", .1f);
    }
    void AddHubAreaToManager()
    {
        GameManager.Instance.hubAreasUnlocked.Add(1);   
    }
}
