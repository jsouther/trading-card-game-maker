<?php
    //Creates a new entry in `games` table

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve game fields from POST
    $userID = $_POST["userID"];
    $gameName = $_POST["gameName"];
    $spellPointsPerTurn = $_POST["spellPointsPerTurn"];
    $startingSpellPoints = $_POST["startingSpellPoints"];
    $handSize = $_POST["handSize"];
    $maxSize = $_POST["maxSize"];
    $cardsPerTurn = $_POST["cardsPerTurn"];
    $maxSummons = $_POST["maxSummons"];
    $elementFactor = $_POST["elementFactor"];
    $timeLimit = $_POST["timeLimit"];
    $turnLimit = $_POST["turnLimit"];
    $startingLife = $_POST["startingLife"];

    
    $createGameQuery = $connection->prepare(
        "INSERT INTO games (
            userID,
            gameName,
            spellPointsPerTurn,
            startingSpellPoints,
            handSize,
            maxSize,
            cardsPerTurn,
            maxSummons,
            elementFactor,
            timeLimit,
            turnLimit,
            startingLife)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
    );

    $createGameQuery->bind_param(
        "isiiiiiiiiii",
        $userID,
        $gameName,
        $spellPointsPerTurn,
        $startingSpellPoints,
        $handSize,
        $maxSize,
        $cardsPerTurn,
        $maxSummons,
        $elementFactor,
        $timeLimit,
        $turnLimit,
        $startingLife
    );

    $createGameQuery->execute();
    $createGameQuery->close();

    echo("Success.");

    mysqli_close($connection);
?>