HyperMock Project
=================

Overview
--------

The project makes use of the VS shared project to enable windows and universal shared code. Both the HyperMock.Windows and HyperMock.Universal are empty. These both reference the HyperMock shared project. 

The NuGet packages are built from the HyperMock.Windows and HyperMock.Universal projects. The nuspec files reside in the
parent directory. 


Examples
--------

The examples project is used as a test bed for the NuGet packages. It does NOT reference the projects directly. 

It contains a simple scenario where the subject under test has a dependency on a service.


Tests
-----

The tests use MSTest. In each test there is a #if statement at the top to load the correct version of MSTest for each platform.

I have tried NUnit but although it seems to work, both R# and Test Explorer do not work consistently well (at time of writing).

To avoid frustration, the tests make use in a few places of #if blocks. The main place this occurrs is around exception testing.

MSTest is not consistent in this area. One platform prefers Assert.ThrowsException<> and the other uses the ExpectedException attribute. Neither have a shared process.

Also due to the inconsistences again, standard DataRow tests are supported by the less elegant:

var data = []{ 1,2,3} - whatever your values are.

This will be reviewed in time when hopefully a framework that works well for all platforms and test runners becomes available.


Branches
--------

From release 2.0.2 onwards, there will be release branches for making changes and fixing bugs. The structure is thus:

master
    release-2.0.2
        pr-123
        pr-456
        etc
    release-2.0.3
        pr-789
        etc

The NuGet packages will be released from the main release branch. At this stage any changes to the release branch will be pushed back into the master branch.

