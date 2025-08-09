Imports System.Windows
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Window allowing configuration of key mapping.
'------------------------------------------------------------------------------
Public Class KeyMappingWindow
    Inherits Window

    Private ReadOnly mapping As KeyMapper

    Public Sub New(mapping As KeyMapper)
        InitializeComponent()
        Me.mapping = mapping
        Dim values = [Enum].GetValues(GetType(WindowsKey))
        RotateUpBox.ItemsSource = values
        RotateDownBox.ItemsSource = values
        RotateUpBtnBox.ItemsSource = values
        RotateDownBtnBox.ItemsSource = values
        ButtonPressBox.ItemsSource = values
        ButtonLongPressBox.ItemsSource = values

        RotateUpBox.SelectedItem = mapping.RotateUp
        RotateDownBox.SelectedItem = mapping.RotateDown
        RotateUpBtnBox.SelectedItem = mapping.RotateUpWithButton
        RotateDownBtnBox.SelectedItem = mapping.RotateDownWithButton
        ButtonPressBox.SelectedItem = mapping.ButtonPress
        ButtonLongPressBox.SelectedItem = mapping.ButtonLongPress
    End Sub

    Private Sub Ok_Click(sender As Object, e As RoutedEventArgs)
        mapping.RotateUp = CType(RotateUpBox.SelectedItem, WindowsKey)
        mapping.RotateDown = CType(RotateDownBox.SelectedItem, WindowsKey)
        mapping.RotateUpWithButton = CType(RotateUpBtnBox.SelectedItem, WindowsKey)
        mapping.RotateDownWithButton = CType(RotateDownBtnBox.SelectedItem, WindowsKey)
        mapping.ButtonPress = CType(ButtonPressBox.SelectedItem, WindowsKey)
        mapping.ButtonLongPress = CType(ButtonLongPressBox.SelectedItem, WindowsKey)
        DialogResult = True
    End Sub
End Class

