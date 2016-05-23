<?php
use \Psr\Http\Message\ServerRequestInterface as Request;
use \Psr\Http\Message\ResponseInterface as Response;

require '../vendor/autoload.php';
require 'config.php';
require 'ItemList.php';
require 'HPEAPI.php';

$app = new \Slim\App(["settings" => $config]);
spl_autoload_register(function ($classname) {
    require ("./" . $classname . ".php");
});

$container = $app->getContainer();
$container['view'] = new \Slim\Views\PhpRenderer("./templates/");
$container['ItemList'] = function($c){
    return new ItemList($c['settings']['db']);
};
$container['HPEAPI'] = function($c){
    return new HPEAPI($c['settings']['HPEAPI']);
};

$app->post   ("/api/$version/audio"           ,'\controller:createTask');
$app->get    ("/api/$version/session/{sess}"  ,'\controller:getSessionResult');

//========Unit Tests=======================
$app->post   ("/api/$version/test"            ,'\unitTest:defaultFun');
$app->post   ("/api/$version/rx"              ,'\unitTest:rx');

//=========================================


$app->run();