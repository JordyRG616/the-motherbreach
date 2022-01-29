using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicBackgroundElement : MonoBehaviour
{
    [SerializeField] protected float changeRate;
    [SerializeField] [Range(1f, 10f)] protected float speed;
    [SerializeField] protected string[] colorNames;
    [SerializeField] [ColorUsage(false, true)] protected List<Color> availableColors;
    [SerializeField] private List<FloatChangeInfo> floatChanges;
    [SerializeField] private List<VectorChangeInfo> vectorChanges;
    protected Queue<Color> colors = new Queue<Color>();
    protected Material _material;
    WaitForSeconds waitTime;
    protected Color[] ogColor = new Color[5];
    protected Color[] targetColor = new Color[5];

    void Start()
    {
        waitTime = new WaitForSeconds(speed / 100f);
        _material = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = _material;

        PreStep();

        StartCoroutine(DoStep());
    }

    protected void EnqueueColors()
    {
        var _list = new List<Color>(availableColors);
        while(_list.Count > 0)
        {
            var rdm = Random.Range(0, _list.Count);
            colors.Enqueue(_list[rdm]);
            _list.RemoveAt(rdm);
        }
    }

    protected void ChangeColor(string colorName, Color color)
    {
        _material.SetColor(colorName, color);
    }

    protected Color GetColor(string colorName)
    {
        return _material.GetColor(colorName);
    }
    protected void ChangeFloat(string floatName, float value)
    {
        _material.SetFloat(floatName, value);
    }

    protected float GetFloat(string floatName)
    {
        return _material.GetFloat(floatName);
    }

    protected void ChangeVector(string vectorName, Vector4 vector)
    {
        _material.SetVector(vectorName, vector);
    }

    protected Vector4 GetVector(string vectorName)
    {
        return _material.GetVector(vectorName);
    }

    protected Color LerpColor(int index, float step)
    {
        var color = Color.Lerp(ogColor[index], targetColor[index], step);
        return color;
    }

    protected IEnumerator DoStep()
    {
        while(true)
        {
            float step = 0;
            
            PreStep();

            while(step <= changeRate)
            {
                EvaluateStep(step);
                step += speed / changeRate;
                yield return waitTime;
            }
        }
    }

    protected void EvaluateStep(float step)
    {
        for (int i = 0; i < colorNames.Length; i++)
        {
            ChangeColor(colorNames[i], LerpColor(i, step / changeRate));
        }
        
        foreach(FloatChangeInfo change in floatChanges)
        {
            var value = Mathf.Lerp(change.ogFloat, change.variation, step / 100);
            ChangeFloat(change.floatName, value);
        }

        foreach(VectorChangeInfo change in vectorChanges)
        {
            var value = Vector4.Lerp(change.ogVector, change.variation, step / 100);
            ChangeVector(change.vectorName, value);
        }
    }

    protected void PreStep()
    {
        if(colors.Count < colorNames.Length)
        {
            EnqueueColors();
        }
        for(int i = 0; i < colorNames.Length; i++)
        {
            ogColor[i] = GetColor(colorNames[i]);
            targetColor[i] = colors.Dequeue();
        }

        foreach(FloatChangeInfo change in floatChanges)
        {
            change.SetVariation(GetFloat(change.floatName));
        }

        foreach(VectorChangeInfo change in vectorChanges)
        {
            change.SetVariation(GetVector(change.vectorName));
        }
    }
}

[System.Serializable]
public class FloatChangeInfo
{
    public string floatName;
    public float maxVariation;
     public float ogFloat;
     public float variation;

    public void SetVariation(float value)
    {
        ogFloat = value;
        variation = value + Random.Range(-maxVariation, maxVariation);
    }
}

[System.Serializable]
public class VectorChangeInfo
{
    public string vectorName;
    public float maxVariation;
     public Vector4 ogVector;
     public Vector4 variation;

    public void SetVariation(Vector4 value)
    {
        ogVector = value;
        variation = value + (Random.Range(-maxVariation, maxVariation) * Vector4.one);
    }
}