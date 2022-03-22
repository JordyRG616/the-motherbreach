using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : MonoBehaviour
{
    [SerializeField] private List<StatusDisplayInfo> statusDisplayInfos;
    [SerializeField] private ParticleSystem particle;
    private Dictionary<Keyword, int> indexes = new Dictionary<Keyword, int>();
    private int index;

    public void ReceiveStatus(Keyword status)
    {
        var icon = statusDisplayInfos.Find(x => x.status == status).icon;

        var _tex = particle.textureSheetAnimation;
        _tex.AddSprite(icon);

        if(!indexes.ContainsKey(status))
        {
            indexes.Add(status, index);
            index ++;
        }
        
        particle.Play();
    }

    public void RemoveStatus(Keyword status)
    {
        var _tex = particle.textureSheetAnimation;
        _tex.RemoveSprite(indexes[status]);
        index--;

        if(_tex.spriteCount > 0) particle.Play();
        else particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

}

[System.Serializable]
public struct StatusDisplayInfo
{
    public Keyword status;
    public Sprite icon;
}