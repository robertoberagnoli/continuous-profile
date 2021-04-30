using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour {

    private TextMeshPro textMesh;
    public string newText = "Ho cambiato il testo";
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = newText;
	}
	
	// Update is called once per frame
	void Update () {
        //textMesh.text = newText;
    }

    public void changeText(string newText)
    {
        textMesh.text = newText;
    }
}
