using UnityEngine;
using System.Collections;

public class OptionsChooserButton : MonoBehaviour, Tappable {
	private OptionsChooser parent;
	private enum Direction { Left, Right, };
	[SerializeField]
	private Direction direction;

	void Start() {
		parent = this.transform.parent.GetComponent<OptionsChooser> ();
	}

	void OnMouseDown() {
		execute ();
	}

	public void handleTap(Vector2 pos1, Vector2 pos2) {
		execute ();
	}

	void execute() {
		switch (direction) {
		case (Direction.Left):
			parent.shiftLeft();
			break;
		case (Direction.Right):
			parent.shiftRight();
			break;
		}
	}
}
