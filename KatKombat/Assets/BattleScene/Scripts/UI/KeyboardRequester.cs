using UnityEngine;
using System.Collections;

public interface KeyboardRequester {
	void handleInput (string input);
	void handleEnter();
}
