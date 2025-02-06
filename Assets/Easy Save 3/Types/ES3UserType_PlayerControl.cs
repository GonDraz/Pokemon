using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_currentState")]
	public class ES3UserType_PlayerControl : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerControl() : base(typeof(Player.PlayerControl)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Player.PlayerControl)obj;
			
			writer.WritePrivateField("_currentState", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Player.PlayerControl)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_currentState":
					instance = (Player.PlayerControl)reader.SetPrivateField("_currentState", reader.Read<Player.PlayerControl.PlayerState>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PlayerControlArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerControlArray() : base(typeof(Player.PlayerControl[]), ES3UserType_PlayerControl.Instance)
		{
			Instance = this;
		}
	}
}