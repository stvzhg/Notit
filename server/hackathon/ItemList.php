<?php

/**
 * Created by PhpStorm.
 * User: patri
 * Date: 2016/5/12
 * Time: 21:59
 */
class ItemList
{
    private $sql;

    public function __construct($sqlConfig) {
        $sql = new mysqli($sqlConfig['host'],
            $sqlConfig['username'],
            $sqlConfig['password'],
            $sqlConfig['database']);
        if ($sql->connect_errno) {
            die("SQL connection failed: ".$sql->connect_error);
        }
        $this->sql = $sql;
    }

    public function __destruct() {
        $this->sql->close();
    }

    private function checkSQLError() {
        if ($this->sql->errno) {
            die('SQL error: ' . $this->sql->error);
        }
    }

    public function createItem($uid, $session, $fid) {
        $query = $this->sql->prepare('INSERT INTO item(uid,session,status,file) VALUES (?, ?, ?, ?)');
        $defaultStatus = '0';
        $query->bind_param('ssss', $uid, $session, $defaultStatus, $fid);
        $query->execute();
        $this->checkSQLError();
        return $this->sql->insert_id;
    }

    public function getItemID($uid, $session) {
        $query = $this->sql->prepare('SELECT itemid FROM item WHERE uid=? AND session=?');
        $query->bind_param('ss', $uid, $session);
        $query->execute();
        $result = $query->get_result();
        $result = $result->fetch_row();
        $this->checkSQLError();
        return $result['status'];
    }

    public function updateItem($itemid, $column, $value) {
        $query = $this->sql->prepare("UPDATE item SET $column=? WHERE itemid=?");
        $query->bind_param('sd', $value, $itemid);
        $query->execute();
        $this->checkSQLError();
    }

    public function getItemByID($itemid) {
        $query = $this->sql->prepare('SELECT * FROM item WHERE itemid=?');
        $query->bind_param('d', $itemid);
        $query->execute();
        $result = $query->get_result();
        $result = $result->fetch_assoc();
        $this->checkSQLError();
        return $result;
    }

    public function getItemBySession($session) {
        $query = $this->sql->prepare('SELECT itemid FROM item WHERE session=?');
        $query->bind_param('s', $session);
        $query->execute();
        $this->checkSQLError();
        $result = $query->get_result();
        $result = $result->fetch_all(MYSQLI_ASSOC);

        foreach ($result as &$row) {
            $row = $row['itemid'];
        }
        return $result;
    }
    
}