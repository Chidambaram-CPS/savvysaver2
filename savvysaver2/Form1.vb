Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports Microsoft.Data.SqlClient

Public Class Form1
    ' SQL Server connection
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")
    ' LOGIN BUTTON (Guna2GradientButton2)
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim userType As String = Guna2ComboBox1.SelectedItem
        Dim username = Guna2TextBox1.Text.Trim
        Dim password = Guna2TextBox2.Text.Trim

        If userType = "" Or username = "" Or password = "" Then
            MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' ==== Admin Login ====
        If userType = "Admin" Then
            If username = "admin" And password = "admin123" Then
                MessageBox.Show("Admin Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Hide()

            Else
                MessageBox.Show("Invalid Admin credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' ==== Customer Login ====
        ElseIf userType = "Customer" Then
            Try
                Dim cmd As New SqlCommand("SELECT COUNT(*) FROM Customer1 WHERE Username = @Username AND Password = @Password", con)
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Password", password)

                con.Open()
                Dim count = Convert.ToInt32(cmd.ExecuteScalar)
                con.Close()

                If count > 0 Then
                    MessageBox.Show("Customer Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Hide()

                Else
                    MessageBox.Show("Invalid Customer credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If con.State = ConnectionState.Open Then con.Close()
            End Try
        Else
            MessageBox.Show("Please select a valid user type!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim reg As New Vendor()
        reg.Show()
        Me.Hide()
    End Sub

    ' CANCEL BUTTON (Guna2GradientButton4)

End Class
