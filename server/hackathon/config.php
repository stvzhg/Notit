<?php
/**
 * Created by PhpStorm.
 * User: patri
 * Date: 2016/5/11
 * Time: 14:32
 */

$config['displayErrorDetails']=true;

$config['db']['host']='MYSQL_HOST';
$config['db']['username']='MYSQL_USERNAME';
$config['db']['password']='MYSQL_PASSWORD';
$config['db']['database']='MYSQL_DATABASE';

$config['HPEAPI']['PostURL'] = 'https://api.havenondemand.com/1/api/async/recognizespeech/v1';
//$config['HPEAPI']['PostURL'] = 'http://59.78.36.78:8080/hackathon/api/v1/rx';
$config['HPEAPI']['APIKey']  = 'YOUR_HPE_API_KEY';
$config['HPEAPI']['QueryURL']= 'https://api.havenondemand.com/1/job/status/';

$version = 'v1';