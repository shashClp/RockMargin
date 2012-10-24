using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockMargin
{
	class CommentsTracker
	{
		class CommentsDefinition
		{
			public readonly string SINGLE_LINE;
			public readonly string MULTI_LINE_START;
			public readonly string MULTI_LINE_END;

			public CommentsDefinition(string single_line, string multi_line_start, string multi_line_end)
			{
				SINGLE_LINE = single_line;
				MULTI_LINE_START = multi_line_start;
				MULTI_LINE_END = multi_line_end;
			}

			public bool IsSingleLineCommentsSupported()
			{
				return SINGLE_LINE != null;
			}

			public bool IsMultiLineCommentsSupported()
			{
				return MULTI_LINE_START != null;
			}
		}

		enum CommentType
		{
			None,
			SingleLine,
			MultiLine,
		}

		CommentType _current = CommentType.None;
		CommentsDefinition _definition;

		private CommentsTracker(CommentsDefinition definition)
		{
			_definition = definition;
		}

		public static CommentsTracker Create(string language)
		{
			CommentsDefinition definition = null;
			switch (language)
			{
				case "CSharp":
				case "C/C++":
					definition = new CommentsDefinition("//", "/*", "*/");
					break;

				case "Lua":
					definition = new CommentsDefinition("--", "--[[", "]]");
					break;

				case "Basic":
					definition = new CommentsDefinition("'", null, null);
					break;

				case "XML":
					definition = new CommentsDefinition(null, "<!--", "-->");
					break;
			}
			return definition != null ? new CommentsTracker(definition) : null;
		}

		public bool TrackComment(string s, int index)
		{
			switch (_current)
			{
				case CommentType.None:
					if (_definition.IsSingleLineCommentsSupported() && StartsWith(s, index, _definition.SINGLE_LINE))
						_current = CommentType.SingleLine;
					else if (_definition.IsMultiLineCommentsSupported() && StartsWith(s, index, _definition.MULTI_LINE_START))
						_current = CommentType.MultiLine;
					break;

				case CommentType.MultiLine:
					if (EndsWith(s, index, _definition.MULTI_LINE_END))
					{
						_current = CommentType.None;
						return true;
					}
					break;
			}

			return _current != CommentType.None;
		}

		public void NewLine()
		{
			if (_current == CommentType.SingleLine)
				_current = CommentType.None;
		}

		static private bool StartsWith(string s, int index, string value)
		{
			if (index + value.Length > s.Length)
				return false;

			for (int i = 0; i < value.Length; ++i)
			{
				if (s[i + index] != value[i])
					return false;
			}

			return true;
		}

		static private bool EndsWith(string s, int index, string value)
		{
			if (index - value.Length < 0)
				return false;

			for (int i = 0; i < value.Length; ++i)
			{
				if (s[index - value.Length + i + 1] != value[i])
					return false;
			}

			return true;
		}
	}
}
