**IN PROGRESS - AWATING UWP SDK TO MOVE TO SUPPORT NET STANDARD 2.1**

# HyperMock

## Getting Started

#### Installing the Package

The package is installed via NuGet:

```bash
dotnet nuget install HyperMock
```

**Note** that this installs the latest. Add _--version X.Y.Z_ for a specific version.

## Overview

The original motivation for this library was to bridge a much needed gap within the UWP development platform for unit testing and the lack of available support for mocking.

The API has been designed to be familiar so should be fairly straight-forward to use.

The library targets .NET standard 2.1 so this means that version 3.0.0 onwards will only work for Windows 10 SDK 19041+. The previous _HyperMock.Universal_ still 
exists, although will only be maintained for minimal support purposes.

The advantage of now supporting .NET Standard 2.1 is that there is one library which simplifies development, maintenance and testing.

**Please note** that the documentation below is not exhaustive and more can be found [here](https://github.com/hypersolutions/hypermock/wiki).
### Creating a mock

To create a mock instance, simply call the _Mock.Create<T>_ static class:

```
var mockService = Mock.Create<IAccountService>();
```

This returns back an instance the _Mock_ class that contains all the setup and verify extensions. It also contains the mocked instance. You can pass this into your class:

```
var controller = new AccountController(mockService.Object);
```

### Setup and Verification

The framework allows for both setup of behaviors and verifying that things occurred. There is no need to specify a setup if you don't need to add any behavior. Under the hood, every call is recorded with args passed.

Below shows a simple example where an account is credited with an amount and the underlying service was called once:

```
[TestMethod]
public void CreditAddsToAccount()
{
    var mockService = Mock.Create<IAccountService>();
    var controller = new AccountController(_mockService.Object);
    var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

    controller.Credit(info);

    mockService.Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Once());
}
```

The _Occurred_ class supports a variety of options. For example, below shows that given an invalid amount expected, the service call is never performed:

```
[TestMethod]
public void CreditFailsWithUnknownAmount()
{
    var mockService = Mock.Create<IAccountService>();
    var controller = new AccountController(_mockService.Object);
    var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

    controller.Credit(info);

    mockService.Verify(s => s.Credit(info.Number, 200), Occurred.Never());
}
```

You can control the mock calls by using the _Setup_ method and its functionality. Below we setup the _Credit_ call on the service to throw an exception if the credit amount is below £1:

```
[TestMethod, ExpectedException(typeof(NotSupportedException))]
public void CreditWithInvalidAmountThrowsException()
{
    var mockService = Mock.Create<IAccountService>();
    var controller = new AccountController(_mockService.Object);
    var info = new AccountInfo { Number = "1234", CreditAmount = -100 };
    mockService.Setup(s => s.Credit(info.Number, Param.Is<int>(p => p < 1))).Throws(new NotSupportedException());

    controller.Credit(info);
}
```

As you can see, the _Controller.Credit_ call throws the appropriate exception when we set the credit amount to -£100.

The above example uses the _Param_ class. In that example, you can see that it applies a predicate to deciding to throw an exception. This can also be extended to verify calls to:

```
[TestMethod]
public void CreditWithAmountAboveMin()
{
    var mockService = Mock.Create<IAccountService>();
    var controller = new AccountController(_mockService.Object);
    var info = new AccountInfo { Number = "1234", CreditAmount = 2 };

    controller.Credit(info);

    mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Once());
}
```

So in this example the _Credit_ call was made but in the one below:

```
[TestMethod]
public void CreditFailsWithAmountBelowMin()
{
    var mockService = Mock.Create<IAccountService>();
    var controller = new AccountController(_mockService.Object);
    var info = new AccountInfo { Number = "1234", CreditAmount = 1 };

    _controller.Credit(info);

    _mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Never());
}
```

With the credit amount set to 1, it never calls the service _Credit_ method.
  
## Developer Notes

### Building and Publishing

From the root, to build, run:

```bash
dotnet build --configuration Release 
```

To run all the tests, run:

```bash
dotnet test --no-build --configuration Release
```

To create a package, run:
 
```bash
cd src/HyperMock
dotnet pack --no-build --configuration Release 
```

To publish the package to the nuget feed on nuget.org:

```bash
dotnet nuget push ./bin/Release/HyperMock.3.0.0.nupkg -k [THE API KEY] -s https://api.nuget.org/v3/index.json 
```

### Links

* **GitFlow** https://datasift.github.io/gitflow/IntroducingGitFlow.html
