using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Vector2 mapSize;

	[Range(0, 1)]
	public float outlinePercent;
	[Range(0, 1)]
	public float obstaclePercent;
	public int seed = 10;

	private List<Coord> allTileCoords;
	private Queue<Coord> shuffledTileCoords;

	private Coord mapCenter;

	void Start()
	{
		GenerateMap();
	}

	public void GenerateMap()
	{
		allTileCoords = new List<Coord>();

		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				allTileCoords.Add(new Coord(x, y));
			}
		}
		shuffledTileCoords = new Queue<Coord>(allTileCoords.ToArray().Shuffle(seed));
		mapCenter = new Coord(mapCenter.x / 2, mapCenter.y / 2);

		string holderName = "Generated Map";

		var childTransform = transform.Find(holderName);
		if (childTransform != null)
		{
			DestroyImmediate(childTransform.gameObject);
		}

		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				Vector3 tilePosition = CoordToPosition(x, y);
				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));

				newTile.localScale = Vector3.one * (1 - outlinePercent);
				newTile.parent = mapHolder;
			}
		}

		bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
		int currentObctacleCount = 0;

		for (int i = 0; i < obstacleCount; i++)
		{
			Coord randomCoord = GetRandomCoord();
			obstacleMap[randomCoord.x, randomCoord.y] = true;
			currentObctacleCount++;

			if (randomCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObctacleCount))
			{
				Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

				Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity);
				newObstacle.parent = mapHolder;
			}
			else
			{
				obstacleMap[randomCoord.x, randomCoord.y] = false;
				currentObctacleCount--;
			}
		}
	}

	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
	{
		bool[,] mapFlags = new bool[obstacleMap.GetLongLength(0), obstacleMap.GetLongLength(1)];
		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(mapCenter);

		return false;
	}

	private Vector3 CoordToPosition(Vector2Int coord)
	{
		return CoordToPosition(coord.x, coord.y);
	}
	private Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
	}
	public Coord GetRandomCoord()
	{
		Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue(randomCoord);
		return randomCoord;
	}
	public struct Coord : System.IEquatable<Coord>
	{
		public int x;
		public int y;

		public Coord(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Coord coord &&
				   x == coord.x &&
				   y == coord.y;
		}

		public bool Equals(Coord other)
		{
			return x.Equals(other.x) && y.Equals(other.y);
		}

		public override int GetHashCode()
		{
			var hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public static bool operator == (Coord lhs, Coord rhs)
		{
			return lhs.Equals(rhs);
		}
		public static bool operator != (Coord lhs, Coord rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}