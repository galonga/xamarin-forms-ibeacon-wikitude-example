using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libWikitudeSDK.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator | LinkTarget.Simulator64, IsCxx=true, SmartLink=true, LinkerFlags="-lc++ -lz -ObjC", Frameworks="Accelerate AssetsLibrary AVFoundation CFNetwork CoreGraphics CoreLocation CoreMedia CoreMotion CoreText CoreVideo Foundation MediaPlayer OpenGLES QuartzCore Security UIKit")]
