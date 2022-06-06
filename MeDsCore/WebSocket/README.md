# The WebSocket namespace short doc 

This namespace provides abstractions to interact with Discord WS APIs.

## Base
`IWebSocketClient` 
This's a base interface, which provides abstractions to send and receive bytes via the WS Connection

## Gateway
Discord Gateway allows you to receive events (like a creating a new message).

`DiscordGatewayConnector` class includes methods to estabilish your Discord Gateway WS connection and sending/receiving bytes from WS Stream

`DiscordMethodExecutor` class includes methods to execute WS API methods.
