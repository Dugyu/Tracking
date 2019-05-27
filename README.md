# Tracking

Particles following an input Vector3 (currently mouse position).

Step1   Tracking
https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/InputSystem/HandTracking.html
Get index finger tip position from hand tracking profile in MRTK (a public property in controller). Use this position as input Vector3.

Step2   Attract/Reset
https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/Input/Gestures.html
Get and gesture events from gesture handler. "Hold/Release" as "attract/reset" commands. 

Step3   Visual Feedback for Haptics
https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/InputSystem/HandTracking.html
Calculate grab strength using hand joints/palm/finger tip positions.
Use this strength to change color/range of particles.

Step4  Spatial Anchor
https://docs.microsoft.com/en-us/azure/spatial-anchors/quickstarts/get-started-unity-hololens
