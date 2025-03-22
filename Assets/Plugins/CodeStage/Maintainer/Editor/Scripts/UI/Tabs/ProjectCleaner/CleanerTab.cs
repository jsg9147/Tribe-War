#region copyright
// -------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// -------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.UI
{
	using System;
	using System.IO;
	using System.Linq;

	using Cleaner;
	using Core;
	using Filters;
	using Settings;
	using Tools;

	using UnityEditor;
	using UnityEngine;

	internal partial class CleanerTab : RecordsTab<CleanerRecord>
	{
		private CleanerResultsStats resultsStats;

		protected override string CaptionName
		{
			get { return ProjectCleaner.ModuleName; }
		}

		protected override Texture CaptionIcon
		{
			get { return CSIcons.Clean; }
		}

		public CleanerTab(MaintainerWindow maintainerWindow) : base(maintainerWindow) {}

		protected override CleanerRecord[] LoadLastRecords()
		{
			var loadedRecords = SearchResultsStorage.CleanerSearchResults;
			if (loadedRecords == null) loadedRecords = new CleanerRecord[0];
			if (resultsStats == null) resultsStats = new CleanerResultsStats();

			return loadedRecords;
		}

		protected override RecordsTabState GetState()
		{
			return UserSettings.Cleaner.tabState;
		}

		protected override void PerformPostRefreshActions()
		{
			base.PerformPostRefreshActions();
			resultsStats.Update(filteredRecords);
		}

		protected override void DrawPagesRightHeader()
		{
			base.DrawPagesRightHeader();

			GUILayout.Label("Sorting:", GUILayout.ExpandWidth(false));

			EditorGUI.BeginChangeCheck();
			UserSettings.Cleaner.sortingType = (CleanerSortingType)EditorGUILayout.EnumPopup(UserSettings.Cleaner.sortingType, GUILayout.Width(100));
			if (EditorGUI.EndChangeCheck())
			{
				ApplySorting();
			}

			EditorGUI.BeginChangeCheck();
			UserSettings.Cleaner.sortingDirection = (SortingDirection)EditorGUILayout.EnumPopup(UserSettings.Cleaner.sortingDirection, GUILayout.Width(80));
			if (EditorGUI.EndChangeCheck())
			{
				ApplySorting();
			}
		}

		protected override void DrawRecord(CleanerRecord record, int recordIndex)
		{
			if (record == null) return;

			// hide cleaned records
			if (record.cleaned) return;

			using (new GUILayout.VerticalScope())
			{
				if (recordIndex > 0 && recordIndex < filteredRecords.Length) UIHelpers.Separator();

				using (new GUILayout.HorizontalScope())
				{
					DrawRecordCheckbox(record);
					DrawExpandCollapseButton(record);
					DrawIcon(record);

					if (record.compactMode)
					{
						DrawRecordButtons(record, recordIndex);
						GUILayout.Label(record.GetCompactLine(), UIHelpers.richLabel);
					}
					else
					{
						GUILayout.Space(5);
						GUILayout.Label(record.GetHeader(), UIHelpers.richLabel);
					}
				}

				if (!record.compactMode)
				{
					UIHelpers.Separator();
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Space(5);
						GUILayout.Label(record.GetBody(), UIHelpers.richLabel);
					}
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Space(5);
						DrawRecordButtons(record, recordIndex);
					}
					GUILayout.Space(3);
				}
			}
		}

		protected override void ApplySorting()
		{
			base.ApplySorting();

			switch (UserSettings.Cleaner.sortingType)
			{
				case CleanerSortingType.Unsorted:
					break;
				case CleanerSortingType.ByPath:
					filteredRecords = UserSettings.Cleaner.sortingDirection == SortingDirection.Ascending ?
						filteredRecords.OrderBy(RecordsSortings.cleanerRecordByPath).ToArray() :
						filteredRecords.OrderByDescending(RecordsSortings.cleanerRecordByPath).ToArray();
					break;
				case CleanerSortingType.ByType:
					filteredRecords = UserSettings.Cleaner.sortingDirection == SortingDirection.Ascending ?
						filteredRecords.OrderBy(RecordsSortings.cleanerRecordByAssetType).ThenBy(RecordsSortings.cleanerRecordByPath).ToArray() :
						filteredRecords.OrderByDescending(RecordsSortings.cleanerRecordByAssetType).ThenBy(RecordsSortings.cleanerRecordByPath).ToArray();
					break;
				case CleanerSortingType.BySize:
					filteredRecords = UserSettings.Cleaner.sortingDirection == SortingDirection.Ascending ?
						filteredRecords.OrderByDescending(RecordsSortings.cleanerRecordBySize).ThenBy(RecordsSortings.cleanerRecordByPath).ToArray() :
						filteredRecords.OrderBy(RecordsSortings.cleanerRecordBySize).ThenBy(RecordsSortings.cleanerRecordByPath).ToArray();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			filteredRecords = filteredRecords.OrderByDescending(RecordsSortings.cleanerRecordByType).ToArray();;
		}

		protected override void SaveSearchResults()
		{
			SearchResultsStorage.CleanerSearchResults = GetRecords();
			resultsStats.Update(filteredRecords);
		}

		protected override string GetModuleName()
		{
			return ProjectCleaner.ModuleName;
		}

		protected override string GetReportHeader()
		{
			return resultsStats != null ? "Total found garbage size: " + CSEditorTools.FormatBytes(resultsStats.totalSize) : null;
		}

		protected override string GetReportFileNamePart()
		{
			return "Cleaner";
		}

		protected override void AfterClearRecords()
		{
			resultsStats = null;
			SearchResultsStorage.CleanerSearchResults = null;
		}

		protected override void OnSelectionChanged()
		{
			resultsStats.Update(filteredRecords);
		}

		private void StartSearch()
		{
			StartSearch(true);
		}

		private CleanerRecord[] StartSearch(bool showResults)
		{
			window.RemoveNotification();
			var result = ProjectCleaner.StartSearch(showResults);
			window.Focus();

			return result;
		}

		private void StartClean()
		{
			window.RemoveNotification();
			ProjectCleaner.StartClean();
			window.Focus();
		}

		private void DrawRecordButtons(CleanerRecord record, int recordIndex)
		{
			DrawShowButtonIfPossible(record);

			var assetRecord = record as AssetRecord;
			if (assetRecord != null)
			{
				DrawDeleteButton(assetRecord, recordIndex);

				if (record.compactMode)
				{
					DrawMoreButton(assetRecord);
				}
				else
				{
					DrawRevealButton(assetRecord);
					DrawCopyButton(assetRecord);
					DrawMoreButton(assetRecord);
				}
			}
		}

		private void DrawIcon(CleanerRecord record)
		{
			Texture icon = null;

			var ar = record as AssetRecord;
			if (ar != null)
			{
				if (ar.isTexture)
				{
					icon = AssetPreview.GetMiniTypeThumbnail(ar.assetType);
				}

				if (icon == null)
				{
					icon = AssetDatabase.GetCachedIcon(ar.assetDatabasePath);
				}
			}

			if (record is CleanerErrorRecord)
			{
				icon = CSEditorIcons.ErrorSmallIcon;
			}
			else if (record is CleanerWarningRecord)
			{
				icon = CSEditorIcons.WarnSmallIcon;
			}

			if (icon != null)
			{
				var position = EditorGUILayout.GetControlRect(false, 16, GUILayout.Width(16));
				GUI.DrawTexture(position, icon);
			}
		}

		private void DrawDeleteButton(CleanerRecord record, int recordIndex)
		{
			if (UIHelpers.RecordButton(record, "Delete", "Deletes this single item.", CSIcons.Delete))
			{
				if (!UserSettings.Cleaner.deletionPromptShown)
				{
					UserSettings.Cleaner.deletionPromptShown = true;
					if (!EditorUtility.DisplayDialog(
						ProjectCleaner.ModuleName,
						"Please note, this action will physically remove asset file from the project! Are you sure you wish to do this?\n" +
						"Author is not responsible for any damage made due to the module usage!\n" +
						"This message shows only once.",
						"Yes", "No"))
					{
						return;
					}
				}

				if (record.Clean())
				{
					DeleteRecords(new[] { recordIndex });
				}
			}
		}

		private void DrawRevealButton(AssetRecord record)
		{
			if (UIHelpers.RecordButton(record, "Reveal", "Reveals item in system default File Manager like Explorer on Windows or Finder on Mac.", CSIcons.Reveal))
			{
				EditorUtility.RevealInFinder(record.path);
			}
		}

		private void DrawMoreButton(AssetRecord assetRecord)
		{
			if (UIHelpers.RecordButton(assetRecord, "Shows menu with additional actions for this record.", CSIcons.More))
			{
				var menu = new GenericMenu();
				if (!string.IsNullOrEmpty(assetRecord.path))
				{
					if (assetRecord.assetType == CSReflectionTools.sceneAssetType)
					{
						menu.AddItem(new GUIContent("Treat as used scene"), false, () =>
						{
							if (!CSFilterTools.IsValueMatchesAnyFilter(assetRecord.assetDatabasePath, ProjectSettings.Cleaner.sceneIgnoresFilters))
							{
								var newFilter = FilterItem.Create(assetRecord.assetDatabasePath, FilterKind.Path, false, true);
								ArrayUtility.Add(ref ProjectSettings.Cleaner.sceneIgnoresFilters, newFilter);

								MaintainerWindow.ShowNotification("Scene added as used: " + assetRecord.assetDatabasePath);
								CleanerFiltersWindow.Refresh();
								
								if (ProjectSettings.Cleaner.rescanAfterContextIgnore)
								{
									StartSearch();
								}
							}
							else
							{
								MaintainerWindow.ShowNotification("Already exists in filters");
							}
						});
					}
					
					menu.AddItem(new GUIContent("Ignore/Full Path"), false, () =>
					{
						if (!CSFilterTools.IsValueMatchesAnyFilter(assetRecord.assetDatabasePath, ProjectSettings.Cleaner.pathIgnoresFilters))
						{
							var newFilter = FilterItem.Create(assetRecord.assetDatabasePath, FilterKind.Path);
							ArrayUtility.Add(ref ProjectSettings.Cleaner.pathIgnoresFilters, newFilter);

							MaintainerWindow.ShowNotification("Ignore added: " + assetRecord.assetDatabasePath);
							CleanerFiltersWindow.Refresh();

							if (ProjectSettings.Cleaner.rescanAfterContextIgnore)
							{
								StartSearch();
							}
						}
						else
						{
							MaintainerWindow.ShowNotification("Already added to the ignores!");
						}
					});

					var dir = Directory.GetParent(assetRecord.assetDatabasePath);
					if (!CSPathTools.IsAssetsRootPath(dir.FullName))
					{
						menu.AddItem(new GUIContent("Ignore/Parent Folder"), false, () =>
						{
							var dirPath = CSPathTools.EnforceSlashes(dir.ToString());

							if (!CSFilterTools.IsValueMatchesAnyFilter(dirPath, ProjectSettings.Cleaner.pathIgnoresFilters))
							{
								var newFilter = FilterItem.Create(dirPath, FilterKind.Directory);
								ArrayUtility.Add(ref ProjectSettings.Cleaner.pathIgnoresFilters, newFilter);

								MaintainerWindow.ShowNotification("Ignore added: " + dirPath);
								CleanerFiltersWindow.Refresh();

								if (ProjectSettings.Cleaner.rescanAfterContextIgnore)
								{
									StartSearch();
								}
							}
							else
							{
								MaintainerWindow.ShowNotification("Already added to the ignores!");
							}
						});
					}

					var extension = Path.GetExtension(assetRecord.path);
					if (!string.IsNullOrEmpty(extension))
					{
						menu.AddItem(new GUIContent("Ignore/\"" + extension + "\" Extension"), false, () =>
						{
							if (!CSFilterTools.IsValueMatchesAnyFilterOfKind(extension, ProjectSettings.Cleaner.pathIgnoresFilters, FilterKind.Extension))
							{
								var newFilter = FilterItem.Create(extension, FilterKind.Extension, true);
								ArrayUtility.Add(ref ProjectSettings.Cleaner.pathIgnoresFilters, newFilter);

								MaintainerWindow.ShowNotification("Ignore added: " + extension);
								CleanerFiltersWindow.Refresh();

								if (ProjectSettings.Cleaner.rescanAfterContextIgnore)
								{
									StartSearch();
								}
							}
							else
							{
								MaintainerWindow.ShowNotification("Already added to the ignores!");
							}
						});
					}
				}
				menu.ShowAsContext();
			}
		}

		private class CleanerResultsStats
		{
			public long totalSize;
			public long selectedSize;

			public void Update(CleanerRecord[] records)
			{
				selectedSize = totalSize = 0;

				foreach (var record in records)
				{
					var assetRecord = record as AssetRecord;
					if (assetRecord == null || assetRecord.cleaned) continue;

					totalSize += assetRecord.size;
					if (assetRecord.selected) selectedSize += assetRecord.size;
				}
			}
		}
	}
}
