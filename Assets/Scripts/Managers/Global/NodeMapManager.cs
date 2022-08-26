using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeMapManager : MonoBehaviour, ISavable
{
    #region Singleton
    private static NodeMapManager _instance;
    public static NodeMapManager Main
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NodeMapManager>();

                if (_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if (container == null)
                    {
                        container = new GameObject("Game manager");
                    }

                    _instance = container.AddComponent<NodeMapManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField] private List<EnemyBox> enemyBoxes;
    [SerializeField] private List<EnemyBox> inRewardEnemyBoxes;
    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject normalMap;
    [SerializeField] private List<MapLine> lines;
    [SerializeField] private Node FirstNode;
    [SerializeField] private GameObject tutorialMap;
    [SerializeField] private List<MapLine> tutorialLines;
    [SerializeField] private Node tutorialFirstNode;

    [Header("Boss Information")]
    [SerializeField] private Image bossImage;
    [SerializeField] private Image coreImage;
    [SerializeField] private TextMeshProUGUI bossDescription;
    [SerializeField] private TextMeshProUGUI coreName;
    [SerializeField] private GameObject bossInfoPanel;
    
    public Node selectedNode { get; private set; }
    private WaitForSeconds time = new WaitForSeconds(.15f);
    private WaveManager waveManager;
    private int currentLineIndex;
    private int nodeIndex;
    public Map selectedMap { get; private set; }

    private void Start()
    {
        waveManager = WaveManager.Main;
        DataManager.Main.ReceiveSavable(this);
    }

    public void CreateMap(Map map)
    {
        selectedMap = map;
        switch(map)
        {
            case Map.Tutorial:
                normalMap.SetActive(false);
                tutorialMap.SetActive(true);
                StartCoroutine(GenerateTutorial());
                break;
            case Map.Valdarkan:
                normalMap.SetActive(true);
                tutorialMap.SetActive(false);
                StartCoroutine(Generate());
                break;
        }
    }

    private IEnumerator Generate()
    {
        yield return time;
        lines[0].InitializeFirstLine(lines[1]);
        for (int i = 1; i < lines.Count - 1; i++)
        {
            lines[i].InitializeLine(lines[i + 1]);
        }

        FirstNode.Activate();
        if (DataManager.Main.SaveFileExists())
        {
            LoadData(DataManager.Main.saveFile);
        }
        else
        {
            FirstNode.SetAvailable();
        }
    }

    private IEnumerator GenerateTutorial()
    {
        yield return time;
        tutorialLines[0].InitializeFirstLine(tutorialLines[1]);
        for (int i = 1; i < tutorialLines.Count - 1; i++)
        {
            tutorialLines[i].InitializeLine(tutorialLines[i + 1]);
        }

        tutorialFirstNode.Activate();
        if (DataManager.Main.SaveFileExists())
        {
            LoadData(DataManager.Main.saveFile);
        }
        else
        {
            tutorialFirstNode.SetAvailable();
            tutorialFirstNode.SetSelected();
        }
    }

    public void ReceiveSelectedNode(Node node)
    {
        if (selectedNode != null) selectedNode.SetAvailable();
        selectedNode = node;
        currentLineIndex = selectedNode.GetComponentInParent<MapLine>().Index;
        nodeIndex = selectedNode.lineIndex;
        SetEnemyInfo(selectedNode);
        SetInRewardEnemyInfo(selectedNode);
    }

    public void SetEnemyInfo(Node node)
    {
        var ids = node.nodeWave.enemiesInWave;

        if(node.bossNode)
        {
            enemyBoxes.ForEach(x => x.DeactivateBox());

            var bossInfo = waveManager.GetBossInformation(ids[0]);
            bossDescription.text = bossInfo.bossDescription;
            bossImage.sprite = bossInfo.bossSprite;
            coreImage.sprite = bossInfo.coreSprite;
            coreName.text = bossInfo.coreName;
            bossInfoPanel.SetActive(true);
            return;
        }

        bossInfoPanel.SetActive(false);

        for (int i = 0; i < ids.Count; i++)
        {
            enemyBoxes[i].SetupBox(waveManager.GetEnemyInformation(ids[i]));
        }
    }

    public void SetInRewardEnemyInfo(Node node)
    {
        var ids = node.nodeWave.enemiesInWave;
        int lines = 0;

        for (int i = 0; i < ids.Count; i++)
        {
            inRewardEnemyBoxes[i].SetupBox(waveManager.GetEnemyInformation(ids[i]));
            lines++;
        }

        var size = rect.sizeDelta; 
        size.y = 15 + (50 * lines);
        rect.sizeDelta = size;

    }

    public void ClearEnemyInfo()
    {
        bossInfoPanel.SetActive(false);
        enemyBoxes.ForEach(x => x.DeactivateBox());
        inRewardEnemyBoxes.ForEach(x => x.DeactivateBox());
        if (selectedNode == null) return; 
        SetEnemyInfo(selectedNode);
        SetInRewardEnemyInfo(selectedNode);
    }

    public void FinishCurrentNode()
    {
        if (selectedNode == null) return;
        selectedNode.SetCurrent();
        selectedNode = null;
    }

    public void ResetMap()
    {
        FirstNode.DeactivateFirstNode();
        for (int i = 1; i < lines.Count - 1; i++)
        {
            lines[i].ResetLine();
        }
    }

    public void SetAllNodesUnavailable()
    {
        if (selectedMap == Map.Tutorial) tutorialLines.ForEach(x => x.DeactivateAvailableNodes());
        else lines.ForEach(x => x.DeactivateAvailableNodes());
    }

    public Dictionary<string, byte[]> GetData()
    {
        var dictionary = new Dictionary<string, byte[]>();

        dictionary.Add("LineIndex", System.BitConverter.GetBytes(currentLineIndex));
        dictionary.Add("NodeIndex", System.BitConverter.GetBytes(nodeIndex));
        dictionary.Add("Map", System.BitConverter.GetBytes((int)selectedMap));

        return dictionary;
    }

    public void LoadData(SaveFile saveFile)
    {
        selectedMap = GameManager.Main.selectedMap;
        currentLineIndex = System.BitConverter.ToInt32(saveFile.GetValue("LineIndex"));
        nodeIndex = System.BitConverter.ToInt32(saveFile.GetValue("NodeIndex"));

        var line = lines[currentLineIndex];
        //line.SetAvailableNodes();
        line.GetNodeByIndex(nodeIndex).SetSelected();
    }
}

public enum Map 
{ 
    None = -1,
    Tutorial = 0, 
    Valdarkan = 1
}
