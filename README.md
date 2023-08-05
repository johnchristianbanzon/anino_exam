# anino_exam

Links available to download

Android(Deploygate): https://dply.me/wryxs5
Drive(APK): https://drive.google.com/file/d/1Xgz0J3LKfY3cDIbRN7_YVRp6Ft0vFmKQ/view?usp=sharing
IOS: 

Documentation in sheets: https://docs.google.com/spreadsheets/d/1qZWIL7M0UeVhMBX0bN71L9xGWLAgIvUnL5Zp5lJBMAo/edit?usp=sharing

Notes:
- DID NOT INCLUDE FIREBASE IN PROJECT REPOSITORY, PLEASE PREPARE CONFIG BEFOREHAND.
- Preferred resolution 2160x1080 (was not able to optimize)
- Build added as external links because Builds are forced to be ignored.


Main Core architecture:
- using DI (IoC) principle with zenject framework (https://github.com/modesttree/Zenject)

Points of interest:
- Scalability  : With the classes injected it would be easier and more cleaner to update or add new code as well as add new functionalities.
- Flexibility  : App has some flexibility thanks to the firebase config.
- MVC : With the usage of DI it is more easier to connect model and view even without a controller as DI will only inject if needed and will be able to use interface to further optimize access modifiers.


Points for improvement:
*App
- Wonky animation, needed some more time for tweaking
- Scatter function (wanted)
- Improvement in spinning

Code
- Incomplete code architecture
- Pop up systems
- Too many temp
- Points of refactor
