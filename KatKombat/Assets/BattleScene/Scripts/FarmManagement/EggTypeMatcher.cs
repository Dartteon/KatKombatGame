using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EggType = KatBreed.EggType;

public class EggTypeMatcher : MonoBehaviour {

	[SerializeField]
	private Sprite[] eggSprites;

	public void matchSpriteToEggType (EggType e) {
		switch (e) {

		case EggType.BlueYellowStripe:
			matchSprite (eggSprites[0]);
			break;
		case EggType.GreenYellowSpore:
			matchSprite (eggSprites[1]);
			break;
		case EggType.GreyShard:
			matchSprite (eggSprites[2]);
			break;

		case EggType.OrangeArcaneCircle:
			matchSprite (eggSprites[3]);
			break;

		case EggType.PinkDot:
			matchSprite (eggSprites[4]);
			break;

		case EggType.RedYellowBoomerang:
			matchSprite (eggSprites[5]);
			break;

		case EggType.RedYellowWrap:
			matchSprite (eggSprites[6]);
			break;

		//case EggType.SkyBlueDiamond:
		default:
			matchSprite (eggSprites[7]);
			break;
		}
	}

	void matchSprite (Sprite s) {
		this.transform.Find ("EggSprite").GetComponent<SpriteRenderer> ().sprite = s;
	}
}
