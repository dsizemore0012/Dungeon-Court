Option Strict On
Option Explicit On

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Audio
Imports Microsoft.Xna.Framework.Content
Imports Microsoft.Xna.Framework.GamerServices
Imports Microsoft.Xna.Framework.Media
'To Do:
'need to finish fixing the multiple dialogue problems when speaking with NPCs.  change the dictionary of string,dialogue to a list with just dialogues with associated dialogue name property.
'the collection of a collection of dialogues needs to be taken out and replaced with a collection of dialogues
Public Class Form1

End Class
Public Class Game
    Inherits Microsoft.Xna.Framework.Game
    'Fields in our game graphic manager etc'
    Dim graphics As GraphicsDeviceManager
    Dim spriteBatch As SpriteBatch
    Dim objwriter As New System.IO.StreamWriter(Directory.GetCurrentDirectory + "\debug.txt")
    Dim txtMap As Texture2D
    Dim txtCursor As Texture2D
    Dim txtDialogueBox As Texture2D
    Dim txtFire As Texture2D
    Dim bolDrawFire As Boolean = False
    Dim intFireFrames As Integer = 0
    Dim colFireFrames As New System.Collections.Generic.List(Of Rectangle)
    Dim intFireCurrentFrame As Integer = 0
    Dim Cursor As New Cursor
    Dim GameState As String = "Map"
    Dim colFlags As New System.Collections.Generic.List(Of String)
    Dim intCursorTime As Integer = 0 ' time it takes for the cursor to be able to move again
    Dim Location1 As New Vector2(355, 225)
    Dim Location2 As New Vector2(385, 225)
    Dim Location3 As New Vector2(355, 257)
    Dim Location4 As New Vector2(385, 257)
    Dim Location5 As New Vector2(355, 289)
    Dim Location6 As New Vector2(385, 289)
    Dim Location7 As New Vector2(355, 321)
    Dim Location8 As New Vector2(385, 321)
    Dim Location9 As New Vector2(0, 0)
    Dim Location10 As New Vector2(0, 0)
    Dim Location11 As New Vector2(0, 0)
    Dim Location12 As New Vector2(0, 0)
    Dim Location13 As New Vector2(0, 0)
    Dim Location14 As New Vector2(0, 0)
    Dim Bard As New PlayerCharacter

    Dim WhiteMage As New NonPlayerCharacter
    Dim Darthen As New NonPlayerCharacter
    Dim Lana As New NonPlayerCharacter
    Dim Tamalia As New NonPlayerCharacter
    Dim Person1 As New NonPlayerCharacter
    Dim Person2 As New NonPlayerCharacter
    Dim Person3 As New NonPlayerCharacter
    Dim Person4 As New NonPlayerCharacter
    Dim Person5 As New NonPlayerCharacter
    Dim Person6 As New NonPlayerCharacter
    Dim Person7 As New NonPlayerCharacter
    Dim Person8 As New NonPlayerCharacter
    Dim Person9 As New NonPlayerCharacter
    Dim HenchmanHouse As New House
    Dim Graveyard As New House
    Dim TitleScreen As New House
    Dim Tavern As New House
    Dim Font1 As SpriteFont
    Dim colScript As New System.Collections.Generic.List(Of System.Collections.Generic.List(Of String))
    'The script is read from one line at a time and executed
    'colDialogue contains all the pieces of Dialogue for a scene
    Dim colDialogue As New System.Collections.Generic.List(Of System.Collections.Generic.List(Of Dialogue))
    Dim CurrentLine As System.Collections.Generic.List(Of String)
    Dim CurrentHouse As House
    Dim colCurrentNPCs As New System.Collections.Generic.List(Of NonPlayerCharacter)
    Dim ActiveNPC As NonPlayerCharacter ' this is used for the NPC that is currently being given commands
    Dim bolDrawPC As Boolean = False
    Dim Index As Integer = 0
    Dim DialogueToDraw As New System.Collections.Generic.List(Of Dialogue)
    Dim DialogPosition As String
    Dim WaitTimer As New Stopwatch
    Dim bolScriptDialogueLock As Boolean = False
    'these are for debugging
    Dim txtActionKey As Texture2D
    Dim txtActionlock As Texture2D
    Dim txtDialoguekey1 As Texture2D
    Dim txtdialoguekey2 As Texture2D
    Dim txtdialoguekey3 As Texture2D

    
    
    Public Sub New()
        graphics = New GraphicsDeviceManager(Me)

    End Sub

    Protected Overrides Sub Initialize()
        MyBase.Initialize()
        'graphics.IsFullScreen = True
        'graphics.PreferredBackBufferHeight = 768
        'graphics.PreferredBackBufferWidth = 1366
        'graphics.ApplyChanges()
        Font1 = Content.Load(Of SpriteFont)("myfont")
        graphics.PreferredBackBufferWidth = 1120
        graphics.PreferredBackBufferHeight = 640
        graphics.ApplyChanges()
        Bard.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Bard.png"))
        WhiteMage.spriteSheet = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\WhiteMage.png")
        Darthen.spriteSheet = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Darthen.png")
        Lana.spriteSheet = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Lana.png")
        Tamalia.spriteSheet = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Tamalia.png")
        Person1.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person1.png"))
        Person2.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person2.png"))
        Person3.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person3.png"))
        Person4.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person4.png"))
        Person5.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person5.png"))
        Person6.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person6.png"))
        Person7.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person7.png"))
        Person8.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person8.png"))
        Person9.SetTexture(GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Person9.png"))
        'these are for debugging
        txtActionKey = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Action Key.png")
        txtActionlock = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Action Lock.png")
        txtDialoguekey1 = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Dialogue Lock 1.png")
        txtdialoguekey2 = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Dialogue Lock 2.png")
        txtdialoguekey3 = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Dialogue Lock 3.png")

        Person1.strName = "Person1"
        Person2.strName = "Person2"
        Person3.strName = "Person3"
        Person4.strName = "Person4"
        Person5.strName = "Person5"
        Person6.strName = "Person6"
        Person7.strName = "Person7"
        Person8.strName = "Person8"
        Person9.strName = "Person9"
        Person1.GetDialogue.Add("Person1a")
        Person1.GetDialogue.Add("Person1b")
        Person1.GetDialogue.Add("Person1c")
        Person1.GetDialogue.Add("Person1d")
        Person1.GetDialogue.Add("Person1e")
        txtFire = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Fire.png")
        WhiteMage.strName = "White Mage"
        Bard.strName = "Bard"
        Darthen.strName = "Darthen"
        Lana.strName = "Lana"
        Tamalia.strName = "Tamalia"
        HenchmanHouse.strName = "Henchman's House"
        HenchmanHouse.ColWalls.Add(New Rectangle(32, 0, 30, 360))
        HenchmanHouse.ColWalls.Add(New Rectangle(32, 350, 220, 60))
        HenchmanHouse.ColWalls.Add(New Rectangle(115, 286, 40, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(60, 0, 450, 80))
        HenchmanHouse.ColWalls.Add(New Rectangle(60, 80, 30, 100))
        HenchmanHouse.ColWalls.Add(New Rectangle(95, 95, 30, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(150, 90, 70, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(310, 90, 70, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(405, 95, 70, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(465, 0, 40, 360))
        HenchmanHouse.ColWalls.Add(New Rectangle(320, 350, 180, 50))
        HenchmanHouse.ColWalls.Add(New Rectangle(180, 225, 200, 30))
        HenchmanHouse.ColWalls.Add(New Rectangle(370, 290, 40, 20))
        Tavern.strName = "Tavern"
        Tavern.ColWalls.Add(New Rectangle(128, 200, 447, 30))
        Tavern.ColWalls.Add(New Rectangle(96, 12, 27, 588))
        Tavern.ColWalls.Add(New Rectangle(96, 12, 894, 122))
        Tavern.ColWalls.Add(New Rectangle(960, 13, 30, 604))
        Tavern.ColWalls.Add(New Rectangle(97, 589, 894, 29))
        Tavern.ColWalls.Add(New Rectangle(160, 332, 310, 87))
        Tavern.ColWalls.Add(New Rectangle(609, 332, 250, 87))
        Bard.recCollision.Width = 32
        Bard.recCollision.Height = 31
        txtDialogueBox = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Dialogue Box.png")
        LoadScript()
        LoadDialogue()
    End Sub

    Protected Overrides Sub LoadContent()

        txtMap = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Map.png")
        Bard.txtPlayGuitar = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\BardGuitar.png")
        HenchmanHouse.Texture = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\HenchmanHouse.png")
        Graveyard.Texture = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Graveyard.png")
        TitleScreen.Texture = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\TitleScreen.png")
        Cursor.spriteSheet = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Cursor.png")
        Tavern.Texture = GetSpriteSheet(GraphicsDevice, Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Tavern.png")
        Cursor.colFrames.Add(New Rectangle(0, 0, 26, 26))
        Cursor.colFrames.Add(New Rectangle(26, 0, 26, 26))
        Cursor.colFrames.Add(New Rectangle(52, 0, 26, 26))
        Cursor.vecPosition = Location1
        spriteBatch = New SpriteBatch(GraphicsDevice)
        colFireFrames.Add(New Rectangle(0, 0, 26, 10))
        colFireFrames.Add(New Rectangle(0, 11, 26, 10))
        colFireFrames.Add(New Rectangle(0, 21, 26, 10))
    End Sub

    Protected Overrides Sub UnloadContent()
        : MyBase.UnloadContent()
        'TODO: Unload any non ContentManager content here'
    End Sub
    Protected Overrides Sub Update(ByVal gameTime As Microsoft.Xna.Framework.GameTime)
        'Allows the game to exit'
        Dim bolEndLine As Boolean = False
        If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed Then
            Me.Exit()
        End If
        If Keyboard.GetState.IsKeyUp(Keys.Space) And Keyboard.GetState.IsKeyUp(Keys.Enter) And Bard.bolDialogueLock2 = False Then
            Bard.bolDialogueLock3 = False
        End If
        If Keyboard.GetState.IsKeyUp(Keys.Space) And Keyboard.GetState.IsKeyUp(Keys.Enter) Then
            bolScriptDialogueLock = False
        End If
        If Keyboard.GetState.IsKeyDown(Keys.Escape) Then
            Me.Exit()
        End If
        Try
            CurrentLine = colScript(0)
        Catch
            Me.Exit()
        End Try
        If CurrentLine(0) = "Transition" Then
            bolDrawFire = False
            If CurrentLine(1) = "Graveyard" Then
                CurrentHouse = Graveyard
                GameState = "House"
            ElseIf CurrentLine(1) = "TitleScreen" Then
                CurrentHouse = TitleScreen
                GameState = "House"
            ElseIf CurrentLine(1) = "Tavern" Then
                CurrentHouse = Tavern
                GameState = "House"
                bolDrawFire = True
            End If
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "Spawn" Then
            ActiveNPC = GetCharacter(CurrentLine(1))
            colCurrentNPCs.Add(ActiveNPC)
            ActiveNPC.recPosition.X = Convert.ToInt32(CurrentLine(2))
            ActiveNPC.recPosition.Y = Convert.ToInt32(CurrentLine(3))
            ActiveNPC.vecMoveTo = New Vector2(ActiveNPC.recPosition.X, ActiveNPC.recPosition.Y)
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "SpawnPlayer" Then
            bolDrawPC = True
            Bard.recPosition.X = Convert.ToInt32(CurrentLine(2))
            Bard.recPosition.Y = Convert.ToInt32(CurrentLine(3))
            Bard.vecMoveTo = New Vector2(Bard.recPosition.X, Bard.recPosition.Y)
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "Despawn" Then
            ActiveNPC = GetCharacter(CurrentLine(1))
            colCurrentNPCs.Remove(ActiveNPC)
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "MoveTo" Or CurrentLine(0) = "MoveToBackwards" Then
            If CurrentLine(0) = "MoveToBackwards" Then
                ActiveNPC.bolWalkBackwards = True
            Else
                ActiveNPC.bolWalkBackwards = False
            End If
            ActiveNPC = GetCharacter(CurrentLine(1))
            ActiveNPC.vecMoveTo.X = Convert.ToInt32(CurrentLine(2))
            ActiveNPC.vecMoveTo.Y = Convert.ToInt32(CurrentLine(3))
            bolEndLine = True
            For Each NPC As NonPlayerCharacter In colCurrentNPCs
                'cycles through all the character's positions to check if they are in position
                If New Vector2(NPC.recPosition.X, NPC.recPosition.Y) <> NPC.vecMoveTo Then
                    bolEndLine = False
                End If
            Next
            If bolEndLine = True Then
                colScript.RemoveAt(0)
            End If
            'This is like MoveTo but allows multiple characters to be given directions
        ElseIf CurrentLine(0) = "MoveToPlus" Then
            ActiveNPC = GetCharacter(CurrentLine(1))
            ActiveNPC.vecMoveTo.X = Convert.ToInt32(CurrentLine(2))
            ActiveNPC.vecMoveTo.Y = Convert.ToInt32(CurrentLine(3))
            bolEndLine = True
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "Wait" Then
            WaitTimer.Start()
            If WaitTimer.Elapsed.TotalMilliseconds > Convert.ToDouble(CurrentLine(1)) Then
                colScript.RemoveAt(0)
                WaitTimer.Stop()
                WaitTimer.Reset()
            End If
        ElseIf CurrentLine(0) = "Turn" Then
            ActiveNPC = GetCharacter(CurrentLine(1))
            ActiveNPC.strFacing = CurrentLine(2)
            If ActiveNPC.strFacing = "Up" Then
                ActiveNPC.colCurrentFrames = ActiveNPC.colBackStand
            ElseIf ActiveNPC.strFacing = "Down" Then
                ActiveNPC.colCurrentFrames = ActiveNPC.colFrontStand
            ElseIf ActiveNPC.strFacing = "Left" Then
                ActiveNPC.colCurrentFrames = ActiveNPC.colLeftStand
            ElseIf ActiveNPC.strFacing = "Right" Then
                ActiveNPC.colCurrentFrames = ActiveNPC.colRightStand
            End If
            ActiveNPC.intCurrentFrame = 0
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "Dialogue" Then
            DialogueToDraw.AddRange(GetDialogues(colDialogue(0), CurrentLine(1)))
            DialogPosition = CurrentLine(2)
            If bolScriptDialogueLock = False Then
                If Keyboard.GetState.IsKeyDown(Keys.Space) Or Keyboard.GetState.IsKeyDown(Keys.Enter) Then
                    colScript.RemoveAt(0)
                    DialogueToDraw.RemoveAt(0)
                    DialogPosition = ""
                    bolScriptDialogueLock = True
                End If
            End If
        ElseIf CurrentLine(0) = "TalkDialogue" Then
            DialogueToDraw.AddRange(GetDialogues(colDialogue(0), CurrentLine(1)))
            'For Each x As String In colDialogue.Keys
            '    If x = CurrentLine(1) Then
            '        DialogueToDraw.Add(colDialogue(x)(0))
            '    End If
            'Next
            'DialogueToDraw = colDialogue(CurrentLine(1))(0)
            DialogPosition = CurrentLine(2)
            Bard.bolDialogueLock = True
            Bard.bolDialogueLock2 = True
            Bard.bolDialogueLock3 = True
            Bard.bolMovementLock = True
            If Bard.bolActionLock = False Then
                If Keyboard.GetState.IsKeyDown(Keys.Space) Or Keyboard.GetState.IsKeyDown(Keys.Enter) Then
                    DialogueToDraw.RemoveAt(0)
                    Bard.bolActionLock = True
                    If DialogueToDraw.Count = 0 Then
                        colScript.RemoveAt(0)
                        Bard.bolMovementLock = False
                        'DialogueToDraw = Nothing
                        'DialogPosition = ""
                        Bard.bolDialogueLock2 = False
                    End If
                End If
            End If
        ElseIf CurrentLine(0) = "DialogueInterrupted" Then
            DialogueToDraw.AddRange(GetDialogues(colDialogue(0), CurrentLine(1)))
            'DialogueToDraw.Add(colDialogue(CurrentLine(1))(0))
            DialogPosition = CurrentLine(2)
            WaitTimer.Start()
            If WaitTimer.Elapsed.TotalMilliseconds > 300 Then
                colScript.RemoveAt(0)
                DialogueToDraw.RemoveAt(0)
                WaitTimer.Stop()
                WaitTimer.Reset()
            End If
        ElseIf CurrentLine(0) = "GuitarPlay" Then
            Bard.bolPlayGuitar = True
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "GuitarStop" Then
            Bard.bolPlayGuitar = False
            colScript.RemoveAt(0)
        ElseIf CurrentLine(0) = "Exploration" Then
            bolDrawPC = True
            GameState = "Exploration"
        Else
            Try
                colScript.RemoveAt(0)
            Catch
            End Try
        End If
            If GameState = "Map" Then
                UpdateMap()
                If Keyboard.GetState.IsKeyDown(Keys.Enter) Or Keyboard.GetState.IsKeyDown(Keys.Space) Then
                    If CheckPosition(Cursor, Location1) Then
                        GameState = "Henchman's House"
                        Bard.recPosition.X = 260
                        Bard.recPosition.Y = 360
                    End If
                End If
            ElseIf GameState = "House" Or GameState = "Exploration" Then
                If bolDrawPC Then
                    CurrentHouse.Update(GameState, Bard)
                    If GameState = "Exploration" Then
                        Bard.UpdatePlayer(CurrentHouse, colCurrentNPCs, colFlags, GameState, colScript)
                    Else
                        Bard.Update(CurrentHouse, colFlags)
                    End If
                Else
                    CurrentHouse.Update(GameState)
                End If
            End If
            For Each NPC As NonPlayerCharacter In colCurrentNPCs
                NPC.Update(CurrentHouse, colFlags)
            Next
            'TODO: Add your update logic here'
            If bolDrawFire Then
                UpdateFire()
            End If
            MyBase.Update(gameTime)
            Bard.bolActionKey = False
    End Sub
    Sub UpdateFire()
        If intFireFrames >= 8 Then
            If intFireCurrentFrame < colFireFrames.Count - 1 Then
                intFireCurrentFrame += 1
            Else
                intFireCurrentFrame = 0
            End If
            intFireFrames = 0
        End If
        intFireFrames += 1
    End Sub
    Protected Overrides Sub Draw(ByVal gameTime As Microsoft.Xna.Framework.GameTime)
        GraphicsDevice.Clear(Color.CornflowerBlue)
        'TODO: Add your drawing code here'
        'Draw the sprite'

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend)
        If GameState = "Map" Then
            DrawMap(spriteBatch)

        ElseIf GameState = "House" Or GameState = "Exploration" Then
            CurrentHouse.Draw(spriteBatch)
            If bolDrawPC Then
                Bard.Draw(spriteBatch)
            End If
            colCurrentNPCs.Sort(Function(x, y) x.recPosition.Y.CompareTo(y.recPosition.Y))
            colCurrentNPCs = colCurrentNPCs.OrderBy(Function(x) x.recPosition.Y).ToList()
            For Each NPC As NonPlayerCharacter In colCurrentNPCs
                NPC.Draw(spriteBatch)
            Next
        End If
        If DialogueToDraw.Count > 0 Then
            DrawDialogue(DialogueToDraw(0), spriteBatch, DialogPosition)
        End If
        If bolDrawFire Then
            spriteBatch.Draw(txtFire, New Vector2(740, 120), colFireFrames(intFireCurrentFrame), Color.White)
        End If
        'these next ones draw colored blocks that represent locks, used for debugging
        If Bard.bolActionKey Then
            spriteBatch.Draw(txtActionKey, New Vector2(0, 0), Color.White)
        End If
        If Bard.bolActionLock Then
            spriteBatch.Draw(txtActionlock, New Vector2(50, 0), Color.White)
        End If
        If Bard.bolDialogueLock Then
            spriteBatch.Draw(txtDialoguekey1, New Vector2(100, 0), Color.White)
        End If
        If Bard.bolDialogueLock2 Then
            spriteBatch.Draw(txtdialoguekey2, New Vector2(150, 0), Color.White)
        End If
        If Bard.bolDialogueLock3 Then
            spriteBatch.Draw(txtdialoguekey3, New Vector2(200, 0), Color.White)
        End If
        spriteBatch.End()
        If Bard.bolDialogueLock3 = False Then
            Bard.bolDialogueLock = False
        End If
        MyBase.Draw(gameTime)
    End Sub

    Public Sub DrawMap(ByRef spritebatch As SpriteBatch)
        spritebatch.Draw(txtMap, New Vector2(0, 0), Color.White)
        Cursor.Draw(spritebatch)
    End Sub
    Public Sub CheckCollision()

    End Sub
    Public Sub UpdateMap()
        Cursor.Update()
        If intCursorTime = 0 Then
            If Keyboard.GetState.IsKeyDown(Keys.A) Or Keyboard.GetState.IsKeyDown(Keys.Left) Then
                If CheckPosition(Cursor, Location2) Then
                    MoveCursor(Cursor, Location1)
                ElseIf CheckPosition(Cursor, Location4) Then
                    MoveCursor(Cursor, Location3)
                ElseIf CheckPosition(Cursor, Location6) Then
                    MoveCursor(Cursor, Location5)
                ElseIf CheckPosition(Cursor, Location8) Then
                    MoveCursor(Cursor, Location7)
                End If
            End If
            If Keyboard.GetState.IsKeyDown(Keys.S) Or Keyboard.GetState.IsKeyDown(Keys.Down) Then
                If CheckPosition(Cursor, Location1) Then
                    MoveCursor(Cursor, Location3)
                ElseIf CheckPosition(Cursor, Location3) Then
                    MoveCursor(Cursor, Location5)
                ElseIf CheckPosition(Cursor, Location5) Then
                    MoveCursor(Cursor, Location7)
                ElseIf CheckPosition(Cursor, Location2) Then
                    MoveCursor(Cursor, Location4)
                ElseIf CheckPosition(Cursor, Location4) Then
                    MoveCursor(Cursor, Location6)
                ElseIf CheckPosition(Cursor, Location6) Then
                    MoveCursor(Cursor, Location8)
                End If
            End If
            If Keyboard.GetState.IsKeyDown(Keys.D) Or Keyboard.GetState.IsKeyDown(Keys.Right) Then
                If CheckPosition(Cursor, Location1) Then
                    MoveCursor(Cursor, Location2)
                ElseIf CheckPosition(Cursor, Location3) Then
                    MoveCursor(Cursor, Location4)
                ElseIf CheckPosition(Cursor, Location5) Then
                    MoveCursor(Cursor, Location6)
                ElseIf CheckPosition(Cursor, Location7) Then
                    MoveCursor(Cursor, Location8)
                End If
            End If
            If Keyboard.GetState.IsKeyDown(Keys.W) Or Keyboard.GetState.IsKeyDown(Keys.Up) Then
                If CheckPosition(Cursor, Location3) Then
                    MoveCursor(Cursor, Location1)
                ElseIf CheckPosition(Cursor, Location5) Then
                    MoveCursor(Cursor, Location3)
                ElseIf CheckPosition(Cursor, Location7) Then
                    MoveCursor(Cursor, Location5)
                ElseIf CheckPosition(Cursor, Location4) Then
                    MoveCursor(Cursor, Location2)
                ElseIf CheckPosition(Cursor, Location6) Then
                    MoveCursor(Cursor, Location4)
                ElseIf CheckPosition(Cursor, Location8) Then
                    MoveCursor(Cursor, Location6)
                End If
            End If
        Else
            intCursorTime -= 1
        End If
    End Sub
    Public Sub MoveCursor(ByRef cursor As Cursor, ByRef location As Vector2)
        cursor.vecPosition.X = location.X
        cursor.vecPosition.Y = location.Y
        intCursorTime = 10
    End Sub
    Public Function CheckPosition(ByRef cursor As Cursor, ByRef location As Vector2) As Boolean
        If cursor.vecPosition.X = location.X And cursor.vecPosition.Y = location.Y Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub UpdateHouse()

    End Sub
    Public Function GetCharacter(ByRef strName As String) As NonPlayerCharacter
        If strName = "WhiteMage" Then
            Return WhiteMage
        ElseIf strName = "Darthen" Then
            Return Darthen
        ElseIf strName = "Tamalia" Then
            Return Tamalia
        ElseIf strName = "Lana" Then
            Return Lana
        ElseIf strName = "Person1" Then
            Return Person1
        ElseIf strName = "Person2" Then
            Return Person2
        ElseIf strName = "Person3" Then
            Return Person3
        ElseIf strName = "Person4" Then
            Return Person4
        ElseIf strName = "Person5" Then
            Return Person5
        ElseIf strName = "Person6" Then
            Return Person6
        ElseIf strName = "Person7" Then
            Return Person7
        ElseIf strName = "Person8" Then
            Return Person8
        ElseIf strName = "Person9" Then
            Return Person9
        End If
        Return Nothing
    End Function
    Public Sub LoadDialogue()
        Dim Temp As New System.Collections.Generic.List(Of String())
        'these next three variables contain the data for an individual piece of Dialogue i.e. a screen of text
        Dim strDialogueName As String = ""
        Dim strCharacter As String = ""
        Dim bolInitial1 As Boolean = True
        Dim ColDialogueLines As New System.Collections.Generic.List(Of String)
        Dim ColDialogues As New System.Collections.Generic.List(Of Dialogue)
        Temp = GetTextFileContents(Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Dialogue File.txt", ":", "Dialogue file not found")
        For Each stringArray As String() In Temp
            If stringArray(0) = "Name" Then
                If bolInitial1 = False Then
                    ColDialogues.Add(New Dialogue(strCharacter, ColDialogueLines, strDialogueName))
                    colDialogue.Add(ColDialogues)
                    ColDialogues = New System.Collections.Generic.List(Of Dialogue)
                    ColDialogueLines = New System.Collections.Generic.List(Of String)
                End If
                bolInitial1 = False
                strDialogueName = stringArray(1)
            ElseIf stringArray(0) = "Character" Then
                strCharacter = stringArray(1)
            ElseIf stringArray(0) = "Line" Then
                ColDialogueLines.Add(stringArray(1))
            End If
        Next
        ColDialogues.Add(New Dialogue(strCharacter, ColDialogueLines, strDialogueName))
        colDialogue.Add(ColDialogues)
    End Sub
    Public Sub LoadScript()
        Dim Temp As New System.Collections.Generic.List(Of String())
        Dim lines As New System.Collections.Generic.List(Of String)
        'All of these Are added to the script collection
        Temp = GetTextFileContents(Convert.ToString(Directory.GetCurrentDirectory) + "\Game Assets\Script File.txt", ":", "Script file not found")
        For Each StringArray As String() In Temp
            For Each strLine As String In StringArray
                lines.Add(strLine)
            Next
            colScript.Add(lines)
            lines = New System.Collections.Generic.List(Of String)
        Next
    End Sub
    Public Sub DrawDialogue(ByRef Dialogue As Dialogue, ByRef SpriteBatch As SpriteBatch, Optional ByRef strPosition As String = "Bottom")
        Dim intLine As Integer = 0
        Dim intX As Integer
        Dim intY As Integer
        If strPosition = "Left" Then
            intX = 70
            intY = 260
        ElseIf strPosition = "Right" Then
            intX = 800
            intY = 260
        ElseIf strPosition = "Bottom" Then
            intX = 500
            intY = 400
        ElseIf strPosition = "Top" Then
            intX = 500
            intY = 70
        Else
            intX = 500
            intY = 70
        End If
        SpriteBatch.Draw(txtDialogueBox, New Vector2(intX, intY), Color.White)
        SpriteBatch.DrawString(Font1, Dialogue.Character, New Vector2(intX + 10, intY + 10), Color.White)
        For Each Line As String In Dialogue.ColText
            SpriteBatch.DrawString(Font1, Line, New Vector2(intX + 25, intY + 30 + (intLine * 20)), Color.White)
            intLine += 1
        Next
    End Sub
    Public Function GetTextFileContents(ByVal FullPath As String, ByVal strDelimiter As String, Optional ByRef ErrInfo As String = "") As System.Collections.Generic.List(Of String())
        Dim colContents As New System.Collections.Generic.List(Of String())
        Dim objReader As StreamReader
        Try

            objReader = New StreamReader(FullPath)
            Do While objReader.Peek <> -1
                colContents.Add(Split(objReader.ReadLine, strDelimiter))
            Loop
            objReader.Close()
            Return colContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
            colContents.Clear()
            Return colContents
        End Try
    End Function
    Public Function GetSpriteSheet(ByVal GD As GraphicsDevice, ByVal strPath As String) As Texture2D
        Dim textureStream As System.IO.Stream = New System.IO.StreamReader(strPath).BaseStream
        Dim Sheet As Texture2D = Texture2D.FromStream(GD, textureStream)
        Return Sheet
    End Function
    Public Function GetDialogues(ByRef colDialogues As System.Collections.Generic.List(Of Dialogue), ByRef strSearch As String) As System.Collections.Generic.List(Of Dialogue)
        Dim colReturn As New System.Collections.Generic.List(Of Dialogue)
        For Each objDialogue As Dialogue In colDialogues
            If objDialogue.GetName = strSearch Then
                colReturn.Add(objDialogue)
            End If
        Next
        Return colReturn
    End Function
End Class
Module mdlMain
    Sub Main()
        : Using game As New Game
            : game.Run()
            : End Using
    End Sub
End Module
Public Class character
    Public strName As String
    Private strType As String 'Used to differentiate between enemy characters and player characters
    Public spriteSheet As Texture2D
    Private strSpriteFile As String
    Public colFrontStand As New System.Collections.Generic.List(Of Rectangle)
    Public colFrontWalk As New System.Collections.Generic.List(Of Rectangle)
    Public colBackStand As New System.Collections.Generic.List(Of Rectangle)
    Public colBackWalk As New System.Collections.Generic.List(Of Rectangle)
    Public colLeftStand As New System.Collections.Generic.List(Of Rectangle)
    Public colLeftWalk As New System.Collections.Generic.List(Of Rectangle)
    Public colRightStand As New System.Collections.Generic.List(Of Rectangle)
    Public colRightWalk As New System.Collections.Generic.List(Of Rectangle)
    Public colCurrentFrames As System.Collections.Generic.List(Of Rectangle)
    Public intCurrentFrame As Integer
    Public intFrames As Integer 'used for counting frames between animation, increments everytime the same frame is displayed
    Public intFrameMax As Integer = 10 'the number of frames until the animation switches
    Public dblSpeed As Double = 2.0
    Public timer As Stopwatch
    'Public vecPosition As Vector2
    Public recPosition As New Rectangle(0, 0, 31, 31)
    Public intHeight As Integer
    Public intWidth As Integer
    Public bolWalkLeft As Boolean = False
    Public bolWalkRight As Boolean = False
    Public bolWalkUp As Boolean = False
    Public bolWalkDown As Boolean = False
    Public strFacing As String = "down" 'which way your character is facing
    Public recCollision As Rectangle
    Public keyMoveUp As Keys = Keys.W
    Public keyMoveDown As Keys = Keys.S
    Public keyMoveRight As Keys = Keys.D
    Public keyMoveLeft As Keys = Keys.A
    Public keyAction1 As Keys = Keys.Space
    Public keyAction2 As Keys = Keys.Enter
    Public bolActionKey As Boolean = False
    Public bolActionLock As Boolean = False
    Public bolWalkBackwards As Boolean = False
    Public ColTextures As New System.Collections.Generic.Dictionary(Of String, Texture2D)

    Public Sub New()
        colFrontStand.Add(New Rectangle(32, 0, 31, 31))
        colFrontWalk.Add(New Rectangle(0, 0, 31, 31))
        colFrontWalk.Add(New Rectangle(32, 0, 31, 31))
        colFrontWalk.Add(New Rectangle(64, 0, 31, 31))
        colFrontWalk.Add(New Rectangle(32, 0, 31, 31))

        colLeftStand.Add(New Rectangle(32, 32, 31, 31))
        colLeftWalk.Add(New Rectangle(0, 32, 31, 31))
        colLeftWalk.Add(New Rectangle(32, 32, 31, 31))
        colLeftWalk.Add(New Rectangle(64, 32, 31, 31))
        colLeftWalk.Add(New Rectangle(32, 32, 31, 31))

        colRightStand.Add(New Rectangle(32, 64, 31, 31))
        colRightWalk.Add(New Rectangle(0, 64, 31, 31))
        colRightWalk.Add(New Rectangle(32, 64, 31, 31))
        colRightWalk.Add(New Rectangle(64, 64, 31, 31))
        colRightWalk.Add(New Rectangle(32, 64, 31, 31))

        colBackStand.Add(New Rectangle(32, 96, 31, 31))
        colBackWalk.Add(New Rectangle(0, 96, 31, 31))
        colBackWalk.Add(New Rectangle(32, 96, 31, 31))
        colBackWalk.Add(New Rectangle(64, 96, 31, 31))
        colBackWalk.Add(New Rectangle(32, 96, 31, 31))
        colCurrentFrames = colFrontStand
    End Sub
    Public Sub SetType(ByVal Type As String)
        strType = Type
    End Sub
    Public Function GetTypeString() As String
        Return strType
    End Function
    Public Function DiceRoll(ByVal intDiceNum As Integer, ByVal intDiceSize As Integer) As Integer
        Randomize()
        Dim intRoll As Integer = 0
        ' This function returns the result of a simulated die or dice roll 
        Dim i As Integer
        For i = 1 To intDiceNum
            intRoll += Convert.ToInt32(((intDiceSize - 1) * Rnd()) + 1)
        Next
        Return intRoll
    End Function
    Public Sub SetTexture(ByRef texture As Texture2D)
        spriteSheet = texture
    End Sub
    Public Sub SetSpriteFile(ByRef File As String)
        strSpriteFile = File
    End Sub
    Public Function getSpriteFile() As String
        Return strSpriteFile
    End Function
    Public Sub move(ByVal testvector As Vector2)
        SetPositionX(Convert.ToInt32(testvector.X))
        SetPositionY(Convert.ToInt32(testvector.Y))
    End Sub

    Public Sub SetHeight(ByVal intNewHeight As Integer)
        intHeight = intNewHeight
        recCollision.Height = intNewHeight
    End Sub
    Public Sub Setwidth(ByVal intNewWidth As Integer)
        intWidth = intNewWidth
        recCollision.Width = intNewWidth
    End Sub

    Public Sub ChangeAnimationFrame(ByVal colFrames As System.Collections.Generic.List(Of Rectangle))

        If colFrames(0).X = colCurrentFrames(0).X And colFrames(0).Y = colCurrentFrames(0).Y Then
            If intFrames < intFrameMax Then
                intFrames += 1
            Else
                intFrames = 0
                If intCurrentFrame + 1 >= colFrames.Count Then
                    intCurrentFrame = 0
                Else
                    intCurrentFrame += 1
                End If
            End If
        Else
            colCurrentFrames = colFrames
            intCurrentFrame = 0
            intFrames = 0
        End If
    End Sub

    Public Function Walk() As Vector2
        Dim vecDestination As Vector2
        Dim dblX As Double
        Dim dblY As Double
        Dim dblSquareRoot As Double
        Dim bolLockDirection As Boolean = False

        If bolWalkUp Then
            dblY += -1
            If bolWalkLeft = False And bolWalkRight = False And bolLockDirection = False Then
                If bolWalkBackwards = False Then
                    ChangeAnimationFrame(colBackWalk)
                Else
                    ChangeAnimationFrame(colFrontWalk)
                End If
                strFacing = "up"
            End If
        End If
        If bolWalkDown Then
            dblY += 1
            If bolWalkLeft = False And bolWalkRight = False And bolLockDirection = False Then
                If bolWalkBackwards = False Then
                    ChangeAnimationFrame(colFrontWalk)
                Else
                    ChangeAnimationFrame(colBackWalk)
                End If
                strFacing = "down"
            End If
        End If
        If bolWalkLeft Then
            dblX += -1
            If bolLockDirection = False Then
                If bolWalkBackwards = False Then
                    ChangeAnimationFrame(colLeftWalk)
                Else
                    ChangeAnimationFrame(colRightWalk)
                End If
                strFacing = "left"
            End If
        End If
        If bolWalkRight Then
            dblX += 1
            If bolLockDirection = False Then
                If bolWalkBackwards = False Then
                    ChangeAnimationFrame(colRightWalk)
                Else
                    ChangeAnimationFrame(colLeftWalk)
                End If
                strFacing = "right"
            End If
        End If

        If dblX = 0 Then
            vecDestination.X = 0
            vecDestination.Y = Convert.ToSingle(dblY * dblSpeed)
        ElseIf dblY = 0 Then
            vecDestination.X = Convert.ToSingle(dblX * dblSpeed)
            vecDestination.Y = 0
        Else
            dblSquareRoot = System.Math.Sqrt(dblSpeed)
            vecDestination.X = Convert.ToSingle(dblX * dblSquareRoot)
            vecDestination.Y = Convert.ToSingle(dblY * dblSquareRoot)
        End If

        Return vecDestination
    End Function
    Public Sub SetPositionX(ByVal intpositionX As Integer)
        recPosition.X = intpositionX
        recCollision.X = intpositionX
    End Sub
    Public Sub SetPositionY(ByVal intpositionY As Integer)
        recPosition.Y = intpositionY
        recCollision.Y = intpositionY
    End Sub
    Public Function detectNPCCollisions(ByVal recCharacterPosition As Rectangle, ByRef colNPCs As System.Collections.Generic.List(Of NonPlayerCharacter)) As Boolean
        For Each NPC As NonPlayerCharacter In colNPCs
            If DetectCollision(recCharacterPosition, NPC.recPosition) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function detectNPCTalkCollisions(ByVal recPosition As Rectangle, ByRef colNPCs As System.Collections.Generic.List(Of NonPlayerCharacter)) As NonPlayerCharacter
        Dim recTalkCollision As Rectangle = recPosition
        If strFacing.ToLower = "up" Then
            recTalkCollision.Y -= 10
        End If
        If strFacing.ToLower = "down" Then
            recTalkCollision.Y += 10
        End If
        If strFacing.ToLower = "left" Then
            recTalkCollision.X -= 10
        End If
        If strFacing.ToLower = "right" Then
            recTalkCollision.X += 10
        End If
        For Each NPC As NonPlayerCharacter In colNPCs
            If DetectCollision(recTalkCollision, NPC.recPosition) Then
                Return NPC
            End If
        Next
        Return Nothing
    End Function
    Public Function detectWallCollisions(ByVal vecDestination As Vector2, ByRef colWalls As System.Collections.Generic.List(Of Rectangle)) As Boolean
        Dim RecTest As New Rectangle(Convert.ToInt32(vecDestination.X), Convert.ToInt32(vecDestination.Y), recCollision.Width, recCollision.Height)
        For Each wall As Rectangle In colWalls
            If DetectCollision(RecTest, wall) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function DetectCollision(ByRef rec1 As Rectangle, ByRef rec2 As Rectangle) As Boolean
        If Not (rec1.Bottom <= rec2.Top Or rec1.Top >= rec2.Bottom Or rec1.Left >= rec2.Right Or rec1.Right <= rec2.Left) Then
            Return True
        End If
        Return False
    End Function
    Public Sub Draw(ByRef SpriteBatch As SpriteBatch)
        SpriteBatch.Draw(spriteSheet, New Vector2(recPosition.X, recPosition.Y), colCurrentFrames.Item(intCurrentFrame), Color.White)
    End Sub
End Class
Public Class Cursor
    Public intFrames As Integer = 0
    Public intframeMax As Integer = 10
    Public Texture As Texture2D
    Public intCurrentFrame As Integer = 0
    Public bolDraw As Boolean = False
    Public vecPosition As Vector2 = New Vector2(0, 0)
    Public spriteSheet As Texture2D
    Public colFrames As New System.Collections.Generic.List(Of Rectangle)
    Public Sub Update()
        If intFrames < intframeMax Then
            intFrames += 1
        Else
            intFrames = 0
            If intCurrentFrame + 1 >= colFrames.Count Then
                intCurrentFrame = 0
            Else
                intCurrentFrame += 1
            End If
        End If
    End Sub
    Public Sub Draw(ByRef SpriteBatch As SpriteBatch)
        SpriteBatch.Draw(spriteSheet, vecPosition, colFrames.Item(intCurrentFrame), Color.White)
    End Sub
End Class
Public Class PlayerCharacter
    Inherits NonPlayerCharacter
    Public bolPlayGuitar As Boolean = False
    Public bolMovementLock As Boolean = False
    Public bolDialogueLock As Boolean = False 'used to fix a dialogue box issue
    Public bolDialogueLock2 As Boolean = False 'used to fix a dialogue box issue
    Public bolDialogueLock3 As Boolean = False 'used to fix a dialogue box issue
    Public colGuitarFrames As New System.Collections.Generic.List(Of Rectangle)
    Public txtPlayGuitar As Texture2D
    Public Sub CheckButtons()
        Dim KeyBoardState As KeyboardState = Keyboard.GetState
        If bolMovementLock = False Then
            If KeyBoardState.IsKeyDown(keyMoveUp) Then
                bolWalkUp = True
            Else
                bolWalkUp = False
            End If
            If KeyBoardState.IsKeyDown(keyMoveLeft) Then
                bolWalkLeft = True
            Else
                bolWalkLeft = False
            End If
            If KeyBoardState.IsKeyDown(keyMoveDown) Then
                bolWalkDown = True
            Else
                bolWalkDown = False
            End If
            If KeyBoardState.IsKeyDown(keyMoveRight) Then
                bolWalkRight = True
            Else
                bolWalkRight = False
            End If
        Else
            bolWalkUp = False
            bolWalkDown = False
            bolWalkRight = False
            bolWalkLeft = False
        End If
        If (KeyBoardState.IsKeyDown(keyAction1) Or KeyBoardState.IsKeyDown(keyAction2)) And bolActionLock = False Then
            bolActionKey = True
            bolActionLock = True
        Else
            bolActionKey = False
        End If
        If KeyBoardState.IsKeyUp(keyAction1) And KeyBoardState.IsKeyUp(keyAction2) And bolActionLock = True Then
            bolActionLock = False
        End If
    End Sub
    Public Overloads Sub Draw(ByRef SpriteBatch As SpriteBatch)
        If bolPlayGuitar Then
            SpriteBatch.Draw(txtPlayGuitar, New Vector2(recPosition.X, recPosition.Y), colCurrentFrames.Item(intCurrentFrame), Color.White)
        Else
            Try
                SpriteBatch.Draw(spriteSheet, New Vector2(recPosition.X, recPosition.Y), colCurrentFrames.Item(intCurrentFrame), Color.White)
            Catch
                Try
                    SpriteBatch.Draw(spriteSheet, New Vector2(recPosition.X, recPosition.Y), colCurrentFrames.Item(intCurrentFrame - 1), Color.White)
                    intCurrentFrame -= 1
                Catch
                    SpriteBatch.Draw(spriteSheet, New Vector2(recPosition.X, recPosition.Y), colCurrentFrames.Item(0), Color.White)
                    intCurrentFrame = 0
                End Try
            End Try
        End If
    End Sub
    Public Sub UpdatePlayer(ByRef room As House, ByRef colNPCs As System.Collections.Generic.List(Of NonPlayerCharacter), ByRef colFlags As System.Collections.Generic.List(Of String), ByRef strGameState As String, ByRef colScript As System.Collections.Generic.List(Of System.Collections.Generic.List(Of String)))
        Dim WalkVector As Vector2
        Dim ActiveNPC As NonPlayerCharacter
        Dim colWalls As System.Collections.Generic.List(Of Rectangle) = room.ColWalls
        Dim strDialogue As String = ""
        Dim colScriptLine As New System.Collections.Generic.List(Of String)
        Dim strNPCdirection As String = ""
        Dim strNPCTurnDirection As String = ""
        If bolPlayGuitar = False Then
            CheckButtons()
            WalkVector = Walk()
            If detectWallCollisions(New Vector2(WalkVector.X + recPosition.X, recPosition.Y), colWalls) = False And detectNPCCollisions(New Rectangle(Convert.ToInt32(WalkVector.X) + recPosition.X, recPosition.Y, 32, 31), colNPCs) = False Then
                SetPositionX(Convert.ToInt32(recPosition.X) + Convert.ToInt32(WalkVector.X))
            End If
            If detectWallCollisions(New Vector2(recPosition.X, WalkVector.Y + recPosition.Y), colWalls) = False And detectNPCCollisions(New Rectangle(recPosition.X, Convert.ToInt32(WalkVector.Y) + recPosition.Y, 32, 31), colNPCs) = False Then
                SetPositionY(Convert.ToInt32(recPosition.Y) + Convert.ToInt32(WalkVector.Y))
            End If
            If bolActionKey = True And bolDialogueLock = False Then
                ActiveNPC = detectNPCTalkCollisions(recPosition, colNPCs)
                If Not ActiveNPC Is Nothing And bolActionLock = True And bolActionKey = True Then
                    strDialogue = ActiveNPC.TalkTo(colFlags)
                    If strDialogue <> "" Then
                        strNPCdirection = ActiveNPC.strFacing
                        If strFacing = "Up" Then
                            strNPCTurnDirection = "Down"
                        ElseIf strFacing = "Down" Then
                            strNPCTurnDirection = "Up"
                        ElseIf strFacing = "Right" Then
                            strNPCTurnDirection = "Left"
                        ElseIf strFacing = "Left" Then
                            strNPCTurnDirection = "Right"
                        Else
                            strNPCTurnDirection = strNPCdirection
                        End If
                        'add turn script lines
                        colScriptLine.Add("TalkDialogue")
                        colScriptLine.Add(strDialogue)
                        colScriptLine.Add("Bottom")
                        colScript.Insert(0, colScriptLine)
                    End If
                End If
            End If
        Else
            ChangeAnimationFrame(colGuitarFrames)
        End If
        'move(WalkVector)
    End Sub
    Public Overloads Sub Update(ByRef room As House, ByRef colFlags As System.Collections.Generic.List(Of String))
        Dim WalkVector As Vector2

        Dim colWalls As System.Collections.Generic.List(Of Rectangle) = room.ColWalls
        'CheckButtons()
        If bolPlayGuitar = False Then
            bolWalkDown = False
            bolWalkUp = False
            bolWalkRight = False
            bolWalkLeft = False
            If vecMoveTo.X > recPosition.X Then
                bolWalkRight = True
            End If
            If vecMoveTo.X < recPosition.X Then
                bolWalkLeft = True
            End If
            If vecMoveTo.Y > recPosition.Y Then
                bolWalkDown = True
            End If
            If vecMoveTo.Y < recPosition.Y Then
                bolWalkUp = True
            End If
            WalkVector = Walk()
            move(New Vector2(recPosition.X + WalkVector.X, recPosition.Y + WalkVector.Y))
        Else
            ChangeAnimationFrame(colGuitarFrames)
        End If
    End Sub

    Public Sub New()
        colGuitarFrames.Add(New Rectangle(0, 0, 31, 31))
        colGuitarFrames.Add(New Rectangle(32, 0, 31, 31))
        colGuitarFrames.Add(New Rectangle(64, 0, 31, 31))
    End Sub
End Class
Public Class House
    Public strName As String = ""
    Public Texture As Texture2D
    Public ColWalls As New Collections.Generic.List(Of Rectangle)
    'Public colNPCs As New Collections.Generic.List(Of NonPlayerCharacter)
    Public Sub Update(ByRef GameState As String, Optional ByRef MainCharacter As PlayerCharacter = Nothing)
        If Not MainCharacter Is Nothing Then
            'If MainCharacter.recPosition.Y >= 420 Then  ' this is used for the map transitions
            '    GameState = "Map"
            'End If
        End If
    End Sub
    Public Sub Draw(ByRef SpriteBatch As SpriteBatch)
        SpriteBatch.Draw(Texture, New Vector2(0, 0), Color.White)
    End Sub
End Class
Public Class Dialogue
    Private strName As String
    Public Character As String = ""
    Public ColText As New System.Collections.Generic.List(Of String)
    Public Sub New(ByVal StrCharacterName As String, ByVal colLines As System.Collections.Generic.List(Of String), ByVal strDialogueName As String)
        Character = StrCharacterName
        ColText = colLines
        strName = strDialogueName
    End Sub
    Public Function GetName() As String
        Return strName
    End Function

End Class
Public Class NonPlayerCharacter
    Inherits character
    Public vecMoveTo As Vector2
    Private ColDialogue As New System.Collections.Generic.List(Of String) 'this is for default Dialogue
    Public intDialogue As Integer = -1 'this is incremented everytime default Dialogue is used so it moves through the individual Dialogues
    Public colFlagDialogue As New System.Collections.Generic.Dictionary(Of String, String) 'this is for flag dependant Dialogue (name of flag, name of Dialogue)
    Public Function TalkTo(ByRef colFlags As System.Collections.Generic.List(Of String)) As String
        For Each strFlag As String In colFlagDialogue.Keys
            If colFlags.Contains(strFlag) And Not colFlags.Contains(colFlagDialogue(strFlag)) Then
                Return colFlagDialogue(strFlag)
            End If
        Next
        intDialogue += 1
        Try
            Return ColDialogue(intDialogue)
        Catch
            Return Nothing
        End Try
    End Function
    Public Function GetDialogue() As System.Collections.Generic.List(Of String)
        Return ColDialogue
    End Function
    Public Sub Update(ByRef room As House, ByRef colFlags As System.Collections.Generic.List(Of String))
        Dim WalkVector As Vector2

        Dim colWalls As System.Collections.Generic.List(Of Rectangle) = room.ColWalls
        'CheckButtons()
        bolWalkDown = False
        bolWalkUp = False
        bolWalkRight = False
        bolWalkLeft = False
        If vecMoveTo.X > recPosition.X Then
            bolWalkRight = True
        End If
        If vecMoveTo.X < recPosition.X Then
            bolWalkLeft = True
        End If
        If vecMoveTo.Y > recPosition.Y Then
            bolWalkDown = True
        End If
        If vecMoveTo.Y < recPosition.Y Then
            bolWalkUp = True
        End If
        WalkVector = Walk()
        move(New Vector2(recPosition.X + WalkVector.X, recPosition.Y + WalkVector.Y))
    End Sub
End Class

