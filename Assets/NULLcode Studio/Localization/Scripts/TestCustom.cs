using UnityEngine;
using System.Collections;

public class TestCustom : MonoBehaviour {

	// тестируем смену текста, после нажатия кнопки
	public void MyButton(int index)
	{
		// отправляем id тега custom, взятый из XML файла локали, и отправляем номер, который должен соответствовать тексту
		Localization.Internal.Custom(-1292960392, index);
	}
}
