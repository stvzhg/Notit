<?php
/**
 * Created by PhpStorm.
 * User: patri
 * Date: 2016/5/12
 * Time: 23:30
 */
function jsonFormater($sqlResult) {
    $data = array();
    foreach($sqlResult as $row){
        $key = $row['itemid'];
        $val = $row['content'];
        $data[$key] = $val;
    }
    if (empty($data)) {
        return '{}';
    }else{
        return json_encode($data);
    }
}

function pythonParser($result) {
    $pycmd = 'python ../lib/asm.py';
    foreach ($result as $uid => $content) {
        $fname = '../upload/'.$uid.'.json';

        $file = fopen ($fname, "w+");
        fwrite($file, $content);
        fclose ($file);

        $pycmd = $pycmd . " $fname";
    }
    $pycmd = $pycmd . " 1234";
    //die($pycmd);  
    system($pycmd);
    //var_dump($pycmd);
    $str = file_get_contents('1234');
    //$str = shell_exec("echo 123");
    return $str;
}