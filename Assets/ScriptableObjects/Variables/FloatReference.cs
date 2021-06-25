
using System;

[Serializable]
public class FloatReference
{
    public bool useConstant;

    public float constantValue;

    public FloatVariable Variable;

    public float Value
    {
        get { return useConstant ? constantValue : Variable.Value; }
    }
}
