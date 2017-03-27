using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatUI : MonoBehaviour{
	[SerializeField] private float min, max;
	[SerializeField] private float number;
	[SerializeField] private Text textDisplay;
	[SerializeField] private Slider slider;
	[SerializeField] private InputField textInput;

	public void setMinAndMax(float min, float max){
		this.min = min;
		this.max = max;
		slider.minValue = min;
		slider.maxValue = max;
	}

	void Start(){
		slider.minValue = min;
		slider.maxValue = max;
		textInput.contentType = InputField.ContentType.DecimalNumber;
		textInput.onValueChanged.AddListener(delegate{
			newInputValue();
		});
		slider.onValueChanged.AddListener(delegate{
			sliderChange();
		});

		updateValue(number, 0);
	}

	/// <summary>
	/// listener for the InputField
	/// </summary>
	public void newInputValue(){
		updateValue(float.Parse(textInput.text), 1);
	}

	/// <summary>
	/// listener for the silder
	/// </summary>
	public void sliderChange(){
		updateValue(slider.value, 2);
	}

	public void updateValue(float newValue){
		updateValue(newValue, 0);
	}

	/// <summary>
	/// Updates the value.
	/// </summary>
	/// <param name="newValue">New value.</param>
	/// <param name="updatedThrough">0 = other, 1 = text input, 2 = slider</param>
	public void updateValue(float newValue, byte updatedThrough){
		number = newValue;

		number = Mathf.Max(min, number);
		number = Mathf.Min(max, number);



		if(textDisplay != null)
			textDisplay.text = number.ToString();

		if(slider != null && updatedThrough != 2)
			slider.value = number;

		if(textInput != null && updatedThrough != 1)
			textInput.text = number.ToString();
	}

	public float getFloat(){
		return number;
	}


}
