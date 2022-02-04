using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class FormController : MonoBehaviour {
    private DataController dataController = new DataController();

    [SerializeField] private GameObject arrayInitMenu;
    [SerializeField] private Dropdown dataTypeDropdown;
    [SerializeField] private InputField arraySizeInputField;
    [SerializeField] private InputField sourceValueInputField;
    [SerializeField] private Toggle randomSourceValueToggle;

    
    [SerializeField] private GameObject operationsMenu;
    [SerializeField] private Dropdown operationTypeDropdown;
    [SerializeField] private InputField parameterValueInputField;
    

    [SerializeField] private GameObject arrayElementsContainer;
    [SerializeField] private GameObject arrayElementComponentPrefab;
    
    [SerializeField] private Dropdown historyDropdown;


    [SerializeField] private int arrayElementsYStep = 42;
    [SerializeField] private int arrayElementsYOffset = 0;
    
    [SerializeField] private InputField filePathSerializableInputField;
    
    [SerializeField] private InputField filePathJsonInputField;
    

    
    private Dictionary<int, GameObject> arrayElementComponents = new Dictionary<int, GameObject>();
    
    private DataHistory history = new DataHistory(new List<DataHistoryElement>());

    public void Awake() {
        filePathSerializableInputField.text = Application.dataPath + "/serializable.history";
        filePathJsonInputField.text = Application.dataPath + "/json_history.json";
    }


    public void onArrayInitButtonPressed() {
        dataController.init(
            dataTypeDropdown.value == 1,
             sourceValueInputField.text,
            int.Parse(arraySizeInputField.text),
            randomSourceValueToggle.isOn);
        
        RenderElements();
        string arrayType = dataController.isFloat ? "FLOAT" : "INT";
        MakeDataHistorySnapshot(currentDateTime() +" Init of "+arrayType+" array, size = "+ dataController.getSize());
    }

    public void onHistroyElementChanged() {
        int historyDropdownValue = historyDropdown.value;
        restoreStateFromHistory(historyDropdownValue);
        RenderElements();
    }

    private void MakeDataHistorySnapshot(string operationDescription) {
        Dictionary<int,string> currentValues = dataController.getStringValues();
        int id = history.getNextId();
        string[] values = new string[currentValues.Count];
        foreach (KeyValuePair<int,string> keyValuePair in currentValues) {
            values[keyValuePair.Key] = keyValuePair.Value;
        }
        DataHistoryElement historyElement = new DataHistoryElement(history.getNextId(), dataController.isFloat, dataController.getSize(), operationDescription, values);
        history.addDataHistoryElement(historyElement);
        renderHistory(id);
    }

    private void restoreStateFromHistory(int id) {
        DataHistoryElement historyElement = history.historyElements[id];
        dataController.restoreStateFromHistory(historyElement);
        
    }

    private void renderHistory(int id) {
        historyDropdown.options.Clear();
        foreach (KeyValuePair<int,string> pair in history.getIdsWithDescriptions()) {
            historyDropdown.options.Add(new Dropdown.OptionData(pair.Value));
        }

        historyDropdown.value = id;
        historyDropdown.RefreshShownValue();
    }


    public void onDoOperationButtonPressed() {
        OperationType operationType = dropdownToOperationType(operationTypeDropdown.value);
        string param = parameterValueInputField.text;
        List<int> errorElements = new List<int>();
        Dictionary<int, string> errorMessages = new Dictionary<int, string>();
        List<int> validElements = dataController.DoOperation(operationType, param, ref errorElements, ref errorMessages);
        RenderElements();
        MarkElements(validElements, errorElements);
        ShowErrorMEssages(errorMessages);
        string operationDescription =
            currentDateTime() + " After operation " + operationType + " for " + validElements.Count;
        MakeDataHistorySnapshot(operationDescription);
    }

    public void onSaveHistorySerializableButtonPressed() {
        string filePath = filePathSerializableInputField.text;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
            binaryFormatter.Serialize(fileStream, history);
            Debug.Log("History saved to binary file => "+ filePath);
        }
    }
    
    public void onLoadHistorySerializableButtonPressed() {
        string filePath = filePathSerializableInputField.text;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
            history = (DataHistory)binaryFormatter.Deserialize(fileStream);
            Debug.Log("History was loaded from binary file => "+ filePath);
        }
        renderHistory(history.historyElements.Count);
    }
    
    
    public void onSaveHistoryJsonButtonPressed() {
        string filePath = filePathJsonInputField.text;
        string json = JsonUtility.ToJson(history);
        File.WriteAllText(filePath, json);
        Debug.Log("History saved to json file: " + filePath);
    }
    
    public void onLoadHistoryJsonButtonPressed() {
        string filePath = filePathJsonInputField.text;
        string json = File.ReadAllText(filePath);
        history = (DataHistory)JsonUtility.FromJson<DataHistory>(json);
        Debug.Log("History was loaded from json file => "+ filePath);
        renderHistory(history.historyElements.Count);
    }

    private string currentDateTime() {
        return "["+DateTime.Now+"]"; 
    }

    private void ShowErrorMEssages(Dictionary<int,string> errorMessages) {
        foreach (KeyValuePair<int,string> keyValuePair in errorMessages) {
            Debug.LogWarning("Operation with element ["+keyValuePair.Key+"] is not well completed! Error message -> " + keyValuePair.Value);
        }
    }

    private void MarkElements(List<int> validElements, List<int> errorElements) {
        foreach (int validElementIndex in validElements) {
            GameObject validElement = arrayElementComponents[validElementIndex];
            ArrayElementController script = validElement.GetComponent<ArrayElementController>();
            script.MarkAsValidResultOfOperation();
        }    
        foreach (int errorElementIndex in errorElements) {
            GameObject errorElement = arrayElementComponents[errorElementIndex];
            ArrayElementController script = errorElement.GetComponent<ArrayElementController>();
            script.MarkAsErrorResultOfOperation();
        }
    }

    private OperationType dropdownToOperationType(int val) {
        return (OperationType)val;
    }

    public void onRandomSourceValueToggleChanged() {
        if (randomSourceValueToggle.isOn) {
            sourceValueInputField.text = "";
            sourceValueInputField.enabled = false;
        }
        else {
            sourceValueInputField.enabled = true;
        }
    }
    

    private void RenderElements() {
        cleanUpArrayElementComponents();
        int i = 0;
        foreach (var keyValuePair in dataController.getStringValues()) {
            Vector3 pos = arrayElementsContainer.transform.position;
            Vector3 elementPosition = new Vector3(pos.x, arrayElementsYOffset + pos.y - arrayElementsYStep * i, pos.z);
            GameObject element = Instantiate(arrayElementComponentPrefab, elementPosition, Quaternion.identity,
                arrayElementsContainer.transform);
            arrayElementComponents.Add(keyValuePair.Key, element);
            ArrayElementController script = element.GetComponent<ArrayElementController>();
            script.RenderData(keyValuePair.Key, keyValuePair.Value);
            i++;
        }
    }
    
    
    private void cleanUpArrayElementComponents() {
        foreach (KeyValuePair<int,GameObject> component in arrayElementComponents) {
            Destroy(component.Value);
        }
        arrayElementComponents.Clear();
    }


}