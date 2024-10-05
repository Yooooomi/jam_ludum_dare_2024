using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject boardTile;
    [SerializeField]
    private Transform tileContainer;

    [SerializeField]
    private Vector2Int boardSize;
    [SerializeField]
    private Vector2 tilePadding;

    [CustomEditor(typeof(BoardGenerator))]
    public class ButtonInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate"))
            {
                var instance = (BoardGenerator)target;
                instance.Generate();
            }
        }
    }

    private void Generate() {
        int maxxxx = 0;
        while (tileContainer.childCount > 0) {
            maxxxx++;
            DestroyImmediate(tileContainer.GetChild(0).gameObject);
            if (maxxxx > 10000000) {
                Debug.Log("CRINGGGEEE");
                return;
            }
        }
        Vector2 center = boardSize / 2;
        for (int x = 0; x < boardSize.x; ++x) {
            for (int y = 0; y < boardSize.y; ++y) {
                Vector2 spawnPos2d = (new Vector2(x, y) - center) * tilePadding + tilePadding / 2;
                Vector3 spawnPos = new Vector3(spawnPos2d.x, 0, spawnPos2d.y);
                Instantiate(boardTile, tileContainer.position + spawnPos, Quaternion.identity, tileContainer);
            }
        }
    }
}
