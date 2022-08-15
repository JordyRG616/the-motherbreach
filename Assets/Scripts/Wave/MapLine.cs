using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomRandom;

public class MapLine : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    [SerializeField] private List<Node> nodesInLine;
    [field: SerializeField] public int lineDifficulty { get; private set; }
    [SerializeField] private float nodeChance;

    public Node GetNodeByIndex(int index)
    {
        if (index < 0) index = 0;
        if (index >= nodesInLine.Count) index = nodesInLine.Count - 1;
        var node = nodesInLine.Find(x => x.lineIndex == index);

        return node;
    }

    public void InitializeLine(MapLine nextLine)
    {
        foreach(Node node in nodesInLine)
        {
            float count = 0;
            if(node.active)
            {
                var id = node.lineIndex;

                for (int i = -1; i < 2; i++)
                {
                    var rdm = RandomManager.GetRandomFloat();
                    if (rdm >= nodeChance)
                    {
                        node.CreateConnection(nextLine.GetNodeByIndex(id + i));
                        count++;
                    }
                }

                if(count == 0)
                {
                    var rdm = RandomManager.GetRandomInteger(-1, 2);
                    node.CreateConnection(nextLine.GetNodeByIndex(id + rdm));
                }
            }
        }
    }

    public void InitializeFirstLine(MapLine nextLine)
    {
        foreach (Node node in nodesInLine)
        {
            if (node.active)
            {
                for (int i = 0; i < 4; i++)
                {
                    var rdm = RandomManager.GetRandomFloat();
                    if (rdm >= nodeChance)
                    {
                        node.CreateConnection(nextLine.GetNodeByIndex(i));
                    }
                }

            }
        }
    }

    public void ResetLine()
    {
        nodesInLine.ForEach(x => x.Deactivate());
    }

    public void DeactivateAvailableNodes()
    {
        nodesInLine.ForEach(x => x.SetUnavailable());
    }

    public void SetAvailableNodes()
    {
        nodesInLine.ForEach(x => x.SetAvailable());
    }
}
