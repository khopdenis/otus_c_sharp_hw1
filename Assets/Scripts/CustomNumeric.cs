public interface CustomNumeric<T> {
    CustomNumeric<T> add(CustomNumeric<T> param);
    CustomNumeric<T> substract(CustomNumeric<T> param);
    CustomNumeric<T> devide(CustomNumeric<T> param);
    CustomNumeric<T> multiply(CustomNumeric<T> param);
    CustomNumeric<T> power(CustomNumeric<T> param);
    
    string getStringValue();

    T getValue();
    
}