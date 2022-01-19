using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Converter : MonoBehaviour {


    [SerializeField] public InputField value;
    [SerializeField] public Dropdown castTypeFrom;
    [SerializeField] public Dropdown castTypeTo;
    [SerializeField] public InputField result;

    enum CastTypeDropdown {
        Integer=0,
        Long=1,
        Float=2,
        Double=3
    }
    
    void Start() {
        setValidRandomValue();
    }


    public void onCastButtonPressed() {
        
        if (!isValueParsedFromText()) {
            result.text = "";
            return;
        }

        int castToType = castTypeTo.value;
        switch (castToType) {
            case (int)CastTypeDropdown.Integer:
                int int_val = castToInt(value.text);
                result.text = int_val.ToString();
                break;
            case (int)CastTypeDropdown.Long:
                long long_val = castToLong(value.text);
                result.text = long_val.ToString();
                break;
            case (int)CastTypeDropdown.Float:
                float float_val = castToFloat(value.text);
                result.text = float_val.ToString();
                break;
            case (int)CastTypeDropdown.Double:
                double double_val = castToDouble(value.text);
                result.text = double_val.ToString();
                break;
        }
        
    }

    private int castToInt(string valueText) {
        int castFromType = castTypeFrom.value;
        int result = 0;
        switch (castFromType) {
            case (int)CastTypeDropdown.Integer:
                result = (int)int.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Integer+"] value " + value.text + " to type ["+ CastTypeDropdown.Integer+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Long:
                result = (int)long.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Long+"] value " + value.text + " to type ["+ CastTypeDropdown.Integer+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Float:
                result = (int)float.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Float+"] value " + value.text + " to type ["+ CastTypeDropdown.Integer+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Double:
                result = (int)double.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Double+"] value " + value.text + " to type ["+ CastTypeDropdown.Integer+ "] converted with result = "+ result);
                break;
        }
        return result;
    }
    
    private long castToLong(string valueText) {
        int castFromType = castTypeFrom.value;
        long result = 0;
        switch (castFromType) {
            case (int)CastTypeDropdown.Integer:
                result = (long)int.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Integer+"] value " + value.text + " to type ["+ CastTypeDropdown.Long+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Long:
                result = (long)long.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Long+"] value " + value.text + " to type ["+ CastTypeDropdown.Long+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Float:
                result = (long)float.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Float+"] value " + value.text + " to type ["+ CastTypeDropdown.Long+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Double:
                result = (long)double.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Double+"] value " + value.text + " to type ["+ CastTypeDropdown.Long+ "] converted with result = "+ result);
                break;
        }
        return result;
    }
    
    private float castToFloat(string valueText) {
        int castFromType = castTypeFrom.value;
        float result = 0;
        switch (castFromType) {
            case (int)CastTypeDropdown.Integer:
                result = (float)int.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Integer+"] value " + value.text + " to type ["+ CastTypeDropdown.Float+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Long:
                result = (float)long.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Long+"] value " + value.text + " to type ["+ CastTypeDropdown.Float+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Float:
                result = (float)float.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Float+"] value " + value.text + " to type ["+ CastTypeDropdown.Float+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Double:
                result = (float)double.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Double+"] value " + value.text + " to type ["+ CastTypeDropdown.Float+ "] converted with result = "+ result);
                break;
        }
        return result;
    }
    
    private double castToDouble(string valueText) {
        
        int castFromType = castTypeFrom.value;
        double result = 0;
        switch (castFromType) {
            case (int)CastTypeDropdown.Integer:
                result = (double)int.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Integer+"] value " + value.text + " to type ["+ CastTypeDropdown.Double+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Long:
                result = (double)long.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Long+"] value " + value.text + " to type ["+ CastTypeDropdown.Double+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Float:
                result = (double)float.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Float+"] value " + value.text + " to type ["+ CastTypeDropdown.Double+ "] converted with result = "+ result);
                break;
            case (int)CastTypeDropdown.Double:
                result = (double)double.Parse(valueText);
                Debug.Log("From type [" + CastTypeDropdown.Double+"] value " + value.text + " to type ["+ CastTypeDropdown.Double+ "] converted with result = "+ result);
                break;
        }
        return result;
    }
    
    public void setValidRandomValue() {
        int castFromType = castTypeFrom.value;
        switch (castFromType) {
            case (int)CastTypeDropdown.Integer:
                value.text = Random.Range(int.MinValue, int.MaxValue).ToString();
                break;
            case (int)CastTypeDropdown.Long:
                value.text = Random.Range(int.MinValue, int.MaxValue).ToString();
                break;
            case (int)CastTypeDropdown.Float:
                value.text = Random.Range(float.MinValue, float.MaxValue).ToString();
                break;
            case (int)CastTypeDropdown.Double:
                value.text = Random.Range(float.MinValue, float.MaxValue).ToString();
                break;
        }
        onValueChanged();
        
    }


    public void onFromCastTypeChanged() {
       
        if (!isValueParsedFromText()) {
            value.text = "";
            result.text = "";
            value.image.color=Color.white;
        }
        else {
            value.image.color=Color.green;    
        }
        
    }
    
    public void onToCastTypeChanged() {
        result.text = "";
    }

    public void onValueChanged() {
        if (isValueParsedFromText()) {
            value.image.color=Color.green;
            
        }
        else {
            value.image.color=Color.red;
        }
    }

    private bool isValueParsedFromText() {
        int castFromType = castTypeFrom.value;
        try {
            switch (castFromType) {
                case (int)CastTypeDropdown.Integer:
                    int int_val = int.Parse(value.text);
                    break;
                case (int)CastTypeDropdown.Long:
                    long long_val = long.Parse(value.text);
                    break;
                case (int)CastTypeDropdown.Float:
                    float float_val = float.Parse(value.text);
                    break;
                case (int)CastTypeDropdown.Double:
                    double double_val = float.Parse(value.text);
                    break;
            }
            return true;
        }
        catch (Exception e) {
            Debug.LogWarning("Value parsing exception: " + e);
            return false;
        }
    }

    [Serializable]
    public class ConverterEntity
    {
        public int castFromType;
        public string value;
        public int castToType;
        public string result;

        public ConverterEntity(int castFromType, string value, int castToType, string result) {
            this.castFromType = castFromType;
            this.value = value;
            this.castToType = castToType;
            this.result = result;
        }
    }


    public ConverterEntity getEntity() {
        return new ConverterEntity(castTypeFrom.value, value.text, castTypeTo.value, result.text);
    }

    public void loadFromEntity(ConverterEntity converterEntity) {
        castTypeFrom.value = converterEntity.castFromType;
        value.text = converterEntity.value;
        castTypeTo.value = converterEntity.castToType;
        result.text = converterEntity.result;
        onValueChanged();
        
    }
}
