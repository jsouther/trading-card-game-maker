<?php
    //Creates a new entry in `decks` table and returns its deckID

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve deck fields from POST
    $userID = $_POST["userID"];
    $deckID = $_POST["deckID"];
    $name = $_POST["name"];

    if ($deckID == -1) { //Deck does not exist, create a new one.
        $createDeckQuery = $connection->prepare("INSERT INTO decks (userID, name) VALUES (?, ?)");
        $createDeckQuery->bind_param("is", $userID, $name);
        $createDeckQuery->execute();
        $createDeckQuery->close();

        //Query database for newly created deckID
        $getDeckIDQuery = $connection->prepare("SELECT MAX(deckID) FROM decks");
        $getDeckIDQuery->execute();
        $result = $getDeckIDQuery->get_result();
        $getDeckIDQuery->close();

        $deckID = $result->fetch_array()[0];

    } else { //Deck already exists, update it
        $updateDeckQuery = $connection->prepare("UPDATE decks SET name = ? WHERE deckID = ?");
        $updateDeckQuery->bind_param("si", $name, $deckID);
        $updateDeckQuery->execute();
        $updateDeckQuery->close();
    }

    echo("$deckID");

    mysqli_close($connection);
?>