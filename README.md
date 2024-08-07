Welcome to the **Roomful Asset Publisher** wiki!

Roomful is your powerful 3D Virtual Collaboration Platform. Create highly engaging interactive experiences for trade Trade Shows, Virtual Offices,
Art Fairs, Science Fairs, Job Fairs, Social Spaces.

---

## What Asset Publisher is used for  
The **Roomful Asset Publisher** is the tool we use to upload and review 3D assets for Roomful. 
With the publisher, you can upload and edit:
* Props
* Styles
* Environment 

The publisher also allow you to not only upload additional content but also define the way your new object would behave in Roomful. 

## How to install  

First of all, you would need to use the exact unity version to ensure asset compatibility with the current Roomful runtime version. <br>
The currently required Unity version is: `2021.3.21f1` <br>
You can get it using [Unity Hub](https://unity.com/download) or [Unity download archive](https://unity.com/releases/editor/archive).

The Asset publisher is distributed as the [Unity Custom Package](https://docs.unity3d.com/Manual/CustomPackages.html)
In Order to use the package, you can download it from the [net.roomful.assets](https://github.com/Roomful/net.roomful.assets) repository and install it as [Local](https://docs.unity3d.com/Manual/CustomPackages.html#LocalMe) or an [Embedded](https://docs.unity3d.com/Manual/CustomPackages.html#EmbedMe) package. Or use our recommended ways to install the package, which are described bellow. 

### Use as Publisher Project
We made the clean empty project with the [net.roomful.assets](https://github.com/Roomful/net.roomful.assets) package preinstalled as well as all the dependencies it may require. This is the easiest way to get your hands on the Roomful Publisher. All you need to do is to download the   [RoomfulAssetPublisher](https://github.com/Roomful/RoomfulAssetPublisher) repository and open the Unity project that comes with it.

### Install from a Git URL
* Open [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window.
* Click the add **+** button in the status bar.
* The options for adding packages appear.
* Select Add package from git URL from the add menu. A text box and an Add button appear.
* Enter the `https://github.com/Roomful/net.roomful.assets` Git URL in the text box and click Add.
* You may also install a specific package version by using the URL with the specified version.
  * `https://github.com/Roomful/net.roomful.assets.git#X.Y.X`
  * Please note that the version `X.Y.Z` stated here is to be replaced with the version you would like to get.
  * You can find all the available releases [here](https://github.com/Roomful/net.roomful.assets/releases).
  * The latest available release version is [![Last Release](https://img.shields.io/github/v/release/roomful/net.roomful.assets)](https://github.com/Roomful/net.roomful.assets/releases/latest)

For more information about what protocols Unity supports, see [Git URLs](https://docs.unity3d.com/Manual/upm-git.html).


## Next Steps
Once you have Roomful Asset Publisher up and running on your machine, it's time to start using it. First if all you need to log in and adjust the publisher setting based on your needs. See the [Account & Settings](https://github.com/Roomful/RoomfulAssetPublisher/wiki/Account) section.

Now it's time to start uploading and editing your content. Use the provided links below to learn how to work with publisher-supported content.
[Props](https://github.com/Roomful/RoomfulAssetPublisher/wiki/Props) | [Styles](https://github.com/Roomful/RoomfulAssetPublisher/wiki/Styles) | [Environments](https://github.com/Roomful/RoomfulAssetPublisher/wiki/Environments)
