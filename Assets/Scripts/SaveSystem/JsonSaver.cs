using System.IO;
using UnityEngine;

namespace CardProject
{
	public class JsonSaver : ISaver<SaveData>
	{
		private const string FileName = "gameSave.json";

		public void Save(SaveData param)
		{
			var str = JsonUtility.ToJson(param);
			File.WriteAllText(Path.Combine(Application.persistentDataPath, FileName), str);
		}

		public SaveData Load()
		{
			if (!File.Exists(Path.Combine(Application.persistentDataPath, FileName)))
			{
				return new SaveData();
			}

			var str = File.ReadAllText(Path.Combine(Application.persistentDataPath, FileName));
			return JsonUtility.FromJson<SaveData>(str);
		}
	}
}