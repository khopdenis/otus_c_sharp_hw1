using System;

public class Int : CustomNumeric<int> {

    private int value;
        
    public Int(int value) {
        this.value = value;
    }
        
    public CustomNumeric<int> add(CustomNumeric<int> param) {
        return of(value + param.getValue());
    }
        
    public CustomNumeric<int> substract(CustomNumeric<int> param) {
        return of(value - param.getValue());
    }

    public CustomNumeric<int> devide(CustomNumeric<int> param) {
        return of(value / param.getValue());
    }

    public CustomNumeric<int> multiply(CustomNumeric<int> param) {
        return of(value * param.getValue());
    }

    public CustomNumeric<int> power(CustomNumeric<int> param) {
        return of((int)Math.Pow((double)value, (double)param.getValue()));
    }

    public string getStringValue() {
        return value.ToString();
    }

    public int getValue() {
        return value;
    }

    public static Int of(int value) {
        return new Int(value);
    }
}
