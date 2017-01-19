# EXCoreDemo
This project is for Exchange Core Team demo use.

There are 3 projects in this solution:
EXCoreDemo.AccessToken, project for getting AccessToken. 
You can call the AccessTokenFactory to get the access token against different resources(Azure Graph, Microsoft Graph, Exchange REST). Currently it only support Web-APP & password and User. The cert is expired and I have not renew it yet.

EXCoreDemo.Core, project for calling API. Currently it only supports use Microsoft Graph fetch users or user(by Id), messages(by user Id) or message(by user Id & message Id), groups & group(by group Id). Before you calling those method, you need to request access token 1st.

EXCoreDemo.Console, project for simple demo.

How to use:
Download the source code, build it and execute the EXCoreDemo.Console.exe directly.
