Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class Registration

    ' SQL Connection String (with TrustServerCertificate to avoid SSL error)
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        ' Basic Validation
        If Guna2TextBox3.Text = "" Or Guna2TextBox1.Text = "" Or Guna2TextBox4.Text = "" Or Guna2TextBox2.Text = "" Or Guna2TextBox5.Text = "" Then
            MessageBox.Show("Please fill in all fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            ' Insert Query
            Dim query As String = "INSERT INTO Customer1 (FullName, Username, Email, Phone, Password) VALUES (@FullName, @Username, @Email, @Phone, @Password)"
            Dim cmd As New SqlCommand(query, con)

            ' Assign values
            cmd.Parameters.AddWithValue("@FullName", Guna2TextBox3.Text)
            cmd.Parameters.AddWithValue("@Username", Guna2TextBox1.Text)
            cmd.Parameters.AddWithValue("@Email", Guna2TextBox4.Text)
            cmd.Parameters.AddWithValue("@Phone", Guna2TextBox2.Text)
            cmd.Parameters.AddWithValue("@Password", Guna2TextBox5.Text)

            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Optionally clear the fields
            Guna2TextBox3.Clear()
            Guna2TextBox1.Clear()
            Guna2TextBox4.Clear()
            Guna2TextBox2.Clear()
            Guna2TextBox5.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

End Class
