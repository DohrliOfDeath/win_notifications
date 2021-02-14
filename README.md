# win_notifications
## Dependencies
* PyQt5

## Usage pyqt5-notification.py
For having the message displayed for 5 seconds:  
```sh
python pyqt-notification.py 5 "Notification Head" "Notification Body"
```

## GetNotifications
Reads all current Notifications, and writes them to $HOME/.notificationsCache.txt

## TODO
* write daemon, which regularly checks for new notifications and calls the python script
* daemon: extend global settings