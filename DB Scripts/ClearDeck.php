<?php
    //Clears all card entries associated with a given deckID

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve deckID from POST
    $deckID = $_POST["deckID"];

    //Remove the currently saved cards in the deck
    $clearCardsInDeckQuery = $connection->prepare("DELETE FROM cardInDeck WHERE deckID = ?");
    $clearCardsInDeckQuery->bind_param("i", $deckID);
    $clearCardsInDeckQuery->execute();
    $clearCardsInDeckQuery->close();

    echo "Success.";

    mysqli_close($connection);
?>