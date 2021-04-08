﻿using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
	public bool autoGenerateInGame;

	public Map[] maps;
	public int mapIndex;

	[Header("Prefabs")]
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Transform mapFloor;
	public Transform navmeshFloor;
	public Transform navmeshMaskPrefab;

	[Header("Properties")]
	public Vector2 maxMapSize;
	[Range(0, 1)]
	public float outlinePercent;
	public float tileSize;

	private List<Coord> allTileCoords;
	private Queue<Coord> shuffledTileCoords;
	private Queue<Coord> shuffledOpenTileCoords;
	private Transform[,] tileMap;

	private Map currentMap;

	public void OnNewWave(int waveNumber)
	{
		mapIndex = waveNumber - 1;

		if (autoGenerateInGame)
		{
			System.Random random = new System.Random();
			maps[mapIndex].seed = random.Next();
		}

		GenerateMap();
	}

	public void GenerateMap()
	{
		currentMap = maps[mapIndex];
		tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
		System.Random random = new System.Random(currentMap.seed);
		

		// Generating coords
		allTileCoords = new List<Coord>();
		for (int x = 0; x < currentMap.mapSize.x; x++)
		{
			for (int y = 0; y < currentMap.mapSize.y; y++)
			{
				allTileCoords.Add(new Coord(x, y));
			}
		}
		shuffledTileCoords = new Queue<Coord>(allTileCoords.ToArray().PhisherShuffle(currentMap.seed));

		// Create map holder object
		string holderName = "Generated Map";
		var childTransform = transform.Find(holderName);
		if (childTransform != null)
		{
			DestroyImmediate(childTransform.gameObject);
		}

		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		// Spawning tiles
		for (int x = 0; x < currentMap.mapSize.x; x++)
		{
			for (int y = 0; y < currentMap.mapSize.y; y++)
			{
				Vector3 tilePosition = CoordToPosition(x, y);
				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));

				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
				tileMap[x, y] = newTile;
			}
		}

		// Spawning obstacles
		bool[,] obstacleMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];

		int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
		int currentObctacleCount = 0;
		List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

		for (int i = 0; i < obstacleCount; i++)
		{
			Coord randomCoord = GetRandomCoord();
			obstacleMap[randomCoord.x, randomCoord.y] = true;
			currentObctacleCount++;

			if (randomCoord != currentMap.mapCenter && MapIsFullyAccessible(obstacleMap, currentObctacleCount))
			{
				float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)random.NextDouble());
				Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

				Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2f, Quaternion.identity);
				newObstacle.parent = mapHolder;
				newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

				Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
				Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
				float colorPercent = randomCoord.y / (float)currentMap.mapSize.y;
				obstacleMaterial.color = Color.Lerp(currentMap.foregroundColor, currentMap.backgroundColor, colorPercent);
				obstacleRenderer.sharedMaterial = obstacleMaterial;

				allOpenCoords.Remove(randomCoord);
			}
			else
			{
				obstacleMap[randomCoord.x, randomCoord.y] = false;
				currentObctacleCount--;
			}
		}

		shuffledOpenTileCoords = new Queue<Coord>(allOpenCoords.ToArray().PhisherShuffle(currentMap.seed));

		// Creating navmesh mask
		Transform maskLeft = Instantiate(navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity);
		maskLeft.parent = mapHolder;
		maskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

		Transform maskRight = Instantiate(navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity);
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

		Transform maskTop = Instantiate(navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity);
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

		Transform maskBottom = Instantiate(navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity);
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

		navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize + Vector3.forward * tileSize;
		mapFloor.localScale = new Vector3(currentMap.mapSize.x * tileSize, currentMap.mapSize.y * tileSize) + Vector3.forward;
	}

	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
	{
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(currentMap.mapCenter);
		mapFlags[currentMap.mapCenter.x, currentMap.mapCenter.y] = true;

		int accessibleTileCount = 1;

		while (queue.Count > 0)
		{
			Coord tile = queue.Dequeue();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;

					if (x == 0 || y == 0)
					{
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
						{
							if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
							{
								mapFlags[neighbourX, neighbourY] = true;
								queue.Enqueue(new Coord(neighbourX, neighbourY));
								accessibleTileCount++;
							}
						}
					}
				}
			}
		}

		int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);

		return targetAccessibleTileCount == accessibleTileCount;
	}

	private Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
	}

	public Transform GetTileFromPosition(Vector3 postion)
	{
		int x = Mathf.RoundToInt(postion.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
		int y = Mathf.RoundToInt(postion.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
		x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
		y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);

		return tileMap[x, y];
	}
	public Coord GetRandomCoord()
	{
		Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue(randomCoord);
		return randomCoord;
	}

	public Transform GetRandomTile()
	{
		Coord randomCoord = shuffledOpenTileCoords.Dequeue();
		shuffledOpenTileCoords.Enqueue(randomCoord);
		return tileMap[randomCoord.x, randomCoord.y];
	}

	[System.Serializable]
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

		public static bool operator ==(Coord lhs, Coord rhs)
		{
			return lhs.Equals(rhs);
		}
		public static bool operator !=(Coord lhs, Coord rhs)
		{
			return !lhs.Equals(rhs);
		}
	}

	[System.Serializable]
	public class Map
	{
		public Coord mapSize;
		[Range(0, 1)]
		public float obstaclePercent;
		public int seed;
		public float minObstacleHeight;
		public float maxObstacleHeight;
		public Color foregroundColor;
		public Color backgroundColor;

		public Coord mapCenter => new Coord(mapSize.x / 2, mapSize.y / 2);
	}
}