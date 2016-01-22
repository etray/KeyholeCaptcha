KeyholeCaptcha is a ready-to-use, moving captcha system that operates by
keeping text continuously obscured, and therefore invisible to computer vision,
but still comfortably readable by humans because of [visual short term memory](https://en.wikipedia.org/wiki/Visual_short-term_memory "Wikipedia"). 

![alt text](https://github.com//etray/KeyholeCaptcha/blob/master/Images/Keyhole.gif?raw=true "Keyhole")
![alt text](https://github.com//etray/KeyholeCaptcha/blob/master/Images/Captcha.gif?raw=true "Captcha")

![alt text](https://github.com//etray/KeyholeCaptcha/blob/master/Images/Screenshot.png?raw=true "Screenshot")

##Features: 
- Dynamic image generation
- Four different visual schemes
- Generate phrases using built-in wordlist or randomly-generated characters
- Passphrase validation and session persistence
- Cryptographically secure random number generation
- Brute force attack detection and prevention
- Code examples and reference implementation that is easy to understand and use
- Compact, lightweight client-side AJAX controls
- Commercial-friendly, MIT-style license

##Contents:
- **KeyholeCaptcha** - contains everything you need, built into one assembly: 
  KeyholeCaptcha.dll.
- **ImageProtoypingApplication** - Windows Forms application that demonstrates
  available schemes and is helpful for prototyping new ones.
- **WebApplication** - Sample ASP.NET web service which integrates KeyholeCaptcha.
- **UnitTest** - tests used to prototype core functionality. Helpful for
  testing future changes.

##Compiling:
- Open KeyholeCaptcha.sln in your favorite .NET IDE, and build.
- You may need to enable nuget to download packages.

##Integrating
(For a hands-on example, see the WebApplication project)
###Integrating into a web service:

1. Add the KeyholeCaptcha.dll assmbly to your .NET web application references and 
   make sure it is included in your deployed application

2. Modify Web.config to include the KeyholeCaptcha Http handler:

```xml
	<system.webServer>
	<handlers>
		...
		<add name="KeyholeCaptchaHandler" verb="*" path="/KeyholeCaptcha" type="KeyholeCaptcha.Web.KeyholeCaptchaHandler" preCondition="integratedMode,runtimeVersionv4.0" />
	</handlers>
	</system.webServer>
```

###In page HTML: 

1. Add the script include:
```html
	<script src="/KeyholeCaptcha?operation=script&version=1"></script>
```
2. Register the api, and it does the rest:
```javascript
	window.onload = function () {

		// Register the KeyholeCaptcha javascript API, and it will handle the rest.
		// first argument to the register method is the function to call when captcha guess is correct.
		// the second arcument is an existing DOM element, into which the captcha control will be built.

		KeyholeCaptcha.register(function(){
				
			// On a successful guess, KeyholeCaptcha.requestGuid holds a token which can be passed to your web service.
				
			window.location = "/BusinessLogic?id=" + KeyholeCaptcha.requestGuid;			

		}, "kcDiv");
	}

```

###Business Logic:
	
1. Client passes the validated id into the web service, and the server checks it before proceeding with business logic.

2. If captcha guess was correct, the id will be recognized as validated (exactly once), and business logic can proceed confidently

```cs
    if (KeyholeCaptcha.Core.Validator.IsValidatedGuid(id))
    {
        // Welcome, human!

		// business logic...
    }
    else
    {
        // Begone, machine!
    }
```
