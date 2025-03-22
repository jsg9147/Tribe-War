#region copyright
// -------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// -------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Issues
{
	using System;

	[Serializable]
	public abstract class AssetIssueRecord : IssueRecord
	{
		public string Path { get; private set; }

		protected AssetIssueRecord(IssueKind kind, RecordLocation location, string path) : base(kind, location)
		{
			Path = path;
		}
	}
}
