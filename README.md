[![Build status](https://ci.appveyor.com/api/projects/status/t6j3xe6cr0mu8si5?svg=true)](https://ci.appveyor.com/project/bchavez/coinbase) [![Nuget](https://img.shields.io/nuget/v/Coinbase.Pro.svg)](https://www.nuget.org/packages/Coinbase/) [![Users](https://img.shields.io/nuget/dt/Coinbase.Pro.svg)](https://www.nuget.org/packages/Coinbase/) <img src="https://raw.githubusercontent.com/bchavez/Coinbase.Pro/master/Docs/coinbase_pro.png" align='right' />

Coinbase.Pro for .NET/C# Library
======================

Project Description
-------------------
A .NET implementation for the [Coinbase Pro API](https://docs.pro.coinbase.com/).

:loudspeaker: ***HEY!*** If you're looking for the [**Coinbase Commerce** API, check this link!](https://github.com/bchavez/Coinbase.Commerce)

#### Supported Platforms
* **.NET Standard 2.0** or later
* **.NET Framework 4.5** or later

#### Crypto Tip Jar
<a href="https://commerce.coinbase.com/checkout/f1f0e303-cb53-4415-b720-4af1df473647"><img src="https://raw.githubusercontent.com/bchavez/Coinbase.Pro/master/Docs/tipjar.png" /></a>
* :dog2: **Dogecoin**: `DGVC2drEMt41sEzEHSsiE3VTrgsQxGn5qe`



### Download & Install
**Nuget Package [Coinbase](https://www.nuget.org/packages/Coinbase/)**

```powershell
Install-Package Coinbase
```

Usage
-----
### API Authentication


----
### Getting Started

For the most part, to get the started, simply new up a new `CoinbaseProClient` object as shown below:
```csharp

```
Once you have a `CoinbaseProClient` object, simply call one of any of the [**Order Endpoints**](https://docs.pro.coinbase.com/?r=1#orders) or [**Market Data Endpoints**](https://docs.pro.coinbase.com/?r=1#market-data). Extensive examples can be [found here]().

In one such example, to get the [spot price](https://developers.coinbase.com/api/v2#get-spot-price) of `ETH-USD`, do the following:
```csharp

```

#### Full API Support
##### Market Data Endpoints

##### Orders Endpoints



### Authentication Details



-------


Easy peasy! **Happy crypto trading!** :tada:


Reference
---------
* [Coinbase Pro API Documentation](https://docs.pro.coinbase.com)


Building
--------
* Download the source code.
* Run `build.cmd`.

Upon successful build, the results will be in the `\__compile` directory. If you want to build NuGet packages, run `build.cmd pack` and the NuGet packages will be in `__package`.



Contributors
---------
Created by [Brian Chavez](http://www.bitarmory.com).

A big thanks to GitHub and all contributors:


---

*Note: This application/third-party library is not directly supported by Coinbase Inc. Coinbase Inc. makes no claims about this application/third-party library.  This application/third-party library is not endorsed or certified by Coinbase Inc.*
