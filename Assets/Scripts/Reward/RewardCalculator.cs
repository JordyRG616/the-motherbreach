using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator : MonoBehaviour
{
    [SerializeField][Range(0, 999)] private int a;
    [SerializeField] private float b;
    [SerializeField] private float x;

    [ContextMenu("Teste")]
    public void test()
    {
        float max = GammaDistribution(2);
        Debug.Log(FactorialFunction(a));
        Debug.Log(GammaDistribution(x, max));
    }


    public float GammaDistribution(float x, float max = 1)
    {
        float y = ((Mathf.Pow(b, a)) * (Mathf.Pow(x, a - 1)) * (Mathf.Exp(-b * x))) / (FactorialFunction(a - 1));
        y *= 1 / max;
        return y;
    }

    public int FactorialFunction(int x)
    {
        int y = 1;
        for (int i = 0; i < x; i++)
        {
            y *= (x-i);
        }
        return y;
    }
}
