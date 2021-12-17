<?php
    //Delete deck from database given by deckID

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve deckID field from POST
    $deckID = $_POST["deckID"];

    $deleteDeckQuery = $connection->prepare("DELETE FROM decks WHERE deckID = ?");
    $deleteDeckQuery->bind_param("i", $deckID);
    $deleteDeckQuery->execute();
    $deleteDeckQuery->close(); 

    echo("Success.");

    mysqli_close($connection);
?>