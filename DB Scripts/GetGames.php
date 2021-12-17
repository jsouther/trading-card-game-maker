<?php
    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve userID field from POST
    $userID = $_POST["userID"];

    //Query database to get games owned by userID
    $getGamesQuery = "SELECT gameName, description, imagePath, startingHealth, timeLimit FROM games WHERE userID='" . $userID . "';";
    $getGames = mysqli_query($connection, $getGamesQuery) or die ("Get games query failed.");

    $array = array();
    while($row = mysqli_fetch_assoc($getGames)) {
        $array[] = $row;
    } 

    echo json_encode($array);

    mysqli_close($connection);
?>