using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    BloodCell,
    Neuron
}

public struct MissionResults
{
    public float CompletionTime;
    public bool IsSucceeded;
    public CellType Type;

    public override string ToString()
    {
        return string.Format("Type: {0}\nCompletion Time: {1}\n{2}", Type.ToString(), (int)CompletionTime, IsSucceeded ? "Success!" : "Failed!");
    }
}

public class LifetimeStats
{
    public CellType Type;
    public int availableCells;
    public int missionsWon = 0;
    public CellType RewardType;
    public int rewardQuantity;
}
