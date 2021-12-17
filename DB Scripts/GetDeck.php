<?php
    //Get only the specific cards and their amount present in the given deck by deckID and userID. Returns json array of card info.
    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve userID field from POST
    $userID = $_POST["userID"];
    $deckID = $_POST["deckID"];

    //Query database to get ONLY cards owned by userID and assigned to deckID
    $getDeckQuery = $connection->prepare("
        SELECT
            cards.*,
            cardInDeck.cardCount
        FROM cards
        JOIN cardInDeck USING (cardID)
        WHERE userID = ? AND deckID = ?"
    );
    $getDeckQuery->bind_param("ii", $userID, $deckID);
    $getDeckQuery->execute();
    $result = $getDeckQuery->get_result();

    $array = array();
    while($row = $result->fetch_array(MYSQLI_ASSOC)) {
        $array[] = $row;
    } 

    $getDeckQuery->close();

    echo json_encode($array);

    mysqli_close($connection);

?>