<?php
    //Delete custom game from database given by gameID

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve gameID field from POST
    $gameID = $_POST["gameID"];

    $deleteGameQuery = $connection->prepare("DELETE FROM games WHERE gameID = ?");
    $deleteGameQuery->bind_param("i", $gameID);
    $deleteGameQuery->execute();
    $deleteGameQuery->close();

    echo("Success.");

    mysqli_close($connection);
?>