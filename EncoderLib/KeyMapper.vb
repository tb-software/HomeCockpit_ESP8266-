'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Holds configurable mapping from encoder actions to key combinations.
'------------------------------------------------------------------------------
Public Class KeyMapper
    Public Property RotateUp As String = "Up"
    Public Property RotateDown As String = "Down"
    Public Property RotateUpWithButton As String = "PageUp"
    Public Property RotateDownWithButton As String = "PageDown"
    Public Property ButtonPress As String = "Enter"
    Public Property ButtonLongPress As String = "Escape"
End Class

