using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;

namespace RockMargin
{
	public class SettingsStore
	{
		IEditorOptions _options;
		IVsWritableSettingsStore _store;

		public SettingsStore(IVsSettingsManager manager, IEditorOptions options)
		{
			_options = options;
			manager.GetWritableSettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out _store);
		}

		public void Load()
		{
			LoadOption(OptionsKeys.Width);
			LoadOption(OptionsKeys.HighlightsEnabled);
			LoadOption(OptionsKeys.MarginColor);
			LoadOption(OptionsKeys.ScrollColor);
			LoadOption(OptionsKeys.ThumbColor);
			LoadOption(OptionsKeys.HighlightColor);
			LoadOption(OptionsKeys.TextColor);
			LoadOption(OptionsKeys.CommentsColor);
			LoadOption(OptionsKeys.BackgroundColor);
			LoadOption(OptionsKeys.TextMarkerBackgroundColor);
			LoadOption(OptionsKeys.TextMarkerForegroundColor);
		}

		public void Save()
		{
			SaveOption(OptionsKeys.Width);
			SaveOption(OptionsKeys.HighlightsEnabled);
			SaveOption(OptionsKeys.MarginColor);
			SaveOption(OptionsKeys.ScrollColor);
			SaveOption(OptionsKeys.ThumbColor);
			SaveOption(OptionsKeys.HighlightColor);
			SaveOption(OptionsKeys.TextColor);
			SaveOption(OptionsKeys.CommentsColor);
			SaveOption(OptionsKeys.BackgroundColor);
			SaveOption(OptionsKeys.TextMarkerBackgroundColor);
			SaveOption(OptionsKeys.TextMarkerForegroundColor);
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
			int ivalue = 0;
			_store.CollectionExists(collection, out ivalue);

			if (ivalue == 0)
				_store.CreateCollection(collection);
		}
	}
}
