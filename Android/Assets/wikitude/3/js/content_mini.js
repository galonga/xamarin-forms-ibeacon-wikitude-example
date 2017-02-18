var World = {
	loaded: false,
	rotating: false,

	init: function initFn() {
		/*
			Disable all sensors in "IR-only" Worlds to save performance. If the property is set to true, any geo-related components (such as GeoObjects and ActionRanges) are active. If the property is set to false, any geo-related components will not be visible on the screen, and triggers will not fire.
		*/
		AR.context.services.sensors = false;
		this.createOverlays();
	},

	createOverlays: function createOverlaysFn() {
		/*
			First an AR.Tracker needs to be created in order to start the recognition engine. It is initialized with a URL specific to the target collection. Optional parameters are passed as object in the last argument. In this case a callback function for the onLoaded trigger is set. Once the tracker is fully loaded the function loadingStep() is called.

			Important: If you replace the tracker file with your own, make sure to change the target name accordingly.
			Use a specific target name to respond only to a certain target or use a wildcard to respond to any or a certain group of targets.
		*/
		this.tracker = new AR.Tracker("wtc/targetcollections_4.x.wtc", {
			onLoaded: this.loadingStep
		});

		/*
			3D content within Wikitude can only be loaded from Wikitude 3D Format files (.wt3). This is a compressed binary format for describing 3D content which is optimized for fast loading and handling of 3D content on a mobile device. You still can use 3D models from your favorite 3D modeling tools (Autodesk® Maya® or Blender) but you'll need to convert them into the wt3 file format. The Wikitude 3D Encoder desktop application (Windows and Mac) encodes your 3D source file. You can download it from our website. The Encoder can handle Autodesk® FBX® files (.fbx) and the open standard Collada (.dae) file formats for encoding to .wt3. 

			Create an AR.Model and pass the URL to the actual .wt3 file of the model. Additional options allow for scaling, rotating and positioning the model in the scene.

			A function is attached to the onLoaded trigger to receive a notification once the 3D model is fully loaded. Depending on the size of the model and where it is stored (locally or remotely) it might take some time to completely load and it is recommended to inform the user about the loading time.
		*/
		this.modelSacharo = new AR.Model("augmentation/models/Sacharimeter.wt3", {
			onLoaded: this.loadingStep,
			scale: {
				x: 0.01,
				y: 0.01,
				z: 0.01
			},
			translate: {
				x: -0.3,
				y: 0.0,
				z: 0.0
			},
			rotate: {
				tilt: 0,	// x
				heading: 0,	//y
				roll: 0		// z
			}
		});

		this.animation1 = new AR.ModelAnimation(this.modelSacharo, "Sacharimeter_animation");


		/*
			Similar to 2D content the 3D model is added to the drawables.cam property of an AR.Trackable2DObject.
		*/
		var trackable = new AR.Trackable2DObject(this.tracker, "*", {
			drawables: {
				cam: [this.modelSacharo]
			},
			onEnterFieldOfVision: this.animate
		});
	},

	loadingStep: function loadingStepFn() {
		if (!World.loaded && World.tracker.isLoaded() && World.modelSacharo.isLoaded()) {
			World.loaded = true;

			// Remove Scan target message after 10 sec.
			setTimeout(function() {
				var e = document.getElementById('loadingMessage');
				e.parentElement.removeChild(e);
			}, 10000);
		}
	},

	animate: function amimateFn(){
		World.animation1.start(-1);
	}
};

World.init();