<?php
class HPEAPI {
    protected $conf;
    
    function __construct($conf) {
        $this->conf = $conf;
    }
    
    function postAudio($filename) {
        $apikey = $this->conf['APIKey'];
        $url    = $this->conf['PostURL'];
        $command = "curl -X POST --form \"apikey=$apikey\" --form \"interval=0\" --form \"file=@$filename\" $url";
        $data = shell_exec($command);
        if($data !== null) {
            $jid = json_decode($data, true)['jobID'];
            return $jid;
        } else {
            return false;
        }
    }

    function getResult($jid) {
        $curl = curl_init();
        $url = $this->conf['QueryURL'] . $jid . '?apikey=' . $this->conf['APIKey'];
        curl_setopt($curl, CURLOPT_URL, $url);
        curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
        $data = curl_exec($curl);
        curl_close($curl);
        $re = json_decode($data, true)['actions'][0];
        if ($re['status'] == 'finished') {
            return $re['result'];
        } else {
            return false;
        }
    }

}