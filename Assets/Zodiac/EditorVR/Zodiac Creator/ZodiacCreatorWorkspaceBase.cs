#if UNITY_EDITOR
using System.Collections.Generic;


using UnityEngine;

using System.Linq;

using UnityEditor;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Workspaces;
using UnityEditor.Experimental.EditorVR.Data;
using UnityEditor.Experimental.EditorVR.Handles;
using UnityEditor.Experimental.EditorVR.Utilities;
using Object = UnityEngine.Object;

namespace Zodiac.EditorVR
{
	abstract class ZodiacCreatorWorkspaceBase : Workspace, ISelectionChanged,ISerializeWorkspace
	{
		public new static readonly Vector3 DefaultBounds = new Vector3 (0.3f, 0.1f, 0.5f);

		[SerializeField]
		GameObject m_ContentPrefab;

		[SerializeField]
		GameObject m_LockPrefab;

		InspectorUI m_InspectorUI;
		GameObject m_SelectedObject;
		LockUI m_LockUI;

		bool m_Scrolling;

		bool m_IsLocked;

		//HERE BEGINETH ZODIAC STUFF--------------------------------------------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------------------------------------------------------------------
		public void OnSelectionChanged ()
		{
			if (m_IsLocked)
				return;
			m_SelectedObject = Selection.activeGameObject;
			UpdateInspectorData (m_SelectedObject, true);
		}

		public object OnSerializeWorkspace ()
		{
			return m_InspectorUI.listView.data;
		}

		public void OnDeserializeWorkspace (object obj)
		{
			m_InspectorUI.listView.data = (List<InspectorData>)obj;
		}

		//selection is not really nessecary
		void UpdateInspectorData (GameObject selection, bool fullReload)
		{
			if (fullReload) {
				m_InspectorUI.listView.data = GetNewData ();
			} else {
				//updates each serializedobject in the listview
				m_InspectorUI.listView.OnObjectModified ();
			}
		}

		protected virtual List<InspectorData> GetNewData ()
		{
			return new List<InspectorData> ();
		}

		//HERE ENDETH ZODIAC STUFF--------------------------------------------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------------------------------------------------------------------


		public override void Setup ()
		{
			// Initial bounds must be set before the base.Setup() is called
			minBounds = new Vector3 (0.375f, MinBounds.y, 0.3f);
			m_CustomStartingBounds = new Vector3 (0.375f, MinBounds.y, 0.6f);

			base.Setup ();
			var contentPrefab = ObjectUtils.Instantiate (m_ContentPrefab, m_WorkspaceUI.sceneContainer, false);
			m_InspectorUI = contentPrefab.GetComponent<InspectorUI> ();

			m_LockUI = ObjectUtils.Instantiate (m_LockPrefab, m_WorkspaceUI.frontPanel, false).GetComponentInChildren<LockUI> ();
			this.ConnectInterfaces (m_LockUI);
			m_LockUI.lockButtonPressed += SetIsLocked;
			EditorApplication.delayCall += m_LockUI.Setup; // Need to write stencilRef after WorkspaceButton does it

			var listView = m_InspectorUI.listView;
			this.ConnectInterfaces (listView);
			listView.data = new List<InspectorData> ();
			//listView.arraySizeChanged += OnArraySizeChanged;	//wont be changing array size

			var scrollHandle = m_InspectorUI.scrollHandle;
			scrollHandle.dragStarted += OnScrollDragStarted;
			scrollHandle.dragging += OnScrollDragging;
			scrollHandle.dragEnded += OnScrollDragEnded;
			scrollHandle.hoverStarted += OnScrollHoverStarted;
			scrollHandle.hoverEnded += OnScrollHoverEnded;

			contentBounds = new Bounds (Vector3.zero, m_CustomStartingBounds.Value);

			var scrollHandleTransform = m_InspectorUI.scrollHandle.transform;
			scrollHandleTransform.SetParent (m_WorkspaceUI.topFaceContainer);
			scrollHandleTransform.localScale = new Vector3 (1.03f, 0.02f, 1.02f); // Extra space for scrolling
			scrollHandleTransform.localPosition = new Vector3 (0f, -0.01f, 0f); // Offset from content for collision purposes


			Undo.postprocessModifications += OnPostprocessModifications;
			Undo.undoRedoPerformed += OnUndoRedo;

		}

		protected void AddChildrenChangingCallback (PropertyData propertyData)
		{
			propertyData.childrenChanging += m_InspectorUI.listView.OnBeforeChildrenChanged;
		}

		void OnUndoRedo ()
		{
			UpdateCurrentObject (true);
		}

		void OnScrollDragStarted (BaseHandle handle, HandleEventData eventData = default(HandleEventData))
		{
			m_Scrolling = true;

			m_WorkspaceUI.topHighlight.visible = true;
			m_WorkspaceUI.amplifyTopHighlight = false;

			m_InspectorUI.listView.OnBeginScrolling ();
		}

		void OnScrollDragging (BaseHandle handle, HandleEventData eventData = default(HandleEventData))
		{
			m_InspectorUI.listView.scrollOffset += Vector3.Dot (eventData.deltaPosition, handle.transform.forward) / this.GetViewerScale ();
		}

		void OnScrollDragEnded (BaseHandle handle, HandleEventData eventData = default(HandleEventData))
		{
			m_Scrolling = false;

			m_WorkspaceUI.topHighlight.visible = false;

			m_InspectorUI.listView.OnScrollEnded ();
		}

		void OnScrollHoverStarted (BaseHandle handle, HandleEventData eventData = default(HandleEventData))
		{
			if (!m_Scrolling) {
				m_WorkspaceUI.topHighlight.visible = true;
				m_WorkspaceUI.amplifyTopHighlight = true;
			}
		}

		void OnScrollHoverEnded (BaseHandle handle, HandleEventData eventData = default(HandleEventData))
		{
			if (!m_Scrolling) {
				m_WorkspaceUI.topHighlight.visible = false;
				m_WorkspaceUI.amplifyTopHighlight = false;
			}
		}

		UndoPropertyModification[] OnPostprocessModifications (UndoPropertyModification[] modifications)
		{
			if (!m_SelectedObject || !IncludesCurrentObject (modifications))
				return modifications;

			UpdateCurrentObject (false);

			return modifications;
		}

		bool IncludesCurrentObject (UndoPropertyModification[] modifications)
		{
			foreach (var modification in modifications) {
				if (modification.previousValue.target == m_SelectedObject)
					return true;

				if (modification.currentValue.target == m_SelectedObject)
					return true;

				foreach (var component in m_SelectedObject.GetComponents<Component>()) {
					if (modification.previousValue.target == component)
						return true;

					if (modification.currentValue.target == component)
						return true;
				}
			}

			return false;
		}

		void UpdateCurrentObject (bool fullReload)
		{
			if (m_SelectedObject)
				UpdateInspectorData (m_SelectedObject, fullReload);
		}

		protected override void OnBoundsChanged ()
		{
			var size = contentBounds.size;
			var listView = m_InspectorUI.listView;
			var bounds = contentBounds;
			size.y = float.MaxValue; // Add height for dropdowns
			size.x -= 0.04f; // Shrink the content width, so that there is space allowed to grab and scroll
			size.z -= 0.15f; // Reduce the height of the inspector contents as to fit within the bounds of the workspace
			bounds.size = size;
			listView.bounds = bounds;

			var listPanel = m_InspectorUI.listPanel;
			listPanel.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, size.x);
			listPanel.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, size.z);
		}

		void SetIsLocked ()
		{
			m_IsLocked = !m_IsLocked;
			m_LockUI.UpdateIcon (m_IsLocked);

			if (!m_IsLocked)
				OnSelectionChanged ();
		}

		protected override void OnDestroy ()
		{
			Undo.postprocessModifications -= OnPostprocessModifications;
			Undo.undoRedoPerformed -= OnUndoRedo;
			base.OnDestroy ();
		}


	}
}
#endif
