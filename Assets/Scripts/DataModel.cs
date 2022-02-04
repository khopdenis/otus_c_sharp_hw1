using System;
using System.Collections.Generic;


public class DataModel<CustomNumeric, T> {
    
    private CustomNumeric<T> sourceValue;
    private CustomNumeric<T>[] array;

    public DataModel(CustomNumeric<T> sourceValue, CustomNumeric<T>[] array) {
        this.sourceValue = sourceValue;
        this.array = array;
    }
    
    public DataModel(CustomNumeric<T> sourceValue, int arraySize) {
        this.sourceValue = sourceValue;
        this.array = new CustomNumeric<T>[arraySize];
        // init all elements of array by source value
        for (int i = 0; i < arraySize; i++) {
            this.array[i] = sourceValue;
        }
    }
    
    public List<int> updateValues(OperationType operationType, CustomNumeric<T> param, ref List<int> errorElements, ref Dictionary<int, string > errorMessages ) {
        List<int> updatedElements = new List<int>();
        for (int i = 0; i < array.Length; i++) {
            try {
                makeOperation(operationType, param, i);
                updatedElements.Add(i);
            }
            catch (Exception e) {
                errorElements.Add(i);
                errorMessages.Add(i, e.Message);
            }
        }
        return updatedElements;
    }

    private void makeOperation(OperationType operationType, CustomNumeric<T> param, int i) {
        switch (operationType) {
            case OperationType.add:
                array[i] = array[i].add(param);
                break;
            case OperationType.substract:
                array[i] = array[i].substract(param);
                break;
            case OperationType.multiply:
                array[i] = array[i].multiply(param);
                break;
            case OperationType.devide:
                array[i] = array[i].devide(param);
                break;
            case OperationType.power:
                array[i] = array[i].power(param);
                break;
        }
    }

    public CustomNumeric<T>[] getValues() {
        return array;
    }
    
    
    public Dictionary<int, string> getStringValues() {
        Dictionary<int, string>  result = new Dictionary<int, string>(); 
        for (int i = 0; i < array.Length; i++) {
            result.Add(i, array[i].getStringValue());    
            
        }
        return result;
    }

    public int getSize() {
        return array.Length;
    }

}
