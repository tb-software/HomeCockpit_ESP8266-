'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Interface for sending keyboard input.
'------------------------------------------------------------------------------
Imports System.Collections.Generic
Public Interface IKeyboardSender
    Sub SendKeys(keys As IReadOnlyList(Of WindowsKey))
End Interface
