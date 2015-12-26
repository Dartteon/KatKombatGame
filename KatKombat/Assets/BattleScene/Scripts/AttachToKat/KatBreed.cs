using UnityEngine;
using System.Collections;
using Egg = KatBreed.EggType;

public class KatBreed : MonoBehaviour {
	public enum Breed
	{
		None,
		Dustfang,
		Wildfang,
		Roughfang,
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
	};

	public static Breed getRandomBreed() {
		Breed[] breeds = {Breed.Dustfang, Breed.Wildfang, Breed.Roughfang, Breed.Triphire, Breed.Trisard, Breed.Trimerald, 
			Breed.Voltyr, Breed.Oceanyr, Breed.Lavyr, Breed.Junglyr, Breed.Pyrozard, Breed.Cyrozard, Breed.Wildfang, Breed.Rubyzard, Breed.Wraithzard};
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
		KatBreed.EggType[] possibleColors = {KatBreed.EggType.BlueYellowStripe, KatBreed.EggType.PinkDot, KatBreed.EggType.GreyShard, KatBreed.EggType.GreenYellowSpore, KatBreed.EggType.OrangeArcaneCircle,
			KatBreed.EggType.RedYellowWrap, KatBreed.EggType.SkyBlueDiamond, KatBreed.EggType.RedYellowBoomerang};

		int index = Random.Range (0, possibleColors.Length);
		return possibleColors[index];
	}
		
}
