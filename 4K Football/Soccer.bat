@echo off
start C:\SoccerDemo\CCV.lnk
echo Remember to calibrate CCV
pause
echo Starting local server on port 80...
start "Soccer Demo" "http://localhost:80/"
node "C:\SoccerDemo\server.js"