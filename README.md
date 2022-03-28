# SMSNET
An unofficial library for [sms-activate.org](https://sms-activate.org)

- [Installation](#installation)
- [Get started](#get-started)
- [Examples](#examples)
- [Notes](#notes)
- [TODO](#todo)
- [License](#license)

### Installation
You can install this package via [nuget](https://www.nuget.org/packages/SMSNET), by downloading the precompiled binaries [here](/releases/tag/release), or by compiling the project yourself

### Get Started

#### Get your API key
You can locate your API key by heading [here](https://sms-activate.org/en/profile)
![API key location on sms-activate.org](https://i.ibb.co/ynbfZzc/api-key.jpg)

#### Create an SMS object
Start by creating an SMS object which will expose all the endpoints that you would need being linked to the API key from the [first step](#get-your-api-key)
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
}
```
> Note that once you create a new SMS object the library will automatically check your balance which is done to verify that the API key is valid

#### Profit!!!!
That's it! Once you create the object everything is ready and you can start using the library however you want!

### Examples
#### Get your balance
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
  
  public async Task PrintBalance() {
    var balance = await Sms.Account.GetBalance();
    Console.WriteLine(balance);
  }
}
```
#### Get the top countries for Telegram
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
  
  public async Task PrintTopCountries() {
    var topCountriesDictionary = await Sms.Activation.GetTopCountries("tg");
    var telegram = topCountriesDictionary["tg"]; // GetTopCountries always returns a dictionary where the key is the service's name
    int i = 0;
    foreach (var country in telegram.TopCountries) {
      Console.WriteLine($"Place #{i} | Country code: {country.Country} | Price: {country.Price} | Quantity: {country.Count}");
      i++;
    }
  }
}
```

#### Get a number for Telegram
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
  
  public async Task PrintMyNumber() {
    var myNumber = await Sms.Activation.GetNumber("tg", (int)Countries.England); 
    // The Countries enum has all sms-activate.org countries, however, you must cast it to int
    Console.WriteLine($"ID: {myNumber.Id} | Created at: {myNumber.CreatedAt.Value} | Number: {myNumber}"); 
    // Casting a Number object to string will always return a properly formatted number (e.g +12029182132)
  }
}
```

#### Get a code from a new number (using GetNumber)
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
  
  public async Task PrintMySMS() {
    var myNumber = await Sms.Activation.GetNumber("tg", (int)Countries.England); 
    // The Countries enum has all sms-activate.org countries, however, you must cast it to int
    Console.WriteLine($"ID: {myNumber.Id} | Created at: {myNumber.CreatedAt.Value} | Number: {myNumber}"); 
    // Casting a Number object to string will always return a properly formatted number (e.g +12029182132)
    
    await Task.Delay(5000); // Wait for 5 seconds (you would usually have to wait longer for an SMS)
    
    var myStatus = await myNumber.GetActivationStatus();
    
    if (myStatus.Key == ActivationStatus.STATUS_OK) {
      Console.WriteLine($"SMS code received! Code is {myStatus.Value}");
    } else {
      Console.WriteLine("SMS code was not received :( Try again later!");
    }
  }
}
```

#### Get a code from an existing number (without having a Number object)
```c#
public class Program() {
  public SMS Sms = new SMS("INSERT YOUR API KEY HERE");
  
  public async Task PrintStrangersSMS() {
    var myNumber = new Number(69420, Sms); // Where 69420 is the activation ID of the phone number 
    Console.WriteLine($"ID: {myNumber.Id} | Created at: UNKNOWN | Number: {myNumber}"); 
    // Casting a Number object to string will always return a properly formatted number (e.g +12029182132)
    // Also note that the CreatedAt field will always be null if you manually create the Number object (like in this scenario)
    
    await Task.Delay(5000); // Wait for 5 seconds (you would usually have to wait longer for an SMS)
    
    var myStatus = await myNumber.GetActivationStatus();
    
    if (myStatus.Key == ActivationStatus.STATUS_OK) {
      Console.WriteLine($"SMS code received! Code is {myStatus.Value}");
    } else {
      Console.WriteLine("SMS code was not received :( Try again later!");
    }
  }
}
```

### Notes
An important feature that sms-activate.org has is the ability to get a number for multiple services. Sadly, during my tests and attempts at implementing it in this library, I found an issue with their API where it does not respond according to their documentation. In fact, it doesn't respond at all, which is why the method in the library is marked as obsolete. Despite my efforts to report the incident to sms-activate.org's technical support team, I've been unable to reach them and will continue to report this issue to them.

### TODO
- [x] Add basic components
- [x] Add all activation endpoints
- [ ] Add all rent endpoints
- [ ] Add internal endpoints and document them
- [ ] Add a method to automatically wait for an SMS to be received (with a timeout)

### License
Copyright (c) 2022 Kan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
