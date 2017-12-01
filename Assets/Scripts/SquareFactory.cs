using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFactory : MonoBehaviour
{
    public Transform ActiveGrid;
    public Square SquarePrefab;

    public List<Vector2> GridPositions;
    public float SquareScale;

    private List<int> availableGridSpots;

    private void Awake()
    {

    }

	private void Start()
	{
		
	}
	
	private void Update()
	{
		
	}

    public void InitializeFactory(GridDefinition definition)
    {
        ActiveGrid = GameObject.Find("Grid").transform;

        GridPositions = new List<Vector2>();
        foreach (Vector2 position in definition.GridPositions)
        {
            GridPositions.Add(position);
        }
        SquareScale = definition.SquareScale;

        availableGridSpots = new List<int>();
        for (int i = 0; i < GridPositions.Count; i++)
        {
            availableGridSpots.Add(i);
        }
    }

    public Square CreateRandomSquare()
    {
        if (availableGridSpots.Count <= 0)
        {
            return null;
        }

        int index = availableGridSpots[Random.Range(0, availableGridSpots.Count)];

        Square newSquare = Instantiate(SquarePrefab);

        newSquare.transform.SetParent(ActiveGrid, false);
        newSquare.transform.localScale = new Vector3(SquareScale, SquareScale);
        newSquare.transform.localPosition = GridPositions[index];

        newSquare.SquareIndex = index;

        newSquare.OnSquareDestroyed += HandleSquareDestroyed;

        availableGridSpots.Remove(index);

        return newSquare;
    }

    public Square CreateSquareAtIndex(int index)
    {
        if (!availableGridSpots.Contains(index))
        {
            return null;
        }
        
        Square newSquare = Instantiate(SquarePrefab);

        newSquare.transform.SetParent(ActiveGrid, false);
        newSquare.transform.localScale = new Vector3(SquareScale, SquareScale);
        newSquare.transform.localPosition = GridPositions[index];

        newSquare.SquareIndex = index;

        newSquare.OnSquareDestroyed += HandleSquareDestroyed;

        availableGridSpots.Remove(index);

        return newSquare;
    }

    public void HandleSquareDestroyed(object sender, OnSquareDestroyedEventArgs args)
    {
        Square destroyed = sender as Square;

        if (destroyed == null)
        {
            return;
        }

        availableGridSpots.Add(destroyed.SquareIndex);
    }

    public void ClearGrid()
    {
        foreach (Transform child in ActiveGrid)
        {
            Destroy(child.gameObject);
        }
    }

    public static SquareFactory CreateFactory()
    {
        GameObject factory = new GameObject("SquareFactory");
        return factory.AddComponent<SquareFactory>();
    }
}
