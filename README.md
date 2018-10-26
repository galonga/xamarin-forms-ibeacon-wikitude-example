### xamarin-forms-ibeacon-wikitude-example

## What is this?

Example for using Xamarin.Forms with [Wikitude](http://www.wikitude.com/) AR library and Bluetooth iBeacons on Android and iOS from 2013 done as a University Project.

## How to use?

Set the UUID of your Beacons in the AppSettings.cs. Use 3 Beacons with Minor 1-3 for 3D model 1-3. 3D Model will load when the matching Beacon is near (~20cm).

#### FAKE options / Tasks

Execute `bin/fake <taskname>` to run a task or `bin/fake --<optionname>` for fake cli options. First run `bin/fake install`.

Available tasks:

```
* Restore
  Clean solution and afterwards restore all packages

* Build
  Build all projects of solution

```

### More infos

Feel free to visit my [Blog](https://galonga.de/augmented-reality-mit-xamarin-und-wikitude/) there is a nice article (in german) with screenshots.
