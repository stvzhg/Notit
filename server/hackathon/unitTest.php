<?php

/**
 * Created by PhpStorm.
 * User: patri
 * Date: 2016/5/21
 * Time: 23:46
 */
use \Psr\Http\Message\ServerRequestInterface as Request;
use \Psr\Http\Message\ResponseInterface as Response;

class unitTest {
    protected $ci;

    public function __construct($ci) {
        $this->ci = $ci;
    }

    public function defaultFun(Request $request, Response $response, $args){
        $itemList = $this->ci->ItemList;
        $api      = $this->ci->HPEAPI;

        $idlist = $itemList->getItemBySession('865084493');
        var_export($idlist);
        die();
    }

    public function rx(Request $request, Response $response, $args) {
        $out=array();
        $out['request']=$request;
        $out['response']=$response;
        $out['file']=$_FILES;
        file_put_contents( '../upload/rx.log', '<?php return '.var_export( $out, true ).";\n" );
        return;
    }



}