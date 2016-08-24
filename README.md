# NMSSaveManager

This utility is a simple loader which lets you manage multiple saves. Currently the in game controls only allow for 1 running save structure. This utility will allow you to create multiple. It performs all of the moving and copying of multiple save directories for you. This is a work in progress, and any feedback, suggestions, or feature requests are welcome.

DISCLAIMER:
I take no responsibility for inadvertant deletion of your save data. If you have a save you are highly invested in, I suggest you back it up before attempting to use this utility. To do so, browse to:

C:\Users\<Username>\AppData\Roaming\HelloGames\NMS

Here you will find a directory that starts with "st_". Copy this directory to a safe location. In the event you lose your data, restore that directory to this path and all will be well.

TODO:
1. This utility launches NMS for you, and I assume the Steam ID is universal for this. If not, I will need to devise a way to either allow the user to modify the launch URI or get the correct Steam ID during the first launch.
2. This was created for use on my Windows 10 machine. I may need to provide a way to allow the user to modify the game data directory if it differs for earlier Windows releases.

USAGE:
When you first run this utility it will detect if you have currently saved data, and allow you to create a new save with this data. I stronly urge you to do this, as creating and launching a new save without doing this will permanently delete any existing data. If you elect no here, when you create a new save, you will have one more opportunity to salvage the current data before it is permanently deleted.

Create New Game - this button will create a new save. Pretty straightforward.
Load Selected Game - this button will move the selected save's game data into the root of the game data directory, and then launch the game.
File -> Import Game - this will allow you to import a save structure from another location.

I hope this utility will help alleviate some of the stress of multi user households who don't want to share 1 save. Again if there are any suggestions, feedback, or feature requests, don't hesitate to let me know and I will see what can be done.
