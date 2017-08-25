﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerGenerator
{
	private int callCount = 0;
	public int TileCountX { get; private set; }
	public int TileCountZ { get; private set; }

	private string seed;
	private bool isRandomSeed;
	private int randomFillPercent;


	private int smoothCount = 5;
	private int surroundWallCount = 4;

	private int[,] map;

	public LayerGenerator(MapSizeSettings mapSizeSets, GeneratorSettings genSets)
	{
		TileCountX = mapSizeSets.TileCountX;
		TileCountZ = mapSizeSets.TileCountZ;

		seed = genSets.seed;
		isRandomSeed = genSets.isRandom;
		randomFillPercent = genSets.fillPercent;
	}

	/// <summary>
	/// Генерирует случайно заполненный массив из 0 и 1.
	/// </summary>
	/// <param name="isSmooth">Соединять близкие 1 в одну область?</param>
	/// <returns></returns>
	public int[,] Generate(bool isSmooth)
	{
		map = new int[TileCountX, TileCountZ];
		RandomFillMap();

		if (isSmooth)
		{
			for (int i = 0; i < smoothCount; i++)
			{
				SmoothMap();
			}
		}

		return map;
	}

	private void RandomFillMap()
	{
		if (isRandomSeed)
		{
			seed = Time.time.ToString();
		}

		int seedHash = seed.GetHashCode() + callCount;
		callCount++;

		System.Random pseudoRandom = new System.Random(seedHash);

		for (int x = 0; x < TileCountX; x++)
		{
			for (int y = 0; y < TileCountZ; y++)
			{
				// Граница карты непроходима
				if (x == 0 || x == TileCountX - 1 || y == 0 || y == TileCountZ - 1)
				{
					map[x, y] = 1;
				}
				else
				{
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	/// <summary>
	/// Соединяет близкие непроходимые/непроходимые области в одну большую
	/// </summary>
	private void SmoothMap()
	{
		for (int x = 0; x < TileCountX; x++)
		{
			for (int y = 0; y < TileCountZ; y++)
			{
				int neighbourWallTiles = GetSurroundWallCount(x, y);

				if (neighbourWallTiles > surroundWallCount)
				{
					map[x, y] = 1;
				}
				else
				{
					// Можно поставить <= или -1 справа
					if (neighbourWallTiles < surroundWallCount)
					{
						map[x, y] = 0;
					}
				}

			}
		}
	}

	/// <summary>
	/// Число стен вокруг клетки [gridX, gridY]
	/// </summary>
	private int GetSurroundWallCount(int gridX, int gridY)
	{
		int wallCount = 0;

		// Зона, в которой ищутся стены - [gridX -+ 1, gridY -+ 1]
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
			{
				// Считаются точки только в соседних клетках
				if (neighbourX == gridX && neighbourY == gridY)
				{
					continue;
				}

				// Вне карты только непроходимые места
				if (neighbourX < 0 || neighbourX >= TileCountX || neighbourY < 0 || neighbourY >= TileCountZ)
				{
					wallCount++;
				}
				else
				{
					wallCount += map[neighbourX, neighbourY];
				}
			}
		}

		return wallCount;
	}
}
