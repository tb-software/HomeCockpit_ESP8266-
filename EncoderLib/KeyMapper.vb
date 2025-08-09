'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Holds configurable mapping from encoder actions to Windows keys.
'------------------------------------------------------------------------------
Public Class KeyMapper
    Public Property RotateUp As WindowsKey = WindowsKey.Up
    Public Property RotateDown As WindowsKey = WindowsKey.Down
    Public Property RotateUpWithButton As WindowsKey = WindowsKey.PageUp
    Public Property RotateDownWithButton As WindowsKey = WindowsKey.PageDown
    Public Property ButtonPress As WindowsKey = WindowsKey.Enter
    Public Property ButtonLongPress As WindowsKey = WindowsKey.Escape
End Class

