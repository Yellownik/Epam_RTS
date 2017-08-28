﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerUnitFactory : UnitFactory {
    public override Unit CreateUnit(Vector3 spawnPosition)
    {
        UnitSettings settings = GameManager.getUnitSettings(UnitType.FootSoldier_Flamer);
        GameObject newUnit = GameObject.Instantiate<GameObject>(settings.Prefab, spawnPosition, settings.Prefab.transform.rotation);
        Unit unit = newUnit.AddComponent<Unit>();
        unit.MaxHealth = settings.MaxHealth;
        unit.Health = settings.MaxHealth;
        unit.Cost = settings.Cost;
        unit.Fraction = settings.Fraction;
        unit.IsLoadable = settings.IsLoadable;
        unit.Icon = settings.Icon;
        unit.Name = settings.Name;
        return unit;
    }
}
