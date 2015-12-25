KeyholeCaptcha is a full-featured, open-source moving captcha system that
relies on the same human ability to filter out noise as is used in static
image catcha, but with better readability.

It picks up where other video captcha schemes leave off, by relying less on
motion and more on noise. The result is that individual frames are less
discernable by a machine and more readable by a human.

Features: 
- Dynamic image generation
- Four different visual schemes to choose from
- Generate phrases using built-in wordlist, or randomly-generated characters
- Passphrase validation and session persistence
- Cryptographically secure random number generation
- Brute force attack detection and prevention
- Code examples and reference implementation that is easy to understand and use
- Compact and lightweight client-side AJAX controls
- Commercial-friendly, MIT-style license

Contents:
- KeyholeCaptcha - contains everything you need, built into one assembly: 
  KeyholeCaptcha.dll.
- ImageProtoypingApplication - Windows Forms application that demonstrates
  available schemes and is helpful for prototyping new ones.
- WebApplication - Sample ASP.NET web service that integrates KeyholeCaptcha.
- UnitTest - tests used to prototype core functionality. Helpful for
  testing future changes.

Compiling:
- Open KeyholeCaptcha.sln in your favorite .NET IDE, and build.
- You may need to enable nuget to download packages.

Integrating into your web service:
(For a hands-on example, see the WebApplication project)

	1.) Add the KeyholeCaptcha.dll assmbly to your .NET web application 
		references and add it to your deployed assmblies

	2.) Modify Web.config to include the KeyholeCaptcha Http handler:

		  <system.webServer>
			<handlers>
			  ...
			  <add name="KeyholeCaptchaHandler" verb="*" path="/KeyholeCaptcha" type="KeyholeCaptcha.Web.KeyholeCaptchaHandler" preCondition="integratedMode,runtimeVersionv4.0" />
			</handlers>
		  </system.webServer>

In your HTML: 

	1.) Add the script include:
    
		<script src="/KeyholeCaptcha?operation=script&version=1"></script>

	2.) Register the api, and it does the rest:

		<script type="text/javascript">

			window.onload = function () {

				// Register the KeyholeCaptcha javascript API, and it will handle the rest.
				// first argument to the register method is the function to call when captcha guess is correct.
				// the second arcument is an existing DOM element, into which the captcha control will be built.

				KeyholeCaptcha.register(function(){
				
					// On a successful guess, KeyholeCaptcha.requestGuid holds a token which can be passed to your web service.
				
					window.location = "/BusinessLogic?id=" + KeyholeCaptcha.requestGuid;			

				}, "kcDiv");
			}

		</script>


Back in your web service:
	
	1.) Client will pass the validated id to your web service.

	2.) If captcha guess was correct, the id is recognized as validated and business logic is allowd to proceed

            if (KeyholeCaptcha.Core.Validator.IsValidatedGuid(id))
            {
                // Welcome, human!
            }
            else
            {
                // Begone, machine!
            }
