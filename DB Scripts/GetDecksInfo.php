<?php
    // Get rows from 'decks' table given by userID. Returns JSON array.

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve userID field from POST
    $userID = $_POST["userID"];

    $getDecksQuery = $connection->prepare("SELECT deckID, userID, name FROM decks WHERE userID = ?");
    $getDecksQuery->bind_param("i", $userID);
    $getDecksQuery->execute();
    $result = $getDecksQuery->get_result();
    
    //Assign SELECT results to array
    $array = array();
    while($row = $result->fetch_array(MYSQLI_ASSOC)) {
        $array[] = $row;
    }

    $getDecksQuery->close();

    echo json_encode($array);

    mysqli_close($connection);
?>