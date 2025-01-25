Imports Sunisoft.IrisSkin 'References IrisSkin2.dll
Imports System.IO

Public Class Form1
    Inherits Form

    Private Iris As SkinEngine
    Dim skinPath As String

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Iris = New SkinEngine
        skinPath = String.Concat(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "\Visual Studio 2008\Projects\IrisSkinSample\IrisSkinSample\IrisSkinVersion3.4\Skins\")
        Dim dllPath As String = String.Concat(My.Computer.FileSystem.CurrentDirectory, "\IrisSkin2.dll")
        If (Iris.Version = 3.4) And (File.Exists(dllPath)) Then
            Dim iSkins As String
            For Each iSkins In My.Computer.FileSystem.GetFiles(skinPath, FileIO.SearchOption.SearchAllSubDirectories, "*.ssk")
                ComboBox1.Items.Add(Path.GetFileName(iSkins))
            Next iSkins
        Else
            MsgBox("You must using IrisSkin2.dll file version 3.4 added debug folder.", MsgBoxStyle.Critical, "Error!")
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        For Each iSkins As String In My.Computer.FileSystem.GetFiles(skinPath, FileIO.SearchOption.SearchAllSubDirectories, "*.ssk")
            If Path.GetFileName(iSkins) = ComboBox1.SelectedItem.ToString Then
                Iris.SkinFile = iSkins
                Exit For
            End If
        Next iSkins
    End Sub
End Class
