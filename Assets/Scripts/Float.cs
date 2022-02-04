using System;

public class Float : CustomNumeric<float> {

    private float value;
        
    public Float(float value) {
        this.value = value;
    }

    public CustomNumeric<float> add(CustomNumeric<float> param) {
        return of(value + param.getValue());
    }

    public CustomNumeric<float> substract(CustomNumeric<float> param) {
        return of(value - param.getValue());
    }

    public CustomNumeric<float> devide(CustomNumeric<float> param) {
        return of(value / param.getValue());
    }

    public CustomNumeric<float> multiply(CustomNumeric<float> param) {
        return of(value * param.getValue());
    }

    public CustomNumeric<float> power(CustomNumeric<float> param) {
        return of((float)Math.Pow(value, param.getValue()));
    }

    public string getStringValue() {
        return value.ToString();
    }

    public float getValue() {
        return value;
    }
    
    public static Float of(float value) {
        return new Float(value);
    }
}