Imports System.Configuration.ConfigurationSettings

Public Class RootObject

    Private Shared m_HomePage As String
    Private Shared m_HomeTopic As String

    Public Shared ReadOnly Property HomePage() As String
        Get
            If m_HomePage Is Nothing Then
                m_HomePage = AppSettings.Item("HomePage")
                If m_HomePage Is Nothing Then
                    Throw New Exception("HomePage could not be found in web.config")
                End If
            End If
            Return m_HomePage
        End Get
    End Property


    Public Shared ReadOnly Property HomeTopic() As String
        Get
            If m_HomeTopic Is Nothing Then
                m_HomeTopic = AppSettings.Item("HomeTopic")
                If m_HomeTopic Is Nothing Then
                    Throw New Exception("HomeTopic could not be found in web.config")
                End If
            End If
            Return m_HomeTopic
        End Get
    End Property

    'todo: move other appsetting values here

End Class
