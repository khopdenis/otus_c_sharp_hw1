using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class DataController {
    
    private bool initIsDone;
    public bool isFloat;
    private DataModel<CustomNumeric<int>, int> intModel;
    private DataModel<CustomNumeric<float>, float> floatModel;

    public bool init(bool isFloat, string value, int arraySize, bool randomly) {
        this.isFloat = isFloat;
        if (isFloat) {
            if (randomly) {
                Float[] randomValues = new Float[arraySize];
                for (int i = 0; i < arraySize; i++) {
                    randomValues[i] = Float.of(Random.Range(-100f, 100f));
                }
                floatModel = new DataModel<CustomNumeric<float>, float>(Float.of(0), randomValues);
            }
            else {

                float val;
                try {
                    val = float.Parse(value);
                }
                catch (Exception e) {
                    Debug.Log(e);
                    return false;
                }

                floatModel = new DataModel<CustomNumeric<float>, float>(Float.of(val), arraySize);
            }
        }
        else {

            if (randomly) {
                Int[] randomValues = new Int[arraySize];
                for (int i = 0; i < arraySize; i++) {
                    randomValues[i] = Int.of(Random.Range(-100, 100));
                }
                intModel = new DataModel<CustomNumeric<int>, int>(Int.of(0), randomValues);
            }
            else {
                
                int val;
                try {
                    val = int.Parse(value);
                }
                catch (Exception e) {
                    Debug.Log(e);
                    return false;
                }

                intModel = new DataModel<CustomNumeric<int>, int>(Int.of(val), arraySize);
            }
        }

        initIsDone = true;
        return true;
    }
    
    public bool restoreStateFromHistory(DataHistoryElement historyElement) {
        isFloat = historyElement.isFloat;
        if (isFloat) {
            Float[] floats = new Float[historyElement.arraySize];

            for (int i = 0; i < historyElement.values.Length; i++) {
                floats[i] = Float.of(float.Parse(historyElement.values[i]));
            }
            floatModel = new DataModel<CustomNumeric<float>, float>(Float.of(0), floats);
        }
        else {
            Int[] ints = new Int[historyElement.arraySize];
            for (int i = 0; i < historyElement.values.Length; i++) {
                ints[i] = Int.of(int.Parse(historyElement.values[i]));
            }
            intModel = new DataModel<CustomNumeric<int>, int>(Int.of(0), ints);
        }

        initIsDone = true;
        return true;
    }

    public void DoOperation(OperationType operationType, string param, ref List<int> errorElements, ref Dictionary<int ,string> errorMessages, out List<int> updatedElements) {
        updatedElements = new List<int>();
        if (!initIsDone) {
            return;
        }
        if (isFloat) {
            float val = float.Parse(param);
            updatedElements = floatModel.updateValues(operationType, Float.of(val), ref errorElements , ref errorMessages );
        }
        else {
            int val = int.Parse(param);
            updatedElements = intModel.updateValues(operationType, Int.of(val), ref errorElements, ref errorMessages);
        }
    }
    
    public Dictionary<int, string> getStringValues() {
        
        if (isFloat) {
            return floatModel.getStringValues();
        }
        else {
            return intModel.getStringValues();
        }
    }

    

    public int getSize() {
        if (isFloat) {
            return floatModel.getSize();
        }
        else {
            return intModel.getSize();
        }
    }
}