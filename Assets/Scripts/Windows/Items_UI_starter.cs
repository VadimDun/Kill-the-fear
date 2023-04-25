using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Items_UI_starter : MonoBehaviour
{

    public Dictionary<string, Vector2> cells_positions = new Dictionary<string, Vector2>();

    private Vector2 start_position;

    void Start()
    {
        start_position = transform.GetChild(0).position;

        GridLayoutGroup gl = transform.gameObject.GetComponent<GridLayoutGroup>();
        
        // Создаю матриу позиций клеток
        Vector2[,] cells = new Vector2[4,5];
        
        int k = 1;

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells_positions.Add($"ItemSlot({k})", start_position + new Vector2(gl.cellSize.x * j, gl.cellSize.y * i));
                k++;
            }
        }

    }



    public Vector2 GetDefaultPosition(string cellName)
    {
        return cells_positions[cellName];
    }

}
