using UnityEngine;

public class Util
{
    public static int GeneralRandom(float[] values, float min = 0, float max = 0)
    {
        values = AdjustVals(values, min, max);

        int selectedElement = 0;
        float cumulative = 0.0f;
        float diceRoll = Random.Range(0f, 1f);
        //for (int i = 0; i < probs.Length; i++)
        //{
        //    Debug.LogWarning("softRandom i:" + i + " val:" + values[i] + " pro:" + probs[i]);
        //}
        for (int i = 0; i < values.Length; i++)
        {
            cumulative += values[i];
            if (diceRoll < cumulative)
            {
                selectedElement = i;
                break;
            }
        }
        return selectedElement;
    }

    public static int SoftRandom(float[] values, float confidence = 1.0f, float min = 0, float max = 0)
    {
        float[] probs = SoftMax(values, confidence, min, max);
        return GeneralRandom(probs, 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <param name="confidence"></param>
    /// <param name="min">价值区间调整</param>
    /// <param name="max">价值区间调整</param>
    /// <returns></returns>
    public static float[] SoftMax(float[] values, float confidence = 1.0f, float min = 0, float max = 0)
    {
        values = AdjustVals(values, min, max);

        float[] softmax_values = new float[values.Length];
        float[] exp_values = new float[values.Length];
        float sum = 0f;
        for (int i = 0; i < values.Length; i++)
        {
            exp_values[i] = Mathf.Exp(values[i] / confidence);
            sum += exp_values[i];
        }

        for (int i = 0; i < values.Length; i++)
        {
            softmax_values[i] = exp_values[i] / sum;
        }
        return softmax_values;
    }

    public static float[] AdjustVals(float[] values, float min = 0, float max = 0)
    {
        if (min == max)
        {
            return values;
        }
        //减小值，避免无穷大
        float minVal = values[0];
        float maxVal = values[0];
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] > maxVal)
            {
                maxVal = values[i];
            }
            if (values[i] < minVal)
            {
                minVal = values[i];
            }
        }
        if (maxVal > minVal)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (values[i] - minVal) / (maxVal - minVal) * (max - min) + min;
            }
        }
        else
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = min;
            }
        }
        return values;
    }
}