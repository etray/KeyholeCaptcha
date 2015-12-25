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
- KeyholeCaptcha - contains everything you need, builds into one assembly: 
  KeyholeCaptcha.dll.
- ImageProtoypingApplication - Windows Forms application that demonstrates
  available schemes and is helpful for prototyping new ones.
- WebApplication - Sample ASP.NET web service that integrates KeyholeCaptcha.
- UnitTest - tests used to prototype core functionality, and helpful for
  testing future changes.

Compiling:
- Open KeyholeCaptcha.sln in your favorite .NET IDE, and build.
- You may need to enable nuget to download packages.

Integrating into your web service:
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

	2.) register a handler, and an element that will accept the captcha control:

		KeyHoleCaptcha.registerSuccessHandler(
			function(id){ 
				// save the Id the server gave you for future human authentication
				this.myCaptchaSuccessId = id;
				// do something here, like submit a request to a web service.
			}
		);

		KeyHoleCaptcha.drawControl("aDomElementId");


	3.) Back in your web service business logic, you can verify the captcha was successful:

		if (KeyholeCaptcha.Core.Validator.ValidatedGuid(id))
		{
			// business logic
			...

			// cleanup (optional):
			KeyholeCaptcha.Core.Validator.DisposeEntry(id);
		}

Troubleshooting:
Problem: Frequently see "Too many bad guesses or expired Id" messages 
Solution: Threshold for bad guesses is set at 10. Requst queue is set for 50. High-volume sites may want to choose a higher value.
