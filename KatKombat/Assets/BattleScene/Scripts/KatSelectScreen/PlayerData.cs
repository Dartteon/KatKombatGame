using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

[Serializable]
public class PlayerData{
	public int highScore;

	void Awake(){
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
	}
}
