using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrayElementController : MonoBehaviour {

    [SerializeField] private Text index;
    [SerializeField] private InputField value;

    public void RenderData(int index, string value) {
        this.index.text = index.ToString();
        this.value.text = value;
        this.value.enabled = false;
    }

    public void MarkAsValidResultOfOperation() {
        value.image.color = Color.green;
        
    }
    
    public void MarkAsErrorResultOfOperation() {
        value.image.color = Color.red;
        
    }


}
