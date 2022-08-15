using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CustomRandom;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool bossNode;
    public int bossIndex;
    [SerializeField] private Image frame;
    [SerializeField] private Image BG;
    [SerializeField] private Image icon;
    [SerializeField] private LineRenderer lineModel;
    [SerializeField] private List<NodeTypeData> nodeTypes;
    [SerializeField] private MapLine parentLine;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Material lineMaterial;
    private Animator anim;
    public int difficulty { get; private set; }
    [SerializeField] private List<Color> difficultyColors;
    private List<Node> connectedNodes = new List<Node>();
    private List<LineRenderer> connections = new List<LineRenderer>();
    public int lineIndex;
    public bool active;
    private bool available;
    private bool selected;
    private Vector3 originalScale = new Vector3(-1, 1, 0);
    public WaveData nodeWave { get; private set; }
    public WaveModifier modifier { get; private set; }
    private NodeMapManager mapManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        mapManager = NodeMapManager.Main;
    }

    public void Activate()
    {
        frame.enabled = true;
        BG.enabled = true;
        active = true;

        difficulty = 0;

        GenerateTypeAndDifficulty();
    }

    private void GenerateTypeAndDifficulty()
    {
        if(bossNode)
        {
            frame.enabled = false;
            icon.color = difficultyColors[4];

            nodeWave = WaveManager.Main.GetRandomBossByLevel(bossIndex);
            icon.sprite = nodeWave.BossIcon;
            return;
        }
        var rdm = RandomManager.GetRandomInteger(0, nodeTypes.Count);
        var type = nodeTypes[rdm];

        icon.sprite = type.icon;
        icon.enabled = true;

        difficulty += type.difficultyModifier;
        difficulty += parentLine.lineDifficulty;
        if (difficulty >= 3) difficulty = 3;
        frame.color = difficultyColors[difficulty];
        icon.color = difficultyColors[difficulty];
        modifier = type.modifier;
        nodeWave = WaveManager.Main.GetRandomWaveByLevel(difficulty);
        modifier.chanceToApply = difficulty / 5;

        modifier.ApplyWaveModifier(nodeWave);
    }

    public void SetCurrent()
    {
        parentLine.DeactivateAvailableNodes();  
        anim.enabled = false;
        frame.sprite = currentSprite;
        connectedNodes.ForEach(x => x.SetAvailable());
        icon.transform.localScale = originalScale;
        ActivateLines();
    }

    private void ActivateLines()
    {
        foreach(LineRenderer line in connections)
        {
            var material = line.material;
            material.SetFloat("_Speed", -2f);
        }
    }

    public void DeactivateLines()
    {
        foreach (LineRenderer line in connections)
        {
            var material = line.material;
            material.SetFloat("_Speed", 0);
        }
    }

    public void SetAvailable()
    {
        selected = false;
        anim.SetBool("Selected", false);
        anim.SetBool("Available", true);
        available = true;
        icon.transform.localScale = originalScale;
    }

    public void SetSelected()
    {
        available = false;
        anim.SetBool("Available", false);
        anim.SetBool("Selected", true);
        mapManager.ReceiveSelectedNode(this);
        selected = true;
    }

    public void SetUnavailable()
    {
        if (selected) return;
        available = false;
        anim.SetBool("Available", false);
    }

    public void Deactivate()
    {
        frame.enabled = false;
        BG.enabled = false;
        icon.enabled = false;
        active = false;
        connectedNodes.Clear();
        connections.ForEach(x => DestroyImmediate(x.gameObject));
        connections.Clear();
    }

    public void DeactivateFirstNode()
    {
        connectedNodes.Clear();
        connections.ForEach(x => DestroyImmediate(x.gameObject));
        connections.Clear();
    }

    public void CreateConnection(Node node)
    {
        var xDifference = node.transform.localPosition.x - transform.localPosition.x;

        var line = Instantiate(lineModel, transform);

        line.positionCount = 2;
        var positionZero = new Vector3(0, -14);
        line.SetPosition(0, positionZero);
        line.SetPosition(1, new Vector3(xDifference, -77));

        var initialColor = difficultyColors[difficulty];
        var endColor = difficultyColors[node.difficulty];

        line.startColor = initialColor;
        line.endColor = endColor;

        line.material = new Material(lineMaterial);

        connectedNodes.Add(node);
        connections.Add(line);
        if(!node.active) node.Activate();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selected) return;
        anim.SetTrigger("Enter");
        icon.transform.localScale = originalScale * 1.25f;
        if (available) ActivateLines();
        if (nodeWave != null) mapManager.SetEnemyInfo(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selected) return;
        anim.SetTrigger("Exit");
        if(available) DeactivateLines();
        icon.transform.localScale = originalScale;
        mapManager.ClearEnemyInfo();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(selected)
        {
            SetAvailable();
        }

        if(available)
        {
            SetSelected();
        }

    }
}
public enum NodeType { Normal, Shield, Assault, Phased, Boss }

[System.Serializable]
public struct NodeTypeData
{
    public NodeType type;
    public Sprite icon;
    public int difficultyModifier;
    public WaveModifier modifier;
}
