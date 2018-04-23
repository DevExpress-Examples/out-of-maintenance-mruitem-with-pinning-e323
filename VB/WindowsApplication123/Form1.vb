Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Repository

Namespace WindowsApplication123
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()

			mru = New MRUItemManager(barManager1, barLinkContainerItem1)
		End Sub

		Private mru As MRUItemManager

		Private Sub barButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barButtonItem1.ItemClick
		End Sub

		Private Sub barButtonItem2_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs) Handles barButtonItem2.ItemClick
			Dim res As DialogResult = openFileDialog1.ShowDialog()
			If res <> System.Windows.Forms.DialogResult.OK Then
				Return
			End If
			mru.Add(openFileDialog1.FileName)
		End Sub
	End Class

	Public Class MRUItemManager
		Private manager As BarManager
		Private list As List(Of BarEditItem)
		Private maxItems As Integer
		Private riCheck As RepositoryItemCheckEdit
		Private container As BarLinkContainerItem

		Public Sub New(ByVal manager As BarManager, ByVal container As BarLinkContainerItem)
			Me.manager = manager
			Me.list = New List(Of BarEditItem)()
			maxItems = 5
			riCheck = New RepositoryItemCheckEdit()
			riCheck.Caption = ""
			riCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			manager.RepositoryItems.Add(riCheck)
			Me.container = container
		End Sub

		Public Sub Add(ByVal obj As Object)
			If obj Is Nothing Then
				Return
			End If
			Dim item As BarEditItem = Nothing
			For i As Integer = 0 To list.Count - 1
				If Equals(list(i).Tag, obj) Then
					item = list(i)
					Exit For
				End If
			Next i
			If item Is Nothing Then
				item = New BarEditItem()
				manager.Items.Add(item)
				item.Edit = riCheck
				item.AutoFillWidth = True
				item.Caption = obj.ToString()
				item.Tag = obj
				item.EditValue = False
				AddHandler item.ItemClick, AddressOf item_ItemClick
				list.Add(item)
			End If
			Touch(item)
		End Sub

		Private Sub item_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			e.Item.Manager.CloseMenus()
			Touch(TryCast(e.Item, BarEditItem))
		End Sub

		Protected Sub Touch(ByVal item As BarEditItem)
			list.Remove(item)
			list.Insert(0, item)
			Dim i As Integer = list.Count - 1
			Do While list.Count > maxItems AndAlso i >= 0
				Dim pin As Boolean = Equals(list(i).EditValue, True)
				If (Not pin) Then
					Dim xitem As BarEditItem = list(i)
					list.RemoveAt(i)
					xitem.Dispose()
				End If
				i -= 1
			Loop
			UpdateList()
		End Sub

		Public Sub UpdateList()
			container.ItemLinks.Clear()
			For i As Integer = 0 To list.Count - 1
				container.AddItem(list(i))
			Next i
		End Sub
	End Class
End Namespace