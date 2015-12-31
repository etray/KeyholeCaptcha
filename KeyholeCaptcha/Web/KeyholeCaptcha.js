// Copyright (c) 2015-2016 - Elton T. Ray
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

if (typeof KeyholeCaptcha === "undefined") {

    (function () {

        var KeyholeCaptchaClass = function(){

            return {
                kcPrefix : "kcx",
                kcLogoBase64: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAbCAYAAACJISRoAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwwAADsMBx2+oZAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTFH80I3AAACy0lEQVRIS72WUUhTURjH55w2tW1MxBbaJqbNoSSpe8mXUbTHIT3IWAWVYlNQWdCSoiAIyYZINlj2EHvpYQ/S6GWatumLgcTQUQuiSRBEjZZt7cFQ9m/nuFt3u7fuDWYHftxz7/n+32+cbfdeCQCJ0WiEwWAoOqQv6U8FEolkz6D9iZFvsVjQ/mIkJSUlUCqVuD0xiXg8jrW1NYyMjPDWFiJKolarMTVxh86t/aNkj7GzvY1TJ07Sa9W1mrz6QkRJZmdnsbIUonNGcn/qHj3vPT+I19Eo5HJ5XoaNoKS1tRWZTCZP8ioSQYW8Ai3tRiSSaSq12WycLIOgxG630yaM5PQ5OzqPdWC/So3wmxhdI8Pj8XCyDIISh8NBmzCSts5ulMnKoNEexufNFF0jw+v1crIMghKz2UybsLfr+tg1Oj8zMIwMXQWcTmdejo2gRCaTIRaL5UnSqRSONDVDWirDk/klbG1tQafTcbIMghKCyWRCcC5A58yvK7SwiNKspL5Rj4sDlzgZNqIkhAatlh4ZCRmDA4OQSqWc2kJESwgWiwXLK6s5BZBKJmHs6uKtZfNPkmAwiNS3TSw+m8PC8xCS39O463Lx1rIRLVEoFPA9ckOtrPp1TVV9ALdc7rw6PkRLjncYoKoqh/qgDoHFJQT8Pqgqy1Hb0JaV1fBmGERLTB3N9Dg0Np77RpC9E4fxKf4FPT09nHo2oiTkNj/a10vnN1yenOL3mJ6e5mTYiJJo6+vw9PEDOj87dDXXGrh5ZRiOy04sv3jJybARJenvu4D014+oq1FgX6UKU+6HcE+OQ15Wiqaj3fixvQON5s/PFFESn89HP/nq8jxaGg/tXs9uYUt7F8LR3Tux1Wrl5BgEJeQfnUgkaCNmbLx7i433H3Jnu2NmZoY3TxCU6PV6rK+vI5J9UP0Nv9/PmycISooB7f9f3ruyW7rHb5CQ/AQpMgGAgzOc0wAAAABJRU5ErkJggg==",
                kcRefresh64: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABsAAAAbCAYAAACN1PRVAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwwAADsMBx2+oZAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTFH80I3AAADBklEQVRIS72WT0tiURjGrYygRVnqIrFtDQgtXBc4BEarMIT+bmKgFoEhiPkpgmBg2rtKXOQ3EGoxm76BFLMoGGiRIpFRvXN/ZzzOvddz1VrMAy/ec95/3uPzPkffIDg7Oxu37Ovq6uq3YDB4uL29XeGTNfv426Gfx97eXjSRSPyIRqO/RkdHxdrqMvbxE0e8tfcxXFxcjG9tbX0PBAJ1a2lsYjLiySPfWvdHpVL5EovFfg4PDxsLhsNh2djYMPow8sinjrX2BgELCwu/rUdjIWx2dlZeXl4kl8vJyMiIMQajjmdDXp1vZD0akyORiKytrUmhUBDw/v4uJycnMjY2ZozHqGc8Us7adHTz8/Nyfn4u9XpdNbGDhqVSSSYmJrryMOpR13r+B1hkIsP6+ro8Pj62S5vx+voqBwcHKt7v9zvyMeo6WAptrQ9H0MrKirRarXZJkaenJ6lWq1IsFts7Is/Pz3J0dCRDQ0NizZ3yk+eutby8TP2/A8ucaAdG4u3tbbukyPX1tcTjceWDIKDRaEg6ne7EX11dqX3yWNvrUV8NPgrgHthsNqsSQa1Wk5mZmY6PZvf397K0tNTZYxyI0yBf+zDq08eH5Ngd/KiXl5ftNJGdnR1HImSANPY9jDgN8t1kU33QOPvm5OSkPDw8qCTmibXd72XEEQ/Id+epPoiqfXN6elqazaZKghR2Xz/ThGJMpqamHL7d3d3Kf3uzUCh0+OHfDJubm+sa5IF+s0HYCNu0b3FxUe7u7hQr9R5sHYiN8L/XnDE/em5SqVRHtnQz5o851Li5ufGeM+ClICgCiShEJpNRiqGBkuCHRBr4k8mkow7WURDgpY1a6/b395UG9gIaipa6a3RpI/BSfQxmlctlpfJucKzcCqZBp16X6oN+9xn31unpqWr49vYmx8fH6n7jnjPFY573Geh3U3Mz5/N5NU92Npqs502tQQDfyOtIsc3NTcc42I088vs20uDVOWsTaXoZ8eR5Hl0vwKJB/zdC7y7WfQYMJAqA5KCln/tH7PP9AZ1QCvMq/JWAAAAAAElFTkSuQmCC",
                domElement: null,
                requestGuid : null,
                captchaServiceUrl: "/KeyholeCaptcha",
                defaultMessage : "Enter the text below:",

                successHandler: function () {
                    throw new Error("Success handler not registered.");
                },

                drawControl: function(element){
                    var self = this;

                    if (typeof element === "string")
                    {
                        this.domElement = document.getElementById(element);
                    }
                    else
                    {
                        this.domElement = element;
                    }

                    if (!this.domElement)
                    {
                        throw new Error("Need valid container element.");
                    }

                    this.domElement.innerHTML = "";
                    
                    var divElement = document.createElement("div");

                    divElement.style.borderRadius = "5px";
                    divElement.style.padding = "8px";
                    divElement.style.color="#dddddd";
                    divElement.style.backgroundColor = "#666688";
                    divElement.style.fontFamily = "sans-serif";
                    divElement.style.fontSize = "12px";
                    divElement.style.fontWeight = "bold";
                    divElement.style.maxWidth="240px";
                    divElement.style.minHeight="80px";

                    var captchaImage = document.createElement("img");
                    captchaImage.id = this.kcPrefix + "captchaImage";
                    captchaImage.onerror = function (error) {
                        self.setMessage("Error loading captcha image.");
                    }

                    var captchaSpan = document.createElement("div");
                    captchaSpan.id = this.kcPrefix + "captchaSpan";
                    captchaSpan.appendChild(captchaImage);
                    captchaSpan.style.cssFloat = "left";
                    captchaSpan.style.width = "200px";
                    captchaSpan.style.height = "50px";
                    captchaSpan.style.marginBottom = "5px";

                    var messageArea = document.createElement("div");
                    messageArea.id = this.kcPrefix + "messageArea";
                    messageArea.innerHTML=this.defaultMessage;

                    var lowerDiv = document.createElement("div");
                    lowerDiv.appendChild(messageArea);
                    lowerDiv.appendChild(captchaSpan);
                    divElement.appendChild(lowerDiv);

                    var buttonSpan = document.createElement("div");

                    var kcLogo = document.createElement("img");
                    kcLogo.style.height = "22px";
                    kcLogo.style.paddingLeft = "3px";
                    kcLogo.src = this.kcLogoBase64;

                    var kcLogoLink = document.createElement("a");
                    kcLogoLink.href = "https://github.com/etray/KeyholeCaptcha";
                    kcLogoLink.style.border = 0;
                    kcLogoLink.style.outline = 0;
                    kcLogoLink.target="_blank";
                    kcLogoLink.appendChild(kcLogo);
                    buttonSpan.appendChild(kcLogoLink);
                    buttonSpan.style.cssFloat = "right";

                    var reloadButton = document.createElement("img");
                    reloadButton.src = this.kcRefresh64;
                    var reloadButtonLink = document.createElement("a");
                    reloadButtonLink.style.border = 0;
                    reloadButtonLink.style.outline = 0;
                    reloadButtonLink.style.display = "block";
                    reloadButtonLink.href = "javascript:void(0);";
                    reloadButtonLink.onclick = function () { KeyholeCaptcha.reload(); }
                    reloadButtonLink.appendChild(reloadButton);
                    buttonSpan.appendChild(reloadButtonLink);

                    lowerDiv.appendChild(buttonSpan);

                    var kcLogo = document.createElement("img");
                    kcLogo.src = this.kcLogoBase64;

                    var submitDiv = document.createElement("div");
                    var inputField = document.createElement("input");
                    inputField.type = "text";
                    inputField.size = "17";
                    inputField.id = this.kcPrefix + "guess";
                    inputField.onkeyup = function (e) {
                        var event = e || window.event;
                        var charCode = event.which || event.keyCode;
                        // submit on enter
                        if ( charCode == '13' ) {
                            self.submit();
                            return false;
                        }
                    }
                    var button = document.createElement("button");
                    button.innerHTML = "Submit";
                    button.onclick = function () {
                        KeyholeCaptcha.submit();
                    }
                    submitDiv.appendChild(inputField);
                    submitDiv.appendChild(button);
                    divElement.appendChild(submitDiv);


                    this.domElement.appendChild(divElement);
                },

                register: function (handler, element) {
                    this.drawControl(element);
                    this.successHandler = handler;

                    // call the get endpoint to get the image url - server side, we create an empty request entry and return a guid
                    // pull off the guid and save as this.requestGuid
                    var self = this;
                    var operationUrl = this.captchaServiceUrl + "?operation=register";

                    this.xhrGet(operationUrl, function (reponse) {
                        self.requestGuid = reponse.Id;
                        KeyholeCaptcha.reload();
                    });
                },

                reload: function () {
                    this.setMessage(this.defaultMessage);
                    var operationUrl = this.captchaServiceUrl + "?operation=reload&id=" + this.requestGuid + "&q=" + this.noCacheString();
                    var captchaImage = document.createElement("img");
                    captchaImage.src = operationUrl;
                    var captchaSpan = document.getElementById(this.kcPrefix + "captchaSpan");

                    captchaSpan.innerHTML = "";

                    captchaSpan.appendChild(captchaImage);
                },

                noCacheString: function ()
                {
                    var result = "";
                    var date = new Date();
                    result += ("0000" + date.getFullYear().toString(16)).slice(-4);
                    result += ("00" + date.getMonth().toString(16)).slice(-2);
                    result += ("00" + date.getDate().toString(16)).slice(-2);
                    result += ("00" + date.getHours().toString(16)).slice(-2);
                    result += ("00" + date.getMinutes().toString(16)).slice(-2);
                    result += ("00" + date.getSeconds().toString(16)).slice(-2);
                    result += (parseInt(("00000" + Math.random()).slice(-5))).toString(16);
                    return result;
                },

                setMessage : function (message)
                {
                    var messageArea = document.getElementById(this.kcPrefix + "messageArea");
                    messageArea.innerHTML = message;
                },

                submit: function () {
                    var guessId = this.kcPrefix + "guess";
                    var guessElem = document.getElementById(guessId);
                    var operationUrl = this.captchaServiceUrl + "?operation=validate&id=" + this.requestGuid + "&guess=" + guessElem.value;

                    var self = this;
                    this.xhrGet(operationUrl, function (response) {
                        if (!response.Success) {
                            self.setMessage(response.Message);
                        }
                        else
                        {
                            self.domElement.innerHTML = "";
                            self.successHandler(response);
                        }
                    });
                    
                },

                xhrGet : function(url, callback)
                {
                    var xhttp = new XMLHttpRequest();
                    xhttp.onreadystatechange = function () {
                        if (xhttp.readyState == 4 && xhttp.status == 200) {
                            callback(JSON.parse(xhttp.responseText));
                        }
                        else if (xhttp.status > 200) {
                            self.setMessage("Error " + xhttp.status + " accessing URL: " + url);
                        }
                    };
                    xhttp.open("GET", url, true);
                    xhttp.send();
                }
            }
        }

        KeyholeCaptcha = new KeyholeCaptchaClass;
    })();
}