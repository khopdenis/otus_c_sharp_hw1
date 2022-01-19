using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [SerializeField] public List<GameObject> converters;
    [SerializeField] public InputField filePath;

    public void onCastAllButtonPressed() {
        foreach (GameObject converter in converters) {
            Converter script = converter.GetComponent<Converter>();
            script.onCastButtonPressed();
        }
        
        //
        
    }

    private ConverterFormEntity getFormEntity() {
        List<Converter.ConverterEntity> entities = new List<Converter.ConverterEntity>();
        foreach (GameObject converter in converters) {
            Converter script = converter.GetComponent<Converter>();
            entities.Add(script.getEntity());
        }
        ConverterFormEntity formEntity = new ConverterFormEntity(entities);
        return formEntity;
    }

    private string getJson() {
        return JsonUtility.ToJson(getFormEntity());
    }
    
    public void onSaveFormButtonPressed() {
        string json = getJson();
        System.IO.File.WriteAllText(filePath.text, json);
        Debug.Log("Form saved to file: " + filePath.text);
        Debug.Log("Json file saved: " + json);
    }
    
    public void onLoadFormButtonPressed() {
        string json = System.IO.File.ReadAllText(filePath.text);
        ConverterFormEntity formEntity = JsonUtility.FromJson<ConverterFormEntity>(json);
        List<Converter.ConverterEntity> converterEntities = formEntity.converters;

        int i = 0;
        foreach (GameObject converter in converters) {
            Converter script = converter.GetComponent<Converter>();
            
            script.loadFromEntity(converterEntities[i]);
            i++;
        }

        Debug.Log("Form loaded from file with path: " + filePath.text);
    }
    

    public void onSetRandomValuesButtonPressed() {
        foreach (GameObject converter in converters) {
            Converter script = converter.GetComponent<Converter>();
            script.setValidRandomValue();
            script.onValueChanged();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        filePath.text = Application.dataPath + "/form_data.json";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [Serializable]
    public class ConverterFormEntity {
        public List<Converter.ConverterEntity> converters;
        public ConverterFormEntity(List<Converter.ConverterEntity> converters) {
            this.converters = converters;
        }

        public string getJson() {
            return JsonUtility.ToJson(this);
        }
    }
}
