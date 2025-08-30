Imports System.ComponentModel

'------------------------------------------------------------------------------
'  Created: 2025-09-01
'  Edited:  2025-09-01
'  Author:  ChatGPT
'  Description: Builds user messages for Win32 keyboard simulation errors.
'------------------------------------------------------------------------------
Public Module Win32ErrorHelper
    Public Function ToMessage(ex As Win32Exception) As String
        Dim code = ex.NativeErrorCode
        If code = 5 OrElse code = 1314 Then
            Return "Die Tasten konnten nicht gesendet werden. Bitte als Administrator ausführen."
        End If
        Return $"Die Tasten konnten nicht gesendet werden. Fehler {code}: {ex.Message}"
    End Function
End Module
