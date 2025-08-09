Imports System

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-10
'  Author:  ChatGPT
'  Description: Processes hardware messages and triggers keyboard actions.
'------------------------------------------------------------------------------
Public Class EncoderInputProcessor

    Private ReadOnly keyboard As IKeyboardSender
    Public Property Mapper As KeyMapper
    Private lastPosition As Integer?
    Private buttonPressed As Boolean
    Private buttonPressStart As DateTime
    Private lastButtonSignal As DateTime
    Private ReadOnly releaseThreshold As TimeSpan = TimeSpan.FromMilliseconds(400)
    Private ReadOnly longPressThreshold As TimeSpan = TimeSpan.FromMilliseconds(800)

    Public Sub New(keyboard As IKeyboardSender)
        Me.keyboard = keyboard
        Me.Mapper = New KeyMapper()
    End Sub

    Public Sub New(keyboard As IKeyboardSender, mapper As KeyMapper)
        Me.keyboard = keyboard
        Me.Mapper = mapper
    End Sub

    Public Sub Process(message As HardwareMessage, timestamp As DateTime)
        If TypeOf message Is ButtonMessage Then
            ProcessButtonPress(timestamp)
        ElseIf TypeOf message Is EncoderMessage Then
            ProcessEncoder(CType(message, EncoderMessage))
        End If
        CheckButtonRelease(timestamp)
    End Sub

    Private Sub ProcessButtonPress(timestamp As DateTime)
        If Not buttonPressed Then
            buttonPressed = True
            buttonPressStart = timestamp
        End If
        lastButtonSignal = timestamp
    End Sub

    Private Sub ProcessEncoder(msg As EncoderMessage)
        If lastPosition.HasValue Then
            Dim stepCount = msg.Position - lastPosition.Value
            If stepCount <> 0 Then
                Dim key As WindowsKey
                If buttonPressed Then
                    key = If(stepCount > 0, Mapper.RotateUpWithButton, Mapper.RotateDownWithButton)
                Else
                    key = If(stepCount > 0, Mapper.RotateUp, Mapper.RotateDown)
                End If
                For i = 1 To Math.Abs(stepCount)
                    keyboard.SendKey(key)
                Next
            End If
        End If
        lastPosition = msg.Position
    End Sub

    Private Sub CheckButtonRelease(timestamp As DateTime)
        If buttonPressed AndAlso timestamp - lastButtonSignal > releaseThreshold Then
            Dim duration = timestamp - buttonPressStart
            If duration >= longPressThreshold Then
                keyboard.SendKey(Mapper.ButtonLongPress)
            Else
                keyboard.SendKey(Mapper.ButtonPress)
            End If
            buttonPressed = False
        End If
    End Sub
End Class
