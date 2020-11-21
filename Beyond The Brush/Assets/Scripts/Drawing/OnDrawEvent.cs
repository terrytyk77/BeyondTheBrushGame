using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;
using UnityEngine;

public class OnDrawEvent : MonoBehaviour
{
	public void OnRecognize(RecognitionResult result)
	{
		StopAllCoroutines();

		
		//Handle the shape
		if (result != RecognitionResult.Empty)
		{
			Debug.Log(result.gesture.id);
		}
        else
        {
			Debug.Log("Doesn't know the shape");
        }

	}

}
