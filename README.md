### SQLWR
###### SQL query helper class

    Add Refrence to SQLWR.dll from bin/Debug/.

    using SQLWR;

    Create Instance: 
        SQL sql = new SQL(@"Server=.\SQLEXPRESS;Database=databsename;Trusted_Connection=True;");
            OR
        SQL sql = new SQL(); sql.ConnectingString = @"Server =.\SQLEXPRESS; Database = databsename; Trusted_Connection = True;";

    Write:
        sql.Write("INSERT INTO[dbo].[User] VALUES(@id, @username, @password);", com => 
        {
            com.Parameters.AddWithValue("@id", 1);
            com.Parameters.AddWithValue("@username", "my_username");
            com.Parameters.AddWithValue("@password", "my_password");
        });

    Read:
        List<User> users = new List<User>();
        sql.Read("SELECT* FROM[dbo].[User]", reader => 
        { 
            User user = new User();
            user.id = int.Parse(reader["ID"].ToString().Trim());
            user.username = reader["username"].ToString().Trim();
            user.password = reader["password"].ToString().Trim();
            users.Add(user);
        });