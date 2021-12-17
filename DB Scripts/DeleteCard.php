<?php
    //Delete a card from the database by cardID

    require('Config.php');

    $connection = mysqli_connect($SERVER_NAME, $DB_USERNAME, $DB_PASSWORD, $DB);

    if (mysqli_connect_errno()) {
        echo "Connection to database failed.";
        exit();
    }

    //Receieve cardID field from POST
    $cardID = $_POST["cardID"];

    $deleteCardQuery = $connection->prepare("DELETE FROM cards WHERE cardID = ?");
    $deleteCardQuery->bind_param("i", $cardID);
    $deleteCardQuery->execute();
    $deleteCardQuery->close();

    echo("Success.");

    mysqli_close($connection);
?>