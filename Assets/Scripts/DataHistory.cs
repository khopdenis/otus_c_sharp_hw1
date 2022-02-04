using System;
using System.Collections.Generic;

[Serializable]
public struct DataHistory {
        
    public DataHistory(List<DataHistoryElement> elements) {
        this.historyElements = new List<DataHistoryElement>();
    }

    public List<DataHistoryElement> historyElements;

    public void addDataHistoryElement(DataHistoryElement historyElement) {
        historyElements.Add(historyElement);
    }

    public int getNextId() {
        return historyElements.Count;
    }

    public Dictionary<int, string> getIdsWithDescriptions() {
        Dictionary<int, string> result = new Dictionary<int, string>();
        foreach (DataHistoryElement historyElement in historyElements) {
            result.Add(historyElement.id, historyElement.description);
        }
        return result;
    }
}

[Serializable]
public struct DataHistoryElement {
    public DataHistoryElement(int id, bool isFloat, int arraySize, string description, string[] values) {
        this.id = id;
        this.isFloat = isFloat;
        this.arraySize = arraySize;
        this.description = description;
        this.values = values;
    }
        
    public int id;
    public bool isFloat;
    public int arraySize;
    public string description;
    public string[] values;
}