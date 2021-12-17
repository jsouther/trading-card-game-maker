<?php
    //Creates a new entry in `cardInDeck` table to assign a card to a deck.

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve cardInDeck fields from POST
    $deckID = $_POST["deckID"];
    $cardID = $_POST["cardID"];
    $cardCount = $_POST["cardCount"];

    //Save the selected card into the deck
    $createAssociationQuery = $connection->prepare("INSERT INTO cardInDeck (deckID, cardID, cardCount) VALUES (?, ?, ?)");
    $createAssociationQuery->bind_param("iii", $deckID, $cardID, $cardCount);
    $createAssociationQuery->execute();
    $createAssociationQuery->close();

    echo "Success.";

    mysqli_close($connection);
?>