#Server Backend for Notit (Team Avalanche for SF-Hackathon 2016)

## Usage:

* configure server web root to `.\hackathon`
* Give read/write permission to `.\hackathon` for your user that runs php (e.g. `www-data`)
    * We see that it's not usaul and safe to do so. This decision is a dirty fix for a bug on our developing server.
    * A fix may be provided in the future.
* Give read/write permission to `.\upload` for your user that executes php
* Fill In your HPE ApiKey into `.\hackathon\config.php`
* Upload this onto your server

## Requirement:
* MySQL
* Python 2.7 Environment
* `curl` programme in default search path.

## Other information
Sorry for the rather dirty code. The server oringinally runs on RPi3. All stuff developed in 20 hours. Possible factor in future.