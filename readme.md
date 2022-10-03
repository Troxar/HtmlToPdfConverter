Implement a web service. The service should accept an HTML file from a web client, convert it to PDF using Puppeteer Sharp, and return it somehow to the client.
Create a simple web client that will communicate with the web service. It should be a web page with “Convert” button, which allows selection of a file from a computer. Then it should display some progress indicator and once the file is converted, allow you to download it.

Note: Do not spend too much time on the web client. We will look mostly at the back-end service.

Things to consider:
· The incoming HTML files may be big. It means conversion may take a long time. Like a few minutes.
· The web service should be stable. It means if client sent a file for conversion and then, after receiving request from a client IIS was restarted, the web service should still be able to return the result PDF to a client after restarting.
· The web service should be scalable. It means we should be able to extend our infrastructure without the need to change service code. Only by the hands of system administrators.