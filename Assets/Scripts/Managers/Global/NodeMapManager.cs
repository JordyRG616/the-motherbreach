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
    [SerializeField] private List<MapLine> lines;
    [SerializeField] private Node FirstNode;
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
    //private Dictionary<int, int> lineAndNodeIndecis

    private void Start()
    {
        waveManager = WaveManager.Main;
        DataManager.Main.ReceiveSavable(this);
    }

    public void CreateMap()
    {
        StartCoroutine(Generate());
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

    public Dictionary<string, byte[]> GetData()
    {
        var dictionary = new Dictionary<string, byte[]>();

        dictionary.Add("LineIndex", System.BitConverter.GetBytes(currentLineIndex));
        dictionary.Add("NodeIndex", System.BitConverter.GetBytes(nodeIndex));

        return dictionary;
    }

    public void LoadData(SaveFile saveFile)
    {
        currentLineIndex = System.BitConverter.ToInt32(saveFile.GetValue("LineIndex"));
        nodeIndex = System.BitConverter.ToInt32(saveFile.GetValue("NodeIndex"));

        var line = lines[currentLineIndex];
        //line.SetAvailableNodes();
        line.GetNodeByIndex(nodeIndex).SetSelected();
    }
}
