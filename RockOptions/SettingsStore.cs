using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;

namespace RockMargin
{
	public class SettingsStore
	{
		private IEditorOptions _options;
		private IVsWritableSettingsStore _store;

		public SettingsStore(IVsSettingsManager manager, IEditorOptions options)
		{
			_options = options;
			manager.GetWritableSettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out _store);
		}

		public IEnumerable<object> GetOptionsKeys()
		{
			return typeof(OptionsKeys).GetFields(BindingFlags.Public | BindingFlags.Static).Select(field => field.GetValue(typeof(OptionsKeys)));
		}

		public void Load()
		{
			foreach (object key in GetOptionsKeys())
			{
				if (key is EditorOptionKey<bool>)
				{
					LoadOption((EditorOptionKey<bool>)key);
				}
				else if (key is EditorOptionKey<uint>)
				{
					LoadOption((EditorOptionKey<uint>)key);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public void Save()
		{
			foreach (object key in GetOptionsKeys())
			{
				if (key is EditorOptionKey<bool>)
				{
					SaveOption((EditorOptionKey<bool>)key);
				}
				else if (key is EditorOptionKey<uint>)
				{
					SaveOption((EditorOptionKey<uint>)key);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		private string GetCollectionName(string name)
		{
			return name.Split("/".ToCharArray())[0];
		}

		private string GetPropertyName(string name)
		{
			return name.Split("/".ToCharArray())[1];
		}

		private void LoadOption<T>(EditorOptionKey<T> key)
		{
			string collection = GetCollectionName(key.Name);
			string property = GetPropertyName(key.Name);

			int ivalue = 0;
			if (VSConstants.S_OK == _store.CollectionExists(collection, out ivalue))
				ivalue = 2;
			
			string svalue;
			if (VSConstants.S_OK == _store.GetString(collection, property, out svalue))
			{
				if (typeof(T) == typeof(bool))
				{
					bool value = bool.Parse(svalue);
					_options.SetOptionValue(key.Name, value);
				}
				else if (typeof(T) == typeof(uint))
				{
					uint argb = uint.Parse(svalue);
					_options.SetOptionValue(key.Name, argb);
				}
			}
		}

		private void SaveOption<T>(EditorOptionKey<T> key)
		{
			string collection = GetCollectionName(key.Name);
			string property = GetPropertyName(key.Name);

			EnsureCollectionExists(collection);

			var value = _options.GetOptionValue(key);
			_store.SetString(collection, property, value.ToString());
		}

		private void EnsureCollectionExists(string collection)
		{
			_store.CollectionExists(collection, out int ivalue);

			if (ivalue == 0)
				_store.CreateCollection(collection);
		}
	}
}
