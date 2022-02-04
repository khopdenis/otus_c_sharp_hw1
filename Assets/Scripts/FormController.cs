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
    [SerializeField] private int arrayElementsXStep = 200;
    [SerializeField] private int arrayElementsYOffset = 0;
    [SerializeField] private int arrayElementsXOffset = 0;

    [SerializeField] private InputField filePathSerializableInputField;
    [SerializeField] private InputField filePathJsonInputField;


    private Dictionary<int, GameObject> arrayElementComponents = new Dictionary<int, GameObject>();

    private DataHistory history = new DataHistory(new List<DataHistoryElement>());

    public void Awake() {
        filePathSerializableInputField.text = Application.dataPath + "/serializable.history";
        filePathJsonInputField.text = Application.dataPath + "/json_history.json";
    }


    public void onArrayInitButtonPressed() {
        if (!validateInitValue()) {
            return;
        }

        dataController.init(
            dataTypeDropdown.value == 1,
            sourceValueInputField.text,
            int.Parse(arraySizeInputField.text),
            randomSourceValueToggle.isOn);

        RenderElements();
        string arrayType = dataController.isFloat ? "FLOAT" : "INT";
        MakeDataHistorySnapshot(currentDateTime() + " Init of " + arrayType + " array, size = " +
                                dataController.getSize());
        operationsMenu.SetActive(true);
    }

    public void onHistroyElementChanged() {
        int historyDropdownValue = historyDropdown.value;
        restoreStateFromHistory(historyDropdownValue);
        RenderElements();
    }

    private void MakeDataHistorySnapshot(string operationDescription) {
        Dictionary<int, string> currentValues = dataController.getStringValues();
        int id = history.getNextId();
        string[] values = new string[currentValues.Count];
        foreach (KeyValuePair<int, string> keyValuePair in currentValues) {
            values[keyValuePair.Key] = keyValuePair.Value;
        }

        DataHistoryElement historyElement = new DataHistoryElement(history.getNextId(), dataController.isFloat,
            dataController.getSize(), operationDescription, values);
        history.addDataHistoryElement(historyElement);
        renderHistory(id);
    }

    private void restoreStateFromHistory(int id) {
        DataHistoryElement historyElement = history.historyElements[id];
        dataController.restoreStateFromHistory(historyElement);
    }

    private void renderHistory(int id) {
        historyDropdown.options.Clear();
        foreach (KeyValuePair<int, string> pair in history.getIdsWithDescriptions()) {
            historyDropdown.options.Add(new Dropdown.OptionData(pair.Value));
        }

        historyDropdown.value = id;
        historyDropdown.RefreshShownValue();
    }

    // OUT and REF parameters used here..
    public void onDoOperationButtonPressed() {
        if (!validateParamValue()) {
            return;
        }
        OperationType operationType = dropdownToOperationType(operationTypeDropdown.value);
        string param = parameterValueInputField.text;
        List<int> errorElements = new List<int>();
        Dictionary<int, string> errorMessages = new Dictionary<int, string>();
        dataController.DoOperation(operationType, param, ref errorElements, ref errorMessages, out var validElements);
        RenderElements();
        MarkElements(validElements, errorElements);
        ShowErrorMEssages(errorMessages);
        string arrayType = dataController.isFloat ? " FLOAT" : " INT";
        string operationDescription =
            currentDateTime() + arrayType + " array, result of operation '" + operationType + "' with param " + param;
        MakeDataHistorySnapshot(operationDescription);
    }

    public void onSaveHistorySerializableButtonPressed() {
        string filePath = filePathSerializableInputField.text;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
            binaryFormatter.Serialize(fileStream, history);
            Debug.Log("History saved to binary file => " + filePath);
        }
    }

    public void onLoadHistorySerializableButtonPressed() {
        string filePath = filePathSerializableInputField.text;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
            history = (DataHistory) binaryFormatter.Deserialize(fileStream);
            Debug.Log("History was loaded from binary file => " + filePath);
        }

        renderHistory(history.historyElements.Count);
        operationsMenu.SetActive(true);
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
        history = JsonUtility.FromJson<DataHistory>(json);
        Debug.Log("History was loaded from json file => " + filePath);
        renderHistory(history.historyElements.Count);
        operationsMenu.SetActive(true);
    }

    private string currentDateTime() {
        return "[" + DateTime.Now + "]";
    }

    private void ShowErrorMEssages(Dictionary<int, string> errorMessages) {
        foreach (KeyValuePair<int, string> keyValuePair in errorMessages) {
            Debug.LogWarning("Operation with element [" + keyValuePair.Key +
                             "] is not well completed! Error message -> " + keyValuePair.Value);
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
        return (OperationType) val;
    }

    public void onRandomSourceValueToggleChanged() {
        if (randomSourceValueToggle.isOn) {
            sourceValueInputField.text = "";
            sourceValueInputField.enabled = false;
        }
        else {
            sourceValueInputField.enabled = true;
        }

        validateInitValue();
    }


    private void RenderElements() {
        cleanUpArrayElementComponents();
        int i = 0;
        foreach (var keyValuePair in dataController.getStringValues()) {
            Vector3 pos = arrayElementsContainer.transform.position;
            float y = arrayElementsYOffset + pos.y - arrayElementsYStep * (i%10);
            float x = arrayElementsXOffset + pos.x + ((int)i/10) * arrayElementsXStep;
            Vector3 elementPosition = new Vector3(x, y, pos.z);
            GameObject element = Instantiate(arrayElementComponentPrefab, elementPosition, Quaternion.identity,
                arrayElementsContainer.transform);
            arrayElementComponents.Add(keyValuePair.Key, element);
            ArrayElementController script = element.GetComponent<ArrayElementController>();
            script.RenderData(keyValuePair.Key, keyValuePair.Value);
            i++;
        }
    }


    private void cleanUpArrayElementComponents() {
        foreach (KeyValuePair<int, GameObject> component in arrayElementComponents) {
            Destroy(component.Value);
        }

        arrayElementComponents.Clear();
    }

    public void onArraySizeValueChanged() {
        if (arraySizeInputField.text == "") {
            arraySizeInputField.text = "1";
            return;
        }

        if (int.Parse(arraySizeInputField.text) > 40) {
            arraySizeInputField.text = "40";
        }
        else if (int.Parse(arraySizeInputField.text) < 1) {
            arraySizeInputField.text = "1";
        }
    }

    public bool validateInitValue() {
        if (!randomSourceValueToggle.isOn) {
            if (sourceValueInputField.text == "") {
                sourceValueInputField.image.color = Color.red;
                return false;
            }
        }

        sourceValueInputField.image.color = Color.white;
        return true;
    }

    public void onInitValueChanged() {
        validateInitValue();
    }

    public bool validateParamValue() {
        if (parameterValueInputField.text == "") {
            parameterValueInputField.image.color = Color.red;
            return false;
        }
        parameterValueInputField.image.color = Color.white;
        return true;
    }
    
    public void onParameterValueChanged() {
        validateParamValue();
    }
}