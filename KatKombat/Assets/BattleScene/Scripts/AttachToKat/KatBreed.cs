using UnityEngine;
using System.Collections;
using Egg = KatBreed.EggType;

public class KatBreed : MonoBehaviour {
	private static Breed[] breeds = {Breed.Dustfang, Breed.Wildfang, Breed.Roughfang, Breed.Triphire, Breed.Trisard, Breed.Trimerald, 
		Breed.Voltyr, Breed.Oceanyr, Breed.Lavyr, Breed.Junglyr, Breed.Pyrozard, Breed.Cyrozard, Breed.Wildfang, Breed.Rubyzard, Breed.Wraithzard};

	private static KatBreed.EggType[] possibleEggColors = {KatBreed.EggType.BlueYellowStripe, KatBreed.EggType.PinkDot, KatBreed.EggType.GreyShard, KatBreed.EggType.GreenYellowSpore, KatBreed.EggType.OrangeArcaneCircle,
		KatBreed.EggType.RedYellowWrap, KatBreed.EggType.SkyBlueDiamond, KatBreed.EggType.RedYellowBoomerang};

	public enum Breed
	{
		None,
		Dustfang,
		Wildfang,
		Roughfang,
		Goldfang,
		Triphire,
		Trisard,
		Trimerald,
		Voltyr,
		Oceanyr,
		Lavyr,
		Junglyr,
		Pyrozard,
		Cyrozard,
		Wispzard,
		Rubyzard,
		Wraithzard,
		Triprism,
	};

	public static Breed getRandomBreed() {
		return breeds[Random.Range (0, breeds.Length)];
	}

	public enum BreedVariant
	{
		None,
		Red,
		Green,
		Blue,
		Dark,
	};

	public enum EggType
	{
		None,
		BlueYellowStripe,
		PinkDot,
		GreyShard,
		GreenYellowSpore,
		OrangeArcaneCircle,
		RedYellowWrap,
		SkyBlueDiamond,
		RedYellowBoomerang,
	}

	public static KatBreed.EggType getRandomEggColor() {

		int index = Random.Range (0, possibleEggColors.Length);
		return possibleEggColors[index];
	}

	public static Breed getRandomRare2Breed() {
		Breed[] rare2Breeds = {Breed.Goldfang, Breed.Triprism};

		return rare2Breeds[Random.Range (0, rare2Breeds.Length)];
	}
		
}
