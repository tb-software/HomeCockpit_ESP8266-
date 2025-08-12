Imports System.Windows
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Window allowing configuration of key mapping.
'------------------------------------------------------------------------------
Namespace EncoderWpfApp
    Partial Class KeyMappingWindow
        Inherits Window

        Private ReadOnly mapping As KeyMapper

        Public Sub New(mapping As KeyMapper)
            InitializeComponent()
            Me.mapping = mapping
            Dim values = [Enum].GetNames(GetType(WindowsKey))
            RotateUpBox.ItemsSource = values
            RotateDownBox.ItemsSource = values
            RotateUpBtnBox.ItemsSource = values
            RotateDownBtnBox.ItemsSource = values
            ButtonPressBox.ItemsSource = values
            ButtonLongPressBox.ItemsSource = values

            RotateUpBox.Text = mapping.RotateUp
            RotateDownBox.Text = mapping.RotateDown
            RotateUpBtnBox.Text = mapping.RotateUpWithButton
            RotateDownBtnBox.Text = mapping.RotateDownWithButton
            ButtonPressBox.Text = mapping.ButtonPress
            ButtonLongPressBox.Text = mapping.ButtonLongPress
        End Sub

        Private Sub Ok_Click(sender As Object, e As RoutedEventArgs)
            mapping.RotateUp = RotateUpBox.Text
            mapping.RotateDown = RotateDownBox.Text
            mapping.RotateUpWithButton = RotateUpBtnBox.Text
            mapping.RotateDownWithButton = RotateDownBtnBox.Text
            mapping.ButtonPress = ButtonPressBox.Text
            mapping.ButtonLongPress = ButtonLongPressBox.Text
            DialogResult = True
        End Sub
    End Class
End Namespace

