<p align="center"> 
   <img src="https://user-images.githubusercontent.com/24996684/35466253-f228fa38-02b6-11e8-8ec6-7d3f57caf2e2.png">
</p>
<p align="center">
   <a href="https://beerpay.io/ArachisH/Tanji"><img src="https://beerpay.io/ArachisH/Tanji/badge.svg?style=beer-square"/></a>
   <a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HMYZ4GB5N2PAU"><img src="https://img.shields.io/badge/style-Donate-009CDE.svg?style=flat-square&label=&logoWidth=16&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8%2F9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMjHxIGmVAAABfUlEQVQ4T2MYfIBXM8dTzGbCczGbPjgWtel7ImTWfoxFJTsDqgwP0KpoZtBv%2F8%2Bg34aJDbr%2BM6jkxEBV4gBaFWvhGnQagbgBgXWb%2FjNoVsyAqsQBtGuugjXrtf5nVMgE4gwElkv%2Fzxg%2B5yFDSrciVDU6kOQCavwKsx2iEWQIFMtl%2FGfoPvdPctGDXqgGNMBtqs1gCPQn0ABGrRpUzbJp%2FxkSl%2F9jmPfgv%2BTie31QHWhAOiaYwaATYoBRwz9Gy8Z%2FjDYtfxkDpv1jaDwC1syw4PF%2FhsKFXlAdqIBFrayWwQAYAzot%2FxmaTvxlWPAIqAGI5wM1zrkHpIGaJ17YAlTKBNGBBrh0a1bAApBh4pW%2FYE2z7%2F5m6Dt9kKHnxFqGlt1pDFpabFDlmIDfrP0iOA2Ydv1jmH3nH9iA%2FlN7odKEgZBJbQ%2BDVtlcBsfOLWCnz7n%2Fn6Hn5FSoNAmgYnUk2L%2FzgIY07EyGihIPbv37x37ly38JEF616j8zVJgWgIEBADmSwSIyemLcAAAAAElFTkSuQmCC"/></a>
   <a href="https://github.com/ArachisH/Tanji/releases/latest"><img src="https://img.shields.io/github/downloads/ArachisH/Tanji/latest/total.svg?style=flat-square"/></a>
   <a href="https://discord.gg/Vyc2gFC"><img src="https://img.shields.io/discord/225010488445108224.svg?style=flat-square"/></a>
   <a href="https://beerpay.io/ArachisH/Tanji?focus=wish"><img src="https://beerpay.io/ArachisH/Tanji/make-wish.svg?style=flat-square"/></a>
</p>

# Tanji
Application built to sniff/manipulate the encrypted traffic between the Habbo client, and the server. Offers a variety of built-in tools for injecting, replacing, and blocking client/server packets. If you're looking to create a module for Tanji, then please proceed to the wiki for further information.

## Origin
I started working on this application a bit after NovoFatum got patched around late 2014(I think), and the original project name used was **Detro[4][2]**. It was only supposed to work on retros at the time(hence the name), since I had no idea how to get around Habbo's crypto after the recent patch. Although, since many retros do not immediately update to the newest revision, that means that they were still susceptible to the *0* exploit that Jose was using in NovoFatum.

*`What is this zero exploit?`*  
To execute this exploit, all you had to do was send a string value of *0* when the client would ask you for the server's public key(I think, I forgot). This would cause the client to generate the RC4 key with *0*, and since this is used for decrypting the server's data this means we can _re-encrypt_ the server's data with *0*.

So cool, we can get this to work on **some** retros, but we want more right? I then begin working on another project currently called [Flazzy](https://github.com/ArachisH/Flazzy), but at the time it was Kroogle, then later changed to FlashInspect. The goal of this project was to allow us to modify the RSA keys in one of the xml files inside of the swf on the fly. If we could do this, then this would allow us to inject our own public RSA keys, and bingo bango MITM attack ez pz. It took me a good while to get Flazzy to the point where it could be able to disassemble, then re-assemble itself without any problems, but it was all worth it.

When I started working on Flazzy I was only focused on bringing further retro support, but as time went on I saw that once I'm able get this working, I would be able to apply this to the original Habbo as a workaround for their recent crypto patch. Tanji now works on the original Habbo aswell, but with limited functionality because I'm too lazy to add anything else to it. I'm getting bored just writing this so I'm not even going to get into [Eavesdrop](https://github.com/ArachisH/Eavesdrop), but basically as time went on I kept improving the ways it would connect/modify. Up to the point where we can fully disassembly/deobfuscate the AVM2 instructions within the swf to have it tell Tanji what key it's using for encryption, and what server it's connecting to.

## How It Works
It just does.

### C'mon, tell me
Alright fine.
1. Intercept the client page, and force it to return a non-cached version of the swf for interception.
2. Intercept the .swf, disassemble, remove host checks, force it to connect to "127.0.0.1", inject key shouter, inject end point shouter, assemble it, and then replace response payload with modified version.
3. Begin listening for the client, since it will try to connect to us, once it connects to us, then wait for it to send us the end point of where it wants us to connect to.
4. We connect.
5. ???
7. Where is 6?
