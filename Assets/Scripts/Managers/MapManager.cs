﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public int MapWidth { get { return mapSizeSets.width; } }
	public int MapLength { get { return mapSizeSets.length; } }

	[SerializeField]
	private MapSettingsManagerSO mapSetsManager;

	private GridManager gridManager;
	private MapCreator mapCreator;
	private MapSizeSettings mapSizeSets;

	private BuildAreaSelecter buildAreaSelecter;

	private void Awake()
	{
		mapSizeSets = mapSetsManager.GetMapSizeSettings();
		CreateMap();

		buildAreaSelecter = gameObject.AddComponent<BuildAreaSelecter>();
		buildAreaSelecter.SetGridManager(gridManager);
	}

	private void CreateMap()
	{
		gridManager = new GridManager(mapSetsManager.GetMapSizeSettings());
		mapCreator = new MapCreator(mapSetsManager, ref gridManager.tileGrid);

		GameObject mapGO = new GameObject();
		mapGO.name = "Map";

		CreateNavMesh(mapGO);
		mapCreator.CreateMapMesh(mapGO);
	}

	private void CreateNavMesh(GameObject mapGO)
	{
		GameObject mapNavMesh = new GameObject();
		mapNavMesh.transform.parent = mapGO.transform;
		mapNavMesh.name = "LocalNavMeshBuilder";

		LocalNavMeshBuilder lnmb = mapNavMesh.AddComponent<LocalNavMeshBuilder>();
		lnmb.transform.position += new Vector3(MapWidth / 2, 0, MapLength / 2);
		lnmb.m_Size = new Vector3(MapWidth, 20, MapLength);
	}

	#region FOR TEST selecting buildArea
	private void Update()
	{
		if (Input.GetKey(KeyCode.Alpha1))
			SelectArea();
		if (Input.GetKeyUp(KeyCode.Alpha1))
			DeselectArea();
	}

	public void SelectArea()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			IsBuildableArea(GetTilePos(hit.point) + Vector3.up * 0.3f, 3.1f, 3f, Race.Fermer);
		}
	}

	public void DeselectArea()
	{
		buildAreaSelecter.DeselectBuildArea();
	}
	#endregion

	public Vector3 GetTilePos(Vector3 position)
	{
		return gridManager.GetTilePos(position);
	}

	public bool IsBuildableArea(Vector3 pos, float areaSizeX, float areaSizeZ, Race race)
	{
		// TODO Nik Cнять заглушку
		// 01.09.17. Сейчас недопилена постройка для горожан
		// Поэтому стоит заглушка
		return buildAreaSelecter.SelectBuildArea(pos, areaSizeX, areaSizeZ, Race.Fermer);
	}

	public Vector3 GetCitizenBasePoint()
	{
		return mapCreator.CitizenBasePoint;
	}

	public Vector3[] GetFermerBasePoints()
	{
		return mapCreator.FermerBasePoints;
	}
}
