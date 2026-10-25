using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 6;
    public int gridHeight = 6;
    public float cellSize = 150f;

    private Block[,] grid;

    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private GameObject key_sliding;
    private InventorySlot inventorySlot;
    public VariableDeterminator variableDeterminator;
    [SerializeField] private public_actions _Public_actions;
    [SerializeField] private AudioSource winMiniGameSound;
    private void Awake()
    {
        grid = new Block[gridWidth, gridHeight];

        foreach (Block block in GetComponentsInChildren<Block>())
        {
            foreach (var pos in block.gridPositions)
            {
                if (IsInsideGrid(pos))
                    grid[pos.x, pos.y] = block;
                else
                    Debug.LogWarning($"Block {block.name} position {pos} out of bounds!");
            }
        }
    }
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public bool IsCellFree(Vector2Int pos)
    {
        if (!IsInsideGrid(pos))
            return false;

        return grid[pos.x, pos.y] == null;
    }

    public Block GetBlockAt(Vector2Int pos)
    {
        if (!IsInsideGrid(pos))
            return null;
        return grid[pos.x, pos.y];
    }

    public bool CanMoveTo(Block block, List<Vector2Int> newPositions)
    {
        foreach (var pos in newPositions)
        {
            if (!IsInsideGrid(pos))
                return false;

            Block occupying = GetBlockAt(pos);
            if (occupying != null && occupying != block)
                return false;
        }
        return true;
    }

    public void UpdateBlockPosition(Block block, List<Vector2Int> newPositions)
    {
        foreach (var pos in block.gridPositions)
        {
            if (IsInsideGrid(pos))
                grid[pos.x, pos.y] = null;
        }

        foreach (var pos in newPositions)
        {
            if (IsInsideGrid(pos))
                grid[pos.x, pos.y] = block;
            else
                Debug.LogWarning($"Trying to move block {block.name} out of bounds!");
        }

        block.gridPositions = new List<Vector2Int>(newPositions);
    }

    public void OnWin()
    {
        public_actions.Instance.OnOff_SpaceTip_Close(false);
        winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
        inventorySlot = GameObject.Find(variableDeterminator.pathOfSlot).GetComponent<InventorySlot>();
        inventorySlot.ClearItem();
        colliderList.ENABLE_COLLIDER();
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Активировать падение key_sliding
        Rigidbody2D rb = key_sliding.GetComponent<Rigidbody2D>();
        rb.simulated = true;

        // Вздрог — маленький импульс вверх и вбок
        rb.AddForce(new Vector2(UnityEngine.Random.Range(-30f, 30f), 80f));

        _Public_actions.StartDisableObjectsAfterFall();
    }
}