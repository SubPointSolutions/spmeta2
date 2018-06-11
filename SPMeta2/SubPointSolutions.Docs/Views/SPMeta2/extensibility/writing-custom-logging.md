---
Title: Custom logging
FileName: custom-logging.html
Order: 600
---

# Logging
Logging is an essential part of software development. 
It provides developers and support teams a deeper overview and enable them to see what is happening in the application. 

SPMeta2 has built-in logging to trace most of the critical operations related to provision. 
A high level abstraction for all logging operation is covered by TraceServiceBase class. TraceServiceBase class is an abstract one. It has a basic set of methods to report information, exception and warning as following:
* TraceServiceBase.Critical(int id, object message)
* TraceServiceBase.Error(int id, object message)
* TraceServiceBase.Warning(int id, object message)
* TraceServiceBase.Information(int id, object message)
* TraceServiceBase.Verbose(int id, object message)

All the codebase refers to logging subsustem as TraceServiceBase never knowing what is the actual implementation of the logging operations.
Obtaining a concrete implementation of the loggin service should be done as following:

Here is an example how a logging service instance can be obtained and used:
<a href="_samples/writing-custom-logging-GetDefaultTraceServiceInstance.sample-ref"></a>

SPMeta2 has a default implementation of TraceServiceBase which is TraceSourceService. We use default .NET logging based on Diagnostic.TraceSource. 
Default TraceSourceService class created a new instance of Diagnostic.TraceSource with the name 'SPMeta2'. It redirects all the logging to the trace listners defined in the app.config file for 'SPMeta2' name.

#### Replacing default logging
As SPMeta2 is used as a 3rd part library in your application, it may be necessary to integrate SPMeta2 logging output with your preferred logging. 
That integration can be accomplished in two steps:
* A custom logging service is to be written
* Default logging service needs to be replaced

Here is an example on a custom logging service implementation:
<a href="_samples/writing-custom-logging-CustomLoggingServiceClass.sample-ref"></a>

Once a custom logging service is implemented, we still need to replace the default logging service with our custom one. That can be accomplished with the following code:
<a href="_samples/writing-custom-logging-RegisterCustomLogginService.sample-ref"></a>

Once done, SPMeta2 will use custom logging service to push all trace messages.