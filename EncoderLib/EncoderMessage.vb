'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Represents an encoder rotation message.
'------------------------------------------------------------------------------
Public Class EncoderMessage
    Inherits HardwareMessage

    Public Sub New(position As Integer, direction As RotationDirection)
        Me.Position = position
        Me.Direction = direction
    End Sub

    Public ReadOnly Property Position As Integer
    Public ReadOnly Property Direction As RotationDirection
End Class
