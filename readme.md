Tray Notifier
=============

A simple Tray Notification application... with plugin support!

By Tom Harvey - [Twitter @tombuildsstuff](http://twitter.com/tombuildsstuff) - [Website](http://ibuildstuff.co.uk)


Get Started
-----------

Tray Notifier is a .net 4 application.

*  Clone It
*  Build It (msbuild)
*  Deploy the 'bin' contents -> some folder
*  Put your own paths in 'TrayNotifier.config' for PluginDirectory and UserDataDirectory
*  (optional) put any extra plugins in the directory specified in 'PluginDirectory'
*  Use It!


Build Your Own Plugin
---------------------

Want to build your own plugin? Awesome.

Here's how:

*  Implement AbstractNotificationSystem (TrayNotifier.Business.AbstractNotificationSystem)

*  (do your coding thing!)

*  Drop your compiled binaries in the PluginDirectory specified in 'TrayNotifier.config'


'Built-In Plugins'
-------------------
*  Cruise Control
*  TeamCity [coming soon]


Where's the Tests?!
-------------------
Coming *real* soon, pinky promise. This went from thought to production in a couple of hours.