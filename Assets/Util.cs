using UnityEngine;

public class Util
{
    public static float[] SoftMax(float[] values, float confidence = 1.0f)
    {
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
                values[i] = (values[i] - minVal) / (maxVal - minVal) * 10 - 1;
            }
        }
        else
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 1;
            }
        }

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

    public static int SoftRandom(float[] values, float confidence = 1.0f)
    {
        float[] probs = SoftMax(values, confidence);
        float cumulative = 0.0f;
        int selectedElement = 0;
        float diceRoll = Random.Range(0f, 1f);
        //for (int i = 0; i < probs.Length; i++)
        //{
        //    Debug.LogWarning("t=" + values[i] + " " + probs[i]);
        //}
        for (int i = 0; i < probs.Length; i++)
        {
            cumulative += probs[i];
            if (diceRoll < cumulative)
            {
                selectedElement = i;
                break;
            }
        }
        return selectedElement;
    }
}