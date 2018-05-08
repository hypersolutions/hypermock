HyperMock Project
=================

Overview
--------

The project makes use of the VS shared project to enable windows and universal shared code. Both the HyperMock.Windows and HyperMock.Universal are empty. These both reference the HyperMock shared project. 

The NuGet packages are built from the HyperMock.Windows and HyperMock.Universal projects. The nuspec files reside in the parent directory. 


Examples
--------

The examples project is used as a test bed for the NuGet packages. It does NOT reference the projects directly. 

It contains a simple scenario where the subject under test has a dependency on a service.


Tests
-----

The tests use XUnit. This allows the common tests to work for both UWP and Desktop frameworks. Again, the tests reside in a shared project.

A note of caution. These tests are picked up by the test explorer built into VS but you need to ensure that the default process architecture is set to match the build configuration othewise the UWP tests may not be listed.


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

