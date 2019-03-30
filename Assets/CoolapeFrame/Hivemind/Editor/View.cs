using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace Hivemind
{

	enum Mode
	{
		NodeAction,
		CanvasAction,
		DragNode,
		PanCanvas,
		ConnectParent,
		ConnectChild,
		ConnectKillChild,
		ConnectLeftChild,
		ConnectLeftKillChild,
		ConnectRightChild,
		ConnectRightKillChild,
		InvokeMenu,
		None

	}

	public class View
	{

		GridRenderer gridRenderer;
		Rect canvas;
		public Vector2 scrollPoint = Vector2.zero;
		NodeRenderer nodeRenderer;

		public Editor nodeInspector;

		BTEditorWindow editorWindow;
		
		Mode currentMode = Mode.None;
		Node contextNode;
		Vector2 initialMousePosition = Vector2.zero;
		public List<Node> selectedNodes = new List<Node> ();

		Vector2 nodeActionOffset = Vector2.zero;

		public View (BTEditorWindow owner)
		{
			editorWindow = owner;
			canvas = new Rect (0, 0, owner.position.width, owner.position.height);
		}

		void DrawNodes (List<Node> nodes)
		{
			if (nodeRenderer == null)
				nodeRenderer = new NodeRenderer ();

			int count = nodes.Count;
			for (int i = 0; i < count; i++) {
				if (nodes [i] != null) {
					nodeRenderer.Draw (nodes [i], selectedNodes.Contains (nodes [i]));
				}
			}
		}

		public bool Draw (Rect position)
		{
			bool needsRepaint = HandleMouseEvents (position, BTEditorManager.Manager.behaviorTree.nodes);

			scrollPoint = GUI.BeginScrollView (new Rect (0, 0, position.width, position.height), scrollPoint, canvas);
			
			if (gridRenderer == null)
				gridRenderer = new GridRenderer ();
			gridRenderer.Draw (scrollPoint, canvas);

			DrawNodes (BTEditorManager.Manager.behaviorTree.nodes);
			if (currentMode == Mode.ConnectChild || currentMode == Mode.ConnectParent
			    || currentMode == Mode.ConnectLeftChild || currentMode == Mode.ConnectRightChild) {
				DrawConnectionLine (Color.white);
				needsRepaint = true;
			} else if (currentMode == Mode.ConnectKillChild
			           || currentMode == Mode.ConnectRightKillChild
			           || currentMode == Mode.ConnectLeftKillChild) {
				DrawConnectionLine (Color.red);
				needsRepaint = true;
			}

			GUI.EndScrollView ();

			return needsRepaint;
		}

		void DrawConnectionLine (Color color)
		{

			Vector3 startPos = Vector3.zero;
			Vector3 startTan = Vector3.zero;
			Vector3 endPos = new Vector3 (Event.current.mousePosition.x, Event.current.mousePosition.y, 0);
			Vector3 endTan = Vector3.zero;

			if (currentMode == Mode.ConnectParent) {
				startPos = new Vector3 (contextNode.editorPosition.x + (NodeRenderer.Width / 2), contextNode.editorPosition.y, 0);
				startTan = startPos + Vector3.down * GridRenderer.step.x * 2;
				endTan = endPos + Vector3.up * GridRenderer.step.x * 2;
			} else if (currentMode == Mode.ConnectChild || currentMode == Mode.ConnectKillChild) {
				startPos = new Vector3 (contextNode.editorPosition.x + (NodeRenderer.Width / 2), contextNode.editorPosition.y + NodeRenderer.Height, 0);
				startTan = startPos + Vector3.up * GridRenderer.step.x * 2;
				endTan = endPos + Vector3.down * GridRenderer.step.x * 2;
			} else if (currentMode == Mode.ConnectLeftChild || currentMode == Mode.ConnectLeftKillChild) {
				startPos = new Vector3 (contextNode.editorPosition.x + (NodeRenderer.Width / 2) - (NodeRenderer.Width / 4), contextNode.editorPosition.y + NodeRenderer.Height, 0);
				startTan = startPos + Vector3.up * GridRenderer.step.x * 2;
				endTan = endPos + Vector3.down * GridRenderer.step.x * 2;
			} else if (currentMode == Mode.ConnectRightChild || currentMode == Mode.ConnectRightKillChild) {
				startPos = new Vector3 (contextNode.editorPosition.x + (NodeRenderer.Width / 2) + (NodeRenderer.Width / 4), contextNode.editorPosition.y + NodeRenderer.Height, 0);
				startTan = startPos + Vector3.up * GridRenderer.step.x * 2;
				endTan = endPos + Vector3.down * GridRenderer.step.x * 2;
			}

			Handles.DrawBezier (startPos, endPos, startTan, endTan, color, null, 4);
//			Handles.ArrowCap(0, endPos, Quaternion.FromToRotation(startPos, endPos), 54);
			Handles.DrawWireCube (endPos, Vector3.one * 10);
		}

		// Returns true if needs a repaint
		bool HandleMouseEvents (Rect position, List<Node> nodes)
		{

			// MouseDown //

			// Identify the control being clicked
			if (Event.current.type == EventType.MouseDown) {
				
				// Do nothing for MouseDown on the horizontal scrollbar, if present
				if (canvas.width > position.width && Event.current.mousePosition.y >= position.height - 20) {
					currentMode = Mode.None;
				}
				
				// Do nothing for MouseDown on the vertical scrollbar, if present
				else if (canvas.height > position.height && Event.current.mousePosition.x >= position.width - 20) {
					currentMode = Mode.None;
				}

				// MouseDown in the canvas, check if in a node or on background
				else {

					// Store the mouse position
					initialMousePosition = Event.current.mousePosition;

					// Loop through nodes and check if their rects contain the mouse position
					for (int i = 0; i < nodes.Count; i++) {
						if (nodes [i] != null && nodeRenderer.rectForNode (nodes [i], scrollPoint).Contains (Event.current.mousePosition)) {

							// Connect a parent to a child
							if (contextNode is NodeBranch) {
								if (currentMode == Mode.ConnectLeftChild) {
									BTEditorManager.Manager.ConnectLeft ((NodeBranch)contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								} else if (currentMode == Mode.ConnectLeftKillChild) {
									BTEditorManager.Manager.ConnectLeftKill ((NodeBranch)contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								} else if (currentMode == Mode.ConnectRightChild) {
									BTEditorManager.Manager.ConnectRight ((NodeBranch)contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								} else if (currentMode == Mode.ConnectRightKillChild) {
									BTEditorManager.Manager.ConnectRightKill ((NodeBranch)contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								} else if (currentMode == Mode.ConnectParent) {
									BTEditorManager.Manager.Connect (nodes [i], contextNode);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								}
								// Perform a node action at key up
								else {
									currentMode = Mode.NodeAction;
									contextNode = nodes [i];
									nodeActionOffset = Event.current.mousePosition - nodes [i].editorPosition;
								}
							} else {
								if (currentMode == Mode.ConnectChild) {
									BTEditorManager.Manager.Connect (contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								} else if (currentMode == Mode.ConnectKillChild) {
									BTEditorManager.Manager.ConnectKill (contextNode, nodes [i]);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								}  
								// Connect a child to a parent
								else if (currentMode == Mode.ConnectParent) {
									BTEditorManager.Manager.Connect (nodes [i], contextNode);
									editorWindow.wantsMouseMove = false;
									currentMode = Mode.None;
									break;
								}
								// Perform a node action at key up
								else {
									currentMode = Mode.NodeAction;
									contextNode = nodes [i];
									nodeActionOffset = Event.current.mousePosition - nodes [i].editorPosition;
								}
							}
						}
					}

					// Cancel the connection
					if (currentMode == Mode.ConnectParent || currentMode == Mode.ConnectChild
					    || currentMode == Mode.ConnectLeftChild || currentMode == Mode.ConnectRightChild
					    || currentMode == Mode.ConnectKillChild || currentMode == Mode.ConnectLeftKillChild
					    || currentMode == Mode.ConnectRightKillChild) {
						editorWindow.wantsMouseMove = false;
						currentMode = Mode.None;
					}

					// MouseDown on the canvas background enables panning the view
					if (currentMode == Mode.None) {
						currentMode = Mode.CanvasAction;
					}
				}
			}

			// Mouse Up //

			// MouseUp resets the current interaction mode to None
			if (Event.current.type == EventType.MouseUp) {

				// Select node
				if (currentMode == Mode.NodeAction && Event.current.button == 0) {
					currentMode = Mode.None;
					SelectNode (contextNode);
					return true;
				}

				// Deselect node
				else if (currentMode == Mode.CanvasAction && Event.current.button == 0) {
					SelectNode (null);
					currentMode = Mode.None;
					return true;
				}

				// Context Menu
				else if (Event.current.button == 1) {

					if (currentMode == Mode.NodeAction) {
						editorWindow.ShowContextMenu (Event.current.mousePosition, contextNode);
					} else if (currentMode == Mode.CanvasAction) {
						editorWindow.ShowContextMenu (Event.current.mousePosition, null);
					}

					currentMode = Mode.None;
				}

				// Resize canvas after a drag

				else if (currentMode == Mode.DragNode) {
					ResizeCanvas ();
					currentMode = Mode.None;
					return true;
				} else {
					currentMode = Mode.None;
				}

			}

			// Mouse Drag //

			if (Event.current.type == EventType.MouseDrag && Event.current.button == 0) {

				// Switch to Pan mode
				if (currentMode == Mode.CanvasAction) {
					currentMode = Mode.PanCanvas;
				}

				// Switch to node dragging mode
				if (currentMode == Mode.NodeAction && contextNode != null) {

					float deltaX = Mathf.Abs (Event.current.mousePosition.x - initialMousePosition.x);
					float deltaY = Mathf.Abs (Event.current.mousePosition.y - initialMousePosition.y);

					// Ignore mouse drags inside nodes lesser than the grid step. These would be rounded,
					// and make selecting a node slightly more difficult.
					if (deltaX >= GridRenderer.step.x || deltaY >= GridRenderer.step.y) {
						currentMode = Mode.DragNode;
					}
				}

				// Pan if the mouse drag initiated by MouseDown outside any windows
				if (currentMode == Mode.PanCanvas) {
					scrollPoint.x += -Event.current.delta.x;
					scrollPoint.y += -Event.current.delta.y;
					currentMode = Mode.PanCanvas;
					return true;
				}

				// Drag a node
				if (currentMode == Mode.DragNode) {
					Vector2 newPositionAbs = Event.current.mousePosition - nodeActionOffset;
					float x = newPositionAbs.x - (newPositionAbs.x % GridRenderer.step.x);
					float y = newPositionAbs.y - (newPositionAbs.y % GridRenderer.step.y);
					DragNode (contextNode, new Vector2 (x, y));
					currentMode = Mode.DragNode;
					return true;
				}
			}

			return false;
		}

		public void ResizeCanvas ()
		{
			Rect newCanvas = new Rect (0, 0, editorWindow.position.width, editorWindow.position.height);
			foreach (Node node in BTEditorManager.Manager.behaviorTree.nodes) {
				float xOffset = node.editorPosition.x + NodeRenderer.Width + GridRenderer.step.x * 2;
				if (xOffset > newCanvas.width) {
					newCanvas.width = xOffset;
				}
				float yOffset = node.editorPosition.y + NodeRenderer.Height + GridRenderer.step.y * 2;
				if (yOffset > newCanvas.height) {
					newCanvas.height = yOffset;
				}
				canvas = newCanvas;
			}
		}

		public void SelectNode (Node node)
		{
			if (node == null) {
				selectedNodes.Clear ();
			} else {
				if (Event.current != null && (Event.current.command || Event.current.control)) {
					if (!selectedNodes.Contains (node)) {
						selectedNodes.Add (node);
					}
				} else {
					selectedNodes.Clear ();
					selectedNodes.Add (node);
				}
			}
			Editor nodeInspector = Editor.CreateEditor (node);

			if (nodeInspector != null) {
				BTEditorManager.Manager.nodeInspector = nodeInspector;
				BTEditorManager.Manager.nodeInspector.Repaint ();
			} else if (BTEditorManager.Manager.btInspector != null) {
				BTEditorManager.Manager.nodeInspector = null;
				BTEditorManager.Manager.btInspector.Repaint ();
			}
		}

		private void DragNode (Node node, Vector2 newPosition)
		{

//			if (Application.isPlaying) {
//				return;
//			}
			Hashtable map = new Hashtable ();
			doDragNode (node, newPosition, ref map);
			map.Clear ();
			map = null;
		}

		private void doDragNode (Node node, Vector2 newPosition, ref Hashtable map)
		{
			Node _node;
			map [node] = true;
			if (Event.current.shift) {
				if (node.ChildCount > 0) {
					for (int i = 0; i < node.ChildCount; i++) {
						_node = node.Children [i];
						Vector2 childOffset = _node.editorPosition - node.editorPosition;
						Vector2 newChildPosition = newPosition + childOffset;
						if (map [_node] == null) {
							doDragNode (_node, newChildPosition, ref  map);
						}
					}
				}

				for (int i = 0; i < node.KillNodes.Count; i++) {
					_node = node.KillNodes [i];
					Vector2 childOffset = _node.editorPosition - node.editorPosition;
					Vector2 newChildPosition = newPosition + childOffset;
					if (map [_node] == null) {
						doDragNode (_node, newChildPosition, ref  map);
					}
				}

				if (node is NodeBranch) {
					for (int i = 0; i < ((NodeBranch)node).ChildrenLeft.Count; i++) {
						_node = ((NodeBranch)node).ChildrenLeft [i];
						Vector2 childOffset = _node.editorPosition - node.editorPosition;
						Vector2 newChildPosition = newPosition + childOffset;
						if (map [_node] == null) {
							doDragNode (_node, newChildPosition, ref map);
						}
					}

					for (int i = 0; i < ((NodeBranch)node).KillLeftNodes.Count; i++) {
						_node = ((NodeBranch)node).KillLeftNodes [i];
						Vector2 childOffset = _node.editorPosition - node.editorPosition;
						Vector2 newChildPosition = newPosition + childOffset;
						if (map [_node] == null) {
							doDragNode (_node, newChildPosition, ref map);
						}
					}

					for (int i = 0; i < ((NodeBranch)node).ChildrenRight.Count; i++) {
						_node = ((NodeBranch)node).ChildrenRight [i];
						Vector2 childOffset = _node.editorPosition - node.editorPosition;
						Vector2 newChildPosition = newPosition + childOffset;
						if (map [_node] == null) {
							doDragNode (_node, newChildPosition, ref map);
						}
					}

					for (int i = 0; i < ((NodeBranch)node).KillRightNodes.Count; i++) {
						_node = ((NodeBranch)node).KillRightNodes [i];
						Vector2 childOffset = _node.editorPosition - node.editorPosition;
						Vector2 newChildPosition = newPosition + childOffset;
						if (map [_node] == null) {
							doDragNode (_node, newChildPosition, ref map);
						}
					}
				}
			}

			BTEditorManager.Manager.SetEditorPosition (node, newPosition);
		}

		public void ConnectParent (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectParent;
		}

		public void ConnectChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectChild;
		}

		public void ConnectKillChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectKillChild;
		}

		public void ConnectLeftChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectLeftChild;
		}

		public void ConnectLeftKillChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectLeftKillChild;
		}

		public void ConnectRightChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectRightChild;
		}

		public void ConnectRightKillChild (Node node)
		{
			editorWindow.wantsMouseMove = true;
			contextNode = node;
			currentMode = Mode.ConnectRightKillChild;
		}
	}

}