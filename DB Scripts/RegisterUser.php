<?php
    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve user input username and password from POST request
    $username = $_POST["username"];
    $hashed_password = password_hash($_POST["password"], PASSWORD_DEFAULT);

    //Query database for username
    $checkUsernameQuery = "SELECT username FROM users WHERE username='" . $username . "';";
    $checkUsername = mysqli_query($connection, $checkUsernameQuery) or die("Check username query failed.");

    //Make sure username is not already taken by another user
    if (mysqli_num_rows($checkUsername) > 0) {
        echo "Username already exists.";
        exit();
    }

    //Query database to create new user in users table
    $createUserQuery = "INSERT INTO users (username, password) VALUES ('" . $username . "', '" . $hashed_password . "');";
    mysqli_query($connection, $createUserQuery) or die ("Create user query failed.");

    echo("Success.");
?>