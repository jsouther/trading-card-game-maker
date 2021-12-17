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
    $getCardsQuery = "SELECT cardName, description, summon, elementString, cost, life, attack, defense, cardMax, spellValue, spellType FROM cards;";
    $getCards = mysqli_query($connection, $getCardsQuery) or die ("Get cards query failed.");

    $array = array();
    while($row = mysqli_fetch_assoc($getCards)) {
        $array[] = $row;
    } 

    echo json_encode($array);

    mysqli_close($connection);
?>