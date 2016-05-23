<?php
/**
 * Created by PhpStorm.
 * User: patri
 * Date: 2016/5/11
 * Time: 14:32
 */

use \Psr\Http\Message\ServerRequestInterface as Request;
use \Psr\Http\Message\ResponseInterface as Response;

require_once('utils.php');

class controller {
    protected $ci;

    public function __construct($ci) {
        $this->ci = $ci;
    }

    public function getSessionResult(Request $request,Response $response, $args) {
        $session = $args['sess'];
        $itemList = $this->ci->ItemList;
        $api      = $this->ci->HPEAPI;

        $idlist = $itemList->getItemBySession($session);

        $out = array();
        $flag = true;
        foreach ($idlist as $iid) {
            $row = $itemList->getItemByID($iid);
            //var_dump($row);
            switch ($row['status']){
                case 2:
                    $out[$row['uid']] = $row['content'];
                    break;
                case 1:
                    $content = $api->getResult($row['jobid']);
                    $flag = false;
                    if($content !== false) {
                        $flag = true;
                        $content = json_encode($content);
                        $out[$row['uid']] = $content;
                        $itemList->updateItem($iid,'content', $content);
                        $itemList->updateItem($iid,'status',2);
                    }
                    break;
                default:
                    $flag = false;
            }
        }
        //var_dump($out);
        if($flag){
            $response = $response->withStatus(200);
            //$response = $response->write(pythonParser($out));
            $response = $response->write(pythonParser($out));
            return $response;
        }
        $response = $response->withStatus(400); //Fail
        return $response;
    }

    public function createTask(Request $request,Response $response, $args) {
        $content = $request->getParsedBody();
        $uid = $content['uid'];
        $mid = $content['mid'];
        $fid = $uid . $mid . date("Y.m.d") . date("h:i:sa");
        if (!file_exists("../upload/" . $_FILES["file"]["name"]))
		{
			$_FILES["file"]["name"]=$fid . ".mp3";
			move_uploaded_file($_FILES["file"]["tmp_name"],
			"../upload/" . $_FILES["file"]["name"]);
		}
        
        $itemList = $this->ci->ItemList;
        $api      = $this->ci->HPEAPI;
        
        $itemid = $itemList->createItem($uid,$mid,$fid);
        $ret = $api->postAudio('../upload/'.$fid.'.mp3');

        if ($ret !== false) {
            $itemList->updateItem($itemid, 'status', 1);
            $itemList->updateItem($itemid, 'jobid', $ret);
            $response = $response->write($ret);
            $response = $response->withStatus(201); //Created
            return $response;
        }
        $response = $response->withStatus(400); //Created
        return $response;
    }
}